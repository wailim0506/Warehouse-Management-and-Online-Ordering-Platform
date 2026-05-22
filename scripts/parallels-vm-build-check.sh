#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage:
  scripts/parallels-vm-build-check.sh [options]

Options:
  --vm-id <uuid>            Parallels VM UUID. If omitted, auto-detect a single running VM.
  --repo-path <path>        Host repo path to validate. Default: current directory.
  --share-script <path>     Host script used to expose repo into VM.
                            Default: /Users/mythic3014/PhpstormProjects/jobs-borads/share-parallels-vm-repo.sh
  --skip-share              Skip repo sharing step.
  --skip-dotnet-install     Skip .NET SDK bootstrap inside VM.
  --skip-test-build         Skip TestController build stage.
  --log-file <path>         Write full workflow log to file.
  -h, --help                Show help.

Workflow:
  1. Ensure VM is running.
  2. Ensure repo is shared into VM.
  3. Resolve VM repo path from \\Mac\\project.
  4. Restore NuGet packages.
  5. Build controller project with net472 override.
  6. Optionally build TestController project.
EOF
}

log() {
  printf '[%s] %s\n' "$(date '+%Y-%m-%d %H:%M:%S')" "$*"
}

fail() {
  log "FAIL: $*"
  exit 1
}

require_cmd() {
  local cmd="$1"
  command -v "$cmd" >/dev/null 2>&1 || fail "Missing required command: $cmd"
}

VM_ID=""
REPO_PATH="$(pwd -P)"
SHARE_SCRIPT="/Users/mythic3014/PhpstormProjects/jobs-borads/share-parallels-vm-repo.sh"
SKIP_SHARE=0
SKIP_DOTNET_INSTALL=0
SKIP_TEST_BUILD=0
LOG_FILE=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --vm-id)
      VM_ID="${2:-}"
      shift 2
      ;;
    --repo-path)
      REPO_PATH="${2:-}"
      shift 2
      ;;
    --share-script)
      SHARE_SCRIPT="${2:-}"
      shift 2
      ;;
    --skip-share)
      SKIP_SHARE=1
      shift
      ;;
    --skip-dotnet-install)
      SKIP_DOTNET_INSTALL=1
      shift
      ;;
    --skip-test-build)
      SKIP_TEST_BUILD=1
      shift
      ;;
    --log-file)
      LOG_FILE="${2:-}"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      fail "Unknown argument: $1"
      ;;
  esac
done

if [[ -n "$LOG_FILE" ]]; then
  mkdir -p "$(dirname "$LOG_FILE")"
  exec > >(tee -a "$LOG_FILE") 2>&1
fi

require_cmd prlctl
require_cmd bash

[[ -d "$REPO_PATH" ]] || fail "Repo path does not exist: $REPO_PATH"

if [[ -z "$VM_ID" ]]; then
  mapfile -t running_ids < <(prlctl list -a | awk '/running/ {gsub(/[{}]/,"",$1); print $1}')
  if [[ "${#running_ids[@]}" -eq 1 ]]; then
    VM_ID="${running_ids[0]}"
    log "Auto-detected running VM: $VM_ID"
  else
    fail "Please pass --vm-id. Running VM candidates: ${#running_ids[@]}"
  fi
fi

vm_exec_cmd() {
  local cmd="$1"
  prlctl exec "$VM_ID" cmd.exe /c "$cmd"
}

vm_exec_ps() {
  local script="$1"
  prlctl exec "$VM_ID" powershell -NoProfile -ExecutionPolicy Bypass -Command "$script"
}

vm_state="$(prlctl list "$VM_ID" -i | awk -F': ' '/^State:/{print $2}')"
if [[ "$vm_state" != "running" ]]; then
  log "Starting VM: $VM_ID"
  prlctl start "$VM_ID" >/dev/null
fi

log "VM ready: $VM_ID"

if [[ "$SKIP_SHARE" -eq 0 ]]; then
  [[ -x "$SHARE_SCRIPT" || -f "$SHARE_SCRIPT" ]] || fail "Share script not found: $SHARE_SCRIPT"
  share_name="$(basename "$REPO_PATH" | tr '[:upper:]' '[:lower:]' | sed 's/[^a-z0-9]/-/g')"
  if prlctl list "$VM_ID" -i | grep -F "  $share_name (+) path='$REPO_PATH' mode='rw'" >/dev/null 2>&1; then
    log "Share already configured, skipping: $share_name"
  else
    log "Sharing repo into VM"
    bash "$SHARE_SCRIPT" --vm-id "$VM_ID" --host-path "$REPO_PATH" >/dev/null
  fi
fi

repo_basename="$(basename "$REPO_PATH")"
VM_REPO_UNC="\\\\Mac\\project\\$repo_basename"
repo_probe_cmd="if exist \"$VM_REPO_UNC\" (echo ok) else exit /b 3"

set +e
vm_repo_path_raw="$(vm_exec_cmd "$repo_probe_cmd" 2>/dev/null)"
probe_status=$?
set -e

if [[ $probe_status -ne 0 ]]; then
  fail "Repo is not visible in VM at $VM_REPO_UNC"
fi

[[ "$(printf '%s' "$vm_repo_path_raw" | tr -d '\r\n')" == "ok" ]] || fail "Unexpected VM repo probe output."
log "VM repo path: $VM_REPO_UNC"

log "Ensuring nuget.exe exists in VM"
vm_exec_ps "New-Item -ItemType Directory -Force -Path C:\Temp | Out-Null; if (-not (Test-Path C:\Temp\nuget.exe)) { Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'C:\Temp\nuget.exe' }"

if [[ "$SKIP_DOTNET_INSTALL" -eq 0 ]]; then
  log "Ensuring local dotnet SDK exists in VM"
  vm_exec_ps "New-Item -ItemType Directory -Force -Path C:\Temp | Out-Null; if (-not (Test-Path C:\dotnet\dotnet.exe)) { Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'C:\Temp\dotnet-install.ps1'; & C:\Temp\dotnet-install.ps1 -Channel 8.0 -InstallDir C:\dotnet }"
fi

[[ "$(vm_exec_cmd "if exist C:\\dotnet\\dotnet.exe (echo yes) else (echo no)" | tr -d '\r')" == "yes" ]] || fail "C:\\dotnet\\dotnet.exe is missing in VM"

log "Restoring NuGet packages"
vm_exec_cmd "pushd \"$VM_REPO_UNC\" >nul 2>&1 && C:\Temp\nuget.exe restore LMCIS-1DG4.sln && popd" >/dev/null

FRAMEWORK_OVERRIDE="C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2"

log "Building controller project"
vm_exec_cmd "pushd \"$VM_REPO_UNC\" >nul 2>&1 && C:\dotnet\dotnet.exe msbuild templatev1\lib\controller\controller\controller.csproj /t:Build /p:Configuration=Debug /p:FrameworkPathOverride=\"$FRAMEWORK_OVERRIDE\" && popd"

if [[ "$SKIP_TEST_BUILD" -eq 0 ]]; then
  log "Building test project"
  vm_exec_cmd "pushd \"$VM_REPO_UNC\" >nul 2>&1 && C:\dotnet\dotnet.exe msbuild TestController\TestController.csproj /t:Build /p:Configuration=Debug /p:FrameworkPathOverride=\"$FRAMEWORK_OVERRIDE\" && popd"
fi

log "DONE"
