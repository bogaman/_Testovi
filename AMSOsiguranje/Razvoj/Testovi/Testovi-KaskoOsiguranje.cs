
namespace Razvoj
{
    [TestFixture, Order(9)]
    [Parallelizable(ParallelScope.Self)]
    public partial class KaskoOsiguranje : Osiguranje
    {
        #region Testovi

        [Test, Order(101)]
        public async Task KA_01_PregledPretragaPolisa()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Klik na Pregled / Pretraga polisa
                await _page.GetByText("Pregled / Pretraga polisa").ClickAsync();
                // Provera da li se otvorila stranica sa gridom pregled polisa
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid sa polisama i da li radi filter na gridu 
                string tipGrida = "Pregled polisa Kasko Osiguranja";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = RucnaUloga;
                //await ProveriFilterGrida(_page, kriterijumFiltera, tipGrida, 9);
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 9, "TekstBoks", 0);

                //Sortiraj po broju dokumenta - Rastuće
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Br. dok. -$") }).Locator("div").Nth(1).ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string brojDokumentaMin = await ProcitajCeliju(1, 1);
                string BrojPolise = await ProcitajCeliju(1, 2);

                //Sortiraj po broju dokumenta - Opadajuće
                await _page.Locator("//div[@class='sort active']//div[@class='sortDir']").ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                BrojPolise = await ProcitajCeliju(1, 2);
                //Pročitaj datum isteka MAX
                string brojDokumentaMax = await ProcitajCeliju(1, 1);

