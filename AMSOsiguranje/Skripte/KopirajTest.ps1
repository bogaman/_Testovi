# Skripta za kopiranje testova iz jednog foldera u drugi, ignoriše određene fajlove

# Primer poziva
# .\KopirajTest.ps1 -sourcePath ".\Razvoj\TestLogin.cs" -destPath ".\Test\TestLogin.cs" -oldNamespace "Razvoj" -newNamespace "Test"

param (
    [string]$sourcePath,
    [string]$destPath,
    [string]$oldNamespace,
    [string]$newNamespace
)

if (!(Test-Path $sourcePath)) {
    Write-Host "Fajl $sourcePath ne postoji."
    exit
}

$content = Get-Content $sourcePath -Raw
$content = $content -replace "namespace\s+$oldNamespace", "namespace $newNamespace"

$destFolder = Split-Path $destPath
if (!(Test-Path $destFolder)) {
    New-Item -ItemType Directory -Path $destFolder -Force | Out-Null
}

Set-Content -Path $destPath -Value $content
Write-Host "Fajl uspešno kopiran i namespace zamenjen."
