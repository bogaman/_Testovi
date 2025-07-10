
using System.Threading.Tasks;

namespace Razvoj
{
    //[Parallelizable(ParallelScope.Self)]
    //[TestFixture]
    //[SetUpFixture]
    public partial class Osiguranje
    {
        protected IBrowser _browser;
        protected IPage _page;
        protected IPlaywright _playwright;
        protected IBrowserContext context;


        public string BOkorisnickoIme_ = string.Empty;
        public string BOlozinka_ = string.Empty;
        public string AkorisnickoIme_ = string.Empty;
        public string Alozinka_ = string.Empty;
        public string SertifikatName_ = string.Empty;
        public string Asaradnik_ = string.Empty;
        public int IdLica_ = 0;

        #region OnTimeSetUp
        //Metoda koja se pokreće samo jednom na početku testiranja
        [OneTimeSetUp]
        //public async Task OneTimeSetUp()
        public async Task OneTimeSetUp()
        {
            try
            {
                //Pročitaj vreme kada je pokrenuto testiranje
                LogovanjeTesta.PocetakTestiranja = DateTime.Now;

                //Pročitaj radni prostor
                NazivNamespace = this.GetType().Namespace!;
                Prostor = NazivNamespace;
                Okruzenje = Prostor;
                DeviceName = Environment.MachineName;

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Način pokretanja:: {NacinPokretanjaTesta}\n" +
                                                   $"Namespace:: {NazivNamespace}.\n" +
                                                   $"Prostor:: {Prostor}\n" +
                                                   $"Okruženje:: {Okruzenje}\n" +
                                                   $"Računar:: {DeviceName}\n",
                                                    "Poruka u OneTimeSetUp",
                                                    MessageBoxButton.OK,
                                                    MessageBoxImage.Information);
                }

                Alati.PokreniVpnAkoTreba();

                //Unosi se u bazu vreme početka testiranja i uzima IDtestiranje
                LogovanjeTesta.IDTestiranje = LogovanjeTesta.UnesiPocetakTestiranja(LogovanjeTesta.PocetakTestiranja, NazivNamespace, NacinPokretanjaTesta, DeviceName);

                // Ovo se upisuje u fajl logTrace.txt
                LogovanjeTesta.LogMessage("-----------------------------------------", false);
                LogovanjeTesta.LogMessage($"[{LogovanjeTesta.PocetakTestiranja:dd.MM.yyyy. HH:mm:ss}] Početak testiranja", false);
                LogovanjeTesta.LogMessage("", false);
                //string logFilePath = logFolder + "\\test_Trace_log.txt";
                //TextWriterTraceListener listener = new TextWriterTraceListener(logFilePath);
                //Trace.Listeners.Add(listener);
                //Trace.AutoFlush = true;  // Osigurava da se podaci odmah upisuju