                if (int.Parse(brojDokumentaMin) < int.Parse(brojDokumentaMax))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"Sortiranje po Broju dokumenta ne radi.");
                    await LogovanjeTesta.LogException($"Sortiranje po Broju dokumenta ne radi..", new Exception("Sortiranje po Broju dokumenta ne radi."));
                    throw new Exception("Sortiranje po Broju dokumenta ne radi.");
                }

                await IzlogujSe(_page);
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
                LogovanjeTesta.LogMessage($"✅ Uspešan test {NazivTekucegTesta} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);

            }
        }



        [Test, Order(102)]
        public async Task KA_02_KalkulisanjeIzmenaPolise_nivo_I()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, "mario.radomir@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                await _page.GetByRole(AriaRole.Link, new() { Name = " Nova polisa" }).ClickAsync();
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Dokument/0";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                #region Osnovni podaci
                await _page.Locator("//div[@etitle='Jednogodišnje']").ClickAsync();
                if (Okruzenje == "UAT")
                {
                    await _page.Locator("#idTrajanje").GetByText("vise od 2").ClickAsync();
                }
                else
                {
                    await _page.Locator("#idTrajanje").GetByText("Više od 2").ClickAsync();
                }


                await _page.Locator("#idBonus > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Bez bonusa i malusa").ClickAsync();
                await _page.Locator("span").Filter(new() { HasText = "Bez bonusa i malusa" }).ClickAsync();
                await _page.Locator("#idBonus").GetByText("1 godina").ClickAsync();

                await _page.GetByText("---EvropaCela TurskaTurska, Gruzija, Jermenija, AzerbejdžanCela AzijaSrbija ---").ClickAsync();
                await _page.GetByText("Turska, Gruzija, Jermenija, Azerbejdžan").ClickAsync();

                await _page.GetByText("---do 10od 11 do 20od 21 do 50od 51 do 100preko 100 ---").ClickAsync();
                await _page.GetByText("od 11 do 20").ClickAsync();

                await _page.GetByText("---Do 10 godOd 10 do 14 godinaViše od 15 godina ---").ClickAsync();
                await _page.GetByText("Više od 15 godina").ClickAsync();

                await _page.GetByText("---Domaće tableInostrane table ---").ClickAsync();
                await _page.GetByText("Domaće table").ClickAsync();

                await _page.GetByText("---MesečnoTromesečnoPolugodišnjeGodišnjeU celosti ---").ClickAsync();
                await _page.GetByText("Mesečno", new() { Exact = true }).ClickAsync();

                #endregion Osnovni podaci


                #region Podaci o vozilu

                await _page.GetByText("Vrsta vozila").ClickAsync();
                //await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("//e-select[@id='selVrstaVozila']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.GetByText("Putničko vozilo").ClickAsync();

                await _page.Locator("//e-select[@id='selKorekcijePremije']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.GetByText("Taksi vozilo").ClickAsync();

                await _page.GetByText("---KataloškaFakturnaDogovorna ---").ClickAsync();
                await _page.GetByText("Kataloška").ClickAsync();
                await _page.Locator("#selMarke > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();

                await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("cher");
                await _page.GetByText("CHERY").First.ClickAsync();
                await _page.Locator("#selTipovi > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("EGO", new() { Exact = true }).ClickAsync();
                await _page.Locator("#selModeli > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("1.3 MT Luxury (zapremina:1297").ClickAsync();
                await _page.GetByText("Godina proizvodnje").ClickAsync();
                await _page.Locator("//e-input[contains(@inner-label,'Godina proizvodnje')]//input[contains(@type,'text')]").FillAsync((DateTime.Now.Year - 10).ToString());

                await _page.GetByText("Reg oznaka").ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string RegistarskaOznaka = DefinisiRegistarskuOznaku();

                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).FillAsync("BG" + RegistarskaOznaka);

                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string _brojSasije = DefinisiBrojSasije();
                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

                #endregion Podaci o vozilu



                #region Zapisnik

                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).FillAsync("Felne");

                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").First.FillAsync("100000");

                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).FillAsync("Ozvučenje");

                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").Nth(1).ClickAsync();
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").Nth(1).FillAsync("200000");

                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).FillAsync("1. Ovo je posebna napomena - Zabeležba\n2. Ovo je posebna napomena - Zabeležba");

                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).FillAsync("1. Ovo je zatečeno stanje vozila - oštećenje\n2. Ovo je zatečeno stanje vozila - oštećenje");

                await _page.Locator("//textarea[@placeholder='Napomena']").ClickAsync();
                await _page.Locator("//textarea[@placeholder='Napomena']").FillAsync("1- Ovo je napomena\n2- Ovo je napomena");

                #endregion Zapisnik

                #region Osnovni paket osiguranja
                await _page.Locator(".col-5 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Standard", new() { Exact = true }).ClickAsync();

                await _page.Locator(".div_rizici > .row > .col-3 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-predmet-osiguranja").GetByText("učešće 10% min. 300€").ClickAsync();
                #endregion Osnovni paket osiguranja

                #region Popusti i doplaci
                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu starosti osiguranika (vlasnika vozila)" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu posebnih ugovaranja" }).Locator("i").ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 100" }).GetByRole(AriaRole.Textbox).FillAsync("1");
                await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(1).FillAsync("10");

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak za vozila inostrane registracije" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za članove AMSS" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Drugi automobil porodice" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust od posebnog poslovnog" }).Locator("i").ClickAsync();
                await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(2).FillAsync("15");

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za ostvareni rezultat po vrsti vozila" }).Locator("i").ClickAsync();
                await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(3).FillAsync("20");

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za invalide" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za zaposlene u AMSO i AMSS" }).Locator("i").ClickAsync();
                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za zaposlene u AMSO i AMSS" }).Locator("div").Nth(3).ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za ekološka vozila" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu procenjenog rizika" }).Locator("i").ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 50" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 50" }).GetByRole(AriaRole.Textbox).FillAsync("25");

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust po osnovu procenjenog rizika" }).Locator("i").ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(5).FillAsync("30");
                await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 30" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 30" }).GetByRole(AriaRole.Textbox).FillAsync("30");
                #endregion Popusti i doplaci

                #region Dopunska osiguranja
                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                await _page.GetByText("Osiguranje prtljaga i alata").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();
                await _page.GetByText("---StandardStandard + ---").First.ClickAsync();
                await _page.GetByText("Standard +").Nth(1).ClickAsync();
                await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard + Paket Po sumi" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard + Paket Po sumi" }).GetByRole(AriaRole.Textbox).FillAsync("200000");

                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                await _page.GetByText("Osiguranje reklamnih i drugih").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();

                await _page.GetByText("---StandardStandard + ---").ClickAsync();
                await _page.GetByText("Standard", new() { Exact = true }).Nth(3).ClickAsync();
                await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard Paket Po sumi" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard Paket Po sumi" }).GetByRole(AriaRole.Textbox).FillAsync("100000");

                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                await _page.GetByText("Osiguranje troškova").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();

                await _page.Locator(".po-vrsta > div:nth-child(4) > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Niža klasa – cena novog").Nth(2).ClickAsync();
                await _page.GetByText("---Standard + ---").ClickAsync();
                await _page.GetByText("Standard +", new() { Exact = true }).Nth(4).ClickAsync();

                #endregion Dopunska osiguranja

                //nalaženje poslednjeg broja dokumenta u Kasko polisama
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaKasko();

                await _page.GetByRole(AriaRole.Button, new() { Name = " Kalkuliši" }).ClickAsync();
                //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());

                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();

                //Proveri štampu zapisnika
                await ProveriStampu404(_page, "Štampaj zapisnik", "Štampa zapisnika polise kasko osiguranja");

                //Finansijska analitika
                await _page.GetByRole(AriaRole.Button, new() { Name = "Finansijska analitika" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plan otplate" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Prilozi" }).ClickAsync();

                //Info ponuda
                await _page.GetByRole(AriaRole.Button, new() { Name = "Info ponuda" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Izmeni" }).ClickAsync();
                await _page.Locator(".no-content > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 0% min. 0€").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 20% min. 200€").ClickAsync();



                await _page.GetByRole(AriaRole.Button, new() { Name = " Snimi i kalkuliši" }).ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());
                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();


                await ProveriStampu404(_page, "Štampaj info ponudu", "Štampa info ponude polise kasko osiguranja");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Aneksi / Obračuni" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Finansijska analitika" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Plan otplate" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Prilozi" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Zahtevi ka administraciji" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Zahtevi ka administraciji" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Aneksi / Obračuni" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Opšti podaci" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Prethodne verzije" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "1 " }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "2 " }).ClickAsync();
                await _page.Locator("e-sidenav").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

                await IzlogujSe(_page);
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
                LogovanjeTesta.LogMessage($"✅ Uspešan test {NazivTekucegTesta} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }



        [Test, Order(103)]

        public async Task KA_03_KalkulisanjeIzmenaPolise_nivo_III()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, "bogdan.mandaric@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                await _page.GetByRole(AriaRole.Link, new() { Name = " Nova polisa" }).ClickAsync();
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Dokument/0";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                #region Osnovni podaci
                await _page.Locator("//div[@etitle='Jednogodišnje']").ClickAsync();
                await _page.Locator("#idTrajanje").GetByText("Jednogodišnje").First.ClickAsync();
                //await _page.Locator("span").Filter(new() { HasText = "Bez bonusa i malusa" }).ClickAsync();
                //await _page.Locator("#idBonus").GetByText("1 godina").ClickAsync();

                //await _page.Locator("#idBonus > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Bez bonusa i malusa").ClickAsync();
                //await _page.Locator("span").Filter(new() { HasText = "Bez bonusa i malusa" }).ClickAsync();
                //await _page.Locator("#idBonus").GetByText("1 godina").ClickAsync();

                await _page.GetByText("---EvropaCela TurskaTurska, Gruzija, Jermenija, AzerbejdžanCela AzijaSrbija ---").ClickAsync();
                await _page.GetByText("Evropa").ClickAsync();

                //await _page.GetByText("---do 10od 11 do 20od 21 do 50od 51 do 100preko 100 ---").ClickAsync();
                //await _page.GetByText("od 11 do 20").ClickAsync();

                await _page.GetByText("---Do 10 godOd 10 do 14 godinaViše od 15 godina ---").ClickAsync();
                await _page.GetByText("Od 10 do 14 godina").ClickAsync();

                //await _page.GetByText("---Domaće tableInostrane table ---").ClickAsync();
                //await _page.GetByText("Domaće table").ClickAsync();

                await _page.GetByText("---MesečnoTromesečnoPolugodišnjeGodišnjeU celosti ---").ClickAsync();
                await _page.GetByText("Mesečno", new() { Exact = true }).ClickAsync();

                #endregion Osnovni podaci

                #region Podaci o vozilu

                await _page.GetByText("Vrsta vozila").ClickAsync();
                //await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("//e-select[@id='selVrstaVozila']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.GetByText("Teretno vozilo").ClickAsync();

                //await _page.Locator("//e-select[@id='selKorekcijePremije']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.GetByText("Taksi vozilo").ClickAsync();

                await _page.GetByText("---KataloškaFakturnaDogovorna ---").ClickAsync();
                await _page.GetByText("Kataloška").ClickAsync();
                await _page.Locator("#selMarke > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();

                await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("fa");
                await _page.GetByText("FAP").First.ClickAsync();
                await _page.Locator("#selTipovi > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("1017 (MERCEDES)", new() { Exact = true }).ClickAsync();
                await _page.Locator("#selModeli > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("KK AL").ClickAsync();
                await _page.GetByText("Godina proizvodnje").ClickAsync();
                await _page.Locator("//e-input[contains(@inner-label,'Godina proizvodnje')]//input[contains(@type,'text')]").FillAsync((DateTime.Now.Year - 2).ToString());

                await _page.GetByText("Nosivost").ClickAsync();
                await _page.Locator("//e-input[contains(@inner-label,'Nosivost')]//input[contains(@type,'text')]").FillAsync("2023");
                await _page.GetByText("Reg oznaka").ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string RegistarskaOznaka = DefinisiRegistarskuOznaku();

                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).FillAsync("BG" + RegistarskaOznaka);

                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string _brojSasije = DefinisiBrojSasije();
                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

                #endregion Podaci o vozilu

                #region Zapisnik

                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).FillAsync("Felne");

                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").First.FillAsync("100000");

                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).FillAsync("Ozvučenje");

                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").Nth(1).ClickAsync();
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").Nth(1).FillAsync("200000");

                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).FillAsync("1. Ovo je posebna napomena - Zabeležba\n2. Ovo je posebna napomena - Zabeležba");

                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).FillAsync("1. Ovo je zatečeno stanje vozila - oštećenje\n2. Ovo je zatečeno stanje vozila - oštećenje");

                await _page.Locator("//textarea[@placeholder='Napomena']").ClickAsync();
                await _page.Locator("//textarea[@placeholder='Napomena']").FillAsync("1- Ovo je napomena\n2- Ovo je napomena");

                #endregion Zapisnik

                #region Osnovni paket osiguranja
                await _page.Locator(".col-5 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Standard +", new() { Exact = true }).ClickAsync();

                await _page.Locator(".div_rizici > .row > .col-3 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-predmet-osiguranja").GetByText("učešće 10% min. 150€").ClickAsync();
                #endregion Osnovni paket osiguranja

                #region Popusti i doplaci
                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu starosti osiguranika (vlasnika vozila)" }).Locator("i").ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu posebnih ugovaranja" }).Locator("i").ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 100" }).GetByRole(AriaRole.Textbox).FillAsync("1");
                //await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(1).FillAsync("10");

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak za vozila inostrane registracije" }).Locator("i").ClickAsync();

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za članove AMSS" }).Locator("i").ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Drugi automobil porodice" }).Locator("i").ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust od posebnog poslovnog" }).Locator("i").ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(2).FillAsync("15");

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za ostvareni rezultat po vrsti vozila" }).Locator("i").ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(3).FillAsync("20");

                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za invalide" }).Locator("i").ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za zaposlene u AMSO i AMSS" }).Locator("i").ClickAsync();
                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za zaposlene u AMSO i AMSS" }).Locator("div").Nth(3).ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za ekološka vozila" }).Locator("i").ClickAsync();

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu procenjenog rizika" }).Locator("i").ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 50" }).GetByRole(AriaRole.Textbox).ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 50" }).GetByRole(AriaRole.Textbox).FillAsync("25");

                //await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust po osnovu procenjenog rizika" }).Locator("i").ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").GetByRole(AriaRole.Textbox).Nth(5).FillAsync("30");
                //await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 30" }).GetByRole(AriaRole.Textbox).ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "min 0, max 30" }).GetByRole(AriaRole.Textbox).FillAsync("30");
                #endregion Popusti i doplaci

                #region Dopunska osiguranja
                //await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                //await _page.GetByText("Osiguranje prtljaga i alata").ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();
                //await _page.GetByText("---StandardStandard + ---").First.ClickAsync();
                //await _page.GetByText("Standard +").Nth(1).ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard + Paket Po sumi" }).GetByRole(AriaRole.Textbox).ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard + Paket Po sumi" }).GetByRole(AriaRole.Textbox).FillAsync("200000");

                //await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                //await _page.GetByText("Osiguranje reklamnih i drugih").ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();

                //await _page.GetByText("---StandardStandard + ---").ClickAsync();
                //await _page.GetByText("Standard", new() { Exact = true }).Nth(3).ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard Paket Po sumi" }).GetByRole(AriaRole.Textbox).ClickAsync();
                //await _page.Locator("e-predmet-osiguranja").Filter(new() { HasText = "Odabir paketa osiguranja ---StandardStandard + Standard Paket Po sumi" }).GetByRole(AriaRole.Textbox).FillAsync("100000");

                //await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                //await _page.GetByText("Osiguranje troškova").ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();

                //await _page.Locator(".po-vrsta > div:nth-child(4) > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Niža klasa – cena novog").Nth(2).ClickAsync();
                //await _page.GetByText("---Standard + ---").ClickAsync();
                //await _page.GetByText("Standard +", new() { Exact = true }).Nth(4).ClickAsync();

                #endregion Dopunska osiguranja

                //nalaženje poslednjeg broja dokumenta u Kasko polisama
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaKasko();

                await _page.GetByRole(AriaRole.Button, new() { Name = " Kalkuliši" }).ClickAsync();
                //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());

                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();

                //Proveri štampu zapisnika
                await ProveriStampu404(_page, "Štampaj zapisnik", "Štampa zapisnika polise kasko osiguranja");

                await _page.PauseAsync();
                //Finansijska analitika
                await _page.GetByRole(AriaRole.Button, new() { Name = "Finansijska analitika" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plan otplate" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Prilozi" }).ClickAsync();

                //Info ponuda
                await _page.GetByRole(AriaRole.Button, new() { Name = "Info ponuda" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Izmeni" }).ClickAsync();
                await _page.Locator(".no-content > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 10% min. 300€").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 10% min. 500€").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Snimi i kalkuliši" }).ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());
                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();


                await ProveriStampu404(_page, "Štampaj info ponudu", "Štampa info ponude polise kasko osiguranja");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Aneksi / Obračuni" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Finansijska analitika" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Aneksi / Obračuni" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plan otplate" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Prilozi" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Zahtevi ka administraciji" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Opšti podaci" }).ClickAsync();

                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Prethodne verzije" }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "1 " }).ClickAsync();
                await _page.GetByRole(AriaRole.Link, new() { Name = "2 " }).ClickAsync();
                await _page.Locator("e-sidenav").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

                await IzlogujSe(_page);
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
                LogovanjeTesta.LogMessage($"✅ Uspešan test {NazivTekucegTesta} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }






        [Test, Order(104)]
        public async Task KA_04_BrisanjePolise()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, "mario.radomir@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Klik na Pregled / Pretraga polisa
                await _page.GetByText("Pregled / Pretraga polisa").ClickAsync();
                // Provera da li se otvorila stranica sa gridom pregled polisa
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid sa polisama i da li radi filter na gridu 
                string tipGrida = "Pregled polisa Kasko Osiguranja";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = "KreiranRaskinutStorniranU izradi";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 8, "Lista", 3);

                kriterijumFiltera = "8888 - Mario Radomir";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 9, "TekstBoks", 0);

                //Sortiraj po datumu izdavanja - Rastuće
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Datum izdavanja -$") }).Locator("div").Nth(1).ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string brojDokumentaMin = await ProcitajCeliju(1, 1);
                string DatumIzdavanja = await ProcitajCeliju(1, 3);

                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + brojDokumentaMin;

                await _page.GetByText(DatumIzdavanja).First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);


                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Obriši dokument" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Ne", Exact = true }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Obriši dokument" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Da!" }).ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await _page.GetByText("Dokument uspešno obrisan").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/0");






                await IzlogujSe(_page);
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
                LogovanjeTesta.LogMessage($"✅ Uspešan test {NazivTekucegTesta} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }









        [Test, Order(111)]
        [TestCase("Jednogodišnje")]
        [TestCase("2 godine")]
        [TestCase("Više od 2")]
        public async Task KA_05_Polisa(string trajanje)
        {

            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, "bogdan.mandaric@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.GetByText("Osiguranje vozila").HoverAsync(); //Pređi mišem preko teksta Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync(); //Klikni na tekst Osiguranje vozila
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync(); //Klikni u meniju na Kasko
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata"); //Provera da li se otvorila stranica sa pregledom polisa AO

                await _page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/0"); //Provera da li se otvorila stranica Nova polisa

                //Ovde proveri padajuće liste Partner, Tender
                //await ProveriPadajucuListu(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");
                //await ProveriPadajucuListu(_page, "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']");

                await _page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("11070");
                await _page.Locator("#selMestaIzdavanja").GetByText("11070 - Beograd (Novi Beograd)").ClickAsync();

                if (Okruzenje == "Više od 2" && trajanje == "Više od 2")
                {
                    trajanje = "vise od 2";
                }

                await _page.Locator("//e-select[@id='idTrajanje']//div[@class='control-main']").ClickAsync();
                await _page.Locator("//e-select[@id='idTrajanje']").GetByText(trajanje).First.ClickAsync();

                await _page.Locator("//e-select[@id='idTeritorijalno']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.Locator("//e-select[@id='idTeritorijalno']").GetByText("Evropa").ClickAsync();

                await _page.Locator("//e-select[@id='idGodineVozStaza']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.Locator("//e-select[@id='idGodineVozStaza']").GetByText("Više od").ClickAsync();

                //await _page.Locator("//e-select[@id='idInostraneTable']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.Locator("//e-select[@id='idInostraneTable']").GetByText("Domaće table").ClickAsync();

                await _page.Locator("//e-select[@id='idDinamikaPlacanja']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.Locator("//e-select[@id='idDinamikaPlacanja']").GetByText("Mesečno", new() { Exact = true }).ClickAsync();



                #region Lične karte

                await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("+38163123456");
                await _page.Locator("#ugovarac").GetByText("Email").ClickAsync();
                await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("amso.bogdan@mail.eonsystem.com");


                string rezultatOcitavanja = await OcitajDokument_1(_page, "Licna1");
                if (rezultatOcitavanja == "")
                {
                    await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").FillAsync("3001985710098");
                    await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").FillAsync(trajanje + "Milojko");
                    await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").PressAsync("Tab");
                    await _page.Locator("#ugovarac #inpPrezime input[type=\"text\"]").FillAsync("Pantić");
                    Console.WriteLine("Podaci su uspešno očitani sa lične karte.");
                }
                else if (rezultatOcitavanja != "" /*== "U čitaču/ima nije pronađena "*/)
                {
                    Console.WriteLine("Podaci nisu uspešno očitani sa lične karte. Unesite podatke ručno.");


                    //var notifyPopupLK = _page.Locator("//div[@class='notify greska']");
                    //bool isPopupVisibleLK = await notifyPopupLK.IsVisibleAsync();
                    //await _page.GetByRole(AriaRole.Button, new() { Name = " Očitaj ličnu kartu" }).ClickAsync();
                    //await _page.Locator("#notify1").ClickAsync();
                    //await _page.Locator("#notify1").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();


                    {
                        Console.WriteLine("Iskačući prozor je vidljiv. Podaci su uneti ručno.");
                        //await _page.Locator("#notify0 button").ClickAsync();

                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").FillAsync("3001985710098");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").PressAsync("Tab");

                        //await _page.Locator(".col-6 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").First.ClickAsync();
                        await _page.Locator("//div[@data-obj='ugovarac']//e-select[@value='0']//div[@class='control-main']").First.ClickAsync();
                        await _page.Locator("#ugovarac e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").ClickAsync();
                        await _page.Locator("#ugovarac e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").FillAsync("1107");
                        await _page.Locator("#ugovarac").GetByText("- Beograd (Novi Beograd)").First.ClickAsync();

                        await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").FillAsync(trajanje + "Nedeljko");
                        await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").PressAsync("Tab");
                        await _page.Locator("#ugovarac #inpPrezime input[type=\"text\"]").FillAsync("Gajić");

                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync("Nehruova");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync("89/16");

                    }
                }
                else
                {
                    Console.WriteLine("Iskačući prozor nije vidljiv. Očitana je LK");

                }

















                #endregion Lične karte





                #region Podaci o vozilu

                await _page.GetByText("Vrsta vozila").ClickAsync();
                //await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("//e-select[@id='selVrstaVozila']//div[@class='multiselect-dropdown input']").ClickAsync();
                await _page.GetByText("Putničko vozilo").ClickAsync();

                //await _page.Locator("//e-select[@id='selKorekcijePremije']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.GetByText("Taksi vozilo").ClickAsync();

                await _page.GetByText("---KataloškaFakturnaDogovorna ---").ClickAsync();
                await _page.GetByText("Kataloška").ClickAsync();
                await _page.Locator("#selMarke > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();

                await _page.Locator(".control-wrapper.field.info-text-field.inner-label-field.focus > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("dac");
                await _page.GetByText("DACIA").First.ClickAsync();
                await _page.Locator("#selTipovi > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("LOGAN Euro 6", new() { Exact = true }).ClickAsync();
                await _page.Locator("#selModeli > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Ambiance 16V").ClickAsync();
                await _page.GetByText("Godina proizvodnje").ClickAsync();
                await _page.Locator("//e-input[contains(@inner-label,'Godina proizvodnje')]//input[contains(@type,'text')]").FillAsync((DateTime.Now.Year - 1).ToString());

                //await _page.GetByText("Nosivost").ClickAsync();
                //await _page.Locator("//e-input[contains(@inner-label,'Nosivost')]//input[contains(@type,'text')]").FillAsync("2023");
                await _page.GetByText("Reg oznaka").ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string RegistarskaOznaka = DefinisiRegistarskuOznaku();

                await _page.Locator("e-input").Filter(new() { HasText = "Reg oznaka" }).GetByRole(AriaRole.Textbox).FillAsync("BG" + RegistarskaOznaka);

                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string _brojSasije = DefinisiBrojSasije();
                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

                #endregion Podaci o vozilu


                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 1" }).GetByRole(AriaRole.Textbox).FillAsync("Ozvučenje 1000W");
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").First.FillAsync("150000");
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).FillAsync("Ozvučenje 1000W");
                await _page.Locator(".vrednostRsd > .control-wrapper > .control > .control-main > .input").Nth(1).FillAsync("200000");
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Posebna napomena - zabeležba" }).FillAsync("Ovo je zabeležba za:\nOzvučenje 1000W i \nFelne");
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Dodatna oprema 2" }).GetByRole(AriaRole.Textbox).FillAsync("Pojačalo 800W");
                //await _page.Locator("div:nth-child(2) > .col-4 > .vrednostRsd > .control-wrapper > .control > .control-main > .input").ClickAsync();
                //await _page.Locator("div:nth-child(2) > .col-4 > .vrednostRsd > .control-wrapper > .control > .control-main > .input").FillAsync("100000");
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zatečeno stanje vozila - ošte" }).FillAsync("Vozilo je u\nveoma dobrom stanju\nVlasnik je odlično održavao.\n");
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Napomena", Exact = true }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Napomena", Exact = true }).FillAsync("Ovo je prvi red napomene\nOvo je drugi red napomene");
                await _page.Locator(".row.div_paket_header > div > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").First.ClickAsync();
                await _page.GetByText("Standard", new() { Exact = true }).ClickAsync();
                await _page.Locator(".div_rizici > .row > div:nth-child(5) > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-predmet-osiguranja").GetByText("učešće 30% min. 300€").ClickAsync();
                await _page.Locator("e-checkbox").Filter(new() { HasText = "Doplatak po osnovu starosti" }).Locator("i").ClickAsync();
                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za članove AMSS" }).Locator("i").ClickAsync();
                await _page.Locator("e-checkbox").Filter(new() { HasText = "Popust za invalide" }).Locator("i").ClickAsync();



                //nalaženje poslednjeg broja dokumenta u Kasko polisama
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaKasko();

                //await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije" }).ClickAsync();


                //await _page.GetByRole(AriaRole.Button, new() { Name = "Snimi" }).ClickAsync();

                //await _page.PauseAsync();
                //var popupLocator = _page.Locator("//div[contains(text(),'Podaci uspešno snimljeni')]");
                //#endregion Dopunska osiguranja


                //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();


                await _page.GetByRole(AriaRole.Button, new() { Name = " Kalkuliši" }).ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());
                //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());

                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                // Sačekaj da se popup pojavi (timeout 5 sekundi)
                //bool isPopupVisible = await popupLocator.WaitForAsync(new() { Timeout = 15000 }).ContinueWith(t => !t.IsFaulted);
                //await Expect(popupLocator).ToBeVisibleAsync(new() { Timeout = 5000 });

                //Assert.That(isPopupVisible, "Popup sa tekstom 'uspešno' se nije pojavio.");

                //await _page.PauseAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();

                //await _page.PauseAsync();
                //popupLocator = _page.Locator("//div[contains(text(),'Podaci uspešno snimljeni')]");

                // Sačekaj da se popup pojavi (timeout 5 sekundi)
                //isPopupVisible = await popupLocator.WaitForAsync(new() { Timeout = 15000 }).ContinueWith(t => !t.IsFaulted);
                //await Expect(popupLocator).ToBeVisibleAsync(new() { Timeout = 5000 });

                // Assert.That(isPopupVisible, "Popup sa tekstom 'uspešno' se nije pojavio.");

                //await _page.PauseAsync();

                //Info ponuda
                await _page.GetByRole(AriaRole.Button, new() { Name = "Info ponuda" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Izmeni" }).ClickAsync();
                await _page.Locator(".no-content > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 10% min. 300€").ClickAsync();
                await _page.Locator("e-tab-group").GetByText("učešće 10% min. 500€").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Snimi i kalkuliši" }).ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + (PoslednjiBrojDokumenta + 1).ToString());
                await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();


                await ProveriStampu404(_page, "Štampaj info ponudu", "Štampa info ponude polise kasko osiguranja");


                await _page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();

                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Da!" }).ClickAsync();


                /******************************
                var page1 = await page.RunAndWaitForPopupAsync(async () =>
                {
                    await page.GetByRole(AriaRole.Button, new() { Name = " Štampaj polisu" }).ClickAsync();
                });
                await page1.CloseAsync();
                var page2 = await page.RunAndWaitForPopupAsync(async () =>
                {
                    await page.GetByRole(AriaRole.Button, new() { Name = " Štampaj uplatnicu/fakturu" }).ClickAsync();
                });
                await page2.CloseAsync();
                var page3 = await page.RunAndWaitForPopupAsync(async () =>
                {
                    await page.GetByRole(AriaRole.Button, new() { Name = " Štampaj zapisnik" }).ClickAsync();
                });
                await page3.CloseAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = " Štampaj ponudu" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = " Štampaj ponudu" }).ClickAsync();
                await page.GetByRole(AriaRole.Link, new() { Name = "" }).ClickAsync();
                await page.GetByRole(AriaRole.Button, new() { Name = " Odjavljivanje" }).ClickAsync();
                *********************************/

                //await _page.PauseAsync();
                var popupLocator = _page.Locator("//div[contains(text(),'uspešno kreirana')]");

                // Sačekaj da se popup pojavi (timeout 5 sekundi)
                bool isPopupVisible = await popupLocator.WaitForAsync(new() { Timeout = 15000 }).ContinueWith(t => !t.IsFaulted);
                //await Expect(popupLocator).ToBeVisibleAsync(new() { Timeout = 5000 });

                Assert.That(isPopupVisible, "Popup sa tekstom 'uspešno kreirana' se nije pojavio.");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }


        [Test, Order(115)]
        public async Task KA_07_Storno()
        {
            try
            {
                //pronađi Kasko polisu koja je Kreirana i nije istekla
                int BrojDokumenta = 0;
                int BrojPonude = 0;
                int grBrojdokumenta = 0;

                Server = OdrediServer(Okruzenje);

                if (Okruzenje == "Razvoj")
                {
                    grBrojdokumenta = 0;
                }

                // Konekcija sa bazom
                string connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                string qBrojDokumenta = $"SELECT TOP 1 idDokument AS IdDokument " +
                                        $"FROM [CascoDB].[casco].[Dokument] " +
                                        $"WHERE ([Dokument].[idDokument] > '{grBrojdokumenta}' AND [idKorisnik] = 1001 AND [idProizvod] = 9 AND [idStatus] = 2  AND [datumIsteka] > CAST(GETDATE() AS DATE)) " +
                                        $"ORDER BY [brojUgovora] DESC, [idDokument] DESC;";

                string qBrojDokumentastari = $"SELECT MIN(idDokument) AS IdDokument " +
                                             $"FROM [CascoDB].[casco].[Dokument] " +
                                             $"WHERE ([Dokument].[idDokument] > '{grBrojdokumenta}' AND [idKorisnik] = 1001 AND [idProizvod] = 9 AND [idStatus] = 2  AND [datumIsteka] > CAST(GETDATE() AS DATE));";
                using SqlConnection konekcija = new(connectionString);
                konekcija.Open();

                using SqlCommand SqlKomanda = new(qBrojDokumenta, konekcija);
                object rezultat = SqlKomanda.ExecuteScalar();
                //BrojDokumenta = (Int32)(SqlKomanda.ExecuteScalar() ?? 0);
                BrojDokumenta = rezultat as int? ?? 0;
                konekcija.Close();
                if (NacinPokretanjaTesta == "ručno")
                    System.Windows.Forms.MessageBox.Show($"BrojDokumenta je: {BrojDokumenta}", "Informacija", MessageBoxButtons.OK);
                if (BrojDokumenta == 0)
                {
                    await LogovanjeTesta.LogException("Nema polisa za storniranje", new Exception("Nema polisa za storniranje"));
                    return;
                    //throw new Exception("Nema polisa za storniranje");
                }



                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, "bogdan.mandaric@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Klik na Pregled / Pretraga polisa
                await _page.GetByText("Pregled / Pretraga polisa").ClickAsync();
                // Provera da li se otvorila stranica sa gridom pregled polisa
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid sa polisama i da li radi filter na gridu 
                string tipGrida = "Pregled polisa Kasko Osiguranja";
                await ProveraPostojiGrid(_page, tipGrida);

                //body/div[@id='container']/div[contains(@class,'row content proizvod-0')]/div[contains(@class,'page-content')]/e-grid[@id='grid_dokumenti']/div[contains(@class,'grid field no-content')]/div[contains(@class,'podaci')]/a/div[contains(@class,'row grid-row dokumentStatus2 row-click')]/div[2]



                await _page.PauseAsync();

                // Filtriraj grid po Id dokument
                await FiltrirajGrid(_page, BrojDokumenta.ToString(), tipGrida, 1, "TekstBoks", 0);

                //await _page.GetByText(BrojDokumenta.ToString()).First.ClickAsync();
                //await _page.Locator("//body/div[@id='container']/div[contains(@class,'row content proizvod-0')]/div[contains(@class,'page-content')]/e-grid[@id='grid_dokumenti']/div[contains(@class,'grid field no-content')]/div[contains(@class,'podaci')]/a/div[contains(@class,'row grid-row dokumentStatus2 row-click')]/div[2]").ClickAsync();
                await _page.Locator($"//div[contains(@class,'column column_1')][normalize-space()='{BrojDokumenta.ToString()}']").ClickAsync();
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Dokument/" + BrojDokumenta.ToString();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                await _page.GetByRole(AriaRole.Button, new() { Name = " Storniraj" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Ne", Exact = true }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Storniraj" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Da!" }).ClickAsync();
                var porukaLocator = _page.Locator($"//div[contains(., 'Polisa broj {BrojDokumenta} uspešno stornirana')]");
                await porukaLocator.WaitForAsync(new() { Timeout = 4000 });

                await _page.GetByRole(AriaRole.Link, new() { Name = " Pregled / Pretraga polisa" }).ClickAsync();
                await _page.PauseAsync();
                return;



                string kriterijumFiltera = RucnaUloga;
                //await ProveriFilterGrida(_page, kriterijumFiltera, tipGrida, 9);
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 9, "TekstBoks", 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }


        [Test, Order(128)]
        public async Task KA_09_SE_PregledPretragaRazduznihListi()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Kasko
                await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Klik na Pregled / Pretraga Razdužnih listi
                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();
                // Provera da li se otvorila stranica sa gridom pregled polisa
                DodatakNaURL = "/Stroga-Evidencija/9/Kasko/Pregled-dokumenata/4";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid sa polisama i da li radi filter na gridu 
                string tipGrida = "Razdužne liste Kasko";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = RucnaUloga;
                //await ProveriFilterGrida(_page, kriterijumFiltera, tipGrida, 9);
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 3, "TekstBoks", 0);
                kriterijumFiltera = "Na verifikacijiU izradiVerifikovan";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 5, "Lista", 2);

                //Sortiraj po broju dokumenta - Rastuće
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Broj dokumenta -$") }).Locator("div").Nth(1).ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                DodatakNaURL = await ProcitajCeliju(1, 8);
                Match idDokumentaMin = Regex.Match(DodatakNaURL, @"(\d+)$");
                string brojDokumentaMin = await ProcitajCeliju(1, 1);


                //Sortiraj po broju dokumenta - Opadajuće
                await _page.Locator("//div[@class='sort active']//div[@class='sortDir']").ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                DodatakNaURL = await ProcitajCeliju(1, 8);
                Match idDokumentaMax = Regex.Match(DodatakNaURL, @"(\d+)$");
                string brojDokumentaMax = await ProcitajCeliju(1, 1);





                if (int.Parse(idDokumentaMin.Value) <= int.Parse(idDokumentaMax.Value))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"Sortiranje po Broju dokumenta ne radi.");
                    await LogovanjeTesta.LogException($"Sortiranje po Broju dokumenta ne radi.", new Exception("Sortiranje po Broju dokumenta ne radi."));
                    throw new Exception("Sortiranje po Broju dokumenta ne radi.");
                }

                await _page.GetByText(brojDokumentaMax).First.ClickAsync();
                // Provera da li se otvorila stranica sa gridom pregled polisa

                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija" + DodatakNaURL);


                // Proveri štampu postojeće razdužne liste
                await ProveriStampu404(_page, "Štampaj dokument", "Greška u štampi razdužne liste.");
                await ProveriStampu404(_page, "Štampaj dokument", "Greška u štampi razdužne liste.");

                await IzlogujSe(_page);
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
                LogovanjeTesta.LogMessage($"✅ Uspešan test {NazivTekucegTesta} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }


        [Test, Order(129)]
        public async Task KA_10_SE_NovaRazduznaLista()
        {
            try
            {
                await Pauziraj(_page);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, "bogdan.mandaric@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Kasko" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/9/Kasko/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Razdužne liste Kasko";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Nova razdužna lista i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Nova razdužna lista").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/9/Kasko/Dokument/4/0");







                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));
                //await _page.GetByLabel(NextDate.ToString("MMMM dd, yyyy")).ClickAsync();
                await _page.GetByLabel(CurrentDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                //await _page.GetByText(Asaradnik).ClickAsync();

                await _page.GetByText("Arhivski magacin Arhivski").ClickAsync();

                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                int PoslednjiDokumentStroga;
                string qPoslednjiDokumentStroga = "SELECT MAX ([IdDokument]) FROM [StrictEvidenceDB].[strictevidence].[tDokumenta];";

                Server = OdrediServer(Okruzenje);

                string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qPoslednjiDokumentStroga, konekcija))
                    {
                        PoslednjiDokumentStroga = (int)command.ExecuteScalar();
                    }
                    konekcija.Close();
                }

                Console.WriteLine($"Poslednji broj dokumenta u strogoj evidenciji je: {PoslednjiDokumentStroga}.\n");

                await _page.Locator("button").Filter(new() { HasText = "Kreiraj razdužnu listu" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/9/Kasko/Dokument/4/{PoslednjiDokumentStroga + 1}");



                //await Pauziraj(_page);
                // Definišite XPath za elemente
                //string xpath = "//div[@class='opsezi']/div//button[@class='left primary flat flex-center-center']";
                string xpath = "//div[@class='opsezi']/div//i[@class='ico-check']";
                // Kreirajte locator za sve elemente
                var elementi = _page.Locator(xpath);

                // Dobijte ukupan broj elemenata
                int brojElemenata = await elementi.CountAsync();
                // Ispis rezultata
                Console.WriteLine($"Ukupan broj redova (obrazaca) za razduživanje je: {brojElemenata - 1}");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Ukupan broj redova (obrazaca) za razduživanje je: {brojElemenata - 1}", "AO - Razdužna lista", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                //await _page.PauseAsync();{}

                //await _page.PauseAsync();
                // Kliknite na svaki element
                for (int i = brojElemenata - 1; i > 1; i--)
                {
                    await elementi.Nth(i).ClickAsync();

                }

                //await _page.PauseAsync();
                // Ovo je brisanje razdužne liste
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");

                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                                                                               //await Pauziraj(_page);
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/9/Kasko/Dokument/4/{PoslednjiDokumentStroga + 1}");
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl za BO da ima Razdužnu listu za verifikaciju nije poslat");

                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/9/Kasko/Pregled-dokumenata/4");


                DodatakNaURL = $"/Stroga-Evidencija/9/Kasko/Dokument/4/{(PoslednjiDokumentStroga + 1).ToString()}";

                await _page.GetByText(oznakaDokumenta).First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                //Ovde treba dodati proveru štampanja
                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije KA:");
                await ProveriStampu404(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije KA:");







                //await _page.Locator(".ico-ams-logo").ClickAsync();

                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.PauseAsync();
                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/9/Kasko/dokument/4/{PoslednjiDokumentStroga + 1}");



                //await _page.PauseAsync();

                string ocekivaniTekst = "Imate novi dokument \"Razdužna lista (OSK)\" za verifikacijuDokument možete pogledati klikom na link: ";
                await _page.GetByText(ocekivaniTekst + oznakaDokumenta).ClickAsync();
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/9/Kasko/dokument/4/{PoslednjiDokumentStroga + 1}");


                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl ka Agentu da je Razdužna lista verifikovana je poslat");

                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiranje verifikovane Razdužna lista KA:");
                await ProveriStampu404(_page, "Štampaj dokument", "Kreiranje verifikovane Razdužna lista KA:");
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/9/Kasko/Pregled-dokumenata/4");


                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, "bogdan.mandaric@eonsystem.com", "Lozinka1!");
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/9/Kasko/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await ProveriStampuPdf(_page, "Štampaj dokument", "Verifikovan dokument stroge evidencije KA:");
                await ProveriStampu404(_page, "Štampaj dokument", "Verifikovan dokument stroge evidencije KA:");

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "Dashboard");
                ocekivaniTekst = $"{oznakaDokumenta}";
                //await ProveraVestNijeObrisana(_page, ocekivaniTekst);
                await VestNePostoji(_page, ocekivaniTekst, oznakaDokumenta);
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
            }
            catch (Exception ex)
            {
                //await LogovanjeTesta.LogException("Greška u testu", ex);
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                throw;
            }
        }
        #endregion Testovi
    }

}


#region SetUp 


// Metoda koja se pokreće pre svakog testa
//[SetUp]
//public async Task SetUp5()
//{

// Definiši naziv trenutnog testa
//NazivTekucegTesta = TestContext.CurrentContext.Test.Name;

/*
// Kreiraj listener koji piše u fajl
TextWriterTraceListener fileListener = new TextWriterTraceListener(logFilePath);
// Dodaj ga u Listeners
Trace.Listeners.Add(fileListener);
File.WriteAllText(logFilePath, string.Empty); // Briše postojeći sadržaj
Trace.AutoFlush = true;
File.AppendAllText(logFilePath, $"[INFO] Pokretanje testa: {TestContext.CurrentContext.Test.Name} u {DateTime.Now}\n");
Trace.WriteLine("Recording a trace: Test setup started");
// Možeš odmah upisati početnu liniju u log
Trace.WriteLine($"[TRACE] Test pokrenut: {DateTime.Now}");
*/

//Definiši početnu stranu u zavisnosti od okruženja
//var (pocetnaStrana, pocetnaStranaWS) = DefinisiPocetnuStranu(Okruzenje);
//PocetnaStrana = pocetnaStrana;
//PocetnaStranaWS = pocetnaStranaWS;


//Trace.WriteLine($"*********************************");
//Trace.WriteLine($"*************************************[{DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")}] Pokrenut test: {NazivTekucegTesta}");

