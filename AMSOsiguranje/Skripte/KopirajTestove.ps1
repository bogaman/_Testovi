# Skripta za kopiranje testova iz jednog foldera u drugi, ignoriše određene fajlove
# Pravi rezervnu kopiju ciljnog foldera pre nego što izbriše bilo šta
# Kopira i prilagođava namespace-a u .cs fajlovima

# Kako se korist:
# .\KopirajTestove.ps1 Razvoj Test
# .\KopirajTestove.ps1 Test UAT
# .\KopirajTestove.ps1 UAT Produkcija

# Ako želiš da dodaš još izuzetaka, samo dopuni liniju:
# $exceptions = @("README.md", "konfiguracije.json", "nestoDrugo.cs")





param (
    [Parameter(Mandatory=$true)]
    [string]$sourceEnv,

    [Parameter(Mandatory=$true)]
    [string]$targetEnv
)

# Formiraj putanje
$sourceFolder = "Testovi.$sourceEnv"
$targetFolder = "Testovi.$targetEnv"

# Proveri da li izvorni folder postoji
if (-not (Test-Path $sourceFolder)) {
    Write-Error "Izvorni folder '$sourceFolder' ne postoji!"
    exit 1
}

# Kreiraj ciljni folder ako ne postoji
if (-not (Test-Path $targetFolder)) {
    New-Item -ItemType Directory -Path $targetFolder -Force | Out-Null
    Write-Host "Kreiran folder: $targetFolder"
}

# ✅ 1. Napravi rezervnu kopiju ciljnog foldera
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupFolder = "${targetFolder}_backup_$timestamp"

Copy-Item -Path $targetFolder -Destination $backupFolder -Recurse -Force
Write-Host "✅ Rezervna kopija foldera napravljena: $backupFolder"

# ✅ 2. Obriši sve *.cs fajlove osim izuzetaka
$exceptions = @("README.md", "konfiguracije.json")

Get-ChildItem -Path $targetFolder -Filter *.cs | Where-Object {
    $exceptions -notcontains $_.Name
} | ForEach-Object {
    Remove-Item $_.FullName -Force
    Write-Host "🗑️  Obrisano: $($_.Name)"
}

# ✅ 3. Kopiraj i prilagodi namespace
Get-ChildItem -Path $sourceFolder -Filter *.cs | Where-Object {
    $exceptions -notcontains $_.Name
} | ForEach-Object {
    $sourceFile = $_.FullName
    $destinationFile = Join-Path -Path $targetFolder -ChildPath $_.Name

    Get-Content $sourceFile | ForEach-Object {
        $_ -replace "namespace Testovi\.$sourceEnv", "namespace Testovi.$targetEnv"
    } | Set-Content -Path $destinationFile

    Write-Host "📄 Kopirano: $($_.Name)"
}
