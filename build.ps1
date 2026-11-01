param([string]$OutputDirectory = (Join-Path $PSScriptRoot 'build'))
$ErrorActionPreference = 'Stop'
$compiler = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler)) { throw 'No se encuentra el compilador de .NET Framework. Consulta docs/COMPILAR.md.' }
$inventory = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'SHA256SUMS.json') -Raw -Encoding UTF8 | ConvertFrom-Json
foreach ($entry in $inventory) {
    $source = Join-Path $PSScriptRoot $entry.path
    if ((Get-FileHash -LiteralPath $source -Algorithm SHA256).Hash.ToLowerInvariant() -ne $entry.sha256) { throw "Contenido cambiado: $($entry.path). Para cambios deliberados, ejecuta Update-Manifest.ps1." }
}
$sources = @('OneClickApp.cs','OneClickBundle.cs','OneClickWorkflow.cs','GlassUI.cs','ProjectLinks.cs','FirmwareCodec.cs','MultiIRCodec.cs','MultiIRSlots.cs','FFTBank.cs','FirmwarePatch.cs','TemplateSource.cs','IRLevels.cs','IRFilters.cs','EngineProbe.cs','CentralAdapter.cs','InProcessSDK.cs','AppVersion.cs') | ForEach-Object { Join-Path $PSScriptRoot $_ }
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$output = Join-Path (Resolve-Path -LiteralPath $OutputDirectory).Path 'CatalystIR.exe'
& $compiler /nologo /platform:x86 /target:winexe /define:IN_PROCESS,AUTOBOOT,MULTI_IR /main:OneClickApp "/out:$output" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Web.Extensions.dll /reference:System.IO.Compression.dll "/win32icon:$PSScriptRoot\assets\catalystir.ico" "/resource:$PSScriptRoot\assets\catalystir.ico,application.ico" "/resource:$PSScriptRoot\assets\catalyst-cx60-front.png,amplifier.png" "/resource:$PSScriptRoot\included\private-bundle.zip,private-bundle.zip" "/resource:$PSScriptRoot\distribution\firmware-multi-patch.json,patch.json" $sources
if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación' }
Get-FileHash -LiteralPath $output -Algorithm SHA256
