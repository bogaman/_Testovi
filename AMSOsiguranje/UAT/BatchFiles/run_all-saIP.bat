@echo off
setlocal
chcp 65001 > nul

REM === Podesi putanju do originalnog .ovpn fajla i auth.txt fajla ===
set OVPN_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\mojvpn.ovpn"
set AUTH_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\auth.txt"

REM === Imena fajlova (bez putanje) ===
set CONFIG=mojvpn.ovpn
set AUTH_FILE=auth.txt

REM === Putanje do OpenVPN GUI i config foldera ===
set OVPN_GUI="C:\Program Files\OpenVPN\bin\openvpn-gui.exe"
set OVPN_TARGET_DIR="C:\Program Files\OpenVPN\config"

REM === Maksimalno vreme cekanja na VPN konekciju (u sekundama) ===
set TIMEOUT=60

echo.
echo [0/6] Kopiram .ovpn i auth.txt fajlove u OpenVPN config folder...
copy /Y %OVPN_SOURCE% %OVPN_TARGET_DIR%
if errorlevel 1 (
    echo Greska pri kopiranju .ovpn fajla.
    exit /b 1
)

copy /Y %AUTH_SOURCE% %OVPN_TARGET_DIR%
if errorlevel 1 (
    echo Greska pri kopiranju auth.txt fajla.
    exit /b 1
)

REM === Kopiraj i key fajl ako je potreban ===
set KEY_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-tls.key"
copy /Y %KEY_SOURCE% %OVPN_TARGET_DIR%

REM === Kopiraj i key fajl ako je potreban ===
set P12_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN.p12"
copy /Y %P12_SOURCE% %OVPN_TARGET_DIR%

pause 
echo.
echo [1/6] Citam trenutnu IP adresu...
for /f "delims=" %%i in ('powershell -Command "(Invoke-WebRequest -Uri 'https://api.ipify.org').Content"') do set ORIGINAL_IP=%%i
echo IP pre VPN: %ORIGINAL_IP%


echo.
echo [2/6] Pokrecem VPN konekciju: %CONFIG%
start "" %OVPN_GUI% --connect %CONFIG%

REM === Cekaj dok se IP adresa ne promeni ===
echo.
echo [3/6] Cekam da se IP adresa promeni (maksimalno %TIMEOUT% sekundi)...

set ELAPSED=0
set INTERVAL=3

:loop
timeout /t %INTERVAL% /nobreak > nul
set /a ELAPSED+=%INTERVAL%

for /f "delims=" %%i in ('powershell -Command "(Invoke-WebRequest -Uri 'https://api.ipify.org').Content"') do set CURRENT_IP=%%i
echo Trenutni IP: %CURRENT_IP%

if not "%CURRENT_IP%"=="%ORIGINAL_IP%" (
    echo IP adresa je promenjena. VPN konekcija je uspesna.
    goto test
)

if %ELAPSED% GEQ %TIMEOUT% (
    echo VPN konekcija NIJE uspostavljena na vreme.
    goto cleanup
)

goto loop

:test
echo.
echo [4/6] Pokrecem testove...

cd "C:\_Testovi\AMSOsiguranje\UAT\"

::@echo off
set BASE_URL=https://master-test.ams.co.rs/
set NACIN_POKRETANJA=automatski

echo. >> Logovi\_log_WS_UAT.txt
echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] WebShopNovi... >> Logovi\_log_WS_UAT.txt
dotnet test --filter FullyQualifiedName=UAT.WebShop >> Logovi\_log_WS_UAT.txt
echo dotnet test --filter FullyQualifiedName=UAT.WebShop | findstr /i "Passed" >> Logovi\_log_AO_UAT.txt 2>&1 

echo.

:cleanup
echo [5/6] Iskljucujem VPN konekciju...
%OVPN_GUI% --command disconnect_all

echo [6/6] Brisem privremene fajlove...
del /f /q "%OVPN_TARGET_DIR%\%CONFIG%"
del /f /q "%OVPN_TARGET_DIR%\%AUTH_FILE%"

echo.
echo Gotovo. VPN zatvoren, fajlovi obrisani.
pause
endlocal
