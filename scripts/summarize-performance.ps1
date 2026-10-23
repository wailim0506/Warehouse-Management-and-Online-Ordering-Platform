[CmdletBinding()]
param(
    [string] $LogPath = (Join-Path (Split-Path $PSScriptRoot -Parent) 'Logs'),
    [string] $OutputPath
)

$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $LogPath)) { throw "Log path was not found: $LogPath" }
$files = if (Test-Path -LiteralPath $LogPath -PathType Leaf) {
    @(Get-Item -LiteralPath $LogPath)
} else {
    @(Get-ChildItem -LiteralPath $LogPath -File -Filter 'Log-*.txt')
}

# Parse only aggregate timings. Never export SQL, credentials, account IDs, or other log text.
$samples = New-Object 'System.Collections.Generic.List[object]'
foreach ($file in $files) {
    foreach ($line in Get-Content -LiteralPath $file.FullName) {
        if ($line -match '\[Performance\] - Authentication completed in (?<ms>\d+) ms\.$') {
            $samples.Add([pscustomobject]@{ Operation = 'Authentication'; Rows = $null; Ms = [long] $Matches.ms })
        } elseif ($line -match '\[Performance\] - Catalog (?<phase>query returned|rendered) (?<rows>\d+) rows in (?<ms>\d+) ms\.$') {
            $operation = if ($Matches.phase -eq 'rendered') { 'Catalog render' } else { 'Catalog query' }
            $samples.Add([pscustomobject]@{ Operation = $operation; Rows = [int] $Matches.rows; Ms = [long] $Matches.ms })
        }
    }
}
if ($samples.Count -eq 0) { throw 'No performance measurements were found. Exercise login or the catalog in the refined app first.' }

# Keep row counts separate so a small catalog is not compared with a large catalog.
$summary = @($samples | Group-Object -Property Operation, Rows | ForEach-Object {
    $durations = @($_.Group.Ms | Sort-Object)
    $count = $durations.Count
    [pscustomobject][ordered]@{
        Operation = $_.Group[0].Operation
        Rows = $_.Group[0].Rows
        Samples = $count
        MinMs = $durations[0]
        P50Ms = $durations[[Math]::Ceiling($count * 0.50) - 1]
        P95Ms = $durations[[Math]::Ceiling($count * 0.95) - 1]
        MaxMs = $durations[$count - 1]
    }
} | Sort-Object Operation, Rows)

$json = ConvertTo-Json -InputObject $summary -Depth 3
if ($OutputPath) { Set-Content -LiteralPath $OutputPath -Value $json -Encoding UTF8 }
$json