//Bira se uloga BackOffice za određene testove, bez obzira na ulogu koja je definisana u fajlu sa podacima Utils.cs
/****************************
            switch (NazivTekucegTesta)
            {
                case "AO_01_SE_PregledPretragaObrazaca":
                case "AO_02_SE_PregledPretragaDokumenata":
                case "AO_03_SE_UlazObrazaca":
                case "AO_04_SE_PrenosObrazaca":
                case "_ProveraDashboard":
                case "ZK_1_SE_PregledPretragaObrazaca":
                case "ZK_2_SE_PregledPretragaDokumenata":
                case "ZK_3_SE_UlazPrenosObrazaca":
                case "JS_1_SE_PregledPretragaRazduznihListi":
                //case "JS_2_SE_PregledPretragaObrazaca":
                case "DK_1_SE_PregledPretragaRazduznihListi":
                case "BO_1_PutnoZdravstvenoOsiguranje":
                case "_AO_4_ZamenaSerijskihBrojeva":
                    // Radi nešto
                    Uloga = "BackOffice";
                    break;
            }
******************************/
// Učitaj podatke za logovanje
//PodaciZaLogovanje(Uloga, Okruzenje, out string mejl, out string ime, out string lozinka);
//KorisnikMejl = mejl;
//KorisnikIme = ime;
//KorisnikPassword = lozinka;
/*******************************
            if (NacinPokretanjaTesta == "ručno")
            {
                System.Windows.MessageBox.Show($"Okruženje:: {Okruzenje}.\n" +
                                               $"URL:: {PocetnaStrana}.\n\n" +
                                               $"Uloga:: {Uloga}.\n" +
                                               $"Korisnik:: {KorisnikIme}.\n" +
                                               $"Mejl:: {KorisnikMejl}.\n" +
                                               $"Lozinka:: {KorisnikPassword}\n", $"Test:: {NazivTekucegTesta}", MessageBoxButton.OK, MessageBoxImage.Information);
            }
*************************************/
// Kreiranje instance Playwright-a
//_playwright = await Playwright.CreateAsync();
/*
            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = ["--start-maximized"]
            };
            if (NacinPokretanjaTesta != "ručno")
            {
                launchOptions = new BrowserTypeLaunchOptions
                {
                    Headless = true,
                    Args = ["--start-maximized"]
                };
            }

            _browser = Pregledac switch
            {
                "Chromium" => await _playwright.Chromium.LaunchAsync(launchOptions),
                "Firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
                "Webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
                _ => throw new ArgumentException("Nepoznat pregledac: " + Pregledac),
            };

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                // Set viewport size to null for maximum size
                ViewportSize = ViewportSize.NoViewport,
                //StorageStatePath = fileAuth // Učitaj sesiju iz fajla
            });
*/
// Kreiranje nove stranice/tab-a
//_page = await context.NewPageAsync();
/*
            await _page.Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
*/
// Upisivanje početka testa u fajl logOpsti.txt
//LogovanjeTesta.UnesiPocetakTesta();
//LogovanjeTesta.LogMessage($"Pokrenut je test: {NazivTekucegTesta}");

//Otvaranje početne strane
//await _page.GotoAsync(PocetnaStrana);
//await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);//sačekaj 

// Obriši token iz Local Storage-a
//await _page.EvaluateAsync("localStorage.removeItem('authToken');");

// Osveži stranicu nakon brisanja tokena - simulacija standardnog F5
//await _page.ReloadAsync();


//await ProveriURL(_page, PocetnaStrana, "/Dashboard");

// Očisti keš
// await context.ClearCookiesAsync();
//await context.ClearPermissionsAsync();

// Ako želiš sve (keš i kolačiće):
//await context.StorageStateAsync(new BrowserContextStorageStateOptions { Path = null });

//await _page.Context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true });
//await _page.Context.ClearCookiesAsync();  // ili _page.ClearCacheAsync() ako dodatno želiš

//await _page.ReloadAsync(); // osvežava stranicu nakon brisanja - simulacija standardnog F5

// Obriši token iz Local Storage-a
//await _page.EvaluateAsync("localStorage.removeItem('authToken');");
// Uradi refreš strane
//await _page.EvaluateAsync("location.reload(true);");
//await _page.ReloadAsync();

//}


#region Ovo pregledaj i doradi
/********************************************************        
        [SetUp]
        public async Task SetUp()
        {
            // Početna strana zavisi od okruženja
            PocetnaStrana = DefinisiPocetnuStranu(Okruzenje);
            string NazivTekucegTesta = TestContext.CurrentContext.Test.Name;
            Console.WriteLine($"Pokreće se test: {NazivTekucegTesta}");

            if (NazivTekucegTesta == "StrogaEvidencijaAO" || NazivTekucegTesta == "ProveraDashboard" || NazivTekucegTesta == "StrogaEvidencijaZK" || NazivTekucegTesta == "PutnoZdravstvenoOsiguranje" || NazivTekucegTesta == "_ZamenaSerijskihBrojeva")
            {
                Uloga = "BackOffice";
            }
            PodaciZaLogovanje(Uloga, out string mejl, out string ime, out string lozinka);
            KorisnikMejl = mejl;
            KorisnikIme = ime;
            KorisnikPassword = lozinka;


            // Naziv i putanja fajla auth_state prema okruženju i korisniku
            string fileAuth = FileFolder + "/Sesije/auth_state-" + Okruzenje + "-" + Uloga + ".json";

            // Da li postoji odgovarajući auth_state fajl
            bool fajlPostoji = File.Exists(fileAuth);
            // Predpostavka je da fajl nije validan
            bool fajlValidan = false;

            Console.WriteLine($"Izabrano je okruženje:: {Okruzenje}.\n" +
                            $"URL je:: {PocetnaStrana}.\n\n" +
                            $"Korisnik ima ulogu:: {Uloga}.\n" +
                            $"Korisnik je:: {KorisnikIme}.\n" +
                            $"Mejl je:: {KorisnikMejl}.\n" +
                            $"Lozinka je:: {KorisnikPassword}\n\n" +
                            $"Postojanje fajla::\n{fileAuth} je:: {fajlPostoji}.\n");


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
**************************************/
/******************************************************************************
MessageBox.Show($"authState.Cookies[0].Expires ima vrednost: {authState.Cookies[0].Expires}.\n" +
                $"expiresToken1 ima vrednost: {expiresToken1}.\n" +
                $"dateTimeOffset1: {dateTimeOffset1}\n" +
                $"localDateTime1: {localDateTime1}\n" +
                $"DateTime.Now: {DateTime.Now}\n" +
                $"razlika1: {razlika1}"
                , "Informacija", MessageBoxButtons.OK);
******************************************************************************/
/************************************************
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
                    System.Windows.Forms.MessageBox.Show($"Validnost fajla je: {fajlValidan}\nGrška je: {ex.Message}. ", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    fajlValidan = false;
                    System.Windows.Forms.MessageBox.Show($"Došlo je do greške:\n + {ex.Message}", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                System.Windows.Forms.MessageBox.Show($@"Fajl auth_state ne postoji ili nije validan, idem da ga napravim", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                //await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);


                // Uradi refreš strane
                await _page.ReloadAsync();

                // Nakon logovanja, dohvatite cookies
                var cookies = await _page.Context.CookiesAsync();
                foreach (var cookie in cookies)
                {
                    // MessageBox.Show($"Kolačić: {cookie.Name},\nVrednost: {cookie.Value}");
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
                System.Windows.Forms.MessageBox.Show("Nije otvorena strana Dashboard", "Informacija", MessageBoxButtons.OK);
                return;
            }

            //MessageBox.Show("Idemo u test");
        }
*************************************************/
/*  
   // Kreiranje instance Playwright-a
   _playwright = await Playwright.CreateAsync();
   var launchOptions = new BrowserTypeLaunchOptions
   {
       Headless = false,
       Args = ["--start-maximized"]
   };

            // Pokretanje browsera (Chrome ili Firefox u headless načinu rada)
            _browser = await _playwright.Chromium.LaunchAsync(launchOptions);
            //_browser = await _playwright.Firefox.LaunchAsync(launchOptions);

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                // Set viewport size to null for maximum size
                ViewportSize = ViewportSize.NoViewport,
                StorageStatePath = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/auth_state.json" // Učitaj sesiju iz fajla
            });

            // Kreiranje nove stranice/tab-a
            _page = await context.NewPageAsync();
*/
// Postavljanje event handler-a za otvaranje popup prozora
//_page.Popup += (_, popup) =>
//{
//_popupPage = popup;
//};

