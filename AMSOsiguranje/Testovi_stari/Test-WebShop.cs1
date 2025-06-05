namespace Razvoj
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public partial class WebShop : Osiguranje 
    {
        //public static string baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "ručno";
        
        
        
        
        #region SetUp 
        // Metoda koja se pokreće pre svakog testa
        [SetUp]
        public async Task SetUp()
        {
            //Definiši početnu stranu u zavisnosti od okruženja
            var (pocetnaStrana, pocetnaStranaWS) = DefinisiPocetnuStranu(Okruzenje);
            PocetnaStrana = pocetnaStranaWS;
            PocetnaStranaWS = pocetnaStranaWS;
            /*
                        // Početna strana zavisi od okruženja
                        if (baseUrl == "ručno")
                        {
                            // Početna strana zavisi od okruženja i da li se test pokreće iz batch fajla ili ručno
                            PocetnaStranaWS = DefinisiPocetnuStranuWS(Okruzenje);
                        }
                        else
                        {
                            PocetnaStranaWS = baseUrl;
                        }
                        PocetnaStrana = PocetnaStranaWS;
                        */
            //PocetnaStranaWS = DefinisiPocetnuStranuWS(Okruzenje);
            nazivTekucegTesta = TestContext.CurrentContext.Test.Name;
            Console.WriteLine($"Pokrenut je test-------------------:: {nazivTekucegTesta}");
            LogovanjeTesta.LogMessage($"Pokrenut test: {nazivTekucegTesta}");
            //Console.WriteLine($"Pokrenut je test:: {nazivTekucegTesta}");
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
            LogovanjeTesta.UnesiPocetakTesta();
            //File.AppendAllText($"{logFajlOpsti}", $"[INFO] Test: {TestContext.CurrentContext.Test.Name}, Okruženje: {Okruzenje} ({PocetnaStrana}), {DateTime.Now.ToString("dd.MM.yyy.")} u {DateTime.Now.ToString("hh:mm:ss")}\n");

            //Otvaranje početne strane
            await _page.GotoAsync(PocetnaStrana);

            // Provera da li je otvorena početna stranica
            var title = await _page.TitleAsync();
            Assert.That(title, Is.EqualTo("AMS Osiguranje Webshop"));

            //await _page.WaitForURLAsync(PocetnaStranaWS + "/Index");
            // Sačekaj da DOM bude učitan
            //await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            Assert.That(title, Is.EqualTo("AMS Osiguranje Webshop"));



            //await SacekajUcitavanjestranice(_page, "/index?url=/");

            await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
            await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
            //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
        }

        #endregion SetUp

        /*
        // Metoda koja se pokreće posle svakog testa
        [TearDown]
        public async Task TearDown_1()
        {

            await _page.Context.Tracing.StopAsync(new() { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\trace_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.zip" });

            string status = TestContext.CurrentContext.Result.Outcome.Status.ToString();
            File.AppendAllText($"{logFolder}\\Logovi\\logOpsti.txt", $"[RESULT] Test {TestContext.CurrentContext.Test.Name} - {status}\n");
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                string logFilePath = "Logovi/test_log_WS.txt";
                string testName = TestContext.CurrentContext.Test.Name;
                string errorMessage = TestContext.CurrentContext.Result.Message;
                string stackTrace = TestContext.CurrentContext.Result.StackTrace;

                string logEntry = $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] TEST FAILED: {testName}\nERROR: {errorMessage}\nStack Trace:\n{stackTrace}\n";

                File.AppendAllText(logFilePath, logEntry);
            }
            // Zatvaranje stranice i browsera nakon svakog testa
            await _page.CloseAsync();
            await _browser.CloseAsync();

            // Oslobađanje resursa
            _playwright.Dispose();
        }

*/

        /***************************************************
                [Test]
                public async Task IndividualnoPutno()
                {


                    //izbor Individualnog osiguranja
                    await _page.GetByRole(AriaRole.Link, new() { NameRegex = MyRegexIndividualno(), Exact = true }).First.ClickAsync();

                    //sačekaj da se učita
                    await SacekajUcitavanjestranice(_page, "/Putno1");
                    await _page.WaitForURLAsync(PocetnaStranaWS + "/Putno1");
                    await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                    await ProveriTitlStranice(_page, "Osnovni podaci");

                    //unos broja putnika
                    await UnosBrojaOdraslih(_page, "1");
                    await UnosBrojaDece(_page, "0");
                    await UnosBrojaSeniora(_page, "0");

                    //unos datuma početka
                    await DatumPocetka(_page);

                    //unos trajanja
                    await UnosTrajanja(_page, "5");

                    //Svrha putovanja
                    //await SvrhaPutovanja(_page, "Doplatak za privremeni rad u");
                    await IzaberiOpcijuIzListe(_page, "#selSvrha", "Doplatak za privremeni rad u inostranstvu", false);

                    // Covid 19
                    await Covid(_page, "Da");

                    // Teritorija Srbije
                    await TeritorijaSrbije(_page);

                    // Dalje
                    await Dalje(_page, "Dalje");

                    //sačekaj da se stranica učita
                    await SacekajUcitavanjestranice(_page, "/Putno2");
                    //Provera da li se otvorila stranica za izbor Paketa pokrića
                    await ProveriTitlStranice(_page, "Paketi pokrića");

                    //Provera stranice Paketi pokrića
                    await ProveraNaStraniciPaketiPokrica(_page);

                    // Izbor paketa pokrića
                    await IzborPaketaPokrica(_page, "Paket1");

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili Nazad

                    //Stranica za unos putnika

                    //sačekaj da se stranica učita
                    await SacekajUcitavanjestranice(_page, "/Putno3");
                    await _page.WaitForURLAsync(PocetnaStranaWS + "/Putno3");
                    await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                    await ProveriTitlStranice(_page, "Osigurana lica");

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Osigurana lica"));
                    await _page.GetByRole(AriaRole.Heading, new() { Name = "INDIVIDUALNO PUTNO OSIGURANJE" }).ClickAsync();
                    await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();


                    // Ugovarač je jedan odrasli
                    await UnesiUgovaraca(_page,
                                         "Petar-DaCovid", "IndividualniPetrović",
                                         "2612962710096", "Japanska", "442",
                                         "111", "- Rušanj", "Pasoš br. 1",
                                         "+381123456789", "bogaman@hotmail.com");

                    //Ugovarač je osigurano lice i prekopiraj podatke
                    await UgovaracJeOsiguranoLice(_page);

                    await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili "Nazad"

                    //sačekaj da se stranica učita
                    await SacekajUcitavanjestranice(_page, "/Putno4");
                    //await _page.WaitForURLAsync(Variables.PocetnaStranaWS + "/Putno1");
                    //await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                    await ProveriTitlStranice(_page, "Rekapitulacija");

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Rekapitulacija"));

                    //Potvrde na stranici rekapitulacija
                    await PotvrdaRekapitulacije(_page);

                    // Klik na Plaćanje
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                    // Na stranici Plaćanje
                    //await Expect(_page).ToHaveTitleAsync(new Regex("VPos stranica plaćanja"));
                    await Placanje(_page, "5342230500001234", "08", "2025");

                    await SacekajUcitavanjestranice(_page, "/Uspesno");
                    await ProveriTitlStranice(_page, "Uspešna kupovina");
                    //await Expect(_page).ToHaveTitleAsync(new Regex("Uspešna kupovina"));
                    //await _page.PauseAsync();
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                    //await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
                    //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();

                    //await _page.PauseAsync();

                    //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                    Assert.Pass();
                    Assert.Pass();

                }

        *******************************************/

        /*************************************************
                [Test]
                public async Task PorodicnoPutno()
                {
                    // izbor Porodičnog osiguranja
                    await _page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("PORODIČNO PUTNO OSIGURANJE"), Exact = true }).First.ClickAsync();

                    //sačekaj da se stranica učita
                    await SacekajUcitavanjestranice(_page, "/Putno1");

                    await ProveriTitlStranice(_page, "Osnovni podaci");

                    //unos broja putnika
                    await UnosBrojaOdraslih(_page, "1");
                    await UnosBrojaDece(_page, "2");
                    //await UnosBrojaSeniora(_page, "0");                                                                                                                  //await page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA"), Exact = true }).First.ClickAsync(); // izbor Individualnog sa više ulazaka

                    //unos datuma početka
                    await DatumPocetka(_page);

                    //unos trajanja
                    await UnosTrajanja(_page, "10");

                    //Svrha putovanja
                    //await SvrhaPutovanja(_page, "Turistički");
                    await IzaberiOpcijuIzListe(_page, "#selSvrha", "Turistički", false);

                    // Covid 19
                    await Covid(_page, "Ne");

                    // Teritorija Srbije
                    await TeritorijaSrbije(_page);

                    // Dalje
                    await Dalje(_page, "Dalje");

                    //sačekaj da se stranica učita
                    //await SacekajUcitavanjestranice(_page);
                    //Provera da li se otvorila stranica za izbor Paketa pokrića
                    await ProveriTitlStranice(_page, "Paketi pokrića");

                    //Provera stranice Paketi pokrića
                    await ProveraNaStraniciPaketiPokrica(_page);

                    // Izbor paketa pokrića
                    await IzborPaketaPokrica(_page, "Paket2");

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili Nazad

                    // Stranica za unos putnika
                    //await Expect(_page).ToHaveTitleAsync(new Regex("Osigurana lica"));
                    await _page.GetByRole(AriaRole.Heading, new() { Name = "PORODIČNO PUTNO OSIGURANJE" }).ClickAsync();
                    await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();

                    // Ugovarač je Jedan odrasli
                    await UnesiUgovaraca(_page,
                                         "Mitar-NoCovid", "PorodičnoMirić",
                                         "2612962710096", "Kojekude", "100",
                                         "111", "- Kaluđerica", "Pasoš br. 123",
                                         "+3819876543216789", "bogaman@hotmail.com");

                    //Ugovarač je osigurano lice i prekopiraj podatke
                    await UgovaracJeOsiguranoLice(_page);

                    await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();
                    await _page.GetByText("OSIGURANIK 1 - ODRASLA OSOBA").ClickAsync();

                    await UnesiDetePrvo(_page, "Prvo", "Dete-Prvo", "3109009780168", "Pasoš dete 1");

                    await UnesiDeteDrugo(_page, "Drugo", "Dete-Drugo", "3111014710210", "Pasoš dete 2");

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili Nazad

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Rekapitulacija"));

                    //Potvrde na stranici rekapitulacija
                    await PotvrdaRekapitulacije(_page);

                    // Klik na Plaćanje
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                    // Na stranici Plaćanje
                    //await Expect(_page).ToHaveTitleAsync(new Regex("VPos stranica plaćanja"));
                    await Placanje(_page, "5342230500001234", "08", "2025");

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Uspešna kupovina"));

                    await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                    //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();

                    //await _page.PauseAsync();
                    //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
                    Assert.Pass();

                }


        *************************************************/

        /*
                [Test]
                public async Task SaViseUlazaka()
                {
                    //await _page.PauseAsync();
                    await _page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA"), Exact = true }).First.ClickAsync(); // izbor Individualnog sa više ulazaka

                    //sačekaj da se stranica učita
                    await SacekajUcitavanjestranice(_page, "/Putno1");

                    await ProveriTitlStranice(_page, "Osnovni podaci");

                    //unos broja putnika
                    await UnosBrojaOdraslih(_page, "1");
                    await UnosBrojaDece(_page, "0");
                    await UnosBrojaSeniora(_page, "1");

                    //unos datuma početka
                    await DatumPocetka(_page);


                    //izbor trajanja
                    await IzborTrajanja(_page, "do 30 dana");

                    //Period pokrića
                    await IzborPeriodaPokrica(_page, "5");

                    //Svrha putovanja
                    //await SvrhaPutovanja(_page, "Doplatak za ekstremne sportove");
                    await IzaberiOpcijuIzListe(_page, "#selSvrha", "Doplatak za ekstremne sportove", false);

                    // Covid 19
                    await Covid(_page, "Da");

                    // Teritorija Srbije
                    await TeritorijaSrbije(_page);

                    // Dalje
                    await Dalje(_page, "Dalje");

                    //sačekaj da se stranica učita
                    //await SacekajUcitavanjestranice(_page);
                    //Provera da li se otvorila stranica za izbor Paketa pokrića
                    await ProveriTitlStranice(_page, "Paketi pokrića");
                    //await Expect(page).ToHaveTitleAsync(RegexPaketiPokrica());

                    //Provera stranice Paketi pokrića
                    await ProveraNaStraniciPaketiPokrica(_page);

                    // Izbor paketa pokrića
                    await IzborPaketaPokrica(_page, "Paket3");

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili Nazad

                    // Stranica za unos putnika
                    //await Expect(_page).ToHaveTitleAsync(new Regex("Osigurana lica"));
                    await _page.GetByRole(AriaRole.Heading, new() { Name = "INDIVIDUALNO PUTNO OSIGURANJE" }).ClickAsync();
                    await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();

                    // Ugovarač je Jedan odrasli
                    await UnesiUgovaraca(_page,
                                         "Zoki-DaCovid", "VišeUlazaka Zorić",
                                         "2612962710096", "Kineska", "bb",
                                         "111", "- Rušanj", "Pasoš br. 333",
                                         "+381123456789", "bogaman@hotmail.com");

                    //Ugovarač je osigurano lice i prekopiraj podatke
                    await UgovaracJeOsiguranoLice(_page);

                    await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();
                    await _page.GetByText("OSIGURANIK 1 - ODRASLA OSOBA").ClickAsync();


                    // Unos seniora
                    await UnesiSeniora(_page,
                                       "Senior", "Više ulazaka",
                                       "3112952710293", "Pasoš Seniorski");

                    // Dalje
                    await Dalje(_page, "Dalje"); //ili Nazad

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Rekapitulacija"));

                    //Potvrde na stranici rekapitulacija
                    await PotvrdaRekapitulacije(_page);

                    // Klik na Plaćanje
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                    // Na stranici Plaćanje
                    //await Expect(_page).ToHaveTitleAsync(new Regex("VPos stranica plaćanja"));
                    await Placanje(_page, "5342230500001234", "08", "2025");

                    //await Expect(_page).ToHaveTitleAsync(new Regex("Uspešna kupovina"));

                    await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                    //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();

                    //await _page.PauseAsync();
                    //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
                    Assert.Pass();

                }
        */
/*****************************************************
        [GeneratedRegex("INDIVIDUALNO PUTNO OSIGURANJE", RegexOptions.IgnoreCase, "sr-Latn-RS")]
        private static partial Regex MyRegexIndividualno();

******************************/
    }
}