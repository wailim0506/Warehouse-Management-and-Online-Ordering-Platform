[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Release',
    [string] $Solution = (Join-Path (Split-Path $PSScriptRoot -Parent) 'LMCIS-1DG4.sln'),
    [string] $ResultsDirectory = (Join-Path (Split-Path $PSScriptRoot -Parent) 'TestResults')
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path $PSScriptRoot -Parent
$solutionPath = [IO.Path]::GetFullPath($Solution)
$resultsPath = [IO.Path]::GetFullPath($ResultsDirectory)
$testProject = Join-Path $repoRoot 'TestController\TestController.csproj'
$testAssembly = Join-Path $repoRoot "TestController\bin\$Configuration\TestController.dll"
$settingsPath = Join-Path $PSScriptRoot 'validation.runsettings'

foreach ($path in @($solutionPath, $testProject, $settingsPath)) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Required validation file was not found: $path"
    }
}

$msbuild = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$vsInstall = if (Test-Path -LiteralPath $vswhere) {
    & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -property installationPath
} else { $null }
if ($msbuild) {
    $msbuildPath = $msbuild.Source
} else {
    if (-not $vsInstall) { throw 'MSBuild.exe was not found on PATH and vswhere.exe could not locate Visual Studio Build Tools.' }
    $msbuildPath = Join-Path $vsInstall 'MSBuild\Current\Bin\MSBuild.exe'
    if (-not (Test-Path -LiteralPath $msbuildPath)) { throw "MSBuild was not found at $msbuildPath" }
}

Write-Host "Restoring NuGet packages for $solutionPath"
& $msbuildPath $solutionPath /t:Restore /m /verbosity:minimal /p:RestorePackagesConfig=true "/p:Configuration=$Configuration"
if ($LASTEXITCODE -ne 0) { throw "MSBuild package restore failed with exit code $LASTEXITCODE" }

Write-Host "Building solution ($Configuration) with $msbuildPath"
& $msbuildPath $solutionPath /t:Build /m /verbosity:minimal "/p:Configuration=$Configuration" /p:RestorePackages=false
if ($LASTEXITCODE -ne 0) { throw "Solution build failed with exit code $LASTEXITCODE" }

if (-not (Test-Path -LiteralPath $testAssembly -PathType Leaf)) { throw "Test assembly was not produced: $testAssembly" }
New-Item -ItemType Directory -Force -Path $resultsPath | Out-Null
$vstest = if ($vsInstall) {
    Join-Path $vsInstall 'Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe'
} else {
    Join-Path (Split-Path $msbuildPath -Parent) 'vstest.console.exe'
}

Write-Host "Running xUnit tests from $testAssembly"
$runId = Get-Date -Format 'yyyyMMdd-HHmmss'
$runResults = Join-Path $resultsPath $runId
New-Item -ItemType Directory -Force -Path $runResults | Out-Null
if (Test-Path -LiteralPath $vstest -PathType Leaf) {
    & $vstest $testAssembly "/Settings:$settingsPath" "/ResultsDirectory:$runResults" '/Logger:trx'
} else {
    $dotnet = Get-Command dotnet.exe -ErrorAction SilentlyContinue
    if (-not $dotnet) { throw 'Neither Visual Studio vstest.console.exe nor dotnet.exe was found to run the tests.' }
    & $dotnet.Source vstest $testAssembly "/Settings:$settingsPath" "/ResultsDirectory:$runResults" '--logger:trx'
}
if ($LASTEXITCODE -ne 0) { throw "VSTest failed with exit code $LASTEXITCODE" }

$trx = Get-ChildItem -LiteralPath $runResults -Filter '*.trx' | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $trx) { throw "VSTest did not produce a TRX result in $runResults" }
[xml] $report = Get-Content -LiteralPath $trx.FullName
$total = [int] $report.TestRun.ResultSummary.Counters.total
if ($total -lt 1) { throw 'No tests were discovered; refusing to report a successful validation.' }
Write-Host "Passed test discovery: $total tests. Results: $($trx.FullName)"
