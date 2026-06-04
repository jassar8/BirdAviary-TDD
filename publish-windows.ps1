# Builds a standalone Windows exe and copies it next to the logo, outside the source folders.
$ErrorActionPreference = "Stop"

$repoRoot = $PSScriptRoot
$appProject = Join-Path $repoRoot "BirdAviaryManagement.App\BirdAviaryManagement.App.csproj"
$publishDir = Join-Path $repoRoot "publish"
$outputExe = Join-Path (Split-Path $repoRoot -Parent) "BirdAviaryManagement.exe"

Write-Host "Publishing Bird Aviary Management for Windows..."
dotnet publish $appProject `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $publishDir

$publishedExe = Join-Path $publishDir "BirdAviaryManagement.exe"
if (-not (Test-Path $publishedExe)) {
    throw "Publish failed: $publishedExe was not created."
}

Copy-Item $publishedExe $outputExe -Force
Write-Host ""
Write-Host "Done!"
Write-Host "Run the app: $outputExe"
