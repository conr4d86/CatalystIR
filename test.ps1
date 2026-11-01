$ErrorActionPreference = 'Stop'
$compiler = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
$output = Join-Path $PSScriptRoot 'build'
$dataRoot = Join-Path $output ('tests-' + [Guid]::NewGuid().ToString('N').Substring(0,8))
New-Item -ItemType Directory -Path $dataRoot -Force | Out-Null
$app = Join-Path $output 'CatalystIR.exe'
if (-not (Test-Path -LiteralPath $app)) { & (Join-Path $PSScriptRoot 'build.ps1') }

function Invoke-OfflineApp([string[]]$Arguments) {
    $quoted = $Arguments | ForEach-Object { if ($_.Contains('"')) { throw 'Ruta incorrecta' }; '"' + $_ + '"' }
    $process = Start-Process -FilePath $app -ArgumentList $quoted -WindowStyle Hidden -Wait -PassThru
    if ($process.ExitCode -ne 0) { throw "Falló la prueba offline: $($Arguments[0]); salida $($process.ExitCode)" }
}
function Run-Test([string]$Name,[string[]]$Sources,[string[]]$Arguments,[switch]$Multi) {
    $paths = $Sources | ForEach-Object { Join-Path $PSScriptRoot $_ }
    $testExe = Join-Path $dataRoot ($Name + '.exe')
    $defines = if ($Multi) { '/define:AUTOBOOT,MULTI_IR' } else { '/define:AUTOBOOT' }
    & $compiler /nologo /platform:x86 /target:exe $defines "/main:$Name" "/out:$testExe" /reference:System.Web.Extensions.dll /reference:System.IO.Compression.dll "/resource:$PSScriptRoot\distribution\firmware-multi-patch.json,patch.json" $paths
    if ($LASTEXITCODE -ne 0) { throw "Falló la compilación de $Name" }
    & $testExe @Arguments
    if ($LASTEXITCODE -ne 0) { throw "Falló $Name" }
}

Run-Test 'IRLevelsTests' @('IRLevelsTests.cs','IRLevels.cs','IRFilters.cs','FirmwareCodec.cs','FFTBank.cs') @((Join-Path $dataRoot 'levels'))
Run-Test 'OneClickWorkflowTests' @('OneClickWorkflowTests.cs','OneClickWorkflow.cs','OneClickBundle.cs','FirmwareCodec.cs','FirmwarePatch.cs','FFTBank.cs','MultiIRCodec.cs') @((Join-Path $dataRoot 'workflow')) -Multi

# Bundle extraction and candidate generation are offline; no USB ports opened.
$bundleRoot = Join-Path $dataRoot 'bundle'
Invoke-OfflineApp @('--bundle-check',$bundleRoot)
$bundle = Get-Content -LiteralPath (Join-Path $bundleRoot 'bundle-check.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$official = Join-Path $bundle.runtime 'official.ctf'
Run-Test 'MultiIRTests' @('MultiIRTests.cs','MultiIRCodec.cs','FirmwareCodec.cs','FirmwarePatch.cs','FFTBank.cs','TemplateSource.cs') @($official,(Join-Path $dataRoot 'containers')) -Multi

$fixtures = Join-Path $dataRoot 'fixtures'
New-Item -ItemType Directory -Path $fixtures -Force | Out-Null
for ($slot=0; $slot -lt 3; $slot++) {
    $path = Join-Path $fixtures ('IR' + ($slot+1) + '.wav')
    $writer = [IO.BinaryWriter]::new([IO.File]::Create($path))
    try {
        $writer.Write([Text.Encoding]::ASCII.GetBytes('RIFF')); $writer.Write([int](36+8192)); $writer.Write([Text.Encoding]::ASCII.GetBytes('WAVEfmt ')); $writer.Write([int]16)
        $writer.Write([uint16]3); $writer.Write([uint16]1); $writer.Write([int]48000); $writer.Write([int]192000); $writer.Write([uint16]4); $writer.Write([uint16]32)
        $writer.Write([Text.Encoding]::ASCII.GetBytes('data')); $writer.Write([int]8192)
        for ($sample=0; $sample -lt 2048; $sample++) {
            [single]$value=0
            if ($sample -eq ($slot*17)) { $value=[single](0.25*($slot+1)) }
            if ($sample -eq 2047) { $value=[single](-0.125) }
            $writer.Write($value)
        }
    } finally { $writer.Dispose() }
}
Run-Test 'MultiIRSlotsTests' @('MultiIRSlotsTests.cs','MultiIRSlots.cs','OneClickWorkflow.cs','OneClickBundle.cs','FirmwareCodec.cs','FirmwarePatch.cs','FFTBank.cs','IRLevels.cs','IRFilters.cs','MultiIRCodec.cs') @((Join-Path $dataRoot 'profiles'),$fixtures) -Multi
Invoke-OfflineApp @('--ui-slot-check',(Join-Path $dataRoot 'ui'),$fixtures,(Join-Path $dataRoot 'ui.json'))
Write-Output "PASS: pruebas offline completas. Resultados: $dataRoot"