//Otvaranje početne strane
////////////await _page.GotoAsync(PocetnaStrana);

// Provera da li je otvorena početna stranica - za logovanje
/////////var title = await _page.TitleAsync();
//Assert.That(title, Is.EqualTo("Strana za logovanje"));
// Sačekaj da se stranica učita
///////////await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu

/*
            #region Logovanje

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
*/

// Da li se otvorila stranica Dashboard                                                                           
//await _page.PauseAsync();
//await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
//CurrentUrl = _page.Url;
//Assert.That(PocetnaStrana + "/Dashboard", Is.EqualTo(CurrentUrl));

/*
            //Definisanje imena korisnika
            if (KorisnikMejl == "bogdan.mandaric@eonsystem.com")
            {
                KorisnikIme = "Bogdan Mandarić";
            }
            else if (KorisnikMejl == "davor.bulic@eonsystem.com")
            {
                KorisnikIme = "Davor Bulić";
            }
            else
            {
                KorisnikIme = "Nenad Žakula";
            }
            */
// }

#endregion Ovo pregledaj i doradi


//public void Setup_original() { }

#endregion SetUp

/*******************************************************************************************
//Sortiraj po broju dokumenta - Opadajuće
                await _page.Locator("//div[@class='sort active']//div[@class='sortDir']").ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string BrojDokumentaKaskoPolise = await ProcitajCeliju(1, 1);





                //Pročitaj datum isteka MAX
                string brojDokumentaMax = await ProcitajCeliju(1, 1);

                if (int.Parse(brojDokumentaMin) < int.Parse(brojDokumentaMax))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"Sortiranje po Broju dokumenta ne radi.");
                    await LogovanjeTesta.LogException($"Sortiranje po Broju dokumenta ne radi..", new Exception("Sortiranje po Broju dokumenta ne radi."));
                    throw new Exception("Sortiranje po Broju dokumenta ne radi.");
                }
                await _page.PauseAsync();
                return; // Zaustavi test ako je pokrenut ručno


                // Izbroj koliko ima redova u gridu
                var redovi = _page.Locator("//div[@class='podaci']//div[contains(@class, 'grid-row')]");
                int brojRedova = await redovi.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Redova ima-1: {brojRedova}", "Informacija", MessageBoxButtons.OK);
                }

                //ili
                var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Redova ima-2: {rows.Count}", "Informacija", MessageBoxButtons.OK);
                }

                //Izbroj koliko ima kolona u gridu
                var koloneUPrvomRedu = _page.Locator("(//div[contains(@class, 'podaci')]//div[contains(@class, 'grid-row')])[1]//div[contains(@class, 'column')]");
                int brojKolona = await koloneUPrvomRedu.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj kolona 1: {brojKolona}", "Informacija", MessageBoxButtons.OK);
                }


                // Pročitaj broj dokumenta Kasako polise (sadržaj ćelije u prvom redu i prvoj koloni)
                string brojDokumenta = await ProcitajCeliju(1, 1);
                // Procitaj Broj polise Kasko (sadržaj ćelije u prvom redu i trećoj koloni)
                string brojPolise = await ProcitajCeliju(1, 3);
                // Procitaj Lokacija obrasca polise AO (sadržaj ćelije u prvom redu i desetoj koloni)
                string kreirao = await ProcitajCeliju(1, 10);
                string status = await ProcitajCeliju(1, 9);

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Serijski broj obrasca: {brojDokumenta} \n" +
                                                         $"Broj polise: {brojPolise} \n" +
                                                         $"Kreirao: {kreirao}\n" +
                                                         $"Status: {status}", "Informacija", MessageBoxButtons.OK);
                }

                try
                {
                    // Pronađi lokator za ćeliju koja sadrži Serijski broj obrasca polise AO
                    var celijaBrojDokumenta = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{brojDokumenta}']");

                    // Provera da li je element vidljiv
                    if (await celijaBrojDokumenta.IsVisibleAsync())
                    {
                        await celijaBrojDokumenta.ClickAsync();
                        Console.WriteLine($"Kliknuto na ćeliju sa vrednošću: {brojDokumenta}");
                        //TestLogger.LogMessage($"Kliknuto na ćeliju sa vrednošću: {prom2}");
                    }
                    else
                    {
                        //TestLogger.LogError($"Ćelija sa vrednošću '{prom2}' nije vidljiva.");
                    }
                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{brojDokumenta}'.", ex);
                }

                await ProveriURL(_page, PocetnaStrana, $"/Kasko-osiguranje-vozila/9/Kasko/Dokument/{brojDokumenta}"); // Provera da li se otvorila odgovarajuća kartica obrasca

                try
                {
                    string[] expectedValues = [brojDokumenta, status, Asaradnik, brojPolise];
                    bool allFound = true;

                    foreach (string value in expectedValues)
                    {
                        // Lokator za input element sa očekivanom vrednošću u atributu value
                        //await _page.Locator($"e-span[value='{value}']").Nth(0).HoverAsync(); // Hover over the input element
                        var tekstBoks = _page.Locator($"e-span[value='{value}']");   //e-input[@value='Autoodgovornost']

                        if (await tekstBoks.CountAsync() == 0)
                        {
                            allFound = false;
                            LogovanjeTesta.LogError($"Nije pronađen element sa vrednošću: '{value}'");
                        }
                        else
                        {
                            LogovanjeTesta.LogMessage($"Uspešno pronađen element sa vrednošću: '{value}'");
                        }
                    }

                    if (allFound)
                    {
                        LogovanjeTesta.LogMessage("Svi očekivani elementi su pronađeni sa tačnim vrednostima.");
                    }
                    else
                    {
                        LogovanjeTesta.LogError("Neki od očekivanih elemenat nisu pronađeni.");
                    }
                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException("Greška prilikom provere elemenata sa vrednostima prom1–prom4.", ex);
                }
                **************************************************************************************************/