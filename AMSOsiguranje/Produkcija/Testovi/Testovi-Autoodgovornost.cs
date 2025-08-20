

namespace Produkcija
{
    [TestFixture, Order(1)]
    [Parallelizable(ParallelScope.Self)]
    public partial class Autoodgovornost : Osiguranje
    {

        //private static string NacinPokretanjaTesta = Environment.GetEnvironmentVariable("BASE_URL") ?? "ručno";
        //private static string logFilePath = "C:/_Projekti/AutoMotoSavezSrbije/Logovi/test_log.txt";
        //private static string logFilePath = Path.Combine("C:/_Projekti/AutoMotoSavezSrbije/Logovi/", "test_log.txt");

        /**********************************************************
        #region Regex

        [GeneratedRegex("Osiguranje vozila", RegexOptions.IgnoreCase, "sr-Latn-RS")]
        private static partial Regex MyRegexOsiguranjeVozila();

        [GeneratedRegex("Strana za logovanje", RegexOptions.IgnoreCase, "sr-Latn-RS")]
        private static partial Regex MyRegexStranaZaLogovanje();

        [GeneratedRegex("PRIJAVA", RegexOptions.IgnoreCase, "sr-Latn-RS")]
        private static partial Regex MyRegexPrijava();        

        [GeneratedRegex("Dobrodošli u eOsiguranje")]
        private static partial Regex MyRegexDobrodosli();

        #endregion Regex
        **********************************************************/

        #region Testovi

        [Test, Order(101)]
        public async Task AO_01_PregledPretragaPolisa()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                //Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                //Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                //Provera da li se otvorila stranica sa gridom polisa AO
                DodatakNaURL = "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Pregled polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = "KreiranRaskinutStorniranU izradi";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 10, "Lista", 0);

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

