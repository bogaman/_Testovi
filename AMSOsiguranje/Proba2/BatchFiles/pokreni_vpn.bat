@echo off
echo 🔄 Pokrecem OpenVPN konekciju...

REM NAVEDI IME TVOG .ovpn fajla bez putanje
set CONFIG=mojvpn.ovpn

REM PUTANJA DO OpenVPN GUI aplikacije
set OVPN_GUI="C:\Program Files\OpenVPN\bin\openvpn-gui.exe"

%OVPN_GUI% --connect %CONFIG%

echo ✅ Konekcija pokrenuta (ako .ovpn fajl postoji).
exit
