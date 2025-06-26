@echo off
echo 🔌 Prekidam sve OpenVPN konekcije...

REM PUTANJA DO OpenVPN GUI aplikacije
set OVPN_GUI="C:\Program Files\OpenVPN\bin\openvpn-gui.exe"

%OVPN_GUI% --command disconnect_all

echo 🛑 VPN konekcije zatvorene.
exit