                // Pročitaj broj dokumenta (sadržaj ćelije u prvom redu i prvoj koloni)
                string brojDokumenta = await ProcitajCeliju(1, 1);
                // Procitaj serijski broj obrasca polise AO (sadržaj ćelije u prvom redu i drugoj koloni)
                string serijskiBrojObrasca = await ProcitajCeliju(1, 2);
                // Procitaj Broj polise AO (sadržaj ćelije u prvom redu i trećoj koloni)
                string brojPolise = await ProcitajCeliju(1, 3);

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj dokumenta: {brojDokumenta} \n" +
                                                         $"Serijski broj obrasca: {serijskiBrojObrasca} \n" +
                                                         $"Broj polise AO: {brojPolise}", "Informacija", MessageBoxButtons.OK);
                }

                await IzlogujSe(_page);
                // Proveri da li si uspešno izlogovan
                DodatakNaURL = "/Login";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

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

        [Test, Order(102)]
        public async Task AO_02_PregledPolise()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                //Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                //Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                //Provera da li se otvorila stranica sa gridom polisa AO
                DodatakNaURL = "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Pregled polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = "KreiranRaskinutStorniranU izradi";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 10, "Lista", 0);

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

                // Pročitaj broj dokumenta (sadržaj ćelije u prvom redu i prvoj koloni)
                string brojDokumenta = await ProcitajCeliju(1, 1);
                // Procitaj serijski broj obrasca polise AO (sadržaj ćelije u prvom redu i drugoj koloni)
                string serijskiBrojObrasca = await ProcitajCeliju(1, 2);
                // Procitaj Broj polise AO (sadržaj ćelije u prvom redu i trećoj koloni)
                string brojPolise = await ProcitajCeliju(1, 3);

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj dokumenta: {brojDokumenta} \n" +
                                                         $"Serijski broj obrasca: {serijskiBrojObrasca} \n" +
                                                         $"Broj polise AO: {brojPolise}", "Informacija", MessageBoxButtons.OK);
                }
                try
                {
                    // Pronađi lokator za ćeliju koja sadrži broj dokumenta polise AO
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
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju koja ima vrednost '{brojDokumenta}'.", ex);
                }
                // Provera da li se otvorila odgovarajuća polisa AO
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}");

                await ProveriStampu404(_page, "Štampaj polisu", "Štampa polise AO");
                //await ProveriStampuPdf(_page, "Štampaj polisu", "Štampa polise AO");
                await ProveriStampu404(_page, "Štampaj uplatnicu/fakturu", "Štampa uplatnice/fakture polise AO");
                //await ProveriStampuPdf(_page, "Štampaj uplatnicu/fakturu", "Štampa uplatnice/fakture polise AO");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije" }).ClickAsync();
                await ProveriStampu404(_page, "Štampaj prepis polise", "Prepis polise AO");



                await _page.GetByRole(AriaRole.Button, new() { Name = " Pregled zahteva ka UOS-u" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Prethodne verzije" }).ClickAsync();
                await _page.Locator("e-sidenav").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = " Skloni panel" }).ClickAsync();

                //Brisanje polise koja je u izradi
                await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga polisa" }).ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                //Sortiraj po datumu kreiranja
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Datum kreiranja -$") }).Locator("div").Nth(1).ClickAsync();
                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                //Filtriraj "U izradi"
                await _page.GetByText("U izradi").First.ClickAsync();
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                //await _page.Locator(".control-wrapper.field.no-content.info-text-field.focus > .control > .control-main > .input").FillAsync("Davor");
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync("90200");
                //await _page.Locator(".control-wrapper.field.no-content.info-text-field.focus > .control > .control-main > .input").PressAsync("Tab");
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");

                string filtriraniBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();

                // Izbroj koliko ima redova u gridu
                redovi = _page.Locator("//div[@class='podaci']//div[contains(@class, 'grid-row')]");
                brojRedova = await redovi.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Redova ima-1: {brojRedova}", "Informacija", MessageBoxButtons.OK);
                }

                if (brojRedova > 0)
                {
                    // Pročitaj broj dokumenta (sadržaj ćelije u prvom redu i prvoj koloni)
                    brojDokumenta = await ProcitajCeliju(1, 1);

                    await _page.GetByRole(AriaRole.Link, new() { Name = $"{brojDokumenta}" }).First.ClickAsync();
                    await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}");
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                    await _page.GetByRole(AriaRole.Button, new() { Name = " Obriši dokument" }).ClickAsync();
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Da!" }).ClickAsync();
                    await _page.GetByText("Dokument uspešno obrisan").ClickAsync();
                    await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");

                }
                else
                {
                    await LogovanjeTesta.LogException($"{NazivTekucegTesta}", new Exception("Nema polisa AO za brisanje"));
                }

                await IzlogujSe(_page);
                // Proveri da li si uspešno izlogovan
                DodatakNaURL = "/Login";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

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
        public async Task AO_03_BrisanjePolise()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                //Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                //Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                //Provera da li se otvorila stranica sa gridom polisa AO
                DodatakNaURL = "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Pregled polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = "KreiranRaskinutStorniranU izradi";
                await FiltrirajGrid(_page, kriterijumFiltera, tipGrida, 10, "Lista", 0);

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

                // Pročitaj broj dokumenta (sadržaj ćelije u prvom redu i prvoj koloni)
                string brojDokumenta = await ProcitajCeliju(1, 1);
                // Procitaj serijski broj obrasca polise AO (sadržaj ćelije u prvom redu i drugoj koloni)
                string serijskiBrojObrasca = await ProcitajCeliju(1, 2);
                // Procitaj Broj polise AO (sadržaj ćelije u prvom redu i trećoj koloni)
                string brojPolise = await ProcitajCeliju(1, 3);

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj dokumenta: {brojDokumenta} \n" +
                                                         $"Serijski broj obrasca: {serijskiBrojObrasca} \n" +
                                                         $"Broj polise AO: {brojPolise}", "Informacija", MessageBoxButtons.OK);
                }
                try
                {
                    // Pronađi lokator za ćeliju koja sadrži broj dokumenta polise AO
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
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju koja ima vrednost '{brojDokumenta}'.", ex);
                }
                // Provera da li se otvorila odgovarajuća polisa AO
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}");






                //Brisanje polise koja je u izradi
                await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga polisa" }).ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                DodatakNaURL = "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                //Sortiraj po datumu kreiranja
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Datum kreiranja -$") }).Locator("div").Nth(1).ClickAsync();
                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                //Filtriraj "U izradi"
                await _page.GetByText("U izradi").First.ClickAsync();
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                //await _page.Locator(".control-wrapper.field.no-content.info-text-field.focus > .control > .control-main > .input").FillAsync("Davor");
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync("90200");
                //await _page.Locator(".control-wrapper.field.no-content.info-text-field.focus > .control > .control-main > .input").PressAsync("Tab");
                await _page.Locator(".column.column_11 > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");

                string filtriraniBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();

                // Izbroj koliko ima redova u gridu
                redovi = _page.Locator("//div[@class='podaci']//div[contains(@class, 'grid-row')]");
                brojRedova = await redovi.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Redova ima-1: {brojRedova}", "Informacija", MessageBoxButtons.OK);
                }

                if (brojRedova > 0)
                {
                    // Pročitaj broj dokumenta (sadržaj ćelije u prvom redu i prvoj koloni)
                    brojDokumenta = await ProcitajCeliju(1, 1);

                    await _page.GetByRole(AriaRole.Link, new() { Name = $"{brojDokumenta}" }).First.ClickAsync();
                    await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}");
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Dodatne opcije " }).ClickAsync();
                    await _page.GetByRole(AriaRole.Button, new() { Name = " Obriši dokument" }).ClickAsync();
                    await _page.GetByRole(AriaRole.Button, new() { Name = "Da!" }).ClickAsync();
                    await _page.GetByText("Dokument uspešno obrisan").ClickAsync();
                    await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");

                }
                else
                {
                    await LogovanjeTesta.LogException($"{NazivTekucegTesta}", new Exception("Nema polisa AO za brisanje"));
                }

                await IzlogujSe(_page);
                // Proveri da li si uspešno izlogovan
                DodatakNaURL = "/Login";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

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

        [Test]
        public async Task AO_03_SE_PregledPretragaObrazaca()
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
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                // Klik na Pregled / Pretraga obrazaca stroge evidencije
                await _page.GetByText("Pregled / Pretraga obrazaca").ClickAsync();
                // Provera da li se otvorila stranica pregled obrazaca
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-obrazaca");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Obrasci polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = RucnaUloga;
                await ProveriFilterGrida(_page, kriterijumFiltera, tipGrida, 3);

                /***********************************************************************************************
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
                ************************************************************************************************/

                // Pročitaj Serijski broj obrasca polise AO (sadržaj ćelije u prvom redu i prvoj koloni)
                string serijskiBrojObrasca = await ProcitajCeliju(1, 1);
                // Procitaj Broj polise AO (sadržaj ćelije u prvom redu i drugoj koloni)
                string brojPolise = await ProcitajCeliju(1, 2);
                // Procitaj Lokacija obrasca polise AO (sadržaj ćelije u prvom redu i trećoj koloni)
                string lokacijaObrasca = await ProcitajCeliju(1, 3);
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Serijski broj obrasca: {serijskiBrojObrasca} \n" +
                                                         $"Broj polise: {brojPolise} \n" +
                                                         $"Lokacija obrasca: {lokacijaObrasca}", "Informacija", MessageBoxButtons.OK);
                }

                try
                {
                    // Pronađi lokator za ćeliju koja sadrži Serijski broj obrasca polise AO
                    var celijaSerijskiBrojObrasca = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']");

                    // Provera da li je element vidljiv
                    if (await celijaSerijskiBrojObrasca.IsVisibleAsync())
                    {
                        await celijaSerijskiBrojObrasca.ClickAsync();
                        Console.WriteLine($"Kliknuto na ćeliju sa vrednošću: {serijskiBrojObrasca}");
                        //TestLogger.LogMessage($"Kliknuto na ćeliju sa vrednošću: {prom2}");
                    }
                    else
                    {
                        //TestLogger.LogError($"Ćelija sa vrednošću '{prom2}' nije vidljiva.");
                    }
                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{serijskiBrojObrasca}'.", ex);
                }

                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Kartica/{serijskiBrojObrasca}"); // Provera da li se otvorila odgovarajuća kartica obrasca

                try
                {
                    string[] expectedValues = [serijskiBrojObrasca, "Autoodgovornost", lokacijaObrasca, brojPolise];
                    bool allFound = true;

                    foreach (string value in expectedValues)
                    {
                        // Lokator za input element sa očekivanom vrednošću u atributu value

                        var tekstBoks = _page.Locator($"//e-input[@value='{value}']");   //e-input[@value='Autoodgovornost']

                        if (await tekstBoks.CountAsync() == 0)
                        {
                            allFound = false;
                            LogovanjeTesta.LogError($"Nije pronađen tekst boks sa vrednošću: '{value}'");
                        }
                        else
                        {
                            LogovanjeTesta.LogMessage($"Uspešno pronađen tekst boks sa vrednošću: '{value}'");
                        }
                    }

                    if (allFound)
                    {
                        LogovanjeTesta.LogMessage("Svi očekivani tekst boksovi su pronađeni sa tačnim vrednostima.");
                    }
                    else
                    {
                        LogovanjeTesta.LogError("Neki od očekivanih tekst boksova nisu pronađeni.");
                    }
                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException("Greška prilikom provere tekst boksova sa vrednostima prom1–prom4.", ex);
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
                throw;
            }
        }

        [Test]
        public async Task AO_03_SE_PregledPretragaDokumenata()
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
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                // Klik na Pregled / Pretraga dokumenata stroge evidencije
                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                // Provera da li se otvorila stranica pregled dokumenata
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Dokumenta stroge evidencije za polise AO";
                //string lokatorGrida = "//e-grid[@id='grid_dokumenti']";
                await ProveraPostojiGrid(_page, tipGrida);

                string kriterijumFiltera = "Verifikovan";
                await ProveriFilterGrida(_page, kriterijumFiltera, tipGrida, 5);

                /***********************************************************************************************
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
                ************************************************************************************************/

                /* Ovaj kod ne vraća dobar broj kolona, proveriti zašto
                //ili
                var kolone = _page.Locator("//e-grid[@id='grid_obrasci']//div[contains(@class, 'ag-row')][1]//div[contains(@class, 'ag-cell')]");
                int brojKolona1 = await kolone.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj kolona 2: {brojKolona1}", "Informacija", MessageBoxButtons.OK);
                }
                */

                // Pročitaj sadržaj ćelije u prvom redu i prvoj koloni)
                string oznakaDokumenta = await ProcitajCeliju(1, 1);
                // Procitaj Broj polise AO (sadržaj ćelije u prvom redu i drugoj koloni)
                string tipDokumenta = await ProcitajCeliju(1, 2);
                // Procitaj Lokacija obrasca polise AO (sadržaj ćelije u prvom redu i trećoj koloni)
                string razduzujeSe = await ProcitajCeliju(1, 3);
                string zaduzujeSe = await ProcitajCeliju(1, 4);
                string statusDokumenta = await ProcitajCeliju(1, 5);
                string linkDokumenta = await ProcitajCeliju(1, 8);
                string brojDokumenta = linkDokumenta.Substring(linkDokumenta.LastIndexOf('/') + 1);

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Oznaka dokumenta: {oznakaDokumenta} \n" +
                                                         $"Tip dokumenta: {tipDokumenta} \n" +
                                                         $"Razdužuje se: {razduzujeSe} \n" +
                                                         $"Zadužuje se: {zaduzujeSe} \n" +
                                                         $"Status dokumenta: {statusDokumenta} \n" +
                                                         $"Link dokumenta: {linkDokumenta} \n" +
                                                         $"brojDokumenta: {brojDokumenta}", "Informacija", MessageBoxButtons.OK);
                }
                try
                {
                    // Pronađi lokator za ćeliju koja sadrži Serijski broj obrasca polise AO
                    var celijaSerijskiBrojObrasca = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{oznakaDokumenta}']").First;

                    // Provera da li je element vidljiv
                    if (await celijaSerijskiBrojObrasca.IsVisibleAsync())
                    {
                        await celijaSerijskiBrojObrasca.ClickAsync();
                        Console.WriteLine($"Kliknuto na ćeliju sa vrednošću: {oznakaDokumenta}");
                        //TestLogger.LogMessage($"Kliknuto na ćeliju sa vrednošću: {prom2}");
                    }
                    else
                    {
                        //TestLogger.LogError($"Ćelija sa vrednošću '{prom2}' nije vidljiva.");
                    }
                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.", ex);
                }
                //await _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']").ClickAsync();

                await ProveriURL(_page, PocetnaStrana + "/Stroga-Evidencija", linkDokumenta);
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/1/Autoodgovornost/Dokument/2/{brojDokumenta}");// Provera da li se otvorila odgovarajuća kartica obrasca
                string[] expectedValues = { oznakaDokumenta, statusDokumenta, razduzujeSe, zaduzujeSe };
                try
                {

                    bool allFound = true;

                    foreach (string value in expectedValues)
                    {
                        // Lokator za input element sa očekivanom vrednošću u atributu value

                        var tekstBoks = _page.Locator($"//e-input[@value='{value}']");   //e-input[@value='Autoodgovornost']
                        var selectBox = _page.Locator($"//div[contains(@etitle,'{value}')]");   //e-input[@value='Autoodgovornost']
                        if (await tekstBoks.CountAsync() == 0 && await selectBox.CountAsync() == 0)
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
                        LogovanjeTesta.LogError("Neki od očekivanih elemenata nisu pronađeni.");
                    }


                }
                catch (Exception ex)
                {
                    await LogovanjeTesta.LogException($"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.", ex);
                }

                await ProveriStampuPdf(_page, "Štampaj dokument", "Štampa dokumenta Stroge evidencije za AO:");
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
                //throw new Exception("Namerna greška u testu.");
                throw;
            }

        }

        [Test]
        public async Task AO_04_SE_UlazObrazaca()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                long PoslednjiSerijski; //Poslednji iskorišćeni serijski broja obrasca u Strogoj evidenciji
                int PoslednjiDokument; //Poslednji broj dokumenta u Strogoj evidenciji
                string konCifraOd, konCifraDo;
                string oznakaDokumenta;
                string ocekivaniTekst;
                string tipGrida;
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                string Magacin = "Centralni magacin 1"; //Magacin u koji se vrši ulaz u centralni magacin
                if (Okruzenje == "UAT")
                {
                    Magacin = "Centralni magacin"; //Magacin u koji se vrši ulaz u centralni magacin
                }

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.GetByText("Osiguranje vozila").HoverAsync(); //Pređi mišem preko teksta Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync(); //Klikni na tekst Osiguranje vozila
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync(); //Klikni u meniju na Autoodgovornost
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); //Provera da li se otvorila stranica sa pregledom polisa AO

                await Pauziraj(_page);
                // Provera da li stranica sadrži grid Dokumenta - Pregled polisa AO
                tipGrida = "Pregled polisa AO";
                //lokatorGrida = "//e-grid[@id='grid_dokumenti']*";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Pregled / Pretraga obrazaca stroge evidencije
                await _page.GetByText("Pregled / Pretraga obrazaca").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-obrazaca"); // Provera da li se otvorila stranica pregled obrazaca

                // Proveri da li stranica sadrži grid Obrasci i 
                // da li radi filter na gridu Obrasci
                tipGrida = "Obrasci polisa AO";
                //lokatorGrida = "//e-grid[@id='grid_obrasci']*";
                await ProveraPostojiGrid(_page, tipGrida);
                await ProveriFilterGrida(_page, "otpisanih", tipGrida, 3);

                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata"); // Provera da li se otvorila stranica pregled dokumenata
                                                                                                                   // Proveri da li stranica sadrži grid Dokumenta stroge evidencije i                                                                                          // da li radi filter na gridu Dokumenta stroge evidencije
                tipGrida = "Dokumenta stroge evidencije za polise AO";
                //lokatorGrida = "//e-grid[@id='grid_dokumenti']*";
                await ProveraPostojiGrid(_page, tipGrida);
                await ProveriFilterGrida(_page, "Magacin kovnice", tipGrida, 3);

                /************************************************
                Formira se ulaz obrazaca AO u Centralni magacin
                *************************************************/
                //Klik u levom meniju na Novi ulaz u centralni magacin i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");

                // Popuni broj otpremnice (unosi se trenutni datum i vreme) i izaberi ko se zadužuje (Centralni magacin 1)
                await _page.Locator("#inpBrojOtpremice").ClickAsync();
                await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("yyyyMMdd-hhmmss"));

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Magacin, false);

                //Nalaženje poslednjeg iskorišćenog serijskog broja obrasca u Strogoj evidenciji
                PoslednjiSerijski = await PoslednjiSerijskiStroga(1, $" AND [SerijskiBroj] BETWEEN {MinSerijskiAO} AND {MaxSerijskiAO}");
                Console.WriteLine($"Poslednji serijski broj obrasca polise AO na svim okruženjima je: {PoslednjiSerijski}.\n");

                // Unesi broj polise Od i Do i testiraj dodavanje, brisanje i izmenu
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                konCifraOd = IzracunajKontrolnuCifru($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();

                konCifraDo = IzracunajKontrolnuCifru($"{PoslednjiSerijski + 25}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                //Obriši opseg od 25 obrazaca
                await _page.Locator("#btnObrisi button").ClickAsync();

                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);

                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();


                /***************************************************
                Provera snimanja dokumenta Ulaz u centralni magacin
                ***************************************************/
                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                PoslednjiDokument = await PoslednjiDokumentStroga();

                await SnimiDokument(_page, PoslednjiDokument, "ulaz u centralni magacin");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                /***************************************************
                Provera brisanja dokumenta Ulaz u centralni magacin
                ***************************************************/
                await ObrisiDokument(_page, PoslednjiDokument);
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");

                /******************************************
                Formiranje novog ulaza u Centralni magacin
                ******************************************/
                // Popuni broj otpremnice (unosi se trenutni datum i vreme) i izaberi ko se zadužuje (Centralni magacin 1)
                await _page.Locator("#inpBrojOtpremice").ClickAsync();
                await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("yyyyMMdd-hhmmss"));

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Magacin, false);

                //Unesi broj polise Od i Do (unosim ukupno tri polise)
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                konCifraDo = IzracunajKontrolnuCifru($"{PoslednjiSerijski + 3}");
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 3}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                //Snimi dokument Ulaz u centralni magacin
                await SnimiDokument(_page, PoslednjiDokument, "ulaz u centralni magacin");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                //Pročitaj stanje poslednjeg  mejla pred slanje na verifikaciju
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                //Pošalji na verifikaciju
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije AO:");

                //Provera mejla za BO da je dokument poslat na verifikaciju
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl ka BO da ima ulaz za verifikaciju nije poslat");
                //Pročitaj oznaku novog dokumenta
                oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();

                //provara da li vest postoji i radi li link
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");

                /*
                ocekivaniTekst = $"Imate novi dokument \"Ulaz u centralni magacin\" za verifikaciju" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GotoAsync($"{PocetnaStrana}/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");
                */
                await ProveraVestPostoji(_page, oznakaDokumenta);

                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                //Pročitaj novo stanje poslednjeg  mejla pred vraćanje u izradu
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //Klik na dugme Vrati u izradu
                await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();

                //Provera mejla za BO da je dokument vraćen u izradu
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl ka BO da je vraćen u izradu ulaz u Centralni magacinza nije poslat");



                //Provera vesti za BO da je dokument vraćen u izradu

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, oznakaDokumenta);
                //await VestPostoji(_page, "Dokument \"Ulaz u centralni magacin\" je vraćen u izradu. \nDokument možete pogledati klikom na link: ", oznakaDokumenta);



                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                //Sada obriši dokument
                await _page.Locator("#btnObrisiDokument button").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");
                //await _page.PauseAsync(); // Pauza da se vidi da li je 
                // Proveri da li je za BO obrisana vest za verifikaciju jer je dokument obrisan
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                ocekivaniTekst = $"{oznakaDokumenta}";
                await ProveraVestNijeObrisana(_page, ocekivaniTekst);
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);

                /****************************
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                LogovanjeTesta.LogMessage($"❌ Proverava se da li je vest obrisana nakon brisanja dokumenta {oznakaDokumenta}.", false);
                //await ProveraVestPostoji(_page, ocekivaniTekst);
                //await ProveraVestNijeObrisana(_page, ocekivaniTekst);

                //string pageText = await _page.InnerTextAsync("//div[@class='podaci']");
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
                **************************************************/

                /********************************************************
                Napravi novi ulaz u Centralni magacin koji će ići agentu
                ********************************************************/
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");
                await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");
                await _page.Locator("#inpBrojOtpremice").ClickAsync();
                await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("yyyyMMdd-hhmmss"));

                //await UnesiMagacin(_page, "#selZaduzenje");
                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Magacin, false);

                // Unesi broj polise Od i Do
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                konCifraOd = IzracunajKontrolnuCifru($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                konCifraDo = IzracunajKontrolnuCifru($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //Provera mejla za BO da je dokument poslat na verifikaciju
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl ka agentu da ima dokument za verifikaciju nije poslat");


                //Provera da li se vidi novi dokument za verifikaciju u gridu
                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                // Proveri da li stranica sadrži grid Dokumenta - Stroga evidencija
                tipGrida = "Dokumenta stroge evidencije za polise AO";
                await ProveraPostojiGrid(_page, tipGrida);
                await _page.Locator("a").Filter(new() { HasText = $"{oznakaDokumenta}" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");
                LogovanjeTesta.LogMessage($"✅ Otvaranje dokumenta iz grida: {PoslednjiDokument + 1} - {oznakaDokumenta}.", false);
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();

                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran verifikovan dokument stroge evidencije AO:");

                //Provera mejla za BO da je dokument verifikovan
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla,"------------------");







                //Provera vesti za BO da je dokument verifikovan
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                /*******************************************
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je verifikovan.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je verifikovan.Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
                *******************************************/
                //await Pauziraj(_page);

                /**********************************************************************************************
                Prenos ka agentu
                
                Prenos iz Centralnog magacina ka agentu
                
                //Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                await _page.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).ClickAsync();
                //Provera da li se otvorila odgovarajuća stranica
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                await _page.GetByText("Novi prenos").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/0");



                //await UnesiMagacin(_page, "#selRazduzenje");
                await IzaberiOpcijuIzListe(_page, "#selRazduzenje", Magacin, false);

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Asaradnik, false);

                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);

                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();



                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");
                oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();


                 
                 //Provera da li je agent dobio mejl da ima novi prenos obrazaca
                
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla,"------------------");


                // Odjavljuje se BackOffice
                await IzlogujSe(_page);

                // Prijavljuje se agent




                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //await _page.Locator(".ico-ams-logo").ClickAsync();
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");

                
                // Provera vesti o dokumentu za verifikaciju za agenta
                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                //await ProveraVestPostoji(_page, ocekivaniTekst);
                
                // Otvori dokument za verifikaciju
                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await _page.GotoAsync($"{PocetnaStrana}/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");

                
                //Provera da li je BO dobio mejl da je dokument prenos obrazaca vraćen u izradu
                
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();

                await ProveriStatusSlanjaMejla(PrethodniZapisMejla,"------------------");

                //LogovanjeTesta.LogMessage($"❌ Proverava se da li je vest arhivirana za agenta nakon vraćanja u izradu: {oznakaDokumenta}.", false);

                await _page.Locator(".ico-ams-logo").ClickAsync();
                //await ProveraVestJeObrisana(_page, ocekivaniTekst);





                // Odjavljuje se agent
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                // Prijavljuje se BackOffice


                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");



                //Provera vesti o vraćenom dokumentu za BackOffice
                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");

                
                //Provera da li agent ima mejl o novom prenosu
                
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //vrati nazad na verifikaciju
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla,"------------------");

                // Proveri da li je vest arhivirana za BO nakon vraćanja na verifikaciju
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                
                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
                
                //await ProveraVestJeObrisana(_page, ocekivaniTekst);

                // Odjavljuje se BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");




                // Prijavljuje se agent


                await UlogujSe(_page, AkorisnickoIme, Alozinka);

                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");




                // Provera vesti o dokumentu za verifikaciju za agenta 
                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju\nDokument možete pogledati klikom na link: {oznakaDokumenta}";
                //await ProveraVestPostoji(_page, ocekivaniTekst);

                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                //await _page.GetByText($"Imate novi dokument za verifikacijuDokument možete pogledati klikom na link: {PoslednjiDokument + 2}").ClickAsync();
                //await _page.GetByText($"{PoslednjiDokument + 2}").ClickAsync();
                await _page.GotoAsync($"{PocetnaStrana}/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");

                
                //Provera da li BO dobio mejl da je prenos verifikovan
                
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //await _page.PauseAsync();
                //if (IdLice == 1002)
                //await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();

                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran verifikovan dokument stroge evidencije AO:");
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla,"------------------");

                
                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju\n" +
                                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");

                //await ProveraVestJeObrisana(_page, ocekivaniTekst);
                
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");



                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");


                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je verifikovan.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);





*****************************************************************************************/
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");









                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }


            //await _page.Locator(".ico-ams-logo").ClickAsync();

            //await _page.Locator(".korisnik").ClickAsync();
            //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();


            //return;


            /******************************************************************************
                        
                        Console.WriteLine("\nPrekidam program ***************************************************\n");
                        return;










                        
                        return;
                        // Pročitaj trenutnu vrednost u prikazu broja strane
                        string selector = ".futerRowsDesc";
                        var trenutnaVrednostBrojaca = await _page.Locator(selector).InnerTextAsync();
                        Console.WriteLine($"Trenutna vrednost je {trenutnaVrednostBrojaca}");

                        // Popuni i pokreni filter koji bira obrasce koje duži Mandaric
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").ClickAsync();
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").FillAsync("Mandari");
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").PressAsync("Enter");

                        // Sačekaj dok vrednost ne postane drugačija
                        //await _page.WaitForFunctionAsync($"selector => document.querySelector(selector).innerText !== '{trenutnaVrednostBrojaca}'", new { selector });
                        string escapedSelector = selector.Replace("'", "\\'");
                        await _page.WaitForFunctionAsync(
                            $"document.querySelector('{escapedSelector}') && document.querySelector('{escapedSelector}').innerText !== '{trenutnaVrednostBrojaca}'");


                        // Proveri novu vrednost
                        var novaVrednost = await _page.Locator(selector).InnerTextAsync();
                        Console.WriteLine($"Vrednost se promenila sa '{trenutnaVrednostBrojaca}' na '{novaVrednost}'");


                        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čeka dok se DOM ne učita
                        await _page.WaitForLoadStateAsync(LoadState.Load); // Čeka da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
                        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu

                        // Broj zapisa (redova) u gridu
                        var rowsGrid = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                        Console.WriteLine($"Redova u zaglavlju ima: {rowsGrid.Count}. To je broj obrazaca kod korisnika: Bogdan Mandarić.\n");



                        // Proveri u bazi koliko zaduženih obrazaca ima Bogdan Mandarić
                        string qBrojZaduzenihObrazacaBogdan = "SELECT COUNT (*) FROM [StrictEvidenceDB].[strictevidence].[tObrasci] WHERE [IdTipObrasca] = 1 AND [IDLokacijaZaduzena] IN (7, 8) AND [_DatumDo] IS NULL;";
                        // Konekcija sa bazom StrictEvidenceDB
                        Server = Okruzenje switch
                        {
                            "Razvoj" => "10.5.41.99",
                            "Proba2" => "49.13.25.19",
                            "UAT" => "10.41.5.5",
                            "Produkcija" => "",
                            _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                        };
                        //string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                        //string brojZaduzenihObrazaca = IzvrsiUpit(connectionStringStroga, qBrojZaduzenihObrazacaBogdan);
                        //Console.WriteLine($"U bazi je broj zaduženih obrazaca: {brojZaduzenihObrazaca}. To je broj obrazaca kod korisnika Bogdan Mandarić.\n");

                        trenutnaVrednostBrojaca = await _page.Locator(selector).InnerTextAsync();
                        string[] parts = trenutnaVrednostBrojaca.Split(new[] { "od " }, StringSplitOptions.None);
                        string z = parts.Length > 1 ? parts[1].Trim() : string.Empty;
                        Console.WriteLine($"Brojač pokazuje ukupno {z} zapisa /broj zaduženih polisa).");






                        string connectionStringMtpl = $"Server = {Server}; Database = MtplDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                        using SqlConnection konekcija = new(connectionStringMtpl);
                        Console.WriteLine($"Initial State konekcije sa MtplDB: {konekcija.State}");
                        konekcija.Open();
                        Console.WriteLine($"State after opening: {konekcija.State}");
                        Console.WriteLine("Uspostavljena je veza sa bazom MtplDB!\n");

                        foreach (var row in rowsGrid)
                        {
                            bool ObrazacSlobodan;
                            var _SerijskiBrojObrasca = await row.QuerySelectorAsync($"div.column_1");
                            string SerijskiBrojObrasca = await _SerijskiBrojObrasca.EvaluateAsync<string>("el => el.innerText");

                            string KontrolnaCifra = IzracunajKontrolnuCifru(SerijskiBrojObrasca);
                            string SerijskiBrojObrascaKC = SerijskiBrojObrasca + KontrolnaCifra;

                            //Proveri da li je obrazac negde upotrebljen
                            string qStanjeZaduzenogObrasca = $"SELECT [idDokument] FROM [MtplDB].[mtpl].[DokumentPodaci] WHERE [serijskiBrojAO] = '{SerijskiBrojObrascaKC}';";
                            SqlCommand command = new(qStanjeZaduzenogObrasca, konekcija);
                            var rezultat = command.ExecuteScalar();
                            if (rezultat != null)
                            {
                                ObrazacSlobodan = false;
                            }
                            else
                            {
                                ObrazacSlobodan = true;
                            }



                            Console.WriteLine($"{SerijskiBrojObrasca}, kontrolna cifra: {KontrolnaCifra}, {SerijskiBrojObrascaKC}, Obrazac je slobodan: {ObrazacSlobodan}");
                            if (ObrazacSlobodan == true)
                            {
                                string SeriskiBrojPoliseAO = SerijskiBrojObrascaKC;
                                Console.WriteLine($"{SeriskiBrojPoliseAO}");
                                //break;
                            }



                        }

                        konekcija.Close();
                        Console.WriteLine($"State after closing: {konekcija.State}.\n");










                        await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                        await _page.Locator("//e-input[@id='inpBrojOtpremice']").ClickAsync();
                        await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("ddMMyyyy-HHmmss"));

                        await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();

                        await _page.Locator("#selZaduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await _page.GetByText("Centralni magacin 1").ClickAsync();
                        await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync("");
                        await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                        await _page.Locator("#inpBrojOtpremice input[type=\"text\"]").ClickAsync();

            ********************************************************************************/
            /*
            while (await reader.ReadAsync())
            {
                string SerijskiBroj = reader["serijskiBroj"].ToString();
                string IdTipObrasca = reader["IdTipObrasca"].ToString();
                string IDLokacijaZaduzena = reader["IDLokacijaZaduzena"].ToString();

                // Dodajte podatke u listu
                results.Add((SerijskiBroj, IdTipObrasca, IDLokacijaZaduzena));

                // Prikaz u konzoli
                Console.WriteLine($"Podatak1: {SerijskiBroj}, Podatak2: {IdTipObrasca}, Podatak3: {IDLokacijaZaduzena}");
            }
            string csvFilePath = "C:\\_Projekti\\AutoMotoSavezSrbije\\Podaci\\IzlazniPodaci\\izlazni_fajl.csv";
            WriteToCsv(csvFilePath, results);

            //int brojIzdatihPolisaAO = 0; // ovde hoću da uzmem vrednost koju vraća upit
            */
        }



        [Test]
        public async Task AO_05_SE_PrenosObrazaca()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                //long PoslednjiSerijski; //Poslednji iskorišćeni serijski broja obrasca u Strogoj evidenciji
                int PoslednjiDokument; //Poslednji broj dokumenta u Strogoj evidenciji
                long PrviSlobodanObrazac;
                string konCifraOd, konCifraDo;
                string oznakaDokumenta;
                string ocekivaniTekst;
                string tipGrida;
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                string Magacin = "Centralni magacin 1"; //Magacin u koji se vrši ulaz u centralni magacin
                if (Okruzenje == "UAT")
                {
                    Magacin = "Centralni magacin"; //Magacin u koji se vrši ulaz u centralni magacin
                }

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.GetByText("Osiguranje vozila").HoverAsync(); //Pređi mišem preko teksta Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync(); //Klikni na tekst Osiguranje vozila
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync(); //Klikni u meniju na Autoodgovornost
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); //Provera da li se otvorila stranica sa pregledom polisa AO

                //await Pauziraj(_page);
                // Provera da li stranica sadrži grid Dokumenta - Pregled polisa AO
                tipGrida = "Pregled polisa AO";
                //lokatorGrida = "//e-grid[@id='grid_dokumenti']*";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Pregled / Pretraga obrazaca stroge evidencije
                await _page.GetByText("Pregled / Pretraga obrazaca").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-obrazaca"); // Provera da li se otvorila stranica pregled obrazaca

                // Proveri da li stranica sadrži grid Obrasci i 
                // da li radi filter na gridu Obrasci
                tipGrida = "Obrasci polisa AO";
                //lokatorGrida = "//e-grid[@id='grid_obrasci']*";
                await ProveraPostojiGrid(_page, tipGrida);
                await ProveriFilterGrida(_page, "otpisanih", tipGrida, 3);

                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata"); // Provera da li se otvorila stranica pregled dokumenata
                                                                                                                   // Proveri da li stranica sadrži grid Dokumenta stroge evidencije i                                                                                          // da li radi filter na gridu Dokumenta stroge evidencije
                tipGrida = "Dokumenta stroge evidencije za polise AO";
                //lokatorGrida = "//e-grid[@id='grid_dokumenti']*";
                await ProveraPostojiGrid(_page, tipGrida);
                await ProveriFilterGrida(_page, "Magacin kovnice", tipGrida, 3);

                /************************************************
                Formira se prenos obrazaca AO ka agentu
                *************************************************/
                //Klik u levom meniju na Novi prenos i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi prenos").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/0");



                await IzaberiOpcijuIzListe(_page, "#selRazduzenje", Magacin, false);

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Asaradnik, false);



                //Nalaženje prvog serijskog broja obrasca u Centralnom magacinu

                string qPrviSlobodanUCM = $"SELECT MIN([SerijskiBroj]) AS PrviSlobodanUCentralnomMagacinu " +
                                          $"FROM (" +
                                                  $"SELECT [SerijskiBroj] " +
                                                  $"FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                                  $"WHERE [IdTipObrasca] = 1 AND [SerijskiBroj] BETWEEN {MinSerijskiAO} AND {MaxSerijskiAO} " +
                                                  $"GROUP BY [SerijskiBroj] " +
                                                  $"HAVING MAX([IDLokacijaZaduzena]) = 0 AND MIN([IDLokacijaZaduzena]) = 0" +
                                                  $") AS Podskup;";

                Server = OdrediServer(Okruzenje);
                string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qPrviSlobodanUCM, konekcija))
                    {
                        PrviSlobodanObrazac = (long)command.ExecuteScalar();
                    }
                    konekcija.Close();
                }
                if (NacinPokretanjaTesta == "ručno")
                    System.Windows.MessageBox.Show($"Slobodan obrazac je #{PrviSlobodanObrazac}# ", "Informacija", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Information);


                //PoslednjiSerijski = PoslednjiSerijskiStroga(1, $" AND [SerijskiBroj] BETWEEN {MinSerijskiAO} AND {MaxSerijskiAO}");
                //Console.WriteLine($"Poslednji serijski broj obrasca polise AO na svim okruženjima je: {PoslednjiSerijski}.\n");

                // Unesi broj polise Od i Do i testiraj dodavanje, brisanje i izmenu
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                konCifraOd = IzracunajKontrolnuCifru($"{PrviSlobodanObrazac}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();

                konCifraDo = IzracunajKontrolnuCifru($"{PrviSlobodanObrazac + 24}");

                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                var notifyPopupOpseg = _page.Locator("//div[contains(.,'nisu zaduženi')]");

                try
                {
                    await notifyPopupOpseg.WaitForAsync(new LocatorWaitForOptions { Timeout = 3000 });

                    // Ako se pojavi, zatvori ga i popuni polja
                    await _page.Locator("#notify0 button").ClickAsync();
                    await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                    await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);

                    // Ponovo klikni na dugme
                    await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                }
                catch (TimeoutException)
                {
                    // Popup se nije pojavio u roku — ništa ne radiš, nastavljaš dalje
                }
                /**********
                await _page.WaitForTimeoutAsync(1500); // Pauza da popup eventualno stigne
                // Provera da li je vidljiv
                bool isPopupVisibleOpseg = await notifyPopupOpseg.IsVisibleAsync();
                if (isPopupVisibleOpseg)
                {
                    await _page.Locator("#notify0 button").ClickAsync();
                    //Console.WriteLine("Iskačući prozor za Saobraćajnu je vidljiv. Uneti su podaci ručno.");
                    await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                    await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                    await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                }


                ******************************/

                //await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PoslednjiSerijski + 1}");
                // await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                //await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();
                //await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);

                //await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();


                /***************************************************
                Provera snimanja dokumenta Prenos obrasca
                ***************************************************/
                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                PoslednjiDokument = await PoslednjiDokumentStroga();

                await SnimiDokument(_page, PoslednjiDokument, "Novi prenos");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 1}");

                /***************************************************
                Provera brisanja dokumenta Ulaz u centralni magacin
                ***************************************************/
                await ObrisiDokument(_page, PoslednjiDokument);
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/0");

                /******************************************
                Formiranje novog prenosa obrazaca
                ******************************************/
                await IzaberiOpcijuIzListe(_page, "#selRazduzenje", Magacin, false);

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Asaradnik, false);



                //Unesi broj obrasca Od i Do (unosim ukupno jedan obrazac)
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                konCifraDo = IzracunajKontrolnuCifru($"{PrviSlobodanObrazac}");
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{PrviSlobodanObrazac}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                //Snimi dokument Prenos obrazaca
                await SnimiDokument(_page, PoslednjiDokument, "ulaz u centralni magacin");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 1}");


                //Pročitaj stanje poslednjeg  mejla pred slanje na verifikaciju
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                //Pošalji na verifikaciju
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //Provera mejla za BO da je dokument poslat na verifikaciju
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije AO:");


                //Pročitaj oznaku novog dokumenta
                oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();

                //provara da li vest postoji i radi li link
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");



                // Odjavljuje se BackOffice
                await IzlogujSe(_page);

                // Prijavljuje se agent
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");










                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                //await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 1}");


                //Pročitaj novo stanje poslednjeg  mejla pred vraćanje u izradu
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //Klik na dugme Vrati u izradu
                await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();

                //Provera mejla za BO da je dokument vraćen u izradu
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");


                //Provera da li je vest obrisana za agenta

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");


                bool tekstPrisutan = await _page.Locator($"text={oznakaDokumenta}").IsVisibleAsync();

                if (tekstPrisutan)
                {
                    throw new Exception($"Tekst '{oznakaDokumenta}' je pronađen na stranici — test neuspešan.");
                }





                // Odjavljuje se agent
                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                // Prijavljuje se BackOffice
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");

                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je vraćen u izradu." +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                //await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 1}");

                //Provera mejla za agenta da je dokument vraćen u izradu
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //Provera mejla za BO da je dokument poslat na verifikaciju
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");


                //Provera da li je vest obrisana za BO

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");


                tekstPrisutan = await _page.Locator($"text={oznakaDokumenta}").IsVisibleAsync();

                if (tekstPrisutan)
                {
                    throw new Exception($"Tekst '{oznakaDokumenta}' je pronađen na stranici — test neuspešan.");
                }




                // Odjavljuje se BO
                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                // Prijavljuje se agent
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");


                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju" +
                                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                //await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 1}");

                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //Provera mejla za BO da je dokument verifikovan
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran verifikovan dokument stroge evidencije AO:");


                //Provera da li je vest obrisana za agenta
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");


                tekstPrisutan = await _page.Locator($"text={oznakaDokumenta}").IsVisibleAsync();

                if (tekstPrisutan)
                {
                    throw new Exception($"Tekst '{oznakaDokumenta}' je pronađen na stranici — test neuspešan.");
                }

                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                // Prijavljuje se BO
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");


                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je verifikovan." +
                                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";

                //await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.HoverAsync();
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
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }


            //await _page.Locator(".ico-ams-logo").ClickAsync();

            //await _page.Locator(".korisnik").ClickAsync();
            //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();


            //return;


            /******************************************************************************
                        
                        Console.WriteLine("\nPrekidam program ***************************************************\n");
                        return;










                        
                        return;
                        // Pročitaj trenutnu vrednost u prikazu broja strane
                        string selector = ".futerRowsDesc";
                        var trenutnaVrednostBrojaca = await _page.Locator(selector).InnerTextAsync();
                        Console.WriteLine($"Trenutna vrednost je {trenutnaVrednostBrojaca}");

                        // Popuni i pokreni filter koji bira obrasce koje duži Mandaric
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").ClickAsync();
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").FillAsync("Mandari");
                        await _page.Locator("//div[@class='column column_3 font08 ']//input[@class='input']").PressAsync("Enter");

                        // Sačekaj dok vrednost ne postane drugačija
                        //await _page.WaitForFunctionAsync($"selector => document.querySelector(selector).innerText !== '{trenutnaVrednostBrojaca}'", new { selector });
                        string escapedSelector = selector.Replace("'", "\\'");
                        await _page.WaitForFunctionAsync(
                            $"document.querySelector('{escapedSelector}') && document.querySelector('{escapedSelector}').innerText !== '{trenutnaVrednostBrojaca}'");


                        // Proveri novu vrednost
                        var novaVrednost = await _page.Locator(selector).InnerTextAsync();
                        Console.WriteLine($"Vrednost se promenila sa '{trenutnaVrednostBrojaca}' na '{novaVrednost}'");


                        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čeka dok se DOM ne učita
                        await _page.WaitForLoadStateAsync(LoadState.Load); // Čeka da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
                        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu

                        // Broj zapisa (redova) u gridu
                        var rowsGrid = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                        Console.WriteLine($"Redova u zaglavlju ima: {rowsGrid.Count}. To je broj obrazaca kod korisnika: Bogdan Mandarić.\n");



                        // Proveri u bazi koliko zaduženih obrazaca ima Bogdan Mandarić
                        string qBrojZaduzenihObrazacaBogdan = "SELECT COUNT (*) FROM [StrictEvidenceDB].[strictevidence].[tObrasci] WHERE [IdTipObrasca] = 1 AND [IDLokacijaZaduzena] IN (7, 8) AND [_DatumDo] IS NULL;";
                        // Konekcija sa bazom StrictEvidenceDB
                        Server = Okruzenje switch
                        {
                            "Razvoj" => "10.5.41.99",
                            "Proba2" => "49.13.25.19",
                            "UAT" => "10.41.5.5",
                            "Produkcija" => "",
                            _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                        };
                        //string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                        //string brojZaduzenihObrazaca = IzvrsiUpit(connectionStringStroga, qBrojZaduzenihObrazacaBogdan);
                        //Console.WriteLine($"U bazi je broj zaduženih obrazaca: {brojZaduzenihObrazaca}. To je broj obrazaca kod korisnika Bogdan Mandarić.\n");

                        trenutnaVrednostBrojaca = await _page.Locator(selector).InnerTextAsync();
                        string[] parts = trenutnaVrednostBrojaca.Split(new[] { "od " }, StringSplitOptions.None);
                        string z = parts.Length > 1 ? parts[1].Trim() : string.Empty;
                        Console.WriteLine($"Brojač pokazuje ukupno {z} zapisa /broj zaduženih polisa).");






                        string connectionStringMtpl = $"Server = {Server}; Database = MtplDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                        using SqlConnection konekcija = new(connectionStringMtpl);
                        Console.WriteLine($"Initial State konekcije sa MtplDB: {konekcija.State}");
                        konekcija.Open();
                        Console.WriteLine($"State after opening: {konekcija.State}");
                        Console.WriteLine("Uspostavljena je veza sa bazom MtplDB!\n");

                        foreach (var row in rowsGrid)
                        {
                            bool ObrazacSlobodan;
                            var _SerijskiBrojObrasca = await row.QuerySelectorAsync($"div.column_1");
                            string SerijskiBrojObrasca = await _SerijskiBrojObrasca.EvaluateAsync<string>("el => el.innerText");

                            string KontrolnaCifra = IzracunajKontrolnuCifru(SerijskiBrojObrasca);
                            string SerijskiBrojObrascaKC = SerijskiBrojObrasca + KontrolnaCifra;

                            //Proveri da li je obrazac negde upotrebljen
                            string qStanjeZaduzenogObrasca = $"SELECT [idDokument] FROM [MtplDB].[mtpl].[DokumentPodaci] WHERE [serijskiBrojAO] = '{SerijskiBrojObrascaKC}';";
                            SqlCommand command = new(qStanjeZaduzenogObrasca, konekcija);
                            var rezultat = command.ExecuteScalar();
                            if (rezultat != null)
                            {
                                ObrazacSlobodan = false;
                            }
                            else
                            {
                                ObrazacSlobodan = true;
                            }



                            Console.WriteLine($"{SerijskiBrojObrasca}, kontrolna cifra: {KontrolnaCifra}, {SerijskiBrojObrascaKC}, Obrazac je slobodan: {ObrazacSlobodan}");
                            if (ObrazacSlobodan == true)
                            {
                                string SeriskiBrojPoliseAO = SerijskiBrojObrascaKC;
                                Console.WriteLine($"{SeriskiBrojPoliseAO}");
                                //break;
                            }



                        }

                        konekcija.Close();
                        Console.WriteLine($"State after closing: {konekcija.State}.\n");










                        await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                        await _page.Locator("//e-input[@id='inpBrojOtpremice']").ClickAsync();
                        await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("ddMMyyyy-HHmmss"));

                        await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();

                        await _page.Locator("#selZaduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await _page.GetByText("Centralni magacin 1").ClickAsync();
                        await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync("");
                        await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                        await _page.Locator("#inpBrojOtpremice input[type=\"text\"]").ClickAsync();

            ********************************************************************************/
            /*
            while (await reader.ReadAsync())
            {
                string SerijskiBroj = reader["serijskiBroj"].ToString();
                string IdTipObrasca = reader["IdTipObrasca"].ToString();
                string IDLokacijaZaduzena = reader["IDLokacijaZaduzena"].ToString();

                // Dodajte podatke u listu
                results.Add((SerijskiBroj, IdTipObrasca, IDLokacijaZaduzena));

                // Prikaz u konzoli
                Console.WriteLine($"Podatak1: {SerijskiBroj}, Podatak2: {IdTipObrasca}, Podatak3: {IDLokacijaZaduzena}");
            }
            string csvFilePath = "C:\\_Projekti\\AutoMotoSavezSrbije\\Podaci\\IzlazniPodaci\\izlazni_fajl.csv";
            WriteToCsv(csvFilePath, results);

            //int brojIzdatihPolisaAO = 0; // ovde hoću da uzmem vrednost koju vraća upit
            */
        }



        private static IEnumerable<string[]> ProcitajPodatkeZaPolisuAutoodgovornost()
        {
            string IzvorPodataka;
            if (NacinPokretanjaTesta == "ručno")
            {
                IzvorPodataka = ProjektFolder + "/Podaci/UlazniPodaci/PoliseAutoodgovornost-hand.csv";
            }
            else
            {
                IzvorPodataka = ProjektFolder + "/Podaci/UlazniPodaci/PoliseAutoodgovornost-auto.csv";
            }
            // Čitanje podataka iz CSV fajla
            //string[] lines = File.ReadAllLines(Variables.FilePath);
            string[] lines = File.ReadAllLines(IzvorPodataka);

            // Preskakanje prvog reda (linije)
            bool skipFirstLine = true;

            foreach (string line in lines)
            {
                // Preskače prvi red (liniju)
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                    continue;
                }
                string[] parts = line.Split(';');
                yield return parts;
            }
        }

        [Test]
        [TestCaseSource(nameof(ProcitajPodatkeZaPolisuAutoodgovornost))]
        public async Task AO_06_Polisa(string _rb, string _tipPolise, string _premijskaGrupa, string _popusti, string _tipUgovaraca, string _tipLica1, string _tipLica2, string _brojDana, string _tegljac, string _podgrupa, string _zapremina, string _snaga, string _nosivost, string _brojMesta, string _oslobodjenPoreza, string _mb1, string _mb2, string _pib1, string _pib2, string _platilac1, string _platilac2)
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                await _page.FocusAsync("body");
                //await _page.ReloadAsync(new PageReloadOptions { WaitUntil = WaitUntilState.Load });
                await _page.EvaluateAsync("location.reload(true);");

                //await _page.Locator("body").PressAsync("ControlOrMeta+F5");
                //await _page.Keyboard.PressAsync("Control+F5");
                //await _page.Keyboard.DownAsync("Control");
                //await _page.Keyboard.DownAsync("F5");
                //await _page.Keyboard.UpAsync("F5");
                //await _page.Keyboard.UpAsync("Control");

                await NovaPolisa(_page, "Nova polisa AO");
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");

                //await ProveriStanjeKontrole(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");

                //await ProveriPadajucuListu(_page, "[id*='selPartner']");
                //await ProveriStanjeKontrole(_page, "[id*='selPartner']");
                //MessageBox.Show($"Za Partnera vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                //if (StanjeKontrole == 0)
                //{
                //await ProveriPartnera(_page);
                //}

                await ProveriPadajucuListu(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");
                await ProveriPadajucuListu(_page, "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']");

                //await ProveriStanjeKontrole(_page, "[id*='selTarife']");
                //MessageBox.Show($"Za Tarifu vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                //if (StanjeKontrole == 0)
                //{
                //await ProveriTarifu(_page);
                //}
                if (_rb == "01" || _rb == "19" || _rb == "31")
                {
                    //await ProveriPrethodnuPolisu(_page);
                    await ObradiPomoc(_page, "#inpSerijskiBrojAO-help");
                    await ObradiPomoc(_page, "#selOsiguranik-help");
                }



                #region OSNOVNI PODACI
                #region Serijski broj obrasca polise

                // Pronađi prvi slobodan serijski broj za polisu AO
                /*
                 Server = Okruzenje switch
                 {
                     "Razvoj" => "10.5.41.99",
                     "Proba2" => "49.13.25.19",
                     "UAT" => "10.41.5.5",
                     "Produkcija" => "",
                     _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                 };
                 */
                Server = OdrediServer(Okruzenje);

                string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                string Lokacija = "(7, 8)";

                if (AkorisnickoIme == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme);

                if (NacinPokretanjaTesta == "automatski" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }


                /*
                            if (OsnovnaUloga == "BackOffice")
                            {
                                Lokacija = "(4)";
                            }
                            */
                //Nalaženje poslednjeg serijskog broja koji je u sistemu
                string qZaduženiObrasciAOBezPolise = $"SELECT [tObrasci].[SerijskiBroj] FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                                     $"LEFT JOIN [StrictEvidenceDB].[strictevidence].[tIzdatePolise] ON [tObrasci].[SerijskiBroj] = [tIzdatePolise].[SerijskiBroj] " +
                                                     $"WHERE [tObrasci].[IdTipObrasca] = 1 AND [IDLokacijaZaduzena] IN {Lokacija} AND [IDLokacijaZaduzena] NOT IN (-2, -1, 5) AND [_DatumDo] IS NULL AND [tIzdatePolise].[SerijskiBroj] IS NULL " +
                                                     $"ORDER BY [tObrasci].[SerijskiBroj] ASC;";
                int SerijskiBrojAO = 0;
                try
                {
                    using SqlConnection konekcija = new(connectionString);
                    konekcija.Open();
                    using SqlCommand cmd = new SqlCommand(qZaduženiObrasciAOBezPolise, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();

                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        SerijskiBrojAO = Convert.ToInt32(reader["SerijskiBroj"]);
                        string qSerijskiBrojAONijeUpotrebljen = $"SELECT COUNT (*) FROM [MtplDB].[mtpl].[Dokument] " +
                                                                $"INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] " +
                                                                $"WHERE [idProizvod] = 1 AND [serijskiBrojAO] LIKE '{SerijskiBrojAO}%';";

                        using SqlConnection konekcija2 = new(connectionString);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new(qSerijskiBrojAONijeUpotrebljen, konekcija2);
                        int imaPolisuAO = (int)(cmd2.ExecuteScalar() ?? 0);
                        Console.WriteLine($"Serijski brojevi obrazaca AO kod Bogdana: {SerijskiBrojAO}, Ima polisu AO: {imaPolisuAO}");

                        // Izbor slobodnog serijskog broja polise AO
                        if (imaPolisuAO == 0)
                        {
                            Console.WriteLine($"Prvi obrazac koji je slobodan je: {SerijskiBrojAO}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");
                            break;
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }

                string strSerijskiBrojAO = SerijskiBrojAO.ToString();
                string kontrolnaCifra = IzracunajKontrolnuCifru(strSerijskiBrojAO);
                Console.WriteLine($"\nKreiraću polisu AO za obrazac br: {SerijskiBrojAO + kontrolnaCifra}\n");

                // Unošenje serijskog broja polise AO
                await _page.GetByText("Serijski broj polise").ClickAsync();
                await _page.Locator("#inpSerijskiBrojAO").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("#inpSerijskiBrojAO").GetByRole(AriaRole.Textbox).FillAsync(SerijskiBrojAO + kontrolnaCifra);
                await _page.Locator("#inpSerijskiBrojAO").GetByRole(AriaRole.Textbox).PressAsync("Tab");
                //Provera unetog serijskog broja preko poruke u labeli ispod tekst boksa
                var labela = _page.Locator("//e-input[@id='inpSerijskiBrojAO']//label[contains(@class,'info-text')]");
                await labela.ClickAsync();
                // Čitanje textContent svojstva
                string labelaText = await labela.EvaluateAsync<string>("el => el.textContent");
                if (labelaText != "")
                {
                    //System.Windows.Forms.MessageBox.Show($"Text Content elementa je: {labelaText}", "Informacija", MessageBoxButtons.OK);
                    Assume.That(labelaText, Is.EqualTo(""), $"Test se preskače jer: {labelaText}. \n");
                }
                //Assert.That(labelaText, Is.EqualTo(""), $"Test se preskače jer: {labelaText}. \n");
                //Assert.Ignore($"Test se preskače jer: {labelaText}.");

                #endregion Serijski broj obrasca polise

                await UnesiTipPolise(_page, _tipPolise);

                #region  Mesto izdavanja
                if (_rb == "01" || _rb == "19" || _rb == "31")
                {
                    await _page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("1234");
                    await _page.Locator("#selMestaIzdavanja").GetByText("- Bački Jarak").ClickAsync();
                }
                #endregion  Mesto izdavanja

                #region Datum i vreme

                #region Broj dana
                if (_tipPolise != "Regularna")
                {
                    await _page.Locator("#inpBrDana").GetByRole(AriaRole.Textbox).ClickAsync();
                    await _page.Locator("#inpBrDana").GetByRole(AriaRole.Textbox).FillAsync(_brojDana);
                }
                #endregion Broj dana

                await DatumOd(_page, "#cal_calDatumOd");
                await _page.Locator("#inpVremeOd").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("#inpVremeOd").GetByRole(AriaRole.Textbox).FillAsync("23:50");
                #endregion Datum i vreme

                #endregion OSNOVNI PODACI

                #region LICA NA POLISI

                #region Izbor Tipa ugovarača 
                // Ako polisa nije granična unesi tip ugovarača
                if (_tipPolise != "Granično osiguranje")
                {

                    //await _page.GetByText("---Lizing kuća (finansijski lizing)Preduzetnička radnja ---").ClickAsync();
                    //////////////////////////await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //////////////////////////await _page.Locator("#selOsiguranik div").Filter(new() { HasTextRegex = new Regex("^---$") }).ClickAsync();
                    //////////////////////////await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.GetByText("---Lizing kuća (finansijski lizing)Preduzetnička radnja ---").ClickAsync();
                    //await _page.GetByText("Lizing kuća (finansijski").ClickAsync();
                    //await _page.Locator("#selOsiguranik div").Filter(new() { HasTextRegex = new Regex("^Lizing kuća (finansijski lizing)$") }).ClickAsync();
                    //////////////////////////await _page.GetByText("Lizing kuća (finansijski lizing)").ClickAsync();
                    //await _page.GetByText("---Lizing kuća (finansijski lizing)Preduzetnička radnja Lizing kuća (").ClickAsync();
                    //////////////////////////////await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    ////////////////////////////await _page.GetByText("Preduzetnička radnja").ClickAsync();
                    ////////////////////////////await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    ///////////////////////////await _page.Locator("#selOsiguranik div").Filter(new() { HasTextRegex = new Regex("^---$") }).ClickAsync();

                    //await _page.GetByText("Lica na polisi").ClickAsync();
                    if (_tipUgovaraca != "---")
                    {
                        await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        //await _page.Locator("#selOsiguranik div").Filter(new() { HasTextRegex = new Regex($"^{_tipUgovaraca}$") }).First.ClickAsync();
                        await _page.GetByText($"{_tipUgovaraca}").First.ClickAsync();
                    }

                    /*
                    await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    
                    await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^---$") }).ClickAsync();
                    await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText("Lizing kuća (finansijski lizing)").ClickAsync();
                    await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText("Preduzetnička radnja").ClickAsync();
                    await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText($"{_tipUgovaraca}").First.ClickAsync();
                    */
                    //await _page.Locator("div.option.selected").GetByText($"{_tipUgovaraca}").ClickAsync();


                    //await _page.GetByText("Lizing kuća").ClickAsync();
                    //await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.GetByText("Preduzetnička radnja").ClickAsync();
                    //await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.GetByText("----").ClickAsync();
                    //await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^" + _tipUgovaraca + "$") }).ClickAsync();
                    //await _page.Locator("div").Filter(new() { HasText = _tipUgovaraca }).ClickAsync();
                }
                #endregion Izbor Tipa ugovarača

                #region Oslobođen poreza

                await UnesiPorez(_page, _oslobodjenPoreza);

                #endregion Oslobođen poreza



                #region Tip lica

                if (_tipPolise == "Granično osiguranje")
                {
                    await _page.Locator("#selTipLicaU > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#ugovarac div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica1}$") }).ClickAsync();
                }
                else if (_tipUgovaraca == "Vlasnik je lizing kuća")
                {
                    _tipLica1 = "Pravno";
                    //await _page.Locator("#selTipLicaU > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.Locator("#ugovarac div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica1}$") }).ClickAsync();
                    await _page.Locator("#selTipLicaK > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#korisnik div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica2}$") }).ClickAsync();
                }
                else if (_tipUgovaraca == "Vlasnik nije korisnik")
                {
                    await _page.Locator("#selTipLicaU > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#ugovarac div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica1}$") }).ClickAsync();
                    await _page.Locator("#selTipLicaK > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#korisnik div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica2}$") }).ClickAsync();
                    await _page.Locator("//e-checkbox[@id='chkPlatilacU']").ClickAsync();
                    if (_platilac1 == "0")
                    {
                        await _page.Locator("//e-checkbox[@id='chkPlatilacK']").ClickAsync();
                    }
                }
                else if (_tipUgovaraca == "Vlasnik je i korisnik")
                {
                    await _page.Locator("#selTipLicaU > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#ugovarac div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica1}$") }).ClickAsync();
                }
                else
                {
                    Console.WriteLine($"Nešto nije OK sa unosom tipa lica!");
                    //await _page.Locator("#selTipLicaU > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.Locator("#ugovarac div").Filter(new() { HasTextRegex = new Regex($"^{_tipLica1}$") }).ClickAsync();
                    return;
                }

                #endregion Tip lica


                #region Lične karte

                if (_tipLica1 == "Fizičko")
                {
                    await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("123456");
                    await _page.Locator("#ugovarac").GetByText("Email").ClickAsync();
                    await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");

                    if (_tipPolise == "Granično osiguranje")
                    {
                        await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").FillAsync("Bogdan");
                        await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").PressAsync("Tab");
                        await _page.Locator("#ugovarac #inpPrezime input[type=\"text\"]").FillAsync("Mandarić");

                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync("Nehruova");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync("89/16");
                    }
                    else
                    {
                        await OcitajDokument(_page, "Licna1");
                        var notifyPopupLK = _page.Locator("//div[@class='notify greska']");
                        bool isPopupVisibleLK = await notifyPopupLK.IsVisibleAsync();

                        if (isPopupVisibleLK)
                        {
                            Console.WriteLine("Iskačući prozor je vidljiv. Podaci su uneti ručno.");
                            await _page.Locator("#notify0 button").ClickAsync();

                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").ClickAsync();
                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").FillAsync("2612962710096");
                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").PressAsync("Tab");
                            await _page.Locator(".col-6 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").First.ClickAsync();
                            await _page.Locator("#ugovarac e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").ClickAsync();
                            await _page.Locator("#ugovarac e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").FillAsync("1107");
                            await _page.Locator("#ugovarac").GetByText("- Beograd (Novi Beograd)").First.ClickAsync();

                            await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").FillAsync("Bogdan");
                            await _page.Locator("#ugovarac #inpIme input[type=\"text\"]").PressAsync("Tab");
                            await _page.Locator("#ugovarac #inpPrezime input[type=\"text\"]").FillAsync("Mandarić");

                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync("Nehruova");
                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                            await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync("89/16");

                        }
                        else
                        {
                            Console.WriteLine("Iskačući prozor nije vidljiv. Očitana je LK");

                        }


                        //var notifyPopupLK = _page.Locator("//div[@class='notify greska']");
                        //var notifyPopupLK = _page.Locator("//div[@class='notify greska']//div[contains(.,'U čitaču/ima nije pronađena lična karta.')]");
                        // Provera da li je vidljiv
                        //bool isPopupVisibleLK = await notifyPopupLK.IsVisibleAsync();

                        /*Ovo je drugi način provere da li se otvorio popup, razradi ga
                        int elementCount = await notifyPopup.CountAsync();
                        // Provera da li je otvoren
                        if (elementCount > 0)
                        {
                            Console.WriteLine("Iskačući prozor nije otvoren. Očitana je LK");
                        }
                        else
                        {
                            Console.WriteLine("Iskačući prozor je otvoren. Podaci su uneti ručno");
                        }



                        */



                    }

                }


                else if (_tipLica1 == "Pravno")
                {
                    if (_tipPolise == "Granično osiguranje")
                    {
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").FillAsync("Microsoft");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync("Evropska");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync("666");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("123456789");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");

                    }
                    else
                    {

                        await _page.Locator("#ugovaracMB input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovaracMB input[type=\"text\"]").FillAsync(_mb1);
                        await _page.Locator("#ugovaracMB input[type=\"text\"]").PressAsync("Tab");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).Locator("input[type=\"text\"]").FillAsync(_pib1);
                        //await _page.Locator("//div[@id='ugovarac']//e-input[@class='sufix-field']").FillAsync(_pib1);
                        await _page.Locator("#btnUgovaracPreuzmiNBS button").ClickAsync();


                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").ClickAsync();
                        string tekstBoksSadrzaj = string.Empty;
                        for (int i = 0; i < 10; i++) // 10 pokušaja
                        {
                            tekstBoksSadrzaj = await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").InputValueAsync();

                            if (!string.IsNullOrEmpty(tekstBoksSadrzaj))
                                break;

                            await Task.Delay(500); // Čeka 500 ms pre sledeće provere
                            await _page.Locator("#btnUgovaracPreuzmiNBS button").ClickAsync();


                        }
                        if (string.IsNullOrEmpty(tekstBoksSadrzaj))
                            throw new Exception("Tekst boks nije popunjen u očekivanom vremenskom okviru!");

                        Console.WriteLine($"Tekst boks je popunjen sa: {tekstBoksSadrzaj}");
                        string UlicaBroj = await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").InputValueAsync();
                        Console.WriteLine($"Ulica i broj su: {UlicaBroj}");
                        string Ulica;
                        string Broj;
                        int lastSpaceIndex = UlicaBroj.LastIndexOf(' '); // Nađi poziciju poslednjeg razmaka
                                                                         // Proveri da li postoji razmak u stringu
                        if (lastSpaceIndex != -1)
                        {
                            Ulica = UlicaBroj.Substring(0, lastSpaceIndex); // Sve do poslednjeg razmaka
                            Broj = UlicaBroj.Substring(lastSpaceIndex + 1); // Sve posle poslednjeg razmaka

                            Console.WriteLine($"Tekst2: {Ulica}");
                            Console.WriteLine($"Tekst3: {Broj}");
                        }
                        else
                        {
                            // Ako nema razmaka, cela vrednost ide u Ulica, a broj dobija vrednost bb
                            Ulica = UlicaBroj;
                            Broj = "bb";

                            Console.WriteLine($"Tekst2: {Ulica}");
                            Console.WriteLine($"Tekst3: {Broj}");
                        }
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync(Ulica);
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync(Broj);

                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("123456789");
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");
                        if (_platilac1 != "")
                        {
                            await _page.Locator("//e-checkbox[@id='chkPlatilacU']").ClickAsync();
                        }

                        if (_platilac1 == "0")
                        {
                            await _page.Locator("//e-checkbox[@id='chkPlatilacK']").ClickAsync();
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Greška u tipu lica 1");
                    return;
                }


                if (_tipLica2 == "Fizičko")
                {
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("654321");
                    await _page.Locator("#korisnik").GetByText("Email").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");

                    await OcitajDokument(_page, "Licna2");
                    var notifyPopupLK = _page.Locator("//div[@class='notify greska']");
                    // Provera da li je vidljiv
                    bool isPopupVisibleLK = await notifyPopupLK.IsVisibleAsync();
                    if (isPopupVisibleLK)
                    {
                        Console.WriteLine("Iskačući prozor je vidljiv. Uneti su podaci ručno.");
                        await _page.Locator("#notify0 button").ClickAsync();

                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "JMBG" }).Locator("input[type=\"text\"]").FillAsync("3001985710098");
                        await _page.Locator("#idIme input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#idIme input[type=\"text\"]").FillAsync("Petar");
                        await _page.Locator("#idIme input[type=\"text\"]").PressAsync("Tab");
                        await _page.Locator("#idPrezime input[type=\"text\"]").FillAsync("Janković");
                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync("Japanska");
                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync("bb");
                        await _page.Locator("#korisnik > div:nth-child(6) > div > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await _page.Locator("#korisnik e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").ClickAsync();
                        await _page.Locator("#korisnik e-select").Filter(new() { HasText = "---24430 - Ada22244 - Adaš" }).GetByPlaceholder("pretraži").FillAsync("1106");
                        await _page.Locator("#korisnik").GetByText("- Beograd (Palilula)").ClickAsync();



                    }

                }
                else if (_tipLica2 == "Pravno")
                {


                    await _page.Locator("#korisnikLizingaMB input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnikLizingaMB input[type=\"text\"]").FillAsync(_mb2);
                    await _page.Locator("#korisnikLizingaMB input[type=\"text\"]").PressAsync("Tab");
                    await _page.Locator("#btnKorisnikPreuzmiNBS button").ClickAsync();

                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").ClickAsync();
                    string tekstBoksSadrzaj = string.Empty;
                    for (int i = 0; i < 10; i++) // 10 pokušaja
                    {
                        tekstBoksSadrzaj = await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Naziv" }).Locator("input[type=\"text\"]").InputValueAsync();

                        if (!string.IsNullOrEmpty(tekstBoksSadrzaj))
                            break;

                        await Task.Delay(500); // Čeka 500 ms pre sledeće provere
                        await _page.Locator("#btnKorisnikPreuzmiNBS button").ClickAsync();


                    }
                    if (string.IsNullOrEmpty(tekstBoksSadrzaj))
                        throw new Exception("Tekst boks nije popunjen u očekivanom vremenskom okviru!");



                    string UlicaBroj = await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").InputValueAsync();
                    Console.WriteLine($"Ulica i broj korisnika su: {UlicaBroj}");


                    string Ulica;
                    string Broj;
                    int lastSpaceIndex = UlicaBroj.LastIndexOf(' '); // Nađi poziciju poslednjeg razmaka
                                                                     // Proveri da li postoji razmak u stringu
                    if (lastSpaceIndex != -1)
                    {
                        Ulica = UlicaBroj.Substring(0, lastSpaceIndex); // Sve do poslednjeg razmaka
                        Broj = UlicaBroj.Substring(lastSpaceIndex + 1); // Sve posle poslednjeg razmaka

                        Console.WriteLine($"Tekst2: {Ulica}");
                        Console.WriteLine($"Tekst3: {Broj}");
                    }
                    else
                    {
                        // Ako nema razmaka, cela vrednost ide u Ulica, a broj dobija vrednost bb
                        Ulica = UlicaBroj;
                        Broj = "bb";

                        Console.WriteLine($"Tekst2: {Ulica}");
                        Console.WriteLine($"Tekst3: {Broj}");
                    }

                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Ulica" }).Locator("input[type=\"text\"]").FillAsync(Ulica);
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Broj" }).Locator("input[type=\"text\"]").FillAsync(Broj);

                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Telefon" }).Locator("input[type=\"text\"]").FillAsync("987654321");
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").ClickAsync();
                    await _page.Locator("#korisnik e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");

                    if (_platilac1 != "")
                    {
                        await _page.Locator("//e-checkbox[@id='chkPlatilacU']").ClickAsync();
                    }

                    if (_platilac1 == "0")
                    {
                        await _page.Locator("//e-checkbox[@id='chkPlatilacK']").ClickAsync();
                    }


                }


                else if (_tipLica2 == "")
                {

                }
                else
                {
                    Console.WriteLine("Greška u tipu lica 2");
                    return;
                }

                #endregion Lične karte

                //Generišem Stranca 1
                if (_tipPolise != "Granično osiguranje")
                {
                    // Definišemo niz elemenata
                    string[] Stranac = { "Da", "Ne" };
                    // Kreiramo instancu klase Random
                    Random randomStranac = new Random();
                    // Generišemo slučajan indeks
                    int slucajanIndeks = randomStranac.Next(Stranac.Length);
                    // Biramo element iz niza na osnovu slučajnog indeksa
                    string SlucajanStranac = Stranac[slucajanIndeks];
                    if (SlucajanStranac == "Da") { await _page.Locator("#chkStranacU").ClickAsync(); }
                }

                //Generišem Stranca 2
                if (_tipLica2 != "")
                {
                    // Definišemo niz elemenata
                    string[] Stranac = { "Da", "Ne" };
                    // Kreiramo instancu klase Random
                    Random randomStranac = new Random();
                    // Generišemo slučajan indeks
                    int slucajanIndeks = randomStranac.Next(Stranac.Length);
                    // Biramo element iz niza na osnovu slučajnog indeksa
                    string SlucajanStranac = Stranac[slucajanIndeks];
                    if (SlucajanStranac == "Da") { await _page.Locator("#chkStranacK").ClickAsync(); }
                }





                #endregion LICA NA POLISI



                #region VOZILO

                #region Vrsta vozila - premijska grupa
                if (_premijskaGrupa != "Putničko vozilo")
                {
                    await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText(_premijskaGrupa).ClickAsync();
                }

                //await _page.GetByText("Teretno vozilo").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Autobusi").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Vučna vozila").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Specijalna motorna vozila").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Motocikli").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Priključna vozila").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Radna vozila").ClickAsync();
                //await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("Putničko vozilo").ClickAsync();

                #endregion Vrsta vozila - premijska grupa




                #region Tegljač
                if (_tegljac == "1")
                {
                    await _page.Locator("#chkTegljac div").Nth(3).ClickAsync();
                }
                #endregion Tegljač



                #region Premijska podgrupa

                if (_podgrupa != "0")
                {
                    await _page.Locator("#selPodvrsta > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText(_podgrupa).ClickAsync();
                }

                #endregion Premijska podgrupa

                #region Popusti i doplaci
                if (_popusti != "0")
                {
                    await _page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByText(_popusti).ClickAsync();
                }
                #endregion Popusti i doplaci


                #region  Država vozila
                if (_tipPolise == "Granično osiguranje")
                {
                    //await _page.Locator("#seldrzavaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("//div[@id='drzavaVozila']//div[@class='control ']").ClickAsync();

                    await _page.Locator("xpath=//e-select[@data-source-id='drzavaVozilaData']//div[@class='multiselect-dropdown input']").ClickAsync();
                    //await _page.Locator("xpath=//e-select[@data-source-id='drzavaVozilaData']//div[@class='multiselect-dropdown input']").FillAsync("alb"); // unos početnih slova
                    await _page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("alb"); // unos početnih slova
                                                                                                                                                                                                           //await _page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
                    await _page.Locator("//div[.='Albanija']").ClickAsync();
                }
                #endregion  Država vozila






                #region Podaci o vozilu

                #region Područje i registarska oznaka

                string RegistarskaOznaka = DefinisiRegistarskuOznaku();
                //Unesi Registarsku oznaku (bez područja)
                if (_premijskaGrupa == "Priključna vozila" && _tipPolise == "Granično osiguranje")
                {
                    await _page.GetByText("Reg oznaka (bez područja)").Nth(1).ClickAsync();
                    await _page.Locator("#reg2 > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(RegistarskaOznaka);
                }

                else if (_premijskaGrupa == "Priključna vozila" && _tipPolise != "Granično osiguranje")
                {
                    await _page.Locator("//div[@class='commonBox font09 vozilo-class']//div[@class='control  ']/div[contains(.,'Reg oznaka  (bez područja)')]").ClickAsync();
                    await _page.Locator("//div[@class='commonBox font09 vozilo-class']/div[3]/div[@class='col-2 column']//input[@class='input']").FillAsync(RegistarskaOznaka);
                    await _page.Locator("#inpRegistrarskaOznaka input[type=\"text\"]").PressAsync("Tab");
                    await _page.Locator("#podrucje2 > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#podrucje2").GetByPlaceholder("pretraži").ClickAsync();
                    await _page.Locator("#podrucje2").GetByPlaceholder("pretraži").FillAsync("bg");
                    await _page.Locator("#podrucje2").GetByText("BG").ClickAsync();
                }

                else if (_premijskaGrupa != "Priključna vozila" && _tipPolise == "Granično osiguranje")
                {
                    await _page.Locator("#inpRegistrarskaOznaka").GetByText("Reg oznaka").ClickAsync();
                    await _page.Locator("//e-input[@id='inpRegistrarskaOznaka']//input[@class='input']").FillAsync(RegistarskaOznaka);
                    await _page.Locator("#inpRegistrarskaOznaka input[type=\"text\"]").PressAsync("Tab");
                }
                else
                {
                    await _page.Locator("#podrucje > e-select > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("#podrucje").GetByPlaceholder("pretraži").ClickAsync();
                    await _page.Locator("#podrucje").GetByPlaceholder("pretraži").FillAsync("bg");
                    await _page.Locator("#podrucje").GetByText("BG").ClickAsync();
                    await _page.Locator("#inpRegistrarskaOznaka").GetByText("Reg oznaka (bez područja)").ClickAsync();
                    await _page.Locator("//e-input[@id='inpRegistrarskaOznaka']//input[@class='input']").FillAsync(RegistarskaOznaka);
                    await _page.Locator("#inpRegistrarskaOznaka input[type=\"text\"]").PressAsync("Tab");
                }

                #endregion Područje  i registarska oznaka

                #region Marka Tip Broj šasije Godište Boja Namena

                // Definišemo niz elemenata za Marku
                string[] Marke = ["Mercedes", "Volvo", "Audi", "Opel", "Toyota"];
                // Kreiramo instancu klase Random
                Random randomMarka = new Random();
                // Generišemo slučajan indeks
                int slucajanIndeksMarka = randomMarka.Next(Marke.Length);
                // Biramo element iz niza na osnovu slučajnog indeksa
                string SlucajanaMarka = Marke[slucajanIndeksMarka];
                await _page.Locator("e-input").Filter(new() { HasText = "Marka" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Marka" }).GetByRole(AriaRole.Textbox).FillAsync(SlucajanaMarka);

                await _page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).FillAsync("ABC");

                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
                string _brojSasije = DefinisiBrojSasije();
                await _page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

                await _page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).FillAsync("2021");

                await _page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).FillAsync("Crvena13");

                await _page.GetByText("Namena", new() { Exact = true }).ClickAsync();
                await _page.Locator("e-input").Filter(new() { HasText = "Namena" }).GetByRole(AriaRole.Textbox).FillAsync("Uneo sam namenu");

                #endregion Marka Tip Broj šasije Godište Boja Namena


                #region Zapremina i Broj mesta
                if (_premijskaGrupa == "Putničko vozilo" || _premijskaGrupa == "Teretno vozilo" || _premijskaGrupa == "Autobusi" || _premijskaGrupa == "Motocikli")
                {
                    await _page.Locator("xpath=//e-input[contains(.,'Zapr. motora [ccm]')]").ClickAsync();
                    await _page.Locator("xpath=//div[@id='zapremina']//input[@class='input']").FillAsync(_zapremina);
                    await _page.Locator("#brMesta").GetByRole(AriaRole.Textbox).FillAsync(_brojMesta);
                    await _page.Locator("#brMesta").GetByRole(AriaRole.Textbox).PressAsync("Tab");
                }
                #endregion Zapremina i Broj mesta

                #region Snaga
                if (_premijskaGrupa != "Priključna vozila" && _premijskaGrupa != "Radna vozila")
                {
                    await _page.Locator("xpath=//e-input[contains(.,'Snaga')]").ClickAsync();
                    await _page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync(_snaga);
                }
                #endregion Snaga

                #region Nosivost
                if ((_premijskaGrupa == "Teretno vozilo" && _tegljac != "1") || _premijskaGrupa == "Priključna vozila")
                {
                    await _page.Locator("xpath=//e-input[contains(.,'Nosivost')]").ClickAsync();
                    await _page.Locator("xpath=//div[@id='nosivost']//input[@class='input']").FillAsync(_nosivost);
                }
                #endregion Nosivost




                await OcitajDokument(_page, "Saobracajna");
                //var notifyPopupSaobracajna = _page.Locator("//div[@class='notify greska']");
                var notifyPopupSaobracajna = _page.Locator("//div[contains(.,'U čitaču/ima nije pronađena saobraćajna dozvola.')]");
                // Provera da li je vidljiv
                bool isPopupVisibleSaobracajna = await notifyPopupSaobracajna.IsVisibleAsync();
                if (isPopupVisibleSaobracajna)
                {
                    await _page.Locator("#notify0 button").ClickAsync();
                    Console.WriteLine("Iskačući prozor za Saobraćajnu je vidljiv. Uneti su podaci ručno.");
                }

                #endregion Podaci o vozilu
                #endregion VOZILO

                #region OSTALO
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).FillAsync($"Ovde je uneta napomena za vozilo čija je registarska oznaka: {RegistarskaOznaka}");


                #endregion OSTALO



                #region SNIMI, IZMENI I KALKULIŠI



                //Nalaženje poslednjeg broja dokumenta u Mtpl
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaMtpl();


                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                //Ovde proveri URL
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{PoslednjiBrojDokumenta + 1}");
                //await _page.EvaluateAsync("location.reload(true);");

                // await _page.PauseAsync();

                int maxPokusaja = 5;
                int brojPokusaja = 0;
                //bool neuspeh = true;
                bool kalkulisanjeUspesno = false;

                while (brojPokusaja < maxPokusaja && kalkulisanjeUspesno == false)
                {
                    brojPokusaja++;
                    LogovanjeTesta.LogMessage($"Pokušaj #{brojPokusaja}: Klik na dugme 'Kalkuliši'");

                    try
                    {
                        // Klik na dugme Kalkuliši
                        await _page.Locator("button:has-text('Kalkuliši')").ClickAsync();
                        //await _page.Locator("button").Filter(new() { HasText = "Kalkuliši" }).ClickAsync();

                        // Čekaj do 4 sekunde da se pojavi bilo koja poruka
                        //var porukaLocator = _page.Locator("text=Podaci uspešno kalkulisani, text=Problem prilikom povezivanja");
                        //var porukaLocator = _page.Locator("//div[contains(.,'Problem prilikom povezivanja')] or //div[contains(.,'Podaci uspešno kalkulisani')]");

                        var porukaLocator = _page.Locator("//div[contains(., 'Problem prilikom povezivanja') or contains(., 'Podaci uspešno kalkulisani')]");
                        await porukaLocator.WaitForAsync(new() { Timeout = 4000 });

                        string tekstPoruke = await porukaLocator.InnerTextAsync();

                        if (tekstPoruke.Contains("Podaci uspešno kalkulisani"))
                        {
                            LogovanjeTesta.LogMessage("✅ Kalkulisanje je uspešno.");
                            kalkulisanjeUspesno = true;
                        }
                        else if (tekstPoruke.Contains("Problem prilikom povezivanja"))
                        {
                            LogovanjeTesta.LogMessage("⚠️ Neuspešno kalkulisanje. Poruka: " + tekstPoruke);

                            // Pokušaj da zatvoriš poruku ako postoji dugme
                            var dugmeZatvori = _page.Locator("#notify0 button, .modal button:has-text('×'), .modal-close");
                            if (await dugmeZatvori.IsVisibleAsync())
                            {
                                await dugmeZatvori.ClickAsync();
                                LogovanjeTesta.LogMessage("❎ Poruka zatvorena.");
                            }

                            await _page.WaitForTimeoutAsync(1000); // pauza pre ponovnog pokušaja
                        }
                        else
                        {
                            LogovanjeTesta.LogMessage("⚠️ Nepoznata poruka: " + tekstPoruke);
                            await _page.WaitForTimeoutAsync(1000);
                        }
                    }
                    catch (TimeoutException)
                    {
                        LogovanjeTesta.LogMessage("⚠️ Nije se pojavila nikakva poruka u roku.");
                        await _page.WaitForTimeoutAsync(1000);
                    }
                    catch (Exception ex)
                    {
                        await LogovanjeTesta.LogException($"❌ Greška tokom pokušaja #{brojPokusaja}", ex);
                    }
                }

                if (kalkulisanjeUspesno == false)
                {
                    LogovanjeTesta.LogError("❌ Kalkulisanje neuspešno nakon 5 pokušaja.");
                    throw new Exception("Kalkulisanje neuspešno nakon maksimalnog broja pokušaja.");
                }

                //***********************************************************************************************
                //await _page.Locator("button").Filter(new() { HasText = "Kalkuliši" }).ClickAsync();
                //var notifyPopupLK = _page.Locator("//div[@class='notify greska']");
                //bool isPopupVisibleLK = await notifyPopupLK.IsVisibleAsync();

                //if (isPopupVisibleLK)
                //{
                // Proveri da li je vidljiv
                //}

                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Otkaži izmenu" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{PoslednjiBrojDokumenta + 1}");
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Obriši dokument" }).ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();



                /*


                await _page.Locator("button").Filter(new() { HasText = "Kalkuliši" }).ClickAsync();
                await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/1/Autoodgovornost/Dokument/6514");
                await _page.GetByText("Registarski broj polise", new() { Exact = true }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.GetByText("Broj dokumenta:").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Obriši dokument" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();


                            //var popup = await _page.RunAndWaitForPopupAsync(async => {
                            //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                            //});
                            //await _page.PauseAsync();
                            //await _page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                            await _page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
                            var locator = _page.Locator("//div[@id='notify0']");
                            //await Expect(locator).ToHaveTextAsync("Nedostaju podaci ili uneti podaci nisu validni");
                            //await Expect(locator).ToHaveTextAsync("Podaci uspešno kalkulisani");
                            // Pomeranje miša za 100px dole i desno
                            int newX = 200;
                            int newY = 200;
                            await _page.Mouse.MoveAsync(newX, newY);

                            //Thread.Sleep(5000);

                            await _page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
                            await _page.GetByText("Da li ste sigurni da želite").ClickAsync();

                            await _page.GetByRole(AriaRole.Button, new() { Name = "Da!"/*, Exact = false }).ClickAsync();
                            //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne", Exact = true }).ClickAsync();
            */



                #region Sertifikat
                try
                {
                    var process = Process.GetProcessesByName(AppName).FirstOrDefault();
                    if (process != null)
                    {
                        //Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                        _application = FlaUI.Core.Application.Attach(process);
                    }
                    else
                    {
                        // Ako aplikacija nije pokrenuta, pokreni je
                        _application = FlaUI.Core.Application.Launch(AppPath);
                    }



                    // Inicijalizacija FlaUI
                    _automation = new UIA3Automation();
                    // Dohvatanje glavnog prozora aplikacije
                    //var mainWindow = _application.GetMainWindow(_automation);
                    var mainWindow = Retry.WhileNull(
                        () => _application.GetMainWindow(_automation),
                        TimeSpan.FromSeconds(10)).Result;
                    // Provera da li je mainWindow null
                    if (mainWindow == null)
                    {
                        throw new Exception("Main window of the application was not found.");
                    }

                    //Pronalazak TreeView elementa
                    //var treeView = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree))?.AsTree();
                    var treeView = Retry.WhileNull(
                        () => mainWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Tree))?.AsTree(),
                        TimeSpan.FromSeconds(5)).Result;


                    //Assert.IsNotNull(treeView, "TreeView not found");

                    // Pronalazak TreeItem sa tekstom "Petrović Petar"
                    //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName("Bogdan Mandarić 200035233"))).AsTreeItem();
                    //var SertifikatName = KorisnikLoader5.Korisnik3?.Sertifikat ?? string.Empty;
                    //var SertifikatName = AKorisnik_?.Sertifikat ?? string.Empty;
                    //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName(SertifikatName))).AsTreeItem();


                    var treeItem = Retry.WhileNull(
                        () => treeView?.FindFirstDescendant(cf =>
                            cf.ByControlType(ControlType.TreeItem).And(cf.ByName(SertifikatName)))?.AsTreeItem(),
                        TimeSpan.FromSeconds(5)).Result;


                    //Assert.IsNotNull(treeItem, "TreeItem 'Bogdan Mandarić' not found");

                    // Klik na TreeItem
                    if (treeItem != null)
                    {
                        treeItem.Click();
                    }
                    else
                    {
                        throw new Exception($"TreeItem '{SertifikatName}' not found.");
                    }

                    // Pronalazak dugmeta "Cancel"
                    //var cancelButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("Cancel"))).AsButton();
                    //Assert.IsNotNull(quitButton, "Quit button not found");
                    // Klik na dugme Quit
                    //cancelButton.Click();

                    // Pronalazak dugmeta "OK"
                    var okButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK Enter"))).AsButton();
                    //Assert.IsNotNull(okButton, "OK button not found");
                    // Klik na dugme OK
                    //if (okButton != null)
                    //{
                    //okButton.Click();
                    //}
                    //else
                    //{
                    //throw new Exception("OK button not found in the application window.");
                    //}
                    if (okButton == null)
                        throw new Exception("OK button not found in the application window.");
                    okButton.WaitUntilClickable(TimeSpan.FromSeconds(5));
                    okButton.Click();


                    var process2 = Process.GetProcessesByName(AppName2).FirstOrDefault();
                    if (process2 != null)
                    {
                        // Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                        _application2 = FlaUI.Core.Application.Attach(process2);
                        // Inicijalizacija FlaUI
                        _automation2 = new UIA3Automation();
                        // Dohvatanje glavnog prozora aplikacije
                        var mainWindow2 = _application2.GetMainWindow(_automation2);

                        // Pronalazak TextBox elementa
                        if (mainWindow2 == null)
                        {
                            throw new Exception("Main window of the second application was not found.");
                        }
                        var treeElement = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree));
                        var textBox = treeElement?.AsTextBox();
                        //Assert.IsNotNull(textBox, "textBox not found");
                        // Unos teksta u TextBox
                        if (textBox != null)
                        {
                            textBox.Enter("73523");
                        }
                        else
                        {
                            throw new Exception("TextBox not found in the second application window.");
                        }
                        // Pronalazak dugmeta "OK"
                        var okButton2 = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK"))).AsButton();
                        //Assert.IsNotNull(okButton2, "OK button not found");
                        // Klik na dugme OK
                        if (okButton2 != null)
                        {
                            okButton2.Click();
                        }
                        else
                        {
                            throw new Exception("OK button not found in the second application window.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                    await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                    await LogovanjeTesta.LogException("FlaUI Sertifikat Selekcija", ex);
                    throw; // ili možeš odlučiti da NE baciš grešku dalje
                }


                #endregion Sertifikat

                //var locator2 = _page.Locator("//div[@class='notify']");
                var locator2 = _page.Locator("//div[contains(.,'Polisa broj')]");
                await locator2.WaitForAsync(new() { Timeout = 20000 });
                //var notifyKreiranaPolisa = _page.Locator("//div[contains(.,'U čitaču/ima nije pronađena saobraćajna dozvola.')]");
                // Provera da li je vidljiv
                //await locator2.WaitForAsync();
                await Assertions.Expect(locator2).ToBeVisibleAsync(new() { Timeout = 10000 });

                #endregion SNIMI, IZMENI I KALKULIŠI


                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                //Assert.Pass();

                //return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }

        [Test]
        public async Task AO_07_PregledZahtevaZaIzmenomPolisaAO()
        {
            if (_page == null)
                throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
            await Pauziraj(_page);
            try
            {
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Otvori Autoodgovornost
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");
                //Otvori grid Pregled / pretraga zahteva za izmenom
                await _page.GetByText("Pregled / Pretraga zahteva za").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-zahteva/Izmene-polisa");
                //Proveri da li postoji grid
                // Proveri da li stranica sadrži grid Autoodgovornost
                string tipGrida = "Zahtevi za izmenu polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                //Izbroj koliko ima redova u gridu
                var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Redova ima:: {rows.Count}", "Informacija");
                }
                //Klikni na prvi zahtev i proveri da li se otvrila odgovarajuća stranica
                // Traži prvi red koji ima broj ugovora
                int rowIndex = 0; // Definiši brojač
                string brojZahteva = "";
                string brojDokumenta = "abc";
                foreach (var row in rows)
                {
                    // Inkrementiraj brojač
                    rowIndex++;
                    var valueInColumn1 = await row.QuerySelectorAsync("div.column_1");
                    var valueInColumn3 = await row.QuerySelectorAsync("div.column_3");

                    // Pročitajte vrednost iz ćelije
                    if (valueInColumn1 != null && valueInColumn3 != null)
                    {
                        brojZahteva = await valueInColumn1.EvaluateAsync<string>("el => el.innerText");
                        brojDokumenta = await valueInColumn3.EvaluateAsync<string>("el => el.innerText");
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"U redu {rowIndex} kolona \nBroj zahteva ima vrednost:: #{brojZahteva}#, a \nBroj dokumenta je:: #{brojDokumenta}#");
                        }
                        break;
                    }
                    else
                    {
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"Proveri grid", "Informacija");
                        }
                        break;
                    }
                }
                //await Pauziraj(_page);
                await _page.GetByText(brojZahteva).First.ClickAsync();
                await _page.Locator("div").Filter(new() { HasText = "Pregled zahteva za izmenom za" }).Nth(4).ClickAsync();
                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.GetByText("Br. zah. -").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu").GetByText(brojZahteva).First.ClickAsync();
                //await _page.GetByText(brojZahteva).ClickAsync();
                await _page.Locator("#pregled-zahteva-naslov").GetByText(brojDokumenta).ClickAsync();
                //await Pauziraj(_page);
                //proveri URL 
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}/{brojZahteva}");
                //await _page.GetByText($"Unos novog zahteva za izmenu podataka Detalji zahteva za izmenu podataka {brojZahteva}").ClickAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.GetByText("Pregled / Pretraga zahteva za izmenom polisa").ClickAsync();



                // Nađi prvu kreiranu polisu koja nema zahtev za izmenu
                // Pročitaj iz baze idDokument za odgovarajući broj ugovora

                /*
                                string Partner = "";
                                Partner = Okruzenje switch
                                {
                                    "Razvoj" => "[idPartner] IN (36, 120)",
                                    "Proba2" => "[idPartner] IN (36, 120)",
                                    "UAT" => "[idPartner] =103",
                                    "Produkcija" => "",
                                    _ => throw new ArgumentException($"Nepoznati partner ID {Partner} na okruženju: {Okruzenje})"),
                                };
                                int BrojDokumenta;
                                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = 1001 AND {Partner};";
                */
                //await Pauziraj(_page);

                /*
                Server = Okruzenje switch
                {
                    "Razvoj" => "10.5.41.99",
                    "Proba2" => "49.13.25.19",
                    "UAT" => "10.41.5.5",
                    "Produkcija" => "",
                    _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                };
                */
                Server = OdrediServer(Okruzenje);

                int BrojDokumenta;
                /****************
                int idKorisnik = 0;
                idKorisnik = RucnaUloga switch
                {
                    "Bogdan" => 1001,
                    "Mario" => 1002,

                    _ => throw new ArgumentException($"Nepoznati idKorisnik:  + {idKorisnik}"),
                };
                ******************/
                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = {IdLice} AND [datumIsteka] > CAST(GETDATE() AS DATE);";
                // Konekcija sa bazom
                //string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                string connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionString))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qBrojDokumenta, konekcija))
                    {
                        object BrojDokumenta1 = command.ExecuteScalar();
                        BrojDokumenta = BrojDokumenta1 != DBNull.Value ? Convert.ToInt32(BrojDokumenta1) : 0;
                        //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
                        //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
                    }
                    konekcija.Close();
                }
                if (BrojDokumenta == 0)
                {
                    LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - Nema dokumenata za izmenu.");
                    throw new Exception("Nema dokumenata za izmenu.");
                }
                //Treba ubaciti proveru o ostalih slučajeva. kada je u izradi.... i ako nema polisa ....
                Console.WriteLine($"ID dokumenta je na okruženju '{Okruzenje}' je: {BrojDokumenta}.\n");
                await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/" + BrojDokumenta);
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{BrojDokumenta}");
                //await Pauziraj(_page);
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Novi zahtev za izmenu" }).ClickAsync();
                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.PauseAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepenaDigitalno odobrenje ---").ClickAsync();
                //await _page.Locator("#selTipZahteva").GetByText("Izmena podataka").ClickAsync();
                //await _page.GetByText("Tip zahteva", new() { Exact = true }).ClickAsync();
                await _page.Locator("//e-select[@id='selTipZahteva']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.Locator("#selTipZahteva span").Filter(new() { HasText = "---" }).ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                await _page.Locator("#selTipZahteva").GetByText("Izmena podataka").ClickAsync();

                //await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();

                await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpNaslov input[type=\"text\"]").FillAsync("Menjam opšte podatke");
                await _page.GetByText("Tekst zahteva").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Tekst zahteva" }).Locator("textarea").FillAsync($"Ovo je tekst zahteva za dokument br. {BrojDokumenta}");

                if (NacinPokretanjaTesta == "ručno")
                {
                    await _page.Locator("button").Filter(new() { HasText = "Dodaj priloge" }).ClickAsync();

                    System.Windows.MessageBox.Show("Dodaj prilog", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                /****************************************
                Provera da li je BO dobio mejl da je dokument prenos obrazaca vraćen u izradu
                *****************************************/
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji zahtev" }).ClickAsync();


                brojZahteva = (int.Parse(brojZahteva) + 1).ToString();

                await _page.GetByText("Broj dokumenta:").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();

                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.GetByText("Izmena podataka").Nth(3).ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("21.01.2025").ClickAsync();
                //await _page.GetByText("Menjam opšte podatke").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2321").ClickAsync();
                //await _page.Locator($"column column_1 txt-center").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();

                /******************************************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je i kome otišao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                **********************************************/
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                //Sada se izloguj, pa uloguj kao BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                //uloguj se kao BO
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                //await _page.Locator("input[type=\"password\"]").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();

                //await _page.PauseAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Imate novi zahtev za izmenom" }).ClickAsync();
                //await _page.Locator("h3").Filter(new() { HasText = "Izmena vesti broj: 964 (" }).Locator("button").ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Imate novi zahtev za izmenom" }).ClickAsync();
                //await _page.Locator("h3").Filter(new() { HasText = "Izmena vesti broj: 964 (" }).Locator("button").ClickAsync();
                string dok = BrojDokumenta.ToString();
                await _page.GetByText($"Imate novi zahtev za izmenomDokument možete pogledati klikom na link: {dok}").HoverAsync();


                await _page.Locator($"//a[.='{dok}']").ClickAsync();

                //await Pauziraj(_page);
                //await _page.GetByText(BrojDokumenta.ToString()).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");

                await _page.GetByText("---RealizovanDelimično realizovanOdbijen ---").ClickAsync();
                await _page.Locator("#selStatusZahteva").GetByText("Delimično realizovan").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").FillAsync("Ovo je odgovor.");
                await _page.Locator("button").Filter(new() { HasText = "Pošalji odgovor" }).ClickAsync();

                /******************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je agentu stigao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ******************************/
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await ArhivirajVest(_page, "Imate novi zahtev za izmenomDokument možete pogledati klikom na link:", dok);

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("bogdan.mandaric@eonsystem.com");
                //await _page.Locator("input[type=\"password\"]").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();


                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.GetByText("Odogovoreno je na zahtev za").First.ClickAsync();
                //await _page.GetByText($"{dok}").First.ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                //await _page.PauseAsync();

                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator(".btnArhiviraj > .left").First.ClickAsync();
                //await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.Locator(".korisnik > a").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();












                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }

        }



        [Test]
        public async Task AO_08_ZahtevZaIzmenu_Podaci()
        {
            if (_page == null)
                throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
            await Pauziraj(_page);
            try
            {
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Otvori Autoodgovornost
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");
                //Otvori grid Pregled / pretraga zahteva za izmenom
                await _page.GetByText("Pregled / Pretraga zahteva za").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-zahteva/Izmene-polisa");
                //Proveri da li postoji grid
                // Proveri da li stranica sadrži grid Autoodgovornost
                string tipGrida = "Zahtevi za izmenu polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                //Izbroj koliko ima redova u gridu
                var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Redova ima:: {rows.Count}", "Informacija");
                }
                //Klikni na prvi zahtev i proveri da li se otvrila odgovarajuća stranica
                // Traži prvi red koji ima broj ugovora
                int rowIndex = 0; // Definiši brojač
                string brojZahteva = "";
                string brojDokumenta = "abc";
                foreach (var row in rows)
                {
                    // Inkrementiraj brojač
                    rowIndex++;
                    var valueInColumn1 = await row.QuerySelectorAsync("div.column_1");
                    var valueInColumn3 = await row.QuerySelectorAsync("div.column_3");

                    // Pročitajte vrednost iz ćelije
                    if (valueInColumn1 != null && valueInColumn3 != null)
                    {
                        brojZahteva = await valueInColumn1.EvaluateAsync<string>("el => el.innerText");
                        brojDokumenta = await valueInColumn3.EvaluateAsync<string>("el => el.innerText");
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"U redu {rowIndex} kolona \nBroj zahteva ima vrednost:: #{brojZahteva}#, a \nBroj dokumenta je:: #{brojDokumenta}#");
                        }
                        break;
                    }
                    else
                    {
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"Proveri grid", "Informacija");
                        }
                        break;
                    }
                }
                //await Pauziraj(_page);
                await _page.GetByText(brojZahteva).First.ClickAsync();
                await _page.Locator("div").Filter(new() { HasText = "Pregled zahteva za izmenom za" }).Nth(4).ClickAsync();
                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.GetByText("Br. zah. -").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu").GetByText(brojZahteva).First.ClickAsync();
                //await _page.GetByText(brojZahteva).ClickAsync();
                await _page.Locator("#pregled-zahteva-naslov").GetByText(brojDokumenta).ClickAsync();
                //await Pauziraj(_page);
                //proveri URL 
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}/{brojZahteva}");
                //await _page.GetByText($"Unos novog zahteva za izmenu podataka Detalji zahteva za izmenu podataka {brojZahteva}").ClickAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.GetByText("Pregled / Pretraga zahteva za izmenom polisa").ClickAsync();



                // Nađi prvu kreiranu polisu koja nema zahtev za izmenu
                // Pročitaj iz baze idDokument za odgovarajući broj ugovora

                /*
                                string Partner = "";
                                Partner = Okruzenje switch
                                {
                                    "Razvoj" => "[idPartner] IN (36, 120)",
                                    "Proba2" => "[idPartner] IN (36, 120)",
                                    "UAT" => "[idPartner] =103",
                                    "Produkcija" => "",
                                    _ => throw new ArgumentException($"Nepoznati partner ID {Partner} na okruženju: {Okruzenje})"),
                                };
                                int BrojDokumenta;
                                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = 1001 AND {Partner};";
                */
                //await Pauziraj(_page);

                /*
                Server = Okruzenje switch
                {
                    "Razvoj" => "10.5.41.99",
                    "Proba2" => "49.13.25.19",
                    "UAT" => "10.41.5.5",
                    "Produkcija" => "",
                    _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                };
                */
                Server = OdrediServer(Okruzenje);

                int BrojDokumenta;
                /****************
                int idKorisnik = 0;
                idKorisnik = RucnaUloga switch
                {
                    "Bogdan" => 1001,
                    "Mario" => 1002,

                    _ => throw new ArgumentException($"Nepoznati idKorisnik:  + {idKorisnik}"),
                };
                ******************/
                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = {IdLice} AND [datumIsteka] > CAST(GETDATE() AS DATE);";
                // Konekcija sa bazom
                //string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                string connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionString))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qBrojDokumenta, konekcija))
                    {
                        object BrojDokumenta1 = command.ExecuteScalar();
                        BrojDokumenta = BrojDokumenta1 != DBNull.Value ? Convert.ToInt32(BrojDokumenta1) : 0;
                        //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
                        //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
                    }
                    konekcija.Close();
                }
                if (BrojDokumenta == 0)
                {
                    LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - Nema dokumenata za izmenu.");
                    throw new Exception("Nema dokumenata za izmenu.");
                }
                //Treba ubaciti proveru o ostalih slučajeva. kada je u izradi.... i ako nema polisa ....
                Console.WriteLine($"ID dokumenta je na okruženju '{Okruzenje}' je: {BrojDokumenta}.\n");
                await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/" + BrojDokumenta);
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{BrojDokumenta}");
                //await Pauziraj(_page);
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Novi zahtev za izmenu" }).ClickAsync();
                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.PauseAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepenaDigitalno odobrenje ---").ClickAsync();
                //await _page.Locator("#selTipZahteva").GetByText("Izmena podataka").ClickAsync();
                //await _page.GetByText("Tip zahteva", new() { Exact = true }).ClickAsync();
                await _page.Locator("//e-select[@id='selTipZahteva']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.Locator("#selTipZahteva span").Filter(new() { HasText = "---" }).ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                await _page.Locator("#selTipZahteva").GetByText("Izmena podataka").ClickAsync();

                //await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();

                await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpNaslov input[type=\"text\"]").FillAsync("Menjam opšte podatke");
                await _page.GetByText("Tekst zahteva").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Tekst zahteva" }).Locator("textarea").FillAsync($"Ovo je tekst zahteva za dokument br. {BrojDokumenta}");

                if (NacinPokretanjaTesta == "ručno")
                {
                    await _page.Locator("button").Filter(new() { HasText = "Dodaj priloge" }).ClickAsync();

                    System.Windows.MessageBox.Show("Dodaj prilog", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                /****************************************
                Provera da li je BO dobio mejl da je dokument prenos obrazaca vraćen u izradu
                *****************************************/
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji zahtev" }).ClickAsync();


                brojZahteva = (int.Parse(brojZahteva) + 1).ToString();

                await _page.GetByText("Broj dokumenta:").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();

                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.GetByText("Izmena podataka").Nth(3).ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("21.01.2025").ClickAsync();
                //await _page.GetByText("Menjam opšte podatke").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2321").ClickAsync();
                //await _page.Locator($"column column_1 txt-center").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();

                /******************************************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je i kome otišao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                **********************************************/
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                //Sada se izloguj, pa uloguj kao BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                //uloguj se kao BO
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                //await _page.Locator("input[type=\"password\"]").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();

                //await _page.PauseAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Imate novi zahtev za izmenom" }).ClickAsync();
                //await _page.Locator("h3").Filter(new() { HasText = "Izmena vesti broj: 964 (" }).Locator("button").ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Imate novi zahtev za izmenom" }).ClickAsync();
                //await _page.Locator("h3").Filter(new() { HasText = "Izmena vesti broj: 964 (" }).Locator("button").ClickAsync();
                string dok = BrojDokumenta.ToString();
                await _page.GetByText($"Imate novi zahtev za izmenomDokument možete pogledati klikom na link: {dok}").HoverAsync();


                await _page.Locator($"//a[.='{dok}']").ClickAsync();

                //await Pauziraj(_page);
                //await _page.GetByText(BrojDokumenta.ToString()).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");


                await _page.GetByText("---RealizovanDelimično realizovanOdbijen ---").ClickAsync();
                await _page.Locator("#selStatusZahteva").GetByText("Delimično realizovan").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").FillAsync("Ovo je odgovor.");
                await _page.Locator("button").Filter(new() { HasText = "Pošalji odgovor" }).ClickAsync();

                /******************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je agentu stigao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ******************************/
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await ArhivirajVest(_page, "Imate novi zahtev za izmenomDokument možete pogledati klikom na link:", dok);

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("bogdan.mandaric@eonsystem.com");
                //await _page.Locator("input[type=\"password\"]").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();



                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.GetByText("Odogovoreno je na zahtev za").First.ClickAsync();
                //await _page.GetByText($"{dok}").First.ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                //await _page.PauseAsync();

                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator(".btnArhiviraj > .left").First.ClickAsync();
                //await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.Locator(".korisnik > a").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();












                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }

        }


        [Test]
        public async Task AO_09_ZahtevZaIzmenu_PremijskiStepen()
        {
            if (_page == null)
                throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
            await Pauziraj(_page);
            try
            {
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //Otvori Autoodgovornost
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");
                //Otvori grid Pregled / pretraga zahteva za izmenom
                await _page.GetByText("Pregled / Pretraga zahteva za").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-zahteva/Izmene-polisa");
                //Proveri da li postoji grid
                // Proveri da li stranica sadrži grid Autoodgovornost
                string tipGrida = "Zahtevi za izmenu polisa AO";
                await ProveraPostojiGrid(_page, tipGrida);

                //Izbroj koliko ima redova u gridu
                var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show($"Redova ima:: {rows.Count}", "Informacija");
                }
                //Klikni na prvi zahtev i proveri da li se otvrila odgovarajuća stranica
                // Traži prvi red koji ima broj ugovora
                int rowIndex = 0; // Definiši brojač
                string brojZahteva = "";
                string brojDokumenta = "abc";
                foreach (var row in rows)
                {
                    // Inkrementiraj brojač
                    rowIndex++;
                    var valueInColumn1 = await row.QuerySelectorAsync("div.column_1");
                    var valueInColumn3 = await row.QuerySelectorAsync("div.column_3");

                    // Pročitajte vrednost iz ćelije
                    if (valueInColumn1 != null && valueInColumn3 != null)
                    {
                        brojZahteva = await valueInColumn1.EvaluateAsync<string>("el => el.innerText");
                        brojDokumenta = await valueInColumn3.EvaluateAsync<string>("el => el.innerText");
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"U redu {rowIndex} kolona \nBroj zahteva ima vrednost:: #{brojZahteva}#, a \nBroj dokumenta je:: #{brojDokumenta}#");
                        }
                        break;
                    }
                    else
                    {
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.MessageBox.Show($"Proveri grid", "Informacija");
                        }
                        break;
                    }
                }
                //await Pauziraj(_page);
                await _page.GetByText(brojZahteva).First.ClickAsync();
                await _page.Locator("div").Filter(new() { HasText = "Pregled zahteva za izmenom za" }).Nth(4).ClickAsync();
                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.GetByText("Br. zah. -").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu").GetByText(brojZahteva).First.ClickAsync();
                //await _page.GetByText(brojZahteva).ClickAsync();
                await _page.Locator("#pregled-zahteva-naslov").GetByText(brojDokumenta).ClickAsync();
                //await Pauziraj(_page);
                //proveri URL 
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{brojDokumenta}/{brojZahteva}");
                //await _page.GetByText($"Unos novog zahteva za izmenu podataka Detalji zahteva za izmenu podataka {brojZahteva}").ClickAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.GetByText("Pregled / Pretraga zahteva za izmenom polisa").ClickAsync();



                // Nađi prvu kreiranu polisu koja nema zahtev za izmenu
                // Pročitaj iz baze idDokument za odgovarajući broj ugovora

                /*
                                string Partner = "";
                                Partner = Okruzenje switch
                                {
                                    "Razvoj" => "[idPartner] IN (36, 120)",
                                    "Proba2" => "[idPartner] IN (36, 120)",
                                    "UAT" => "[idPartner] =103",
                                    "Produkcija" => "",
                                    _ => throw new ArgumentException($"Nepoznati partner ID {Partner} na okruženju: {Okruzenje})"),
                                };
                                int BrojDokumenta;
                                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = 1001 AND {Partner};";
                */
                //await Pauziraj(_page);

                /*
                Server = Okruzenje switch
                {
                    "Razvoj" => "10.5.41.99",
                    "Proba2" => "49.13.25.19",
                    "UAT" => "10.41.5.5",
                    "Produkcija" => "",
                    _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                };
                */
                Server = OdrediServer(Okruzenje);

                int BrojDokumenta;
                /****************
                int idKorisnik = 0;
                idKorisnik = RucnaUloga switch
                {
                    "Bogdan" => 1001,
                    "Mario" => 1002,

                    _ => throw new ArgumentException($"Nepoznati idKorisnik:  + {idKorisnik}"),
                };
                ******************/
                string qBrojDokumenta = $"SELECT MIN ([Dokument].[idDokument]) FROM [MtplDB].[mtpl].[Dokument] " +
                                        $"LEFT JOIN [MtplDB].[mtpl].[ZahtevZaIzmenu] ON [Dokument].[idDokument] = [ZahtevZaIzmenu].[idDokument] " +
                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = {IdLice} AND [datumIsteka] > CAST(GETDATE() AS DATE);";
                // Konekcija sa bazom
                //string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                string connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionString))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qBrojDokumenta, konekcija))
                    {
                        object BrojDokumenta1 = command.ExecuteScalar();
                        BrojDokumenta = BrojDokumenta1 != DBNull.Value ? Convert.ToInt32(BrojDokumenta1) : 0;
                        //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
                        //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
                    }
                    konekcija.Close();
                }
                //Treba ubaciti proveru o ostalih slučajeva. kada je u izradi.... i ako nema polisa ....
                Console.WriteLine($"ID dokumenta je na okruženju '{Okruzenje}' je: {BrojDokumenta}.\n");
                await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/" + BrojDokumenta);
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{BrojDokumenta}");
                //await Pauziraj(_page);
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Novi zahtev za izmenu" }).ClickAsync();
                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.PauseAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepenaDigitalno odobrenje ---").ClickAsync();
                //await _page.Locator("#selTipZahteva").GetByText("Izmena podataka").ClickAsync();
                //await _page.GetByText("Tip zahteva", new() { Exact = true }).ClickAsync();
                await _page.Locator("//e-select[@id='selTipZahteva']//div[@class='multiselect-dropdown input']").ClickAsync();
                //await _page.Locator("#selTipZahteva span").Filter(new() { HasText = "---" }).ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                //await _page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await _page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                await _page.Locator("#selTipZahteva").GetByText("Izmena premijskog stepena").ClickAsync();

                //await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();

                await _page.Locator("#inpNaslov input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpNaslov input[type=\"text\"]").FillAsync("Menjam premijski stepen");
                await _page.GetByText("Tekst zahteva").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Tekst zahteva" }).Locator("textarea").FillAsync($"Ovo je promena premijskog stepena za dokument br. {BrojDokumenta}");


                /****************************************
                Provera da li je BO dobio mejl da je dokument prenos obrazaca vraćen u izradu
                *****************************************/
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji zahtev" }).ClickAsync();


                brojZahteva = (int.Parse(brojZahteva) + 1).ToString();

                await _page.GetByText("Broj dokumenta:").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2313").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();

                //await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.GetByText("Izmena premijskog stepena").Nth(3).ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("21.01.2025").ClickAsync();
                //await _page.GetByText("Menjam opšte podatke").ClickAsync();
                //await _page.Locator("#grid_zahtevi_za_izmenu").GetByText("2321").ClickAsync();
                //await _page.Locator($"column column_1 txt-center").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();

                /******************************************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je i kome otišao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                **********************************************/
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                //Sada se izloguj, pa uloguj kao BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");



                //Uloguj se kao BO


                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.PauseAsync();

                string dok = BrojDokumenta.ToString();
                await _page.GetByText($"Imate novi zahtev za korekciju premijskog stepenaDokument možete pogledati klikom na link: {dok}").HoverAsync();


                await _page.Locator($"//a[.='{dok}']").ClickAsync();

                await Pauziraj(_page);
                //await _page.GetByText(BrojDokumenta.ToString()).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");


                await _page.GetByText("---RealizovanDelimično realizovanOdbijen ---").ClickAsync();
                await _page.Locator("#selStatusZahteva").GetByText("Odbijen").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").ClickAsync();
                await _page.Locator("e-text").Filter(new() { HasText = "Odgovor" }).Locator("textarea").FillAsync("Ovo je odgovor - odbijen.");
                await _page.Locator("button").Filter(new() { HasText = "Pošalji odgovor" }).ClickAsync();

                /******************************
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.MessageBox.Show("Proveri da li je agentu stigao mejl", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ******************************/
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await ArhivirajVest(_page, "Imate novi zahtev za izmenomDokument možete pogledati klikom na link:", dok);

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");



                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("bogdan.mandaric@eonsystem.com");
                //await _page.Locator("input[type=\"password\"]").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();



                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.GetByText("Odogovoreno je na zahtev za").First.ClickAsync();
                //await _page.GetByText($"{dok}").First.ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
                //await _page.PauseAsync();

                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu button").First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Skloni panel" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pregled zahteva" }).First.ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                //await _page.Locator("a").Filter(new() { HasText = "Odogovoreno na zahtev za" }).First.ClickAsync();
                //await _page.Locator(".btnArhiviraj > .left").First.ClickAsync();
                //await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.Locator(".korisnik > a").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();












                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }

        }



        [Test]
        public async Task AO_10_RazduznaLista()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Razdužne liste AO";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Nova razdužna lista i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Nova razdužna lista").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");





                //await _page.PauseAsync();

                //await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.GetByLabel("Februar 7,").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync("07.02.2025.");
                //await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.GetByLabel("Februar 8,").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync("08.02.2025.");
                //await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.GetByLabel("Februar 7,").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync("07.02.2025.");

                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));
                //await _page.GetByLabel(NextDate.ToString("MMMM dd, yyyy")).ClickAsync();
                await _page.GetByLabel(CurrentDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik).ClickAsync();

                await _page.GetByText("Arhivski magacin Arhivski").ClickAsync();

                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                int PoslednjiDokumentStroga;
                string qPoslednjiDokumentStroga = "SELECT MAX ([IdDokument]) FROM [StrictEvidenceDB].[strictevidence].[tDokumenta];";
                /*
                Server = Okruzenje switch
                {
                    "Razvoj" => "10.5.41.99",
                    "Proba2" => "49.13.25.19",
                    "UAT" => "10.41.5.5",
                    "Produkcija" => "",
                    _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                };
                */
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
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/{PoslednjiDokumentStroga + 1}");


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
                //await _page.PauseAsync();

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


                //await Pauziraj(_page);
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //Ovde treba dodati proveru štampanja

                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                /**********************
                                await _page.Locator(".korisnik").ClickAsync();
                                await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                                ***************/
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                /**********************
                await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                await _page.Locator("css = [type='password']").ClickAsync();
                await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                await _page.Locator("a").First.ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
************************/

                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //string tekst = "Imate novi dokument \"Otpis\" za verifikacijuDokument možete pogledati klikom na link: ";
                //await _page.GetByText(tekst + oznakaDokumenta).ClickAsync();
                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();

                //Imate novi dokument "Razdužna lista (OSK)" za verifikaciju
                //Dokument možete pogledati klikom na link: RAO-88888-2025-1070
                // $"Imate novi dokument \"Ulaz u centralni magacin\" za verifikaciju\n" +
                //                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                //string ocekivaniTekst = $"Imate novi dokument \"Razdužna lista (OSK)\" za verifikaciju<br>Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                //await _page.GetByText(ocekivaniTekst).ClickAsync();

                //await _page.GetByText($"{PoslednjiDokumentStroga + 2}").ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{PoslednjiDokumentStroga + 1}" }).ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{oznakaDokumenta}" }).ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/{PoslednjiDokumentStroga + 1}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/1/Autoodgovornost/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                //await _page.Locator(".ico-ams-logo").ClickAsync();

                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                // kada se Davor odjavi, uloguj se kao Bogdan i proveri vesti

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
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


        [Test]
        public async Task AO_11_Otpis()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi otpis").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/3/0");


                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d")).First.ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(CurrentDate.ToString("MMMM d")).First.ClickAsync();

                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik).ClickAsync();

                await _page.Locator("textarea").First.ClickAsync();
                await _page.Locator("textarea").First.ClickAsync();
                await _page.Locator("textarea").First.FillAsync("Dragan je prosuo kafu i rakiju!");

                //await Pauziraj(_page);

                // Pronađi prvi slobodan serijski broj za polisu AO
                /*
                 Server = Okruzenje switch
                 {
                     "Razvoj" => "10.5.41.99",
                     "Proba2" => "49.13.25.19",
                     "UAT" => "10.41.5.5",
                     "Produkcija" => "",
                     _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                 };
                 */
                Server = OdrediServer(Okruzenje);

                string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                /*
                                string Lokacija = "(7,8)";

                                                if (OsnovnaUloga == "BackOffice")
                                                {
                                                    Lokacija = "(4)";
                                                }
                                                */


                string Lokacija = "(7, 8)";

                if (AkorisnickoIme == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme);

                if (NacinPokretanjaTesta == "automatski" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }




                //Nalaženje poslednjeg serijskog broja koji je u sistemu
                string qZaduženiObrasciAOBezPolise = $"SELECT [tObrasci].[SerijskiBroj] FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                                     $"LEFT JOIN [StrictEvidenceDB].[strictevidence].[tIzdatePolise] ON [tObrasci].[SerijskiBroj] = [tIzdatePolise].[SerijskiBroj] " +
                                                     $"WHERE [tObrasci].[IdTipObrasca] = 1 AND [IDLokacijaZaduzena] IN {Lokacija} AND [IDLokacijaZaduzena] NOT IN (-2, -1, 5) AND [_DatumDo] IS NULL AND [tIzdatePolise].[SerijskiBroj] IS NULL " +
                                                     $"ORDER BY [tObrasci].[SerijskiBroj] ASC;";
                int SerijskiBrojAO = 0;
                try
                {
                    using SqlConnection konekcija = new(connectionString);
                    konekcija.Open();
                    using SqlCommand cmd = new(qZaduženiObrasciAOBezPolise, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();

                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        SerijskiBrojAO = Convert.ToInt32(reader["SerijskiBroj"]);
                        string qSerijskiBrojAONijeUpotrebljen = $"SELECT COUNT (*) FROM [MtplDB].[mtpl].[Dokument] " +
                                                        $"INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] " +
                                                        $"WHERE [idProizvod] = 1 AND [serijskiBrojAO] LIKE '{SerijskiBrojAO}%';";

                        using SqlConnection konekcija2 = new(connectionString);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new(qSerijskiBrojAONijeUpotrebljen, konekcija2);
                        int imaPolisuAO = (int)(cmd2.ExecuteScalar() ?? 0);
                        Console.WriteLine($"Serijski brojevi obrazaca AO kod Bogdana: {SerijskiBrojAO}, Ima polisu AO: {imaPolisuAO}");

                        // Izbor slobodnog serijskog broja polise AO
                        if (imaPolisuAO == 0)
                        {
                            Console.WriteLine($"Prvi obrazac koji je slobodan je: {SerijskiBrojAO}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");
                            break;
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }

                string strSerijskiBrojAO = SerijskiBrojAO.ToString();
                Console.WriteLine($"\nKreiraću razdužnu listu za serijski broj obrasca AO: {strSerijskiBrojAO}\n");

                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync(strSerijskiBrojAO);
                string konCifraOd = IzracunajKontrolnuCifru($"{SerijskiBrojAO}");
                //await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);

                await _page.Locator("button").Filter(new() { HasText = "+" }).ClickAsync();

                string konCifraDo = IzracunajKontrolnuCifru($"{SerijskiBrojAO + 24}");
                //await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                //await _page.GetByText("Brojevi obrazaca: ").ClickAsync();
                //await _page.Locator("#notify0 button").ClickAsync();
                await Pauziraj(_page);
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync(strSerijskiBrojAO);
                konCifraDo = IzracunajKontrolnuCifru($"{SerijskiBrojAO}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("#btnObrisi button").ClickAsync();

                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync(strSerijskiBrojAO);
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);

                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync(strSerijskiBrojAO);
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();


                //await _page.PauseAsync();

                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                int PoslednjiDokumentStroga;
                string qPoslednjiDokumentStroga = "SELECT MAX ([IdDokument]) FROM [StrictEvidenceDB].[strictevidence].[tDokumenta];";
                /*
                Server = Okruzenje switch
                {
                    "Razvoj" => "10.5.41.99",
                    "Proba2" => "49.13.25.19",
                    "UAT" => "10.41.5.5",
                    "Produkcija" => "",
                    _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
                };
                */
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
                //await Pauziraj(_page);
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                //await Pauziraj(_page);

                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/3/{PoslednjiDokumentStroga}");
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //await Pauziraj(_page);
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                //await _page.Locator("css = [type='password']").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await Pauziraj(_page);
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //string tekst = "Imate novi dokument \"Otpis\" za verifikacijuDokument možete pogledati klikom na link: ";
                //await _page.GetByText($"{oznakaDokumenta}").HoverAsync();
                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Stroga-Evidencija/1/Autoodgovornost/dokument/3/{PoslednjiDokumentStroga}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/dokument/3/{PoslednjiDokumentStroga}");
                //await _page.PauseAsync();
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();










                /*
                            await _page.Locator(".ico-ams-logo").ClickAsync();

                            await _page.Locator(".korisnik").ClickAsync();
                            await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();

                            //await Pauziraj(_page);

                */


                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }


        }


        [Test]
        public async Task AO_12_InformativnoKalkulisanje()
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
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                // Klik na Pregled / Pretraga obrazaca stroge evidencije
                await _page.GetByText("Informativno kalkulisanje").ClickAsync();
                // Provera da li se otvorila stranica pregled obrazaca
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Informativno-kalkulisanje");

                await Pauziraj(_page);

                await _page.Locator(".multiselect-dropdown").First.ClickAsync();
                await _page.GetByText("Regularno (u trajanju od").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox).PressAsync("Enter");
                await _page.Locator("#chkRaspon div").Nth(3).ClickAsync();
                await _page.Locator("#selpremijskiStepenOd").GetByText("123456789101112 4").ClickAsync();
                await _page.Locator("#selpremijskiStepenOd").GetByText("1", new() { Exact = true }).ClickAsync();
                await _page.GetByText("123456789101112 4").ClickAsync();
                await _page.Locator("#selpremijskiStepenDo").GetByText("5").ClickAsync();
                await _page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.Locator("#selVrstaVozila").GetByText("Putničko vozilo").ClickAsync();
                await _page.Locator("#snaga").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("#snaga").GetByRole(AriaRole.Textbox).FillAsync("100");
                await _page.Locator("#snaga").GetByRole(AriaRole.Textbox).PressAsync("Enter");
                await _page.Locator("#snaga").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.GetByText("---Taxi voziloRent a carVozilo invalida ---").ClickAsync();
                await _page.GetByText("Taxi vozilo").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj" }).ClickAsync();
                await _page.Locator("div:nth-child(2) > .korak0 > div > .col-4 > #selVrstaOsiguranja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Regularno (u trajanju od").Nth(2).ClickAsync();
                await _page.Locator("div:nth-child(2) > .korak2 > div:nth-child(2) > .col-3 > #selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Autobusi").Nth(1).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox).Nth(3).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox).Nth(3).FillAsync("50");
                await _page.Locator("div:nth-child(2) > #vozilo > #podvrstaVozila > #selPodvrsta > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Autobusi za međugradski javni").ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Izračunaj" }).ClickAsync();
                await _page.GetByText("Vrsta osiguranja").Nth(2).ClickAsync();
                await _page.GetByText("Vrsta vozila", new() { Exact = true }).Nth(2).ClickAsync();
                await _page.GetByText("Autobusi", new() { Exact = true }).Nth(3).ClickAsync();
                await _page.Locator(".prikazInfoKalkulacije > div:nth-child(2) > div").First.ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Štampaj" }).ClickAsync();
                await ProveriStampuPdf(_page, "Štampaj", "Informativno kalkulisanje");
                await _page.GetByRole(AriaRole.Button, new() { Name = "" }).Nth(2).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Izračunaj" }).ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Štampaj" }).ClickAsync();

                await ProveriStampuPdf(_page, "Štampaj", "Informativno kalkulisanje");

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
//await _page.Context.ClearCookiesAsync();  // ili page.ClearCacheAsync() ako dodatno želiš

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