                // Logovanje neobrađenih grešaka
                AppDomain.CurrentDomain.UnhandledException += async (sender, e) =>
                {
                    if (e.ExceptionObject is Exception ex)
                    {
                        await LogovanjeTesta.LogException("UnhandledException", ex);
                    }
                };
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException("OneTimeSetUp", ex);
                throw;
            }
        }
        #endregion OnTimeSetUp




        #region SetUp 
        // Metoda koja se pokreće pre svakog pojedinačnog testa
        [SetUp]
        public async Task SetUp()
        {
            try
            {
                // Odredi kada je počeo trenutni test
                LogovanjeTesta.PocetakTesta = DateTime.Now;
                // Odredi naziv trenutnog testa
                NazivTekucegTesta = TestContext.CurrentContext.Test.Name;

                // Upisivanje početka testa bazu i uzimanje IDTesta
                LogovanjeTesta.IDTestaSQL = LogovanjeTesta.UnesiPocetakTesta(LogovanjeTesta.IDTestiranje, NazivTekucegTesta, LogovanjeTesta.PocetakTesta);

                LogovanjeTesta.LogMessage($"[{LogovanjeTesta.PocetakTesta:dd.MM.yyyy. HH:mm:ss}] pokrenut je test: {NazivTekucegTesta}", false);

                Console.WriteLine($"Namespace je ---------------------:: {NazivNamespace}");
                //Prostor = NazivNamespace;
                // Pročitaj Okruženje u kom se testira
                Console.WriteLine($"Prostor je:-----------------------:: {Prostor}");
                //Okruzenje = Prostor;
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

                // Definiši početnu stranu u zavisnosti od vrste testa (klasa OsiguranjeVozila ili WebShop) 
                // i okruženja (Razvoj, Proba2, UAT ili Produkcija)
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
                // 1. Kreiraj context
                context = await _browser.NewContextAsync(new BrowserNewContextOptions
                {
                    // Set viewport size to null for maximum size
                    ViewportSize = ViewportSize.NoViewport
                    //StorageStatePath = fileAuth // Učitaj sesiju iz fajla


                });


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
                //await ForceZoomAsync(_page);


                /*
                // I u popup hook-u:
                context.Page += async (_, popup) =>
                {
                    await popup.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    await ForceZoomAsync(popup);
                };
                */
                //await _page.PauseAsync();
                if (nazivKlase == "WebShop")
                {
                    // Provera da li je otvorena početna stranica
                    var title = await _page.TitleAsync();
                    if (title != "AMS Osiguranje Webshop")
                    {
                        await LogovanjeTesta.LogException("SetUp", new Exception("Očekuje se title stranice: 'AMS Osiguranje Webshop'."));
                        throw new Exception($"Title početne strane nije dobar. Očekuje se 'AMS Osiguranje Webshop', a ne '{title}'.");
                    }
                    //Assert.That(title, Is.EqualTo("AMS Osiguranje Webshop333"));
                    // Klik na pop-up prozor
                    await _page.GetByText("Webshop AMS Osiguranja").ClickAsync();
                    // potvrda kolačića
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();
                    // odbijanje kolačića
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); 
                }
                else if (nazivKlase == "OsiguranjeVozila" || nazivKlase == "TestDevelopment")
                {

                    var korisnici_ = KorisnikLoader.UcitajKorisnike();

                    var BOkorisnik_ = korisnici_.ElementAtOrDefault(0);
                    BOkorisnickoIme_ = BOkorisnik_?.KorisnickoIme ?? string.Empty;
                    BOlozinka_ = BOkorisnik_?.Lozinka1 ?? string.Empty;

                    if (Prostor == "Produkcija")
                    {
                        //Ako je radno okruženje produkcija, koristi za Davora Lozinka2
                        BOlozinka_ = BOkorisnik_?.Lozinka2 ?? string.Empty;
                    }

                    //Agent je Mario za automatsko pokretanje
                    var Akorisnik_ = korisnici_.ElementAtOrDefault(2);
                    if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Bogdan")
                    {
                        //Ako je ručno pokretanje testa, koristi korisnika Bogdan
                        Akorisnik_ = korisnici_.ElementAtOrDefault(1);
                    }
                    else if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Mario")
                    {
                        //Ako je ručno pokretanje testa, koristi korisnika Mario
                        Akorisnik_ = korisnici_.ElementAtOrDefault(2);
                    }
                    AkorisnickoIme_ = Akorisnik_?.KorisnickoIme ?? string.Empty;
                    Alozinka_ = Akorisnik_?.Lozinka1 ?? string.Empty;
                    SertifikatName_ = Akorisnik_?.Sertifikat ?? string.Empty;
                    Asaradnik_ = Akorisnik_?.Saradnik1 ?? string.Empty;
                    IdLica_ = int.TryParse(Akorisnik_?.IdLica, out var idLica) ? idLica : 0;
                    // Proveri da li je korisnik učitan
                    if (BOkorisnik_ == null || Akorisnik_ == null)
                    {
                        throw new Exception("Korisnici nisu učitani iz SQL baze.");
                    }

                    if (NacinPokretanjaTesta == "ručno")
                    {
                        System.Windows.MessageBox.Show($"Korisnik Back Office: {BOkorisnik_?.Ime}\n" +
                                                       $"Korisničko ime: {BOkorisnickoIme_}\n" +
                                                       $"Lozinka: {BOlozinka_}\n\n" +
                                                       $"Agent je: {Akorisnik_?.Ime}\n" +
                                                       $"Korisničko ime: {AkorisnickoIme_}\n" +
                                                       $"Lozinka: {Alozinka_}\n\n" +
                                                       $"Saradnik: {Akorisnik_?.Saradnik1}\n",
                                                       $"Ko će Testirati", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {

                    await LogovanjeTesta.LogException("SetUp", new Exception("PocetnaStrana nije određena u SetUp sekciji."));
                    throw new Exception($"PocetnaStrana nije određena u sekciji  SetUp.");
                }
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Test:: {NazivTekucegTesta}\n" +
                                                   $"Okruženje:: {Okruzenje}.\n" +
                                                   $"URL:: {PocetnaStrana}.\n\n" +
                                                   $"Osnovna Uloga:: {OsnovnaUloga}.", "Poruka u SetUp", MessageBoxButton.OK, MessageBoxImage.Information);
                    //$"Korisnik:: {KorisnikIme}.\n" +
                    //$"Mejl:: {KorisnikMejl}.\n" +
                    //$"Lozinka:: {KorisnikPassword}
                }
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException("SetUp", ex);
                throw;
            }
        }

        #endregion SetUp


        #region TearDown
        // Metoda koja se pokreće posle svakog testa
        [TearDown]
        public async Task TearDown()
        {
            try
            {
                LogovanjeTesta.KrajTesta = DateTime.Now;
                TimeSpan trajanjeTesta = LogovanjeTesta.KrajTesta - LogovanjeTesta.PocetakTesta;
                TestStatus StatusTesta = TestContext.CurrentContext.Result.Outcome.Status;
                Console.WriteLine($"******************************Test {NazivTekucegTesta} je završio sa statusom: {StatusTesta}.");
                string errorMessage = TestContext.CurrentContext.Result.Message ?? string.Empty;
                string stackTrace = TestContext.CurrentContext.Result.StackTrace ?? string.Empty;
                //string poruka = TestContext.CurrentContext.Result.

                //LogovanjeTesta.UnesiRezultatTesta1(LogovanjeTesta.IDTesta1, LogovanjeTesta.KrajTesta, StatusTesta, errorMessage, stackTrace, AkorisnickoIme_);
                // Adjust the arguments below to match the correct overload of UnesiRezultatTestaSQL
                // For example, if the method expects 6 arguments, remove one argument (e.g., BOkorisnickoIme_):
                LogovanjeTesta.UnesiRezultatTesta(LogovanjeTesta.IDTestaSQL, LogovanjeTesta.KrajTesta, StatusTesta, errorMessage, stackTrace, AkorisnickoIme_, BOkorisnickoIme_);
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
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException("TearDown", ex);
                throw;
            }
        }

        #endregion TearDown



        #region OneTimeTearDown
        // Ova metoda se pokreće jednom, nakon svih testovaS
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            try
            {
                //Pročitaj vreme kada su završeni svi testovi
                LogovanjeTesta.KrajTestiranja = DateTime.Now;
                //Odreti koliko je trajalo testiranje
                TimeSpan trajanjeTestiranja = LogovanjeTesta.KrajTestiranja - LogovanjeTesta.PocetakTestiranja;
                //Unosi se u bazu vreme završetka testiranja i podaci o uspešnosti testiranja
                //LogovanjeTesta.UnesiRezultatTestiranja(LogovanjeTesta.IDTestiranja, LogovanjeTesta.FailedTests, LogovanjeTesta.PassTests, LogovanjeTesta.SkippedTests, LogovanjeTesta.UkupnoTests, LogovanjeTesta.KrajTestiranja);
                //Unosi se u bazu vreme završetka testiranja i podaci o uspešnosti testiranja
                //LogovanjeTesta.UnesiRezultatTestiranja2(LogovanjeTesta.IDTestiranja1, LogovanjeTesta.FailedTests, LogovanjeTesta.PassTests, LogovanjeTesta.SkippedTests, LogovanjeTesta.UkupnoTests, LogovanjeTesta.KrajTestiranja);
                LogovanjeTesta.UnesiRezultatTestiranja(LogovanjeTesta.IDTestiranje, LogovanjeTesta.FailedTests, LogovanjeTesta.PassTests, LogovanjeTesta.SkippedTests, LogovanjeTesta.UkupnoTests, LogovanjeTesta.KrajTestiranja);

                LogovanjeTesta.LogMessage($"[{LogovanjeTesta.KrajTestiranja:dd.MM.yyyy. HH:mm:ss}] Kraj testiranja, trajanje: {(LogovanjeTesta.KrajTestiranja - LogovanjeTesta.PocetakTestiranja).TotalSeconds} sekundi.", false);
                LogovanjeTesta.LogMessage($"[{LogovanjeTesta.KrajTestiranja:dd.MM.yyyy. HH:mm:ss}] Kraj testiranja, trajanje: {trajanjeTestiranja} sekundi.", false);
                // Upiši u sumarni log fajl
                LogovanjeTesta.FormirajSumarniIzvestaj();
                //LogovanjeTesta.LogMessage("Kraj testiranja");
                LogovanjeTesta.LogMessage($"", false);
                //Trace.WriteLine($"");
                //Trace.WriteLine($"");

                // Simulacija asinhronog rada
                //await Task.Delay(1);
                Console.WriteLine("🧹 [GlobalniSetup] Isključujem VPN konekciju...");
                if (DeviceName == "LT-TESTER")
                {
                    // Ako je pokrenuto na LT-TESTER, isključi VPN
                    Alati.IskljuciVpn();
                }

                //Alati.IskljuciVpn();

            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException("OneTimeTearDovn", ex);
                throw;
            }
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
