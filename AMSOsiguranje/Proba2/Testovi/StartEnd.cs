
namespace Proba2
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public partial class Osiguranje
    {
        public IBrowser? _browser;
        public IPage? _page;
        public IPlaywright? _playwright;



        #region OnTimeSetUp
        // Metoda koja se pokreće samo jednom na početku testiranja
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            // Simulacija asinhronog rada
            await Task.Delay(1);

            // Uzmi vreme kada su pokrenuti svi testovi
            LogovanjeTesta.PocetakTestiranja = DateTime.Now;
            // Unosi se u bazu vreme početka testiranja i uzima IDtestiranja
            LogovanjeTesta.IDTestiranja = LogovanjeTesta.UnesiPocetakTestiranja(LogovanjeTesta.PocetakTestiranja);


            // Ovo se upisuje u fajl logTrace.txt
            LogovanjeTesta.LogMessage("-----------------------------------------", false);
            LogovanjeTesta.LogMessage($"[{LogovanjeTesta.PocetakTestiranja:dd.MM.yyyy. HH:mm:ss}] Početak testiranja", false);
            LogovanjeTesta.LogMessage("", false);
            //string logFilePath = logFolder + "\\test_Trace_log.txt";
            //TextWriterTraceListener listener = new TextWriterTraceListener(logFilePath);
            //Trace.Listeners.Add(listener);
            //Trace.AutoFlush = true;  // Osigurava da se podaci odmah upisuju
        }
        #endregion OnTimeSetUp

        #region SetUp 
        // Metoda koja se pokreće pre svakog pojedinačnog testa
        [SetUp]
        public async Task SetUp()
        {
            // Odredi kada je počeo trenutni test
            LogovanjeTesta.PocetakTesta = DateTime.Now;
            // Odredi naziv trenutnog testa
            NazivTekucegTesta = TestContext.CurrentContext.Test.Name;
            // Upisivanje početka testa bazu i uzimanje IDTesta
            LogovanjeTesta.IDTesta = LogovanjeTesta.UnesiPocetakTesta(LogovanjeTesta.IDTestiranja, NazivTekucegTesta, LogovanjeTesta.PocetakTesta);


            LogovanjeTesta.LogMessage($"[{LogovanjeTesta.PocetakTesta:dd.MM.yyyy. HH:mm:ss}] pokrenut je test: {NazivTekucegTesta}", false);

            // Pročitaj radni prostor
            NazivNamespace = this.GetType().Namespace!;
            Console.WriteLine($"Namespace je ---------------------:: {NazivNamespace}");
            Prostor = NazivNamespace;
            // Pročitaj Okruženje u kom se testira
            Console.WriteLine($"Prostor je:-----------------------:: {Prostor}");
            Okruzenje = Prostor;
            Console.WriteLine($"Radno okruženje za testiranje je -:: {Okruzenje}");

            /*************************************
            // Namespace roditeljske klase ili druge klase, nisam siguran šta i kako čita
            string ns = this.GetType().Namespace!;
            Console.WriteLine($"Namespace rodit ------------------:: {ns}");
            ***************************************/

            // Odredi klasu testa
            string punNazivKlase = this.GetType().FullName!;
            Console.WriteLine($"Pun naziv klase-------------------:: {punNazivKlase}");
            var nazivKlase = this.GetType().Name;
            Console.WriteLine($"Naziv klase je--------------------:: {nazivKlase}");

            // Definiši početnu stranu u zavisnosti od vrste testa Osiguranje vozila ili Putno osiguranje
            // Početna strana se određuje i na osnovu klase testa
            PocetnaStrana = DefinisiPocetnuStranu(nazivKlase, Okruzenje);
            Console.WriteLine($"Početna strana je-----------------:: {PocetnaStrana}");

            // Kreiranje instance Playwright-a
            _playwright = await Playwright.CreateAsync();
            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = ["--start-maximized"]
            };

            _browser = Pregledac switch
            {
                "Chromium" => await _playwright.Chromium.LaunchAsync(launchOptions),
                "Firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
                "Webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
                _ => throw new ArgumentException("Nepoznat pregledač: " + Pregledac),
            };

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                // Set viewport size to null for maximum size
                ViewportSize = ViewportSize.NoViewport
                //StorageStatePath = fileAuth // Učitaj sesiju iz fajla
            });

            // Kreiranje nove stranice/tab-a
            _page = await context.NewPageAsync();

            await _page.Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });


            // Upisivanje početka testa u fajl logOpsti.txt
            LogovanjeTesta.UnesiPocetakTesta_1();

            //File.AppendAllText($"{logFajlOpsti}", $"[INFO] Test: {TestContext.CurrentContext.Test.Name}, Okruženje: {Okruzenje} ({PocetnaStrana}), {DateTime.Now.ToString("dd.MM.yyy.")} u {DateTime.Now.ToString("hh:mm:ss")}\n");

            //Otvaranje početne strane
            await _page.GotoAsync(PocetnaStrana);

            //await _page.PauseAsync();
            if (nazivKlase == "WebShop")
            {
                // Provera da li je otvorena početna stranica
                var title = await _page.TitleAsync();
                Assert.That(title, Is.EqualTo("AMS Osiguranje Webshop"));
                await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
                await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
            }
            else if (nazivKlase == "OsiguranjeVozila")
            {

                var ucitaniKorisnici = KorisnikLoader.UcitajKorisnike();
                Console.WriteLine("Učitani korisnici:");
                foreach (var korisnik in ucitaniKorisnici)
                {
                    Console.WriteLine($"- Ime: {korisnik.Ime}, Prezime: {korisnik.Prezime}, Uloga: {korisnik.Uloga}");
                    // Možeš ispisati i ostale atribute korisnika ovde
                }

                // Primer upotrebe:
                Console.WriteLine("Primer upotrebe:");
                Console.WriteLine($"--Korisnik1: {KorisnikLoader.Korisnik1?.Ime} {KorisnikLoader.Korisnik1?.Prezime} {KorisnikLoader.Korisnik1?.KorisnickoIme}");
                Console.WriteLine($"--Korisnik2: {KorisnikLoader.Korisnik2?.Ime} {KorisnikLoader.Korisnik2?.Prezime} {KorisnikLoader.Korisnik2?.KorisnickoIme}");
                Console.WriteLine($"--Korisnik3: {KorisnikLoader.Korisnik3?.Ime} {KorisnikLoader.Korisnik3?.Prezime} {KorisnikLoader.Korisnik3?.KorisnickoIme}");

                var KorisnikBO = KorisnikLoader.Korisnik1;
                var KorisnikA = KorisnikLoader.Korisnik3;
                if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Bogdan")
                {
                    // Ako je ručno pokretanje testa, koristi korisnika Bogdan
                    KorisnikA = KorisnikLoader.Korisnik2;
                }
                else if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Mario")
                {
                    // Ako je ručno pokretanje testa, koristi korisnika Mariv
                    KorisnikA = KorisnikLoader.Korisnik3;
                }
                else if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Aleksandar")
                {
                    // Ako je ručno pokretanje testa, koristi korisnika Aleksandar
                    KorisnikA = KorisnikLoader.Korisnik3;
                }

                {
                    // Proveri da li je korisnik učitan
                    if (KorisnikBO == null || KorisnikA == null)
                    {
                        throw new Exception("Korisnici nisu učitani iz fajla.");
                    }
                    // Prikaz informacija o korisnicima
                    Console.WriteLine($"Korisnik BO: {KorisnikBO.Ime} {KorisnikBO.Prezime}, Uloga: {KorisnikBO.Uloga}, Email: {KorisnikBO.Email}");
                    Console.WriteLine($"Korisnik A: {KorisnikA.Ime} {KorisnikA.Prezime}, Uloga: {KorisnikA.Uloga}, Email: {KorisnikA.Email}");
                }
                OsnovnaUloga = "Agent";
                //Bira se uloga BackOffice za određene testove, bez obzira na ulogu koja je definisana u fajlu sa podacima Utils.cs
                switch (NazivTekucegTesta)
                {
                    case "AO_1_SE_PregledPretragaObrazaca":
                    case "AO_2_SE_PregledPretragaDokumenata":
                    case "AO_3_SE_UlazPrenosObrazaca":
                    case "_ProveraDashboard":
                    case "ZK_1_SE_PregledPretragaObrazaca":
                    case "ZK_2_SE_PregledPretragaDokumenata":
                    case "ZK_3_SE_UlazPrenosObrazaca":
                    case "JS_1_SE_PregledPretragaRazduznihListi":
                    //case "JS_2_SE_PregledPretragaObrazaca":
                    case "DK_1_SE_PregledPretragaRazduznihListi":
                    case "BO_1_PutnoZdravstvenoOsiguranje":
                    case "_AO_4_ZamenaSerijskihBrojeva":
                        OsnovnaUloga = "BackOffice";
                        break;
                }

                //OsiguranjeVozila.PodaciZaLogovanje(OsnovnaUloga, Okruzenje, out string mejl, out string ime, out string lozinka);
                //KorisnikMejl = mejl;
                //KorisnikIme = ime;
                //KorisnikPassword = lozinka;
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Okruženje:: {Okruzenje}.\n" +
                                                   $"URL:: {PocetnaStrana}.\n\n" +
                                                   $"Osnovna Uloga:: {OsnovnaUloga}.\n" +
                                                   $"Korisnik:: {KorisnikIme}.\n" +
                                                   $"Mejl:: {KorisnikMejl}.\n" +
                                                   $"Lozinka:: {KorisnikPassword}\n", $"Test:: {NazivTekucegTesta}", MessageBoxButton.OK, MessageBoxImage.Information);
                }


                string korisnikMejl = KorisnikA.Email;
                string korisnikPassword = KorisnikA.Lozinka1;


                await OsiguranjeVozila.UlogujSe_1(_page, OsnovnaUloga, RucnaUloga);
                //await _page.PauseAsync(); // Pauza za ručno proveravanje da li je korisnik uspešno ulogovan
                //await OsiguranjeVozila.UlogujSe(_page, KorisnikMejl, KorisnikPassword);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

            }
            else
            {
                PocetnaStrana = "";
            }
        }

        #endregion SetUp


        #region TearDown
        // Metoda koja se pokreće posle svakog testa
        [TearDown]
        public async Task TearDown()
        {

            LogovanjeTesta.KrajTesta = DateTime.Now;
            TimeSpan trajanjeTesta = LogovanjeTesta.KrajTesta - LogovanjeTesta.PocetakTesta;
            TestStatus StatusTesta = TestContext.CurrentContext.Result.Outcome.Status;
            Console.WriteLine($"******************************Test {NazivTekucegTesta} je završio sa statusom: {StatusTesta}.");
            string errorMessage = TestContext.CurrentContext.Result.Message ?? string.Empty;
            string stackTrace = TestContext.CurrentContext.Result.StackTrace ?? string.Empty;
            //string poruka = TestContext.CurrentContext.Result.
            LogovanjeTesta.UnesiRezultatTesta(LogovanjeTesta.IDTesta, LogovanjeTesta.KrajTesta, StatusTesta, errorMessage, stackTrace);

            // Upisivanje opšteg rezultata testa u logOpsti.txt
            LogovanjeTesta.UnesiKrajTesta();

            //await _page.Context.Tracing.StopAsync(new() { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\trace_{DateTime.Now.ToString("yyyy-MM-dd")}.zip" });
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string logFilePath = $"C:\\_Testovi\\AMSOsiguranje\\Razvoj\\Logovi\\test_log_AO.txt";
                string testName = TestContext.CurrentContext.Test.Name;
                //string errorMessage = TestContext.CurrentContext.Result.Message;
                //string stackTrace = TestContext.CurrentContext.Result.StackTrace ?? string.Empty;

                string logEntry = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] TEST FAILED: {testName}\nERROR: {errorMessage}\nStack Trace:\n{stackTrace}\n";

                File.AppendAllText(logFilePath, logEntry);
            }

            // Upisivanje rezultata testa u fajl logTrace.txt

            LogovanjeTesta.LogMessage($"[{LogovanjeTesta.KrajTesta:dd.MM.yyyy. HH:mm:ss}] kraj testa: {NazivTekucegTesta}, trajanje: {(LogovanjeTesta.KrajTesta - LogovanjeTesta.PocetakTesta).TotalSeconds} sekundi.", false);
            LogovanjeTesta.LogMessage($"", false);
            //LogovanjeTesta.LogMessage($"Trajanje testa: {(KrajTesta - PocetakTesta).TotalSeconds} sekundi", false);
            //LogovanjeTesta.LogMessage($"Trajanje testiranja: {(KrajTestiranja - PocetakTestiranja).TotalSeconds} sekundi", false);
            //LogovanjeTesta.LogMessage($"[{KrajTesta:dd.MM.yyyy. HH:mm:ss}] Pokrenut je test: {NazivTekucegTesta}", false);
            //LogovanjeTesta.LogMessage($"Kraj testa.", true);
            //LogovanjeTesta.LogTestResult(NazivTekucegTesta, true);
            //LogovanjeTesta.LogMessage($"-----------------------------------------", false);
            //Trace.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")}] Kraj testaaaaaaaaaaaaaaaaaaaaa: {NazivTekucegTesta}");
            //Trace.WriteLine($"");

            // Čišćenje resursa
            // Zatvaranje stranice i browsera nakon svakog testa
            if (_page != null)
            {
                await _page.CloseAsync();
            }
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
            _playwright?.Dispose();
            /*****************************
            // Zatvaranje aplikacije samo ako je pokrenuta u ovom testu
            if (_application != null && !_application.HasExited)
            {
            _application.Close();
            }
            // Oslobađanje resursa
            _automation.Dispose();
            ******************************/
        }

        #endregion TearDown

        #region OneTimeTearDown
        // Ova metoda se pokreće jednom, nakon svih testovaS
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            LogovanjeTesta.KrajTestiranja = DateTime.Now;
            TimeSpan trajanjeTestiranja = LogovanjeTesta.KrajTestiranja - LogovanjeTesta.PocetakTestiranja;
            LogovanjeTesta.UnesiRezultatTestiranja(LogovanjeTesta.IDTestiranja, LogovanjeTesta.FailedTests, LogovanjeTesta.PassTests, LogovanjeTesta.SkippedTests, LogovanjeTesta.UkupnoTests, LogovanjeTesta.KrajTestiranja);
            LogovanjeTesta.LogMessage($"[{LogovanjeTesta.KrajTestiranja:dd.MM.yyyy. HH:mm:ss}] Kraj testiranja, trajanje: {(LogovanjeTesta.KrajTestiranja - LogovanjeTesta.PocetakTestiranja).TotalSeconds} sekundi.", false);
            LogovanjeTesta.LogMessage($"[{LogovanjeTesta.KrajTestiranja:dd.MM.yyyy. HH:mm:ss}] Kraj testiranja, trajanje: {trajanjeTestiranja} sekundi.", false);
            // Upiši u sumarni log fajl
            LogovanjeTesta.FormirajSumarniIzvestaj();
            //LogovanjeTesta.LogMessage("Kraj testiranja");
            LogovanjeTesta.LogMessage($"", false);
            //Trace.WriteLine($"");
            //Trace.WriteLine($"");

            // Simulacija asinhronog rada
            await Task.Delay(1);
        }
        #endregion OneTimeTearDown

    }
}


