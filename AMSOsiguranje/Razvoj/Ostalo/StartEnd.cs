
namespace Razvoj
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public partial class Osiguranje
    {


        #region OnTimeSetUp

        // Metoda koja se pokreće samo jednom
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await Task.Delay(1);

            LogovanjeTesta.LogMessage("", false);
            LogovanjeTesta.LogMessage("Početak testiranja");

            //string logFilePath = logFolder + "\\test_Trace_log.txt";
            //TextWriterTraceListener listener = new TextWriterTraceListener(logFilePath);
            //Trace.Listeners.Add(listener);
            //Trace.AutoFlush = true;  // Osigurava da se podaci odmah upisuju

            /**********************************
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
        }

        #endregion OnTimeSetUp


        #region TearDown
        // Metoda koja se pokreće posle svakog testa
        [TearDown]
        public async Task TearDown()
        {


            // Upisivanje opšteg rezultata testa u logOpsti.txt
            LogovanjeTesta.UnesiKrajTesta();

            //await _page.Context.Tracing.StopAsync(new() { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\trace_{DateTime.Now.ToString("yyyy-MM-dd")}.zip" });
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string logFilePath = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO.txt";
                string testName = TestContext.CurrentContext.Test.Name;
                string errorMessage = TestContext.CurrentContext.Result.Message;
                string stackTrace = TestContext.CurrentContext.Result.StackTrace;

                string logEntry = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] TEST FAILED: {testName}\nERROR: {errorMessage}\nStack Trace:\n{stackTrace}\n";

                File.AppendAllText(logFilePath, logEntry);
            }
            LogovanjeTesta.LogMessage($"Kraj testa.", true);
            //LogovanjeTesta.LogTestResult(nazivTekucegTesta, true);
            LogovanjeTesta.LogMessage($"", false);
            //Trace.WriteLine($"[{DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")}] Kraj testaaaaaaaaaaaaaaaaaaaaa: {nazivTekucegTesta}");
            //Trace.WriteLine($"");

            // Čišćenje resursa
            // Zatvaranje stranice i browsera nakon svakog testa
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
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
        // Ova metoda se pokreće jednom, nakon svih testova
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            //// Simulacija asinhronog rada
            await Task.Delay(10);

            // Upiši u sumarni log fajl
            LogovanjeTesta.FormirajSumarniIzvestaj();
            LogovanjeTesta.LogMessage("Kraj testiranja");
            LogovanjeTesta.LogMessage($"", false);
            //Trace.WriteLine($"");
            //Trace.WriteLine($"");
        }
        #endregion OneTimeTearDown

    }
}