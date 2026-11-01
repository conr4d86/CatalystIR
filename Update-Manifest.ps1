$ErrorActionPreference = 'Stop'
# Deliberate source changes only. This does not build or contact a device.
$repoRoot = (Resolve-Path -LiteralPath $PSScriptRoot).Path
$entries = Get-ChildItem -LiteralPath $repoRoot -File -Recurse | Where-Object {
    $relative = $_.FullName.Substring($repoRoot.Length + 1).Replace('\','/')
    $relative -ne 'SHA256SUMS.json' -and
    $relative -notmatch '^(\.git|build|runtime|backups|settings|sessions)/' -and
    $relative -notmatch '(^|/)\.env($|\.)' -and
    $_.Extension.ToLowerInvariant() -notin @('.exe','.dll','.pdb','.wav','.ctf','.catl6p','.catl6a')
} | Sort-Object FullName | ForEach-Object {
    [PSCustomObject]@{path=$_.FullName.Substring($repoRoot.Length + 1).Replace('\','/');sha256=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
}
$encoded = ConvertTo-Json -InputObject @($entries) -Depth 4
[IO.File]::WriteAllText((Join-Path $repoRoot 'SHA256SUMS.json'), $encoded, [Text.UTF8Encoding]::new($false))
Write-Output "Inventario actualizado: $($entries.Count) archivos."