/******************************************************************************
Ovo je deo koji se nalazio u OnTimeSetUp metodi i odnosi se na rad sa sesijama
******************************************************************************/


/*********************************
           // Početna strana zavisi od okruženja
           PocetnaStrana = DefinisiPocetnuStranu(Okruzenje);

           // Naziv i putanja fajla auth_state prema okruženju i korisniku
           string fileAuth = FileFolder + "/Sesije/auth_state-" + Okruzenje + "-" + Uloga + ".json";

           // Da li postoji odgovarajući auth_state fajl
           bool fajlPostoji = File.Exists(fileAuth);
           // Predpostavka je da fajl nije validan
           bool fajlValidan = false;


           if (fajlPostoji) //Ako fajl postoji proveri validnost
           {
               //MessageBox.Show($"Fajl postoji, proveriću validnost", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               double expiresToken1;
               double expiresToken2;
               DateTimeOffset dateTimeOffset1;
               DateTimeOffset dateTimeOffset2;
               DateTime localDateTime1;
               DateTime localDateTime2;
               TimeSpan razlika1;
               TimeSpan razlika2;

               try
               {
                   // Učitaj sadržaj JSON fajla
                   string jsonContent = File.ReadAllText(fileAuth);
                   // Pokušaj da parsiraš JSON sadržaj
                   var authState = JsonSerializer.Deserialize<AuthState>(jsonContent);

                   fajlValidan = true;

                   var StanjeKukija = authState.Cookies;
                   int BrojKukija = authState.Cookies.Length;
                   if (authState.Cookies != null && BrojKukija == 2)
                   {

                       //vrednosti kada token ističe
                       expiresToken1 = authState.Cookies[0].Expires;
                       expiresToken2 = authState.Cookies[1].Expires;

                       // Pretvaranje u Unix epoch datum i vreme
                       dateTimeOffset1 = DateTimeOffset.FromUnixTimeSeconds((long)expiresToken1);
                       dateTimeOffset2 = DateTimeOffset.FromUnixTimeSeconds((long)expiresToken2);

                       // Konverzija u lokalno vreme - dodaje se dva sata
                       localDateTime1 = dateTimeOffset1.LocalDateTime;
                       localDateTime2 = dateTimeOffset2.LocalDateTime;

                       //računanje razlike između vremena kada token ističe i trenutnog vremena 
                       razlika1 = localDateTime1 - DateTime.Now;
                       razlika2 = localDateTime2 - DateTime.Now;



                       if (razlika1.Hours <= 0 || razlika2.TotalHours <= 0) //Provera isteka tokena
                       {
                           fajlValidan = false;
                       }
                   }
                   else
                   {
                       fajlValidan = false;
                   }
               }
               catch (JsonException ex)
               {
                   fajlValidan = false;
                   MessageBox.Show($"Validnost fajla je: {fajlValidan}\nGrška je: {ex.Message}. ", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               }
               catch (Exception ex)
               {
                   fajlValidan = false;
                   MessageBox.Show($"Došlo je do greške:\n + {ex.Message}", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               }

           }

           // Kreiranje instance Playwright-a
           _playwright = await Playwright.CreateAsync();
           var launchOptions = new BrowserTypeLaunchOptions
           {
               Headless = false,
               Args = ["--start-maximized"]
           };

           _browser = Pregledac switch
           {
               "Chromium" => await _playwright.Chromium.LaunchAsync(launchOptions),
               "Firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
               "Webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
               _ => throw new ArgumentException("Nepoznat pregledac: " + Pregledac),
           };


           if (fajlPostoji == false || fajlValidan == false) //Ako fajl auth_state ne postoji ili nije validan uloguj se i napravi ga
           {
               MessageBox.Show($@"Fajl auth_state ne postoji ili nije validan, idem da ga napravim", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               var context = await _browser.NewContextAsync(new BrowserNewContextOptions
               {
                   // Set viewport size to null for maximum size
                   ViewportSize = ViewportSize.NoViewport,
                   //StorageStatePath = fileAuth // Učitaj sesiju iz fajla
               });
               // Kreiranje nove stranice/tab-a
               _page = await context.NewPageAsync();

               //Otvaranje početne strane
               await _page.GotoAsync(PocetnaStrana);
               await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);//sačekaj 

               //MessageBox.Show($@"Pred logovanje", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);

               #region Logovanje
               PodaciZaLogovanje(Uloga, out string mejl, out string ime, out string lozinka);
               KorisnikMejl = mejl;
               KorisnikIme = ime;
               KorisnikPassword = lozinka;
               // Unesi korisničko ime
               await _page.GetByText("Korisničko ime").ClickAsync();
               await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
               await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(KorisnikMejl);
               // Unesi lozinku
               await _page.GetByText("Lozinka").ClickAsync();
               await _page.Locator("input[type=\"password\"]").FillAsync(KorisnikPassword);
               // Klik na Prijava
               await _page.GetByRole(AriaRole.Button, new() { NameRegex = MyRegexPrijava() }).ClickAsync();
               #endregion Logovanje

               //MessageBox.Show($"Gotovo logovanje", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);

               // Sačekaj na URL posle logovanja
               await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
               // Čekaj da se učita glavna stranica nakon logovanja
               await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

               // Uradi refreš strane
               await _page.ReloadAsync();

               // Nakon logovanja, dohvatite cookies
               var cookies = await _page.Context.CookiesAsync();
               foreach (var cookie in cookies)
               {
                   MessageBox.Show($"Kolačić: {cookie.Name},\nVrednost: {cookie.Value}");
               }

               // Definišite putanju fajla u koji ćete čuvati cookies
               string fileKuki = FileFolder + "/Sesije/Kuki.json";

               // Nakon što ste dohvatili cookies, serializujte ih u JSON format
               var cookiesJson = JsonSerializer.Serialize(cookies);
               if (cookies.Any())
               {
                   // Upisivanje sadržaja u fajl
                   await File.WriteAllTextAsync(fileKuki, cookiesJson);
               }

               // Sačuvaj stanje sesije
               var StorageStateAsync = await _page.Context.StorageStateAsync(new BrowserContextStorageStateOptions
               {
                   Path = fileAuth // Sačuvaj sesiju u fajl
               });
               //MessageBox.Show($"Napravljen je fajl auth_state.json", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
           else
           {

               var context = await _browser.NewContextAsync(new BrowserNewContextOptions
               {
                   // Set viewport size to null for maximum size
                   ViewportSize = ViewportSize.NoViewport,
                   StorageStatePath = fileAuth // Učitaj sesiju iz fajla
               });
               // Kreiranje nove stranice/tab-a
               _page = await context.NewPageAsync();
               //Otvaranje početne strane
               await _page.GotoAsync(PocetnaStrana);
               await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);//sačekaj 
           }

           await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
           if (_page.Url == PocetnaStrana + "/Dashboard")
           {
               //MessageBox.Show("Otvorena je strana Dashboard", "Informacija", MessageBoxButtons.OK);
           }
           else
           {
               MessageBox.Show("Nije otvorena je strana Dashboard", "Informacija", MessageBoxButtons.OK);
               return;
           }

           //MessageBox.Show("Idemo u test");
************************/
