cls
cd "C:\_Testovi\AMSOsiguranje\Proba2\" 

::@echo off
::set BASE_URL=https://razvojamso-master.eonsystem.rs
set NACIN_POKRETANJA=automatski
::set BASE_URL=https://proba2amsomaster.eonsystem.rs
::set BASE_URL=https://master-test.ams.co.rs
::set BASE_URL=https://eos.ams.co.rs

::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] Proba2.TestDevelopment... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.TestDevelopment | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 

::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] Proba2.OsiguranjeVozila.AO_4_Polisa... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila.AO_4_Polisa | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 

echo. > Logovi\_log_AO_Proba2.txt
echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] Proba2.OsiguranjeVozila.AO_3_SE_UlazPrenosObrazaca... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila.AO_3_SE_UlazPrenosObrazaca | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 
::dotnet test --filter "(FullyQualifiedName~Proba2.OsiguranjeVozila | FullyQualifiedName~Proba2.WebShop)" | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1
::dotnet test --filter "(FullyQualifiedName~Proba2.OsiguranjeVozila | FullyQualifiedName~Proba2.WebShop)" >> Logovi\_log_AO_Proba2.txt 2>&1

echo ==== TEST POKRETANJE %DATE% %TIME% ==== >> Logovi\_log_AO_Proba2.txt
dotnet test --filter "(FullyQualifiedName~Proba2.Osiguranje | FullyQualifiedName~Proba2.Web)" >> Logovi\_log_AO_Proba2.txt 2>&1
echo ==== TEST ZAVRŠEN ==== >> Logovi\_log_AO_Proba2.txt
echo. >> Logovi\_log_AO_Proba2.txt

::dotnet test --filter "FullyQualifiedName!~Proba2.TestDevelopment" | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1
::# Primer slozenog filtra (ako bi ti zaista trebao):
::# Pokreni testove koji su u Osiguranju ILI Web-u, I ISTOVREMENO NISU u TestDevelopmentu
::dotnet test --filter "(FullyQualifiedName~Proba2.OsiguranjeVozila | FullyQualifiedName~Proba2.WebShop) & FullyQualifiedName!~Proba2.TestDevelopment" | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1


::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_3_SE_UlazPrenosObrazaca... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila.AO_3_SE_UlazPrenosObrazaca | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 

::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_1_SE_PregledPretragaObrazaca... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila.AO_1_SE_PregledPretragaObrazaca | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 


::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila._TestTestova | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 

::echo. > Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter FullyQualifiedName=Proba2.OsiguranjeVozila.AO_3_SE_UlazPrenosObrazaca | findstr /i "Passed" >> Logovi\_log_AO_Proba2.txt 2>&1 


::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] BO_PutnoZdravstvenoOsiguranje... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=BO_PutnoZdravstvenoOsiguranje >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_1_StrogaEvidencija... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=AO_1_StrogaEvidencija >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_2_Polisa... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=AO_2_Polisa >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_3_ZahtevZaIzmenom... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=AO_3_ZahtevZaIzmenom >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_5_RazduznaLista... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=AO_5_RazduznaLista >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] AO_6_Otpis... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=AO_6_Otpis >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] ZK_StrogaEvidencija... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=ZK_StrogaEvidencija >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] ZK_Polisa... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=ZK_Polisa >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] ZK_RazduznaLista... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=ZK_RazduznaLista >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] ZK_Otpis... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=ZK_Otpis >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] JS_Polisa... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=JS_Polisa >> Logovi\_log_AO_Proba2.txt

::echo. >> Logovi\_log_AO_Proba2.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] DK_Polisa... >> Logovi\_log_AO_Proba2.txt
::dotnet test --filter Name=DK_Polisa >> Logovi\_log_AO_Proba2.txt




::dotnet test --filter Name=_ProveraDashboard
::dotnet test --filter Name=AO_1_StrogaEvidencija
::dotnet test --filter Name=AO_2_Polisa
::dotnet test --filter Name=AO_3_ZahtevZaIzmenom
::dotnet test --filter Name=AO_5_RazduznaLista
::dotnet test --filter Name=AO_6_Otpis


::echo. > Logovi\test_log.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\test_log.txt
::dotnet test --filter Name=_TestTestova | findstr /i "Passed" >> Logovi\test_log.txt 2>&1


::echo. >> Logovi\test_log.txt
::echo [%DATE:~4,2%.%DATE:~7,2%.%DATE:~10,4%. %TIME%] _TestTestova... >> Logovi\test_log.txt
::dotnet test --filter Name=_TestTestova | findstr /i "Failed" >> Logovi\test_log.txt


::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _TestTestova... >> Logovi\test_log.txt
::dotnet test --filter Name=_TestTestova  
REM >> C:\_Projekti\AutoMotoSavezSrbije\Logovi\test_log.txt

::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _ProveraDashboard... >> Logovi\test_log.txt
::dotnet test --filter Name=_ProveraDashboard | findstr /i "Failed" >> C:\_Projekti\AutoMotoSavezSrbije\Logovi\test_log.txt



::echo. >> Logovi\test_log.txt
::echo [%DATE% %TIME%] _ProveraDashboard... >> Logovi\test_log.txt
::dotnet test --filter Name=_ProveraDashboard >> C:\_Projekti\AutoMotoSavezSrbije\Logovi\test_log.txt

