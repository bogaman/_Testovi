@echo off
setlocal
chcp 65001 > nul

REM === Podesi putanju do originalnog .ovpn fajla ===
set OVPN_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\mojvpn.ovpn"

REM === Podesi naziv fajla bez putanje ===
set CONFIG=mojvpn.ovpn

REM === OpenVPN GUI i destinacija ===
set OVPN_GUI="C:\Program Files\OpenVPN\bin\openvpn-gui.exe"
::set OVPN_TARGET_DIR="C:\Program Files\OpenVPN\config"

echo.
::echo 🔁 [0/4] Kopiram .ovpn fajl u OpenVPN config folder...
::copy /Y %OVPN_SOURCE% %OVPN_TARGET_DIR%
::if errorlevel 1 (
    ::echo ❌ Greška pri kopiranju fajla.
    ::exit /b 1
::)
echo.

cd "C:\Program Files\OpenVPN\bin\"
::"C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\mojvpn.ovpn"
echo 🔄 [1/4] Pokrećem VPN konekciju: %CONFIG%
::%OVPN_GUI% --connect %CONFIG%
::start  "" %OVPN_GUI% --connect %OVPN_SOURCE%
openvpn-gui.exe --connect ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\mojvpn.ovpn
echo.

REM === Sačekaj da se VPN stabilizuje ===
echo ⏳ [2/4] Čekam 30 sekundi da se VPN stabilizuje...
timeout /t 30 /nobreak > nul
echo.

REM === Pokreni testove ===
echo 🚀 [3/4] Pokrećem testove...

::cls
cd "C:\_Testovi\AMSOsiguranje\Produkcija\"

::@echo off
set BASE_URL=https://master-test.ams.co.rs/
set NACIN_POKRETANJA=automatski

echo. >> Logovi\_log_WS_Produkcija.txt
echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] WebShopNovi... >> Logovi\_log_WS_Produkcija.txt
dotnet test --filter FullyQualifiedName=Produkcija.WebShop >> Logovi\_log_WS_Produkcija.txt
echo dotnet test --filter FullyQualifiedName=Produkcija.WebShop | findstr /i "Passed" >> Logovi\_log_AO_Produkcija.txt 2>&1 

::dotnet test
echo.

REM === Diskonektuj VPN ===
echo 🔌 [4/4] Zatvaram VPN konekciju...
%OVPN_GUI% --command disconnect_all
echo.

REM === (Opcionalno) obriši fajl iz config foldera ===
::echo 🧹 Brišem privremeni .ovpn fajl iz config foldera...
::del /f /q "%OVPN_TARGET_DIR%\%CONFIG%"

echo ✅ Završeno! Testovi gotovi, VPN zatvoren, fajl uklonjen.
pause
endlocal
