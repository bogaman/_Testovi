cls
cd "C:\_Testovi\AMSOsiguranje\Razvoj\"

::@echo off
set BASE_URL=https://Razvojamso-webshop.eonsystem.rs/
set NACIN_POKRETANJA=automatski
::set BASE_URL=https://proba2amsomaster.eonsystem.rs
::set BASE_URL=https://master-test.ams.co.rs
::set BASE_URL=https://eos.ams.co.rs


REM echo. > Logovi\_log_WS_Razvoj.txt
REM echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] IndividualnoPutno... >> Logovi\_log_WS_Razvoj.txt
REM dotnet test --filter Name=IndividualnoPutno | findstr /i "Passed" >> Logovi\_log_WS_Razvoj.txt 2>&1  

REM echo. >> Logovi\_log_WS_Razvoj.txt
REM echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] PorodicnoPutno... >> Logovi\_log_WS_Razvoj.txt
REM dotnet test --filter Name=PorodicnoPutno | findstr /i "Passed" >> Logovi\_log_WS_Razvoj.txt 2>&1  

REM echo. >> Logovi\_log_WS_Razvoj.txt
REM echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] SaViseUlazaka... >> Logovi\_log_WS_Razvoj.txt
REM dotnet test --filter Name=SaViseUlazaka | findstr /i "Passed" >> Logovi\_log_WS_Razvoj.txt 2>&1  

echo. >> Logovi\_log_WS_Razvoj.txt
echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] WebShopNovi... >> Logovi\_log_WS_Razvoj.txt
dotnet test --filter FullyQualifiedName=Razvoj.WebShop >> Logovi\_log_WS_Razvoj.txt
echo dotnet test --filter FullyQualifiedName=Razvoj.OsiguranjeVozilaNovi._TestTestova | findstr /i "Passed" >> Logovi\_log_AO_Razvoj.txt 2>&1 

::PAUSE


::echo. > Logovi\test_log.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\test_log.txt
::dotnet test --filter Name=_TestTestova | findstr /i "Passed" >> Logovi\test_log.txt 2>&1


::echo. >> Logovi\test_log.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\test_log.txt
::dotnet test --filter Name=_TestTestova | findstr /i "Failed" >> Logovi\test_log.txt


::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _TestTestova... >> Logovi\test_log.txt


REM >> C:\_Testovi\AMSOsiguranje\Razvoj\Logovi\test_log.txt

::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _ProveraDashboard... >> Logovi\test_log.txt
::dotnet test --filter Name=_ProveraDashboard | findstr /i "Failed" >> C:\_Projekti\AutoMotoSavezSrbije\Logovi\test_log.txt



::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _ProveraDashboard... >> Logovi\test_log.txt
::dotnet test --filter Name=_ProveraDashboard >> C:\_Projekti\AutoMotoSavezSrbije\Logovi\test_log.txt

