@echo off
REM setlocal
chcp 65001 > nul
setlocal ENABLEDELAYEDEXPANSION

REM === Podesi putanje do originalnog .ovpn. auth.txt. .key i .P12 fajla ===
set OVPN_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\mojvpn.ovpn"
set AUTH_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\auth.txt"
set KEY_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-tls.key"
set P12_SOURCE="C:\Program Files\OpenVPN\bin\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-config\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN\ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN.p12"

REM === Imena fajlova (bez putanje) ===
set CONFIG_FILE=mojvpn.ovpn
set AUTH_FILE=auth.txt
set KEY_FILE=ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN-tls.key
set P12_FILE=ovpn-pfs-UDP4-11944-AMSO-GUEST-OPENVPN.p12

REM === Putanje do OpenVPN GUI i config foldera ===
set OVPN_GUI="C:\Program Files\OpenVPN\bin\openvpn-gui.exe"
set OVPN_TARGET_DIR="C:\Program Files\OpenVPN\config"

REM === Maksimalno vreme cekanja na VPN konekciju (u sekundama) ===
::set TIMEOUT=60

REM Interna VPN IP ili hostname koji je dostupan SAMO preko VPN-a
::set VPN_INTERNAL_IP=10.0.0.1

REM Pauza za uspostavljanje konekcije (u sekundama)
set WAIT_SECONDS=10

REM === Parametri za ping petlju ===
set TIMEOUT_SECONDS=60
set INTERVAL_SECONDS=5
set /a ELAPSED=0


echo.
REM === Kopiranje .ovpn. auth.txt. .key i .P12 fajla u Config folder ===
::echo [0/7] Kopiram .ovpn i auth.txt fajlove u OpenVPN config folder...
::copy /Y %OVPN_SOURCE% %OVPN_TARGET_DIR%
::if errorlevel 1 (
    :.echo Greska pri kopiranju .ovpn fajla.
    ::exit /b 1
::)
::copy /Y %AUTH_SOURCE% %OVPN_TARGET_DIR%
::if errorlevel 1 (
    :.echo Greska pri kopiranju auth.txt fajla.
    :.exit /b 1
::)
::copy /Y %KEY_SOURCE% %OVPN_TARGET_DIR%
::if errorlevel 1 (
    ::echo Greska pri kopiranju .key fajla.
    ::exit /b 1
::)
::copy /Y %P12_SOURCE% %OVPN_TARGET_DIR%
:.if errorlevel 1 (
    ::echo Greska pri kopiranju .p12 fajla.
    ::exit /b 1
::)



pause 
echo.
echo [1/7] Citam trenutnu IP adresu...
for /f "delims=" %%i in ('powershell -Command "(Invoke-WebRequest -Uri 'https://api.ipify.org').Content"') do set ORIGINAL_IP=%%i
echo IP pre VPN: %ORIGINAL_IP%


echo.
echo [2/7] Pokrecem VPN konekciju: %CONFIG_FILE%
start "" %OVPN_GUI% --connect %CONFIG_FILE%


echo.
echo [3/7] Cekam %WAIT_SECONDS% sekundi da se konekcija uspostavi...
timeout /t %WAIT_SECONDS% /nobreak > nul

echo.
echo [4/7] Proveravam da li je VPN adapter aktivan...
netsh interface show interface | findstr /i "VPN" > nul
if errorlevel 1 (
    echo VPN adapter NIJE aktivan. Izlazim.
    goto cleanup
)
echo ✅ VPN adapter je aktivan.

REM === [4/7] Ping IP u opsegu 10.41.0.1 - 10.41.254.254 ===
echo.
echo [4/7] Trazim aktivnu IP adresu u opsegu 10.41.x.x ...

:ping_loop
for /L %%x in (0,1,254) do (
    for /L %%y in (1,1,254) do (
        set "TARGET_IP=10.41.%%x.%%y"
        ping -n 1 !TARGET_IP! > nul
        if !errorlevel! == 0 (
            echo ✅ Pronadjena aktivna VPN IP: !TARGET_IP!
            goto test
        )
    )
)

set /a ELAPSED+=%INTERVAL_SECONDS%
if !ELAPSED! GEQ %TIMEOUT_SECONDS% (
    echo ❌ Nije pronadjena aktivna IP u roku od %TIMEOUT_SECONDS% sekundi.
    goto cleanup
)

timeout /t %INTERVAL_SECONDS% /nobreak > nul
goto ping_loop

REM === [5/7] Pokretanje testova ===
:test
echo.
echo [6/7] Pokrecem testove...

cd "C:\_Testovi\AMSOsiguranje\UAT\"

::@echo off
set BASE_URL=https://master-test.ams.co.rs/
set NACIN_POKRETANJA=automatski

echo. >> Logovi\_log_WS_Proba2.txt
echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] WebShopNovi... >> Logovi\_log_WS_Proba2.txt
dotnet test --filter FullyQualifiedName=UAT.WebShop >> Logovi\_log_WS_Proba2.txt
echo dotnet test --filter FullyQualifiedName=UAT.WebShop | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 

echo.

:cleanup
echo [7/7] Iskljucujem VPN konekciju...
%OVPN_GUI% --command disconnect_all

::echo [6/6] Brisem privremene fajlove...
::del /f /q "%OVPN_TARGET_DIR%\%CONFIG_FILE%"
::del /f /q "%OVPN_TARGET_DIR%\%AUTH_FILE%"
::del /f /q "%OVPN_TARGET_DIR%\%KEY_FILE%"
::del /f /q "%OVPN_TARGET_DIR%\%P12_FILE%"


echo.
echo Gotovo. VPN zatvoren, fajlovi obrisani.
pause
endlocal
