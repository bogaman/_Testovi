namespace Proba2
{
    [TestFixture, Order(1)]
    [Parallelizable(ParallelScope.Self)]
    public partial class OsiguranjeVozila : Osiguranje
    {

        //private static string NacinPokretanjaTesta = Environment.GetEnvironmentVariable("BASE_URL") ?? "ručno";
        //private static string logFilePath = "C:/_Projekti/AutoMotoSavezSrbije/Logovi/test_log.txt";
        //private static string logFilePath = Path.Combine("C:/_Projekti/AutoMotoSavezSrbije/Logovi/", "test_log.txt");

        /***************************************
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

        #region AuthState
        // Klasa za reprezentaciju AuthState
        public class AuthState
        {
            [JsonPropertyName("cookies")]
            public Cookie[] Cookies { get; set; } = [];

            [JsonPropertyName("origins")]
            public object[] Origins { get; set; } = [];
        }
        #endregion AuthState

        #region Cookie
        // Klasa za reprezentaciju kolačića
        public class Cookie
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = "";

            [JsonPropertyName("value")]
            public string Value { get; set; } = "";

            [JsonPropertyName("domain")]
            public string Domain { get; set; } = "";

            [JsonPropertyName("path")]
            public string Path { get; set; } = "";

            [JsonPropertyName("expires")]
            public double Expires { get; set; } = 0;

            [JsonPropertyName("httpOnly")]
            public bool HttpOnly { get; set; } = false;

            [JsonPropertyName("secure")]
            public bool Secure { get; set; } = false;

            [JsonPropertyName("sameSite")]
            public string SameSite { get; set; } = "";
        }

        #endregion Cookie



        #region Testovi









        [Test]
        public async Task AO_1_SE_PregledPretragaObrazaca()
        {
            try
            {
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{serijskiBrojObrasca}'.");
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
                    LogovanjeTesta.LogException(ex, "Greška prilikom provere tekst boksova sa vrednostima prom1–prom4.");
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
        public async Task AO_2_SE_PregledPretragaDokumenata()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page!.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.");
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.");
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
        public async Task AO_3_SE_UlazPrenosObrazaca()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
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

                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.GetByText("Osiguranje vozila").HoverAsync(); //Pređi mišem preko teksta Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync(); //Klikni na tekst Osiguranje vozila
                await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).First.ClickAsync(); //Klikni u meniju na Autoodgovornost
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); //Provera da li se otvorila stranica sa pregledom polisa AO

                await Pauziraj(_page);
                // Provera da li stranica sadrži grid Dokumenta - Polise AO
                tipGrida = "Polise AO";
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
                                                                                                                   // Proveri da li stranica sadrži grid Dokumenta stroge evidencije i 
                                                                                                                   // da li radi filter na gridu Dokumenta stroge evidencije
                tipGrida = "Dokumenta stroge evidencije za polise AO";
                //lokatorGrida = "//e-grid[@id='grid_dokumenti']*";
                await ProveraPostojiGrid(_page, tipGrida);
                await ProveriFilterGrida(_page, "Magacin kovnice", tipGrida, 3);

                /************************************************
                Formira se ulaz u Centralni magacin
                *************************************************/
                // Klik u levom meniju na Novi ulaz u centralni magacin i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");

                // Popuni broj otpremnice (unosi se trenutni datum i vreme) i izaberi ko se zadužuje (Centralni magacin 1)
                await _page.Locator("#inpBrojOtpremice").ClickAsync();
                await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("yyyyMMdd-hhmmss"));

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Magacin, false);

                //Nalaženje poslednjeg iskorišćenog serijskog broja obrasca u Strogoj evidenciji
                PoslednjiSerijski = PoslednjiSerijskiStroga(1, $" AND [SerijskiBroj] BETWEEN {MinSerijskiAO} AND {MaxSerijskiAO}");
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
                PoslednjiDokument = PoslednjiDokumentStroga();

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

                // Unesi broj polise Od i Do (unosim ukupno tri polise)
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

                //Pošalji na verifikaciju
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije AO:");

                //Pročitaj oznaku novog dokumenta
                oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();

                await _page.GotoAsync($"{PocetnaStrana}/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                //Pročitaj novo stanje poslednjeg  mejla pred vraćanje u izradu
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //Klik na dugme Vrati u izradu
                await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();

                //Provera mejla za BO da je dokument vraćen u izradu
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);


                //Provera vesti za BO da je dokument vraćen u izradu
                /**************************************************
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                ocekivaniTekst = $"Dokument \"Ulaz u centralni magacin\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                //await VestPostoji(_page, "Dokument \"Ulaz u centralni magacin\" je vraćen u izradu. \nDokument možete pogledati klikom na link: ", oznakaDokumenta);
                ************************************************/


                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/{PoslednjiDokument + 1}");

                //Sada obriši dokument
                await _page.Locator("#btnObrisiDokument button").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");

                // Proveri da li je za BO obrisana vest za verifikaciju jer je dokument obrisan
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                //ocekivaniTekst = $"{oznakaDokumenta}";
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
                await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();

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

                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //Provera mejla za BO da je dokument poslat na verifikaciju
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);


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
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);







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
                await Pauziraj(_page);
                /***************************************
                Prenos iz Centralnog magacina ka agentu
                ***************************************/
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

                await IzaberiOpcijuIzListe(_page, "#selZaduzenje", Asaradnik_, false);

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


                /************************************************************* 
                 Provera da li je agent dobio mejl da ima novi prenos obrazaca
                **************************************************************/
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);


                // Odjavljuje se BackOffice
                await IzlogujSe(_page);

                // Prijavljuje se agent




                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //await _page.Locator(".ico-ams-logo").ClickAsync();
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");

                /***********************************************
                // Provera vesti o dokumentu za verifikaciju za agenta
                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                //await ProveraVestPostoji(_page, ocekivaniTekst);
                ************************************************/
                // Otvori dokument za verifikaciju
                //await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await _page.GotoAsync($"{PocetnaStrana}/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");

                /****************************************
                Provera da li je BO dobio mejl da je dokument prenos obrazaca vraćen u izradu
                *****************************************/
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();

                await ProveriStatusSlanjaMejla(PrethodniZapisMejla);

                //LogovanjeTesta.LogMessage($"❌ Proverava se da li je vest arhivirana za agenta nakon vraćanja u izradu: {oznakaDokumenta}.", false);

                await _page.Locator(".ico-ams-logo").ClickAsync();
                //await ProveraVestJeObrisana(_page, ocekivaniTekst);





                // Odjavljuje se agent
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");

                // Prijavljuje se BackOffice


                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");



                //Provera vesti o vraćenom dokumentu za BackOffice
                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/{PoslednjiDokument + 2}");

                /**************************
                Provera da li agent ima mejl o novom prenosu
                ***************************/
                //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //vrati nazad na verifikaciju
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);

                // Proveri da li je vest arhivirana za BO nakon vraćanja na verifikaciju
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                /*
                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je vraćen u izradu.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
                */
                //await ProveraVestJeObrisana(_page, ocekivaniTekst);

                // Odjavljuje se BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");




                // Prijavljuje se agent


                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);

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

                /**************************
                Provera da li BO dobio mejl da je prenos verifikovan
                ***************************/
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                LogovanjeTesta.LogMessage($"✅ Poslednji mejl -> ID: {PrethodniZapisMejla.PoslednjiID}, IDMail: {PrethodniZapisMejla.PoslednjiIDMail}, Status: {PrethodniZapisMejla.Status}, Opis: {PrethodniZapisMejla.Opis}, Datum: {PrethodniZapisMejla.Datum}, Subject: {PrethodniZapisMejla.Subject}", false);

                //await _page.PauseAsync();
                //if (IdLice_ == 1002)
                //await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();

                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran verifikovan dokument stroge evidencije AO:");
                //await ProveriStatusSlanjaMejla(PrethodniZapisMejla);

                /*********************************************************
                ocekivaniTekst = $"Imate novi dokument \"Prenos zaduženja polisa\" za verifikaciju\n" +
                                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");

                //await ProveraVestJeObrisana(_page, ocekivaniTekst);
                ************************************************************/
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");



                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");


                ocekivaniTekst = $"Dokument \"Prenos zaduženja polisa\" je verifikovan.\n" +
                                 $"Dokument možete pogledati klikom na link: {oznakaDokumenta}";
                await ProveraVestPostoji(_page, ocekivaniTekst);
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Dashboard");
                await ArhivirajVest(_page, ocekivaniTekst, oznakaDokumenta);
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
        public async Task AO_4_Polisa(string _rb, string _tipPolise, string _premijskaGrupa, string _popusti, string _tipUgovaraca, string _tipLica1, string _tipLica2, string _brojDana, string _tegljac, string _podgrupa, string _zapremina, string _snaga, string _nosivost, string _brojMesta, string _oslobodjenPoreza, string _mb1, string _mb2, string _pib1, string _pib2, string _platilac1, string _platilac2)
        {
            try
            {
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                await _page!.Locator(".ico-ams-logo").ClickAsync();
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

                if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme_);

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

                await DatumOd(_page);
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

                //await page.GetByText("Teretno vozilo").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Autobusi").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Vučna vozila").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Specijalna motorna vozila").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Motocikli").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Priključna vozila").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Radna vozila").ClickAsync();
                //await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await page.GetByText("Putničko vozilo").ClickAsync();

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
                    //await page.Locator("#seldrzavaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("//div[@id='drzavaVozila']//div[@class='control ']").ClickAsync();

                    await _page.Locator("xpath=//e-select[@data-source-id='drzavaVozilaData']//div[@class='multiselect-dropdown input']").ClickAsync();
                    //await page.Locator("xpath=//e-select[@data-source-id='drzavaVozilaData']//div[@class='multiselect-dropdown input']").FillAsync("alb"); // unos početnih slova
                    await _page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("alb"); // unos početnih slova
                                                                                                                                                                                                           //await page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
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
                int PoslednjiBrojDokumenta = OdrediBrojDokumenta();


                await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                //Ovde proveri URL
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{PoslednjiBrojDokumenta + 1}");
                //await _page.EvaluateAsync("location.reload(true);");

                //await _page.PauseAsync();

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
                        LogovanjeTesta.LogException(ex, $"❌ Greška tokom pokušaja #{brojPokusaja}");
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


                await page.Locator("button").Filter(new() { HasText = "Kalkuliši" }).ClickAsync();
                await page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/1/Autoodgovornost/Dokument/6514");
                await page.GetByText("Registarski broj polise", new() { Exact = true }).ClickAsync();
                await page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await page.GetByText("Broj dokumenta:").ClickAsync();
                await page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                await page.Locator("button").Filter(new() { HasText = "Obriši dokument" }).ClickAsync();
                await page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();


                            //var popup = await page.RunAndWaitForPopupAsync(async => {
                            //await page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                            //});
                            //await page.PauseAsync();
                            //await page.GetByText("Podaci uspešno snimljeni").ClickAsync();
                            await page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
                            var locator = page.Locator("//div[@id='notify0']");
                            //await Expect(locator).ToHaveTextAsync("Nedostaju podaci ili uneti podaci nisu validni");
                            //await Expect(locator).ToHaveTextAsync("Podaci uspešno kalkulisani");
                            // Pomeranje miša za 100px dole i desno
                            int newX = 200;
                            int newY = 200;
                            await page.Mouse.MoveAsync(newX, newY);

                            //Thread.Sleep(5000);

                            await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
                            await page.GetByText("Da li ste sigurni da želite").ClickAsync();

                            await page.GetByRole(AriaRole.Button, new() { Name = "Da!"/*, Exact = false }).ClickAsync();
                            //await page.GetByRole(AriaRole.Button, new() { Name = "Ne", Exact = true }).ClickAsync();
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
                    //var SertifikatName_ = KorisnikLoader5.Korisnik3?.Sertifikat ?? string.Empty;
                    //var SertifikatName_ = AKorisnik_?.Sertifikat ?? string.Empty;
                    //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName(SertifikatName_))).AsTreeItem();


                    var treeItem = Retry.WhileNull(
                        () => treeView?.FindFirstDescendant(cf =>
                            cf.ByControlType(ControlType.TreeItem).And(cf.ByName(SertifikatName_)))?.AsTreeItem(),
                        TimeSpan.FromSeconds(5)).Result;


                    //Assert.IsNotNull(treeItem, "TreeItem 'Bogdan Mandarić' not found");

                    // Klik na TreeItem
                    if (treeItem != null)
                    {
                        treeItem.Click();
                    }
                    else
                    {
                        throw new Exception($"TreeItem '{SertifikatName_}' not found.");
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
        public async Task AO_5_ZahtevZaIzmenu_Podaci()
        {
            await Pauziraj(_page);
            try
            {
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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
                //await Pauziraj(_page!);
                await _page.GetByText(brojZahteva).First.ClickAsync();
                await _page.Locator("div").Filter(new() { HasText = "Pregled zahteva za izmenom za" }).Nth(4).ClickAsync();
                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.GetByText("Br. zah. -").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu").GetByText(brojZahteva).First.ClickAsync();
                //await _page.GetByText(brojZahteva).ClickAsync();
                await _page.Locator("#pregled-zahteva-naslov").GetByText(brojDokumenta).ClickAsync();
                //await Pauziraj(_page!);
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
                //await Pauziraj(_page!);

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
                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = {IdLice_} AND [datumIsteka] > CAST(GETDATE() AS DATE);";
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
                //await Pauziraj(_page!);
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
                //await page.Locator("#selTipZahteva span").Filter(new() { HasText = "---" }).ClickAsync();
                //await page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                //await page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
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
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla);
                //Sada se izloguj, pa uloguj kao BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, "/Login");


                //uloguj se kao BO


                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
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
                //await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/1/Autoodgovornost/Dokument/{dok}/{brojZahteva}");
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
                //await _page.Locator(".ico-ams-logo").ClickAsync();
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();



                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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
        public async Task AO_6_ZahtevZaIzmenu_PremijskiStepen()
        {
            await Pauziraj(_page);
            try
            {
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla(); //Poslednji zapis o poslatim mejlovima pre prvog slanja novog mejla
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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
                //await Pauziraj(_page!);
                await _page.GetByText(brojZahteva).First.ClickAsync();
                await _page.Locator("div").Filter(new() { HasText = "Pregled zahteva za izmenom za" }).Nth(4).ClickAsync();
                await _page.Locator(".pregled-zahteva-gore").ClickAsync();
                await _page.GetByText("Br. zah. -").ClickAsync();
                await _page.Locator("#grid_zahtevi_za_izmenu").GetByText(brojZahteva).First.ClickAsync();
                //await _page.GetByText(brojZahteva).ClickAsync();
                await _page.Locator("#pregled-zahteva-naslov").GetByText(brojDokumenta).ClickAsync();
                //await Pauziraj(_page!);
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
                //await Pauziraj(_page!);

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
                                        $"WHERE [ZahtevZaIzmenu].[idDokument] IS NULL AND [idProizvod] = 1 AND [Dokument].[idStatus] = 2 AND [Dokument].[idkorisnik] = {IdLice_} AND [datumIsteka] > CAST(GETDATE() AS DATE);";
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
                //await Pauziraj(_page!);
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
                //await page.Locator("#selTipZahteva span").Filter(new() { HasText = "---" }).ClickAsync();
                //await page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
                //await page.Locator(".zahtev > div:nth-child(5)").ClickAsync();
                //await page.GetByText("---Izmena podatakaIzmena premijskog stepena ---").ClickAsync();
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
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla);
                //Sada se izloguj, pa uloguj kao BackOffice
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, "/Login");


                //Uloguj se kao BO


                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
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
                //await _page.Locator(".ico-ams-logo").ClickAsync();
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();



                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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
        public async Task AO_7_RazduznaLista()
        {
            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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

                //await page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await page.GetByLabel("Februar 7,").ClickAsync();
                //await page.Locator("e-calendar input[type=\"text\"]").FillAsync("07.02.2025.");
                //await page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await page.GetByLabel("Februar 8,").ClickAsync();
                //await page.Locator("e-calendar input[type=\"text\"]").FillAsync("08.02.2025.");
                //await page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await page.GetByLabel("Februar 7,").ClickAsync();
                //await page.Locator("e-calendar input[type=\"text\"]").FillAsync("07.02.2025.");

                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                //await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));
                //await _page.GetByLabel(NextDate.ToString("MMMM dd, yyyy")).ClickAsync();
                await _page.GetByLabel(CurrentDate.ToString("MMMM d,")).ClickAsync();
                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik_).ClickAsync();

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


                //await Pauziraj(_page!);
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


                //await Pauziraj(_page!);
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
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
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
                LogovanjeTesta.LogException(ex, "Greška u testu");
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                throw;
            }


        }


        [Test]
        public async Task AO_8_Otpis()
        {
            try
            {
                await Pauziraj(_page);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
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
                await _page.GetByText(Asaradnik_).ClickAsync();

                await _page.Locator("textarea").First.ClickAsync();
                await _page.Locator("textarea").First.ClickAsync();
                await _page.Locator("textarea").First.FillAsync("Dragan je prosuo kafu i rakiju!");

                //await Pauziraj(_page!);

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

                if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme_);

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
                await Pauziraj(_page!);
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
                //await Pauziraj(_page!);
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                //await Pauziraj(_page!);

                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/3/{PoslednjiDokumentStroga}");
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //await Pauziraj(_page!);
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
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await Pauziraj(_page!);
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

                            //await Pauziraj(_page!);

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
        public async Task ZK_1_SE_PregledPretragaObrazaca()
        {

            try
            {
                await UlogujSe(_page!, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Zeleni karton" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

                // Klik na Pregled / Pretraga obrazaca stroge evidencije
                await _page.GetByText("Pregled / Pretraga obrazaca").ClickAsync();
                // Provera da li se otvorila stranica pregled obrazaca
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-obrazaca");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Obrasci polisa ZK";
                //string lokatorGrida = "//e-grid[@id='grid_obrasci']";

                string kriterijumFiltera = "Bogdan";
                await ProveraPostojiGrid(_page, tipGrida);
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

                /* Ovaj kod ne vraća dobar broj kolona, proveriti zašto
                //ili
                var kolone = _page.Locator("//e-grid[@id='grid_obrasci']//div[contains(@class, 'ag-row')][1]//div[contains(@class, 'ag-cell')]");
                int brojKolona1 = await kolone.CountAsync();
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Broj kolona 2: {brojKolona1}", "Informacija", MessageBoxButtons.OK);
                }
                */

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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{serijskiBrojObrasca}'.");
                }
                //await _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']").ClickAsync();

                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Kartica/{serijskiBrojObrasca}"); // Provera da li se otvorila odgovarajuća kartica obrasca

                try
                {
                    string[] expectedValues = { serijskiBrojObrasca, "Zeleni karton", lokacijaObrasca, brojPolise };
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
                    LogovanjeTesta.LogException(ex, "Greška prilikom provere tekst boksova sa vrednostima prom1–prom4.");
                }

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
        public async Task ZK_2_SE_PregledPretragaDokumenata()
        {
            try
            {
                await UlogujSe(_page!, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Zeleni karton" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

                // Klik na Pregled / Pretraga dokumenata stroge evidencije
                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                // Provera da li se otvorila stranica pregled dokumenata
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-dokumenata");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Dokumenti Stroge evidencije za polise ZK";
                //string lokatorGrida = "//e-grid[@id='grid_dokumenti']";


                string kriterijumFiltera = "kovnice";
                await ProveraPostojiGrid(_page, tipGrida);
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
                    // Pronađi lokator za ćeliju koja sadrži Serijski broj obrasca polise ZK
                    var celijaSerijskiBrojObrasca = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{oznakaDokumenta}']");

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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.");
                }
                //await _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']").ClickAsync();
                //await _page.PauseAsync();
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.");
                }

                await ProveriStampuPdf(_page, "Štampaj dokument", "Štampa dokumenta Stroge evidencije za ZK:");
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
        public async Task ZK_3_SE_UlazPrenosObrazaca()
        {

            try
            {
                await Pauziraj(_page!);
                await UlogujSe(_page!, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");


                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.GetByRole(AriaRole.Button, new() { Name = "Zeleni karton" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

                // Proveri da li stranica sadrži grid Dokumenta - Polise
                string tipGrida = "Polise ZK";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Pregled / Pretraga obrazaca i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga obrazaca").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-obrazaca");

                // Proveri da li stranica sadrži grid Obrasci
                tipGrida = "Obrasci polisa ZK";
                await ProveraPostojiGrid(_page, tipGrida);

                // Proveri da li radi filter na gridu
                Trace.Write($"Filter na gridu obrazaca AO za admina - ");
                string ukupanBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                await _page.Locator("div:nth-child(3) > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                await _page.Locator("div:nth-child(3) > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync("otpisanih");
                await _page.Locator("div:nth-child(3) > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms) da se filter učita
                string filtriraniBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                if (Convert.ToInt32(ukupanBrojStrana) > Convert.ToInt32(filtriraniBrojStrana))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"NE RADI");
                    return;
                }

                // Klik na Novi ulaz u centralni magacin i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi ulaz u centralni magacin").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/1/0");

                // Popuni broj otpremnice (unosi se trenutni datum i vreme) i izaberi ko se zadužuje (koji Centralni magacin)
                await _page.Locator("#inpBrojOtpremice").ClickAsync();
                await _page.Locator("//e-input[@id='inpBrojOtpremice']//input[@class='input']").FillAsync(DateTime.Now.ToString("yyyyMMdd-hhmmss"));

                await _page.Locator("#selZaduzenje").ClickAsync();
                if (Okruzenje == "Razvoj")
                {
                    await _page.GetByText("Centralni magacin 1").ClickAsync();
                }
                else if (Okruzenje == "Proba2")
                {
                    await _page.GetByText("Centralni magacin 2").ClickAsync();
                }
                else
                {
                    await _page.GetByText("Centralni magacin", new() { Exact = true }).ClickAsync();
                }

                //Nalaženje poslednjeg serijskog broja obrasca ZK koji je u sistemu
                long PoslednjiSerijskiZK;
                string qMaksimalniSerijskiZK = $"SELECT MAX ([SerijskiBroj]) FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                               $"WHERE [IdTipObrasca] = 4 AND [SerijskiBroj] < 1234567;";
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

                // Konekcija sa bazom StrictEvidenceDB
                string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qMaksimalniSerijskiZK, konekcija))
                    {
                        object PoslednjiSerijskiZK1 = command.ExecuteScalar();
                        PoslednjiSerijskiZK = PoslednjiSerijskiZK1 != DBNull.Value ? Convert.ToInt64(PoslednjiSerijskiZK1) : 0;
                        //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
                        //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
                    }
                    konekcija.Close();
                }

                Console.WriteLine($"Poslednji serijski broj obrasca polise ZK na okruženju '{Okruzenje}' je: {PoslednjiSerijskiZK}.\n");


                // Unesi broj polise Od i Do i testiraj šta se tu dešava
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{(PoslednjiSerijskiZK + 1).ToString("D8")}");
                string konCifraOd = IzracunajKontrolnuCifruZK($"{PoslednjiSerijskiZK + 1}");
                //await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();
                string konCifraDo = IzracunajKontrolnuCifruZK($"{PoslednjiSerijskiZK + 25}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await Pauziraj(_page!);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).Nth(2).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasTextString = "^Dodaj$" }).ClickAsync();
                //await _page.Locator("button:text('Dodaj')").ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                //await Pauziraj(_page!);
                await _page.Locator("#btnObrisi button").ClickAsync();

                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{(PoslednjiSerijskiZK + 1).ToString("D8")}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                await _page.Locator("button").Filter(new() { HasText = "+2" }).ClickAsync();
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{(PoslednjiSerijskiZK + 1).ToString("D8")}");
                konCifraDo = IzracunajKontrolnuCifruZK($"{PoslednjiSerijskiZK + 1}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                //Nalaženje poslednjeg broja dokumenta u Strogoj evidenciji
                string qPoslednjiDokumentZKStroga = "SELECT MAX ([IdDokument]) FROM [StrictEvidenceDB].[strictevidence].[tDokumenta];";
                int PoslednjiDokumentStroga;

                //connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qPoslednjiDokumentZKStroga, konekcija))
                    {
                        PoslednjiDokumentStroga = (int)command.ExecuteScalar();
                    }
                    konekcija.Close();
                }

                Console.WriteLine($"Poslednji broj dokumenta u strogoj evidenciji ZK je: {PoslednjiDokumentStroga}.\n");

                //Snimanje ulaza u Centralni magacin
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/4/Zeleni-karton/Dokument/1/{PoslednjiDokumentStroga + 1}");

                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/1/0");

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Vrati u izradu" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                await _page.Locator("#btnVerifikuj").ClickAsync();


                //Ovde treba dodati proveru štampanja

                //Prenos obrazaca ka agentu
                await _page.GetByText("Novi prenos").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/0");

                await _page.Locator("#selRazduzenje").ClickAsync();
                if (Okruzenje == "Razvoj")
                {
                    await _page.Locator("#selRazduzenje").GetByText("Centralni magacin 1").ClickAsync();
                }
                else if (Okruzenje == "Proba2")
                {
                    //await _page.GetByText("Centralni magacin 2").ClickAsync(); 
                    await _page.Locator("#selRazduzenje").GetByText("Centralni magacin 2").ClickAsync();
                }
                else
                {
                    //await _page.GetByText("Centralni magacin", new() { Exact = true }).ClickAsync();
                    await _page.Locator("#selRazduzenje").GetByText("Centralni magacin").ClickAsync();
                }
                //await _page.Locator("#selRazduzenje").GetByText("Centralni magacin 1").ClickAsync();
                await _page.Locator("#selZaduzenje").ClickAsync();
                if (Okruzenje == "Razvoj")
                {
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik_).ClickAsync();
                }
                else if (Okruzenje == "Proba2")
                {
                    //await _page.GetByText("Centralni magacin 2").ClickAsync(); 
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik_).ClickAsync();
                }
                else
                {
                    //await _page.GetByText("90202 - Bogdan Mandarić", new() { Exact = true }).ClickAsync();
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik_).ClickAsync();
                }

                //await _page.PauseAsync();

                //await _page.Locator("#selZaduzenje").GetByText("Bogdan Mandarić").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{(PoslednjiSerijskiZK + 1).ToString("D8")}");
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);

                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{(PoslednjiSerijskiZK + 1).ToString("D8")}");
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                await _page.Locator("#inpOdBrojaLista").ClickAsync();
                await _page.Locator("#inpDoBrojaLista").ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/{PoslednjiDokumentStroga + 2}");
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();

                //await _page.PauseAsync();
                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("bogdan.mandaric@eonsystem.com");
                //await _page.Locator("css = [type='password']").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //await Pauziraj(_page!);
                //await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").ClickAsync();      
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{oznakaDokumenta}" }).ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/{PoslednjiDokumentStroga + 2}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/{PoslednjiDokumentStroga + 2}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                //await IzlogujSe(_page);
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
        }


        [Test]
        public async Task ZK_4_Polisa()
        {
            //await _page.PauseAsync();
            try
            {
                await Pauziraj(_page!);
                await UlogujSe(_page!, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");


                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null when calling NovaPolisa.");
                await NovaPolisa(_page, "Novi zeleni");
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Dokument/0");

                // Pronađi odgovarajuću polisu AO koja nema ZK
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

                string connectionStringMtpl = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                string istekla = DateTime.Now.ToString("yyyy-MM-dd");
                Console.WriteLine(istekla);
                int GranicniBrojdokumenta;
                if (Okruzenje == "Razvoj")
                {
                    GranicniBrojdokumenta = 1584; // zašto mi je ovo granični .br.dok.
                }
                else if (Okruzenje == "Proba2")
                {
                    GranicniBrojdokumenta = 0;
                }
                else
                {
                    GranicniBrojdokumenta = 0;
                }
                string qPoliseAOnisuIstekle = $"SELECT [Dokument].[idDokument], [Dokument].[brojUgovora], [Dokument].[idProizvod], [Dokument].[datumIsteka], [Dokument].[idStatus], [DokumentPodaci].[tipPolise], * FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] WHERE ([Dokument].[idDokument] > '{GranicniBrojdokumenta}' AND [idProizvod] = 1 AND [idStatus] = 2 AND [tipPolise] = 1 AND [Dokument].[brojUgovora] IS NOT NULL AND [datumIsteka] > CAST(GETDATE() AS DATE)) ORDER BY [Dokument].[idDokument] ASC;";
                int BrojPoliseAO = 0;
                string BrojPoliseAOstring = "";
                try
                {
                    using SqlConnection konekcija = new(connectionStringMtpl);
                    konekcija.Open();
                    using SqlCommand cmd = new(qPoliseAOnisuIstekle, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();
                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        int BrojDokumenta = Convert.ToInt32(reader["idDokument"]); // Čitanje vrednosti po imenu kolone
                        BrojPoliseAO = Convert.ToInt32(reader["brojUgovora"]); // Čitanje kao int
                        BrojPoliseAOstring = Convert.ToInt32(reader["brojUgovora"]).ToString("D8"); // Čitanje kao string
                        DateTime DatumIsteka = Convert.ToDateTime(reader["DatumIsteka"]); // Čitanje kao DateTime

                        string qPolisaAONemaZK = $"SELECT * FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] WHERE ([idProizvod] = 4 AND [idStatus] = 2 AND [idDokument] > 164 AND [registarskiBroj] = {BrojPoliseAO});";
                        using SqlConnection konekcija2 = new SqlConnection(connectionStringMtpl);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new(qPolisaAONemaZK, konekcija2);
                        int imaPolisuZK = (int)(cmd2.ExecuteScalar() ?? 0);

                        // Ispis rezultata u konzoli
                        if (imaPolisuZK != 0)
                        {
                            Console.WriteLine($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisuZK: {imaPolisuZK}");
                        }
                        else
                        {
                            Console.WriteLine($"Prva koja nema:BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisuZK: {imaPolisuZK}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }

                Console.WriteLine($"\nKreiraću polisu ZK za polisu AO br: {BrojPoliseAO}\n");

                await _page.Locator("//e-input[@id='inpRegistarskiBrojAO']").ClickAsync();

                await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").FillAsync(BrojPoliseAOstring);
                await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").PressAsync("Tab");

                // Sada uzmi prvu slobodnu polisu ZK
                // Konekcija sa bazom StrictEvidenceDB 
                //long PrviSlobodanSerijskiZK = 0;


                string Lokacija = "(7, 8)";

                if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme_);

                if (NacinPokretanjaTesta == "automatski" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }

                string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                //Nalaženje poslednjeg serijskog broja koji je u sistemu
                string qSlobodniSerijskiZK = $"SELECT [tObrasci].[SerijskiBroj] FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                             $"LEFT JOIN [StrictEvidenceDB].[strictevidence].[tIzdatePolise] ON [tObrasci].[SerijskiBroj] = [tIzdatePolise].[SerijskiBroj] " +
                                             $"WHERE [tObrasci].[IdTipObrasca] = 4 AND [IDLokacijaZaduzena] IN {Lokacija} AND [IDLokacijaZaduzena] NOT IN (-2, 5) AND [_DatumDo] IS NULL AND [tIzdatePolise].[SerijskiBroj] IS NULL AND [tObrasci].[SerijskiBroj] > 655 " +
                                             $"ORDER BY [tObrasci].[SerijskiBroj] ASC;";
                int SerijskiBrojZK = 0;
                try
                {
                    using SqlConnection konekcija = new(connectionStringStroga);
                    konekcija.Open();
                    using SqlCommand cmd = new SqlCommand(qSlobodniSerijskiZK, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();

                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        //int BrojDokumenta = Convert.ToInt32(reader["idDokument"]); // Čitanje vrednosti po imenu kolone
                        //BrojPoliseAO = Convert.ToInt32(reader["brojUgovora"]); // Čitanje kao int
                        //BrojPoliseAOstring = Convert.ToInt32(reader["brojUgovora"]).ToString("D8"); // Čitanje kao string
                        //DateTime DatumIsteka = Convert.ToDateTime(reader["DatumIsteka"]); // Čitanje kao DateTime
                        SerijskiBrojZK = Convert.ToInt32(reader["SerijskiBroj"]);
                        string qSerijskiBrojZKNijeUpotrebljen = $"SELECT COUNT(*) FROM [MtplDB].[mtpl].[Dokument] WHERE [idProizvod] = 4 AND [brojUgovora] = {SerijskiBrojZK} AND [idStatus] = 1 ;";
                        using SqlConnection konekcija2 = new(connectionStringMtpl);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new(qSerijskiBrojZKNijeUpotrebljen, konekcija2);
                        int imaPolisuZK = (int)(cmd2.ExecuteScalar() ?? 0);

                        Console.WriteLine($"Ima polisu ZK: {imaPolisuZK}");
                        // Ispis rezultata u konzoli

                        if (imaPolisuZK == 1)
                        {
                            Console.WriteLine($"SerijskiBrojZK: {SerijskiBrojZK}");
                        }
                        else
                        {
                            Console.WriteLine($"Prvi obrazac koji je slobodan: {SerijskiBrojZK}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }


                Console.WriteLine($"\nKreiraću polisu ZK za obrazac br: {SerijskiBrojZK}\n");

                await _page.Locator("//e-input[@id='inpSerijskiBrojZK']").ClickAsync();
                string konCifraZK = IzracunajKontrolnuCifruZK($"{SerijskiBrojZK}");
                //await _page.Locator("#inpSerijskiBrojZK input[type=\"text\"]").FillAsync(SerijskiBrojZK.ToString("D8"));

                await _page.Locator("#inpSerijskiBrojZK input[type=\"text\"]").FillAsync(SerijskiBrojZK.ToString("D8") + konCifraZK);
                await _page.Locator("#inpSerijskiBrojZK input[type=\"text\"]").PressAsync("Tab");

                //Nalaženje poslednjeg broja dokumenta u Mtpl
                int PoslednjiBrojDokumenta = OdrediBrojDokumenta();

                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();

                //Ovde proveri URL
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/4/Zeleni-karton/Dokument/{PoslednjiBrojDokumenta + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Otkaži izmenu" }).ClickAsync();

                //await _page.PauseAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await _page.PauseAsync();
                var popupLocator = _page.Locator("//div[contains(text(),'uspešno')]");

                // Sačekaj da se popup pojavi (timeout 5 sekundi)
                bool isPopupVisible = await popupLocator.WaitForAsync(new() { Timeout = 5000 }).ContinueWith(t => !t.IsFaulted);
                //await Expect(popupLocator).ToBeVisibleAsync(new() { Timeout = 5000 });

                Assert.That(isPopupVisible, "Popup sa tekstom 'uspešno' se nije pojavio.");

                //await page.GetByText("Polisa broj 00000569 uspešno").ClickAsync();


                //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.ReloadAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();
                await _page.Locator("//div[@class='right-menu']").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

                //Assert.Pass();

                //await ProveriStanjeKontrole(_page, "[id*='selPartner']");
                //MessageBox.Show($"Za Partnera vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                //if (StanjeKontrole == 0)
                //{
                //await ProveriPartnera(_page);
                //}

                //await _page.PauseAsync();
                /*
                            await ProveriStanjeKontrole(_page, "[id*='selTarife']");
                            //MessageBox.Show($"Za Tarifu vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                            if (StanjeKontrole == 0)
                            {
                                await ProveriTarifu(_page);
                            }

                            //await Pauziraj(_page!);
                            */
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
        public async Task ZK_6_RazduznaLista()
        {

            try
            {
                await Pauziraj(_page!);
                await UlogujSe(_page!, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");


                await _page!.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();

                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Zeleni karton" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-dokumenata");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "grid Dokumenti";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Nova razdužna lista i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Nova razdužna lista").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/4/0");






                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d")).ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));


                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText(Asaradnik_).ClickAsync();

                //await _page.GetByText("Arhivski magacin Arhivski").ClickAsync();

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
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/4/{PoslednjiDokumentStroga + 1}");


                //await _page.PauseAsync();
                // Definišite XPath za elemente
                //string xpath = "//div[@class='opsezi']/div//button[@class='left primary flat flex-center-center']";
                string xpath = "//div[@class='opsezi']/div//i[@class='ico-check']";
                // Kreirajte locator za sve elemente
                var elementi = _page.Locator(xpath);

                // Dobijte ukupan broj elemenata
                int brojElemenata = await elementi.CountAsync();
                // Ispis rezultata
                Console.WriteLine($"Ukupan broj redova (obrazaca) za razduživanje je: {brojElemenata}");

                for (int i = brojElemenata - 1; i > 1; i--)
                {
                    await elementi.Nth(i).ClickAsync();

                }

                //await _page.PauseAsync();
                // Ovo je brisanje razdužne liste
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");

                //await Pauziraj(_page!);

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //Ovde treba dodati proveru štampanja

                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                //await _page.Locator("css = [type='password']").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{PoslednjiDokumentStroga + 1}" }).ClickAsync();

                //await _page.PauseAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/4/Zeleni-karton/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "Ručno")
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
        public async Task ZK_7_Otpis()
        {

            try
            {

                await Pauziraj(_page!);
                await UlogujSe(_page!, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page!, PocetnaStrana, "/Dashboard");


                //await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page!.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Zeleni karton" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Novi otpis").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/3/0");


                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d")).ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(CurrentDate.ToString("MMMM d")).First.ClickAsync();

                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik_).ClickAsync();

                await _page.Locator("textarea").First.ClickAsync();
                await _page.Locator("textarea").First.FillAsync("Dragan je prosuo kafu i rakiju!");

                //await _page.PauseAsync();

                // Pronađi prvi slobodan serijski broj za polisu ZK
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

                if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
                {
                    Lokacija = "(3)";
                }
                else if (AkorisnickoIme_ == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }



                //else throw new ArgumentException("Nepoznata uloga: " + AkorisnickoIme_);

                if (NacinPokretanjaTesta == "automatski" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
                {
                    Lokacija = "(11)";
                }


                //Nalaženje poslednjeg serijskog broja koji je u sistemu
                string qZaduženiObrasciAOBezPolise = $"SELECT [tObrasci].[SerijskiBroj] FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                                     $"LEFT JOIN [StrictEvidenceDB].[strictevidence].[tIzdatePolise] ON [tObrasci].[SerijskiBroj] = [tIzdatePolise].[SerijskiBroj] " +
                                                     $"WHERE [tObrasci].[SerijskiBroj] > 655 AND [tObrasci].[IdTipObrasca] = 4 AND [IDLokacijaZaduzena] IN {Lokacija} AND [IDLokacijaZaduzena] NOT IN (-2, -1, 5) AND [_DatumDo] IS NULL AND [tIzdatePolise].[SerijskiBroj] IS NULL " +
                                                     $"ORDER BY [tObrasci].[SerijskiBroj] ASC;";
                int SerijskiBrojZK = 0;
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
                        SerijskiBrojZK = Convert.ToInt32(reader["SerijskiBroj"]);
                        string qSerijskiBrojZKNijeUpotrebljen = $"SELECT COUNT (*) FROM [MtplDB].[mtpl].[Dokument] " +
                                                        $"INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] " +
                                                        $"WHERE [idProizvod] = 4 AND [serijskiBrojAO] LIKE '{SerijskiBrojZK}%';";

                        using SqlConnection konekcija2 = new SqlConnection(connectionString);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new SqlCommand(qSerijskiBrojZKNijeUpotrebljen, konekcija2);
                        int imaPolisuZK = (int)(cmd2.ExecuteScalar() ?? 0);
                        Console.WriteLine($"Serijski brojevi obrazaca AO kod Bogdana: {SerijskiBrojZK}, Ima polisu AO: {imaPolisuZK}");

                        // Izbor slobodnog serijskog broja polise AO
                        if (imaPolisuZK == 0)
                        {
                            Console.WriteLine($"Prvi obrazac koji je slobodan je: {SerijskiBrojZK}");
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

                string strSerijskiBrojZK = SerijskiBrojZK.ToString();
                Console.WriteLine($"\nKreiraću razdužnu listu za serijski broj obrasca AO: {strSerijskiBrojZK}\n");

                //await _page.PauseAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync($"{(SerijskiBrojZK).ToString("D8")}");
                string konCifraOd = IzracunajKontrolnuCifruZK($"{SerijskiBrojZK}");
                //await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpOdBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraOd);
                //await _page.Locator("button").Filter(new() { HasText = "+" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                //await _page.GetByText("Brojevi obrazaca: ").ClickAsync();
                //await _page.Locator("#notify0 button").ClickAsync();
                //await _page.PauseAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync(strSerijskiBrojZK);

                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync($"{(SerijskiBrojZK).ToString("D8")}");
                string konCifraDo = IzracunajKontrolnuCifruZK($"{SerijskiBrojZK}");
                //await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBrojaKontrolna input[type=\"text\"]").FillAsync(konCifraDo);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();

                //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();
                //await _page.Locator("#btnObrisi button").ClickAsync();

                //await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#inpOdBroja input[type=\"text\"]").FillAsync(strSerijskiBrojZK);

                //await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                //await _page.Locator("#inpDoBroja input[type=\"text\"]").FillAsync(strSerijskiBrojZK);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).ClickAsync();


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
                //await _page.PauseAsync();
                await _page.Locator("#inpOdBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpDoBroja input[type=\"text\"]").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/4/Zeleni-karton/Dokument/3/{PoslednjiDokumentStroga + 1}");
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //await _page.PauseAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.GetByText("Pregled / Pretraga dokumenata").ClickAsync();
                await _page.Locator(".ico-ams-logo").ClickAsync();

                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                //await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                //await _page.Locator("css = [type='password']").ClickAsync();
                //await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                //await _page.Locator("a").First.ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/4/Zeleni-karton/Dokument/3/{PoslednjiDokumentStroga + 1}");
                //await _page.PauseAsync();
                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();











                await _page.Locator(".ico-ams-logo").ClickAsync();

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
        public async Task JS_1_SE_PregledPretragaRazduznihListi()
        {

            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page!.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje putnika u javnom saobraćaju" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-dokumenata");

                // Klik na Pregled / Pretraga dokumenata stroge evidencije
                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();
                // Provera da li se otvorila stranica pregled dokumenata
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/6/Osiguranje-putnika/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Pregled razdužnih listi za JS";
                //string lokatorGrida = "//e-grid[@id='grid_dokumenti']";


                string kriterijumFiltera = "Bogdan";
                await ProveraPostojiGrid(_page, tipGrida);
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
                    var celijaSerijskiBrojObrasca = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{oznakaDokumenta}']");

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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.");
                }
                //await _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']").ClickAsync();
                //await _page.PauseAsync();
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.");
                }

                await ProveriStampuPdf(_page, "Štampaj dokument", "Štampa dokumenta Stroge evidencije za ZK:");
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
        public async Task JS_2_Polisa()
        {
            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await NovaPolisa(_page, "Nova polisa JS");
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Dokument/0");

                // Pronađi odgovarajuću polisu AO koja nema ZK
                Server = OdrediServer(Okruzenje);

                string connectionStringMtpl = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                string istekla = DateTime.Now.ToString("yyyy-MM-dd");
                Console.WriteLine(istekla);
                int GranicniBrojdokumenta = 0;
                if (Okruzenje == "Razvoj")
                {
                    GranicniBrojdokumenta = 0;
                }
                else if (Okruzenje == "Proba2")
                {
                    GranicniBrojdokumenta = 0;
                }
                else if (Okruzenje == "UAT")
                {
                    GranicniBrojdokumenta = 196;
                }
                else
                {
                    GranicniBrojdokumenta = 0;
                }

                string qPoliseAOnisuIstekle = $"SELECT [Dokument].[idDokument], [Dokument].[brojUgovora], [Dokument].[idProizvod], [Dokument].[datumIsteka], [Dokument].[idStatus], [DokumentPodaci].[tipPolise], [oznakaPremijskaGrupaPodgrupa], * " +
                                              $"FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] " +
                                              $"WHERE ([oznakaPremijskaGrupaPodgrupa] LIKE '01.%' AND ([taksiVozilo] =1 OR [rentacar] = 1)  AND  [Dokument].[idDokument] > '{GranicniBrojdokumenta}' AND [idProizvod] = 1 AND [idStatus] = 2 AND [tipPolise] = 1 AND [Dokument].[brojUgovora] IS NOT NULL  AND [trajanje] = 1 AND [datumIsteka] > CAST(GETDATE() AS DATE) ) ORDER BY [Dokument].[datumIsteka] ASC;"; //ORDER BY [Dokument].[idDokument] ASC

                int BrojPoliseAO = 0;
                string BrojPoliseAOstring = "";
                int BrojDokumenta = 0;
                try
                {
                    using SqlConnection konekcija = new(connectionStringMtpl);
                    konekcija.Open();
                    using SqlCommand cmd = new(qPoliseAOnisuIstekle, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();
                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        BrojDokumenta = Convert.ToInt32(reader["idDokument"]); // Čitanje vrednosti po imenu kolone
                        BrojPoliseAO = Convert.ToInt32(reader["brojUgovora"]); // Čitanje kao int
                        BrojPoliseAOstring = Convert.ToInt32(reader["brojUgovora"]).ToString("D8"); // Čitanje kao string
                        DateTime DatumIsteka = Convert.ToDateTime(reader["datumIsteka"]); // Čitanje kao DateTime
                        /*
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.Forms.MessageBox.Show($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}", "Prva polisa AO koja nije istekla", MessageBoxButtons.OK);
                        }
                        */
                        string qPolisaAONemaJS = $"SELECT COUNT (*) FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] WHERE ([idProizvod] = 6 AND [idStatus] = 2 AND [registarskiBroj] = {BrojPoliseAO});";
                        using SqlConnection konekcija2 = new(connectionStringMtpl);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new(qPolisaAONemaJS, konekcija2);
                        int imaPolisuJS = (int)(cmd2.ExecuteScalar() ?? 0);
                        //int imaPolisuJS = cmd2.ExecuteNonQuery();
                        /*
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.Forms.MessageBox.Show($"Ima polisu JS : {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisu JS: {imaPolisuJS}", "Informacija", MessageBoxButtons.OK);
                        }
                        */
                        // Ispis rezultata u konzoli
                        if (imaPolisuJS == 1)
                        {
                            //await _page.PauseAsync();
                            Console.WriteLine($"Prva koja ima: BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisu AO: {imaPolisuJS}");
                            //konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");

                            Console.WriteLine($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisu AO: {imaPolisuJS}");
                            /*
                            if (NacinPokretanjaTesta == "ručno")
                            {
                                System.Windows.Forms.MessageBox.Show($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisu AO: {imaPolisuJS}", "Prva koja ima polisu JS", MessageBoxButtons.OK);
                            }
                            */
                        }
                        else
                        {
                            Console.WriteLine($"Prva koja nema:BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisuJS: {imaPolisuJS}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");


                            break;
                        }

                        Console.WriteLine($"\nKreiraću polisu JS za polisu AO br: {BrojPoliseAO}\n");
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.Forms.MessageBox.Show($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Ima polisu AO: {imaPolisuJS}", "Prva koja nema polisu JS", MessageBoxButtons.OK);
                        }


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }



                //await _page.PauseAsync();




                await _page!.Locator("//e-input[@id='inpRegistarskiBrojAO']").ClickAsync();
                //await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").ClickAsync();

                await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").FillAsync(BrojPoliseAOstring);
                await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").PressAsync("Tab");
                //await _page.PauseAsync();

                await _page.Locator("#selVrstaVozilaJS > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText("Za putnike u taksi vozilima i").ClickAsync();
                //await _page.PauseAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "Email Obavezan podatak" }).Locator("input[type=\"text\"]").ClickAsync();
                //await _page.Locator("e-input").Filter(new() { HasText = "Email" }).Locator("input[type=\"text\"]").FillAsync("bogaman@hotmail.com");
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();


                // Ovde ubaci proveru URL nakon Kreiranja polise JS
                //await _page.PauseAsync();
                /*
                                       // Prvo otvori grid Autoodgovornost
                                       // Pređi mišem preko teksta Osiguranje vozila
                                       await _page.GetByText("Osiguranje vozila").HoverAsync();
                                       // Klikni u meniju na Autoodgovornost
                                       await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).ClickAsync();
                                       await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

                                       // Opcionalno: Proveri da li je stranica vidljiva ili da sadrži neki ključni element
                                       var element = await _page.QuerySelectorAsync("//e-grid[@id='grid_dokumenti']");
                                       if (element != null)
                                       {
                                           Console.WriteLine("Stranica se učitala i element grid je pronađen.");
                                       }
                                       else
                                       {
                                           Console.WriteLine("Stranica se učitala, ali element grid nije pronađen.");
                                           return;
                                       }

                                       Console.WriteLine("******************************************************************");
                                       Console.WriteLine();

                                       // Redovi u zaglavlju 
                                       //var rowsZaglavlje = await _page.QuerySelectorAllAsync(".zaglavlje");
                                       var rowsZaglavlje = await _page.QuerySelectorAllAsync("//div[@class='zaglavlje row']");
                                       //Console.WriteLine($"Redova u zaglavlju ima: {rowsZaglavlje.Count}.");

                                       // Pronađi kolone samo u prvom redu (indeks 0) od celog grida
                                       //var columnsZaglavlje = await rowsZaglavlje[0].QuerySelectorAllAsync("div.column");       
                                       // Pronađi sve kolone unutar zaglavlja
                                       var columnsZaglavlje = await _page.QuerySelectorAllAsync("div.zaglavlje.row > div.column");
                                       if (rowsZaglavlje.Count > 0 && columnsZaglavlje.Count > 0)
                                       {
                                           // Prikaz broja redova i kolona u zaglavlju
                                           Console.WriteLine($"Redova u zaglavlju ima: {rowsZaglavlje.Count}.");
                                           Console.WriteLine($"Kolona u zaglavlju ima:: {columnsZaglavlje.Count}.");
                                       }
                                       else
                                       {
                                           Console.WriteLine("Nema redova ili kolona za prikaz."); return;
                                       }

                                       Console.WriteLine("******************************************************************");
                                       Console.WriteLine();

                                       int rbKoloneStatus = -1;
                                       int rbKoloneBrojDokumenta = -1;
                                       int rbKoloneBrojPolise = -1;
                                       int columnIndex = 1;

                                       foreach (var column in columnsZaglavlje)
                                       {

                                           //Console.WriteLine($"---- Kolona ---- {columnIndex}");

                                           // Dohvati sve moguće atribute ručno
                                           /*******************************************************************************
                                           //string[] attributeNames = ["class", "columnkey", "columnsortkey", "id", "name"];
                                           string[] attributeNames = ["class", "columnkey"];
                                           foreach (var attributeName in attributeNames)
                                           {
                                               var attributeValue = await column.GetAttributeAsync(attributeName);

                                               if (attributeValue != null)
                                               {
                                                   //Console.WriteLine($"{attributeName}: {attributeValue}");
                                               }
                                           }
                                           *********************************************************/

                /****************************************************************************
                // Pročitaj unutrašnji tekst kolone
                string innerTextCeo = await column.InnerTextAsync();
                //Console.WriteLine($"Tekst kolone: {innerTextCeo}");
                string innerText = innerTextCeo.TrimEnd('-').Trim();

                //string innerText = innerText1.TrimEnd('-').Trim();
                //Console.WriteLine($"Tekst kolone {columnIndex}: {innerText}");
                if (innerText == "Status")
                {
                    rbKoloneStatus = columnIndex;
                    Console.WriteLine($"Tekst kolone {columnIndex}: {innerText}, ---{rbKoloneStatus}");
                }
                if (innerText == "Broj polise")
                {
                    rbKoloneBrojPolise = columnIndex;
                    Console.WriteLine($"Tekst kolone {columnIndex}: {innerText}, ---{rbKoloneBrojPolise}");
                }
                if (innerText == "Br. dok.")
                {
                    rbKoloneBrojDokumenta = columnIndex;
                    Console.WriteLine($"Tekst kolone {columnIndex}: {innerText}, ---{rbKoloneBrojDokumenta}");
                }
                //Console.WriteLine();
                columnIndex++;
            }

            //Obrada grida
            var rowsGrid = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
            Console.WriteLine();
            Console.WriteLine($"Redova u gridu ima:: {rowsGrid.Count}");

            string Status = "";
            string brojPolise = "";
            string brojDokumenta = "";

            // Definiši brojač redova
            int rowIndex = 1;

            foreach (var row in rowsGrid)
            {
                var valueStatus = await row.QuerySelectorAsync($"div.column_{rbKoloneStatus}");
                if (valueStatus != null)
                {
                    // Odredi status
                    Status = await valueStatus.EvaluateAsync<string>("el => el.innerText");
                    if (Status == "Kreiran")
                    {
                        // Pročitaj vrednost iz kolone koja čuva Broj dokumenta
                        var valueBrojDokumenta = await row.QuerySelectorAsync($"div.column_{rbKoloneBrojDokumenta}");
                        brojDokumenta = await valueBrojDokumenta.EvaluateAsync<string>("el => el.innerText");
                        var valueBrojPolise = await row.QuerySelectorAsync($"div.column_{rbKoloneBrojPolise}");
                        brojPolise = await valueBrojPolise.EvaluateAsync<string>("el => el.innerText");

                        Console.WriteLine($"U redu {rowIndex} Kolona {rbKoloneStatus} ima Status:: {Status}");
                        Console.WriteLine($"U redu {rowIndex} Kolona {rbKoloneBrojDokumenta} ima Broj dokumenta:: {brojDokumenta}");
                        Console.WriteLine($"U redu {rowIndex} Kolona {rbKoloneBrojPolise} ima Broj polise:: {brojPolise}");
                        Console.WriteLine("******************************************************************");
                        Console.WriteLine();
                        break; // Prekinite petlju nakon pronalaska prvog reda
                    }
                    // Inkrementiraj brojač
                    rowIndex++;

                }
            }
            // Otvori Dashboard
            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");
            // Pređi mišem preko teksta Osiguranje vozila
            await _page.GetByText("Osiguranje vozila").HoverAsync();
            // Klikni u meniju na Osiguranje putnika u javnom saobraćaju
            await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje putnika u" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-dokumenata");

            // Opcionalno: Proveri da li je stranica vidljiva ili da sadrži neki ključni element
            element = await _page.QuerySelectorAsync("//e-grid[@id='grid_dokumenti']");
            if (element != null)
            {
                Console.WriteLine("Stranica se učitala i element grid je pronađen.");
            }
            else
            {
                Console.WriteLine("Stranica se učitala, ali element grid nije pronađen.");
                return;
            }

            Console.WriteLine("******************************************************************");
            Console.WriteLine();

            //await NovaPolisa(_page, "Nova polisa OP");
            await _page.GetByText("Nova polisa").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Dokument/0");

            /*********
            await ProveriStanjeKontrole(_page, "[id*='selPartner']");
            MessageBox.Show($"Vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
            await ProveriStanjeKontrole(_page, "[id*='selTarife']");
            MessageBox.Show($"Vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
            await UnesiRegistarskiBrojAO(_page, "346613730");
            await ProveriTarifu(_page);
            **************/
                /*
                           //await Pauziraj(_page!);

                           await UnesiRegistarskiBrojAO(_page, brojPolise);
                           await _page.Locator("#selVrstaVozilaJS > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                           await _page.GetByText("Za putnike u taksi vozilima i").ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Obriši dokument" }).ClickAsync();
                           //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                           await _page.Locator("#confirmBoxCancelButton button").ClickAsync();
                           //await Pauziraj(_page!);
                           /**************************************************************************************
                                       // Prolazimo kroz svaku kolonu u zaglavlju i čitamo sve atribute
                                       int index = 1;
                                       foreach (var column in columnsZaglavlje)
                                       {
                                           var outerHtml = await column.EvaluateAsync<string>("el => el.outerHTML");
                                           Console.WriteLine($"-----Kolona {index}:");
                                           // Parsiranje atributa iz outerHTML koristeći Regex
                                           var attributeRegex = new Regex(@"(\w+)\s*=\s*""([^""]*)""");

                                           var matches = attributeRegex.Matches(outerHtml);

                                           if (matches.Count > 0)
                                           {
                                               foreach (Match match in matches)
                                               {
                                                   Console.WriteLine($"{match.Groups[1].Value}: {match.Groups[2].Value}");
                                               }
                                           }
                                           else
                                           {
                                               Console.WriteLine("Nema atributa za prikaz.");
                                           }

                                           // Pročitaj unutrašnji tekst kolone
                                           var innerText1 = await column.InnerTextAsync();
                                           string innerText = innerText1.TrimEnd('-').Trim();
                                           Console.WriteLine($"Tekst kolone: {innerText.TrimEnd()}");
                                           Console.WriteLine();
                                           index++;
                                       }

                                       Console.WriteLine("******************************************************************");
                                       Console.WriteLine("");

                                       ***********************************************************/

                /******************************************************************************
                            // Čitanje određenih atributa u Playwright-u
                            element = await _page.QuerySelectorAsync("//div[@class='zaglavlje row']");
                            var classAttribute = await element.GetAttributeAsync("class");
                            Console.WriteLine($"Class atribut: {classAttribute}");

                            // Primer za čitanje innerText
                            var innerText = await _page.EvaluateAsync<string>(@"el => el.innerText", element);
                            Console.WriteLine($"InnerText: {innerText}");

                            // Primer za čitanje innerHTML
                            var innerHTML = await _page.EvaluateAsync<string>(@"el => el.innerHTML", element);
                            Console.WriteLine($"InnerHTML: {innerHTML}");

                            // Primer za čitanje style svojstva
                            var style = await _page.EvaluateAsync<string>(@"el => el.style.cssText", element);
                            Console.WriteLine($"Style: {style}");
                         ********************************************************************************************/


                /**************************************
                //OVO MI NIJE JASNO

                            var prototype = await _page.EvaluateAsync<string>(@"el => Object.getPrototypeOf(el)", element);
                            Console.WriteLine($"Prototip: {prototype}");
                            var proto = await _page.EvaluateAsync<string>(@"el => el.__proto__", element);
                            Console.WriteLine($"Prototip objekta: {proto}");

                            var events = await _page.EvaluateAsync<string[]>(@"el => Object.getOwnPropertyNames(el)", element);
                            foreach (var eventName in events)
                            {
                                Console.WriteLine($"Event: {eventName}");
                            }
                            var attributes2 = await _page.EvaluateAsync<Dictionary<string, string>>(@"
                                                                    el => {
                                                                        const attrs = {};
                                                                        for (let i = 0; i < el.attributes.length; i++) {
                                                                            const attr = el.attributes[i];
                                                                            attrs[attr.name] = attr.value;
                                                                        }
                                                                        return attrs;
                                                                    }", element);

                            Console.WriteLine("Atributi:");
                            foreach (var attr in attributes2)
                            {
                                Console.WriteLine($"****{attr.Key}: {attr.Value}");
                            }



                            var children = await _page.EvaluateAsync<List<string>>(@"
                                el => {
                                    const childElements = [];
                                    for (let i = 0; i < el.children.length; i++) {
                                        childElements.push(el.children[i].outerHTML);
                                    }
                                    return childElements;
                                }", element);

                            Console.WriteLine("Children elements:");
                            foreach (var child in children)
                            {
                                Console.WriteLine(child);
                            }




                            var childNodes = await _page.EvaluateAsync<List<string>>(@"
                                el => {
                                    const nodeList = [];
                                    for (let i = 0; i < el.childNodes.length; i++) {
                                        nodeList.push(el.childNodes[i].nodeName);
                                    }
                                    return nodeList;
                                }", element);

                            Console.WriteLine("Child nodes:");
                            foreach (var node in childNodes)
                            {
                                Console.WriteLine(node);
                            }

                ***************************************/




                /*************************************************
                Ovo radi, čita sve atribute


                                        int nekiBrojac = 1;

                                        foreach (var column in columnsZaglavlje)
                                        {

                                            // Pročitaj outerHTML kolone
                                            var outerHTML = await column.EvaluateAsync<string>("el => el.outerHTML");
                                            //Console.WriteLine($"OuterHTML kolone:\n{outerHTML}");



                                            var attributeRegex = new System.Text.RegularExpressions.Regex(@"\s([a-zA-Z\-]+)=""([^""]*)""");
                                            var matches = attributeRegex.Matches(outerHTML);

                                            if (matches.Count > 0)
                                            {

                                                Console.WriteLine($"Kolona br. {nekiBrojac}");
                                                foreach (System.Text.RegularExpressions.Match match in matches)
                                                {

                                                    Console.WriteLine($"Atribut: {match.Groups[1].Value}, Vrednost: {match.Groups[2].Value}");

                                                }

                                            }
                                            else
                                            {
                                                Console.WriteLine("Nema atributa za prikaz.");
                                            }

                                            // Pročitaj unutrašnji tekst kolone
                                            var innerText = await column.InnerTextAsync();
                                            Console.WriteLine($"Tekst kolone: {innerText}");
                                            Console.WriteLine("---- Kraj kolone ----\n");
                                            Console.WriteLine();
                                            nekiBrojac++;
                                        }
                ******************************************/
                /***********************************************************************
                //Ručno dohvatanje određenih atributa

                            int columnIndex = 1;

                            foreach (var column in columnsZaglavlje)
                            {
                                Console.WriteLine($"---- Kolona ----{columnIndex} ----");

                                // Dohvati sve moguće atribute ručno
                                string[] attributeNames = ["class", "columnkey", "columnsortkey", "id", "name"];

                                foreach (var attributeName in attributeNames)
                                {
                                    var attributeValue = await column.GetAttributeAsync(attributeName);

                                    if (attributeValue != null)
                                    {
                                        Console.WriteLine($"{attributeName}: {attributeValue}");
                                    }
                                }

                                // Pročitaj unutrašnji tekst kolone
                                var innerText = await column.InnerTextAsync();
                                Console.WriteLine($"Tekst kolone: {innerText}");
                                Console.WriteLine();
                                columnIndex++;
                            }




                *************************************/


                await _page.Locator(".ico-ams-logo").ClickAsync();

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
        public async Task JS_6_RazduznaLista()
        {

            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page!.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();

                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje putnika u javnom saobraćaju" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/6/Osiguranje-putnika/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "grid Obrasci";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Nova razdužna lista i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Nova razdužna lista (OSK)").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/6/Osiguranje-putnika/Dokument/4/0");






                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d")).First.ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));


                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik_).ClickAsync();
                //await _page.GetByText("Arhivski magacin Arhivski").ClickAsync();

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
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/6/Osiguranje-putnika/Dokument/4/{PoslednjiDokumentStroga + 1}");


                //await _page.PauseAsync();
                // Definišite XPath za elemente
                //string xpath = "//div[@class='opsezi']/div//button[@class='left primary flat flex-center-center']";
                string xpath = "//div[@class='opsezi']/div//i[@class='ico-check']";
                // Kreirajte locator za sve elemente
                var elementi = _page.Locator(xpath);

                // Dobijte ukupan broj elemenata
                int brojElemenata = await elementi.CountAsync();
                // Ispis rezultata
                Console.WriteLine($"Ukupan broj redova (obrazaca) za razduživanje je: {brojElemenata}");

                for (int i = brojElemenata - 1; i > 1; i--)
                {
                    await elementi.Nth(i).ClickAsync();

                }

                //await _page.PauseAsync();
                // Ovo je brisanje razdužne liste
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");

                //await Pauziraj(_page!);

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //Ovde treba dodati proveru štampanja

                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                /*
                                await _page.Locator(".korisnik").ClickAsync();
                                await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                                await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                                await _page.Locator("css = [type='password']").ClickAsync();
                                await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                                await _page.Locator("a").First.ClickAsync();
                                await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                */
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").HoverAsync();
                await _page.GetByText($"{oznakaDokumenta}").ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{PoslednjiDokumentStroga + 1}" }).ClickAsync();

                //await _page.PauseAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/6/Osiguranje-putnika/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

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
        public async Task DK_1_SE_PregledPretragaRazduznihListi()
        {

            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page!.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Autoodgovornost
                await _page.GetByRole(AriaRole.Button, new() { Name = "Lom stakla i auto nezgoda" }).First.ClickAsync();
                // Provera da li se otvorila stranica sa pregledom polisa AO
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");

                // Klik na Pregled / Pretraga dokumenata stroge evidencije
                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();
                // Provera da li se otvorila stranica pregled dokumenata
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci i da li radi filter na gridu Obrasci
                string tipGrida = "Pregled razdužnih listi za DK";
                //string lokatorGrida = "//e-grid[@id='grid_dokumenti']";


                string kriterijumFiltera = "Bogdan";
                await ProveraPostojiGrid(_page, tipGrida);
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
                    var celijaSerijskiBrojObrasca = _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{oznakaDokumenta}']");

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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.");
                }
                //await _page.Locator($"//div[@class='podaci']//div[contains(@class, 'column') and normalize-space(text())='{serijskiBrojObrasca}']").ClickAsync();
                //await _page.PauseAsync();
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
                    LogovanjeTesta.LogException(ex, $"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.");
                }

                await ProveriStampuPdf(_page, "Štampaj dokument", "Štampa dokumenta Stroge evidencije za DK:");
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
        public async Task DK_2_Polisa()
        {
            try
            {

                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page!.GetByText("Osiguranje vozila").HoverAsync();
                await _page!.GetByText("Osiguranje vozila").ClickAsync();
                // Klikni u meniju na Lom stakala i auto nezgoda i proveri da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Lom stakla i auto nezgoda" }).ClickAsync();

                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");

                await _page.GetByText("Nova polisa").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");

                // Pronađi odgovarajuću polisu AO koja nema ZK
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

                string connectionStringMtpl = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

                string istekla = DateTime.Now.ToString("yyyy-MM-dd");
                Console.WriteLine(istekla);
                int GranicniBrojdokumenta;
                if (Okruzenje == "Razvoj")
                {
                    GranicniBrojdokumenta = 1882;
                }
                else if (Okruzenje == "Proba2")
                {
                    GranicniBrojdokumenta = 0;
                }
                else
                {
                    GranicniBrojdokumenta = 0;
                }

                string qPoliseAOnisuIstekle = $"SELECT [Dokument].[idDokument], [Dokument].[brojUgovora], [Dokument].[idProizvod], [Dokument].[datumIsteka], [Dokument].[idStatus], [DokumentPodaci].[tipPolise], * FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] WHERE ([Dokument].[idDokument] > '{GranicniBrojdokumenta}' AND [idProizvod] = 1 AND [idStatus] = 2 AND [tipPolise] = 1 AND [Dokument].[brojUgovora] IS NOT NULL AND [datumIsteka] > CAST(GETDATE() AS DATE) AND [oznakaPremijskaGrupaPodgrupa] NOT IN ('06.01', '06.02', '06.03', '06.04', '06.05', '06.06', '06.07', '07.01', '07.02', '07.03', '07.04', '07.05', '07.06', '07.07') ) ORDER BY [Dokument].[idDokument] ASC;";

                int BrojPoliseAO = 0;
                string BrojPoliseAOstring = "";
                string premijskaGrupa_1 = "";
                string premijskaGrupa = "";
                int taksi = -1;
                int rentacar = -1;

                try
                {
                    using SqlConnection konekcija = new(connectionStringMtpl);
                    konekcija.Open();
                    using SqlCommand cmd = new SqlCommand(qPoliseAOnisuIstekle, konekcija);
                    // Izvršavanje upita i dobijanje SqlDataReader objekta
                    using SqlDataReader reader = cmd.ExecuteReader();
                    // Prolazak kroz redove rezultata
                    while (reader.Read())
                    {
                        // Čitanje vrednosti iz trenutnog reda
                        int BrojDokumenta = Convert.ToInt32(reader["idDokument"]); // Čitanje vrednosti po imenu kolone
                        BrojPoliseAO = Convert.ToInt32(reader["brojUgovora"]); // Čitanje kao int
                        BrojPoliseAOstring = Convert.ToInt32(reader["brojUgovora"]).ToString("D8"); // Čitanje kao string
                        DateTime DatumIsteka = Convert.ToDateTime(reader["DatumIsteka"]); // Čitanje kao DateTime
                        premijskaGrupa_1 = Convert.ToString(reader["oznakaPremijskaGrupaPodgrupa"]);
                        premijskaGrupa = "01." + premijskaGrupa_1.Substring(0, premijskaGrupa_1.Length - 3);
                        taksi = Convert.ToInt32(reader["taksiVozilo"]);
                        rentacar = Convert.ToInt32(reader["rentacar"]);
                        string qPolisaAONemaDK = $"SELECT * FROM [MtplDB].[mtpl].[Dokument] INNER JOIN [MtplDB].[mtpl].[DokumentPodaci] ON [Dokument].[idDokument] = [DokumentPodaci].[idDokument] WHERE ([idProizvod] = 7 AND [idStatus] = 2 AND [registarskiBroj] = {BrojPoliseAO});";

                        using SqlConnection konekcija2 = new SqlConnection(connectionStringMtpl);
                        konekcija2.Open();
                        using SqlCommand cmd2 = new SqlCommand(qPolisaAONemaDK, konekcija2);
                        int imaPolisuDK = (int)(cmd2.ExecuteScalar() ?? 0);

                        // Ispis rezultata u konzoli
                        Console.WriteLine($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Premijska grupa: {premijskaGrupa}, Taksi: {taksi}, Rentakar: {rentacar}, Ima polisu AO: {imaPolisuDK}");

                        if (imaPolisuDK != 0)
                        {
                            Console.WriteLine($"BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Premijska grupa: {premijskaGrupa}, Taksi: {taksi}, Rentakar: {rentacar}, Ima polisu AO: {imaPolisuDK}");
                        }
                        else
                        {
                            Console.WriteLine($"Prva koja nema:BrojDokumenta: {BrojDokumenta}, BrojUgovora: {BrojPoliseAO}, BrojPolise: {BrojPoliseAOstring}, DatumIsteka: {DatumIsteka}, Premijska grupa: {premijskaGrupa}, Taksi: {taksi}, Rentakar: {rentacar}, Ima polisuZK: {imaPolisuDK}");
                            konekcija.Close();
                            konekcija2.Close();
                            Console.WriteLine($"Konekcija: {konekcija.State}");
                            Console.WriteLine($"Konekcija 2: {konekcija2.State}");

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }

                Console.WriteLine($"\nKreiraću polisu JS za polisu AO br: {BrojPoliseAO}\n");

                await _page.GetByText("Registarski broj AO").ClickAsync();
                //await ProveriStanjeKontrole(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");
                //await ProveriStanjeKontrole(_page, "[id*='selPartner']");
                //MessageBox.Show($"Vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                //await ProveriStanjeKontrole(_page, "[id*='selTarife']");
                //MessageBox.Show($"Vraćena je vrednost {StanjeKontrole}.", "Informacija", MessageBoxButtons.OK);
                //await UnesiRegistarskiBrojAO(_page, "346613730");
                await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).FillAsync(BrojPoliseAOstring);
                await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).PressAsync("Tab");
                //await _page.Locator("#inpRegistarskiBrojAO input[type=\"text\"]").PressAsync("Tab");

                //await Pauziraj(_page!);
                await DatumOd(_page);
                await _page.Locator("#inpVremeOd").GetByRole(AriaRole.Textbox).ClickAsync();
                await _page.Locator("#inpVremeOd").GetByRole(AriaRole.Textbox).FillAsync("23:50");

                //await ProveriTarifu(_page);


                await Pauziraj(_page);
                //e-select[@id='selKorekcijePremije']

                //await _page.GetByText("---Auto škola ---").ClickAsync();
                await _page.Locator("//e-select[@id='selKorekcijePremije']").ClickAsync();
                //Izbor korekcije premije
                //await _page.GetByText("Auto škola").ClickAsync();
                await _page.Locator("#chkLom i").ClickAsync();
                await _page.GetByText("Osigurana suma", new() { Exact = true }).ClickAsync();


                // Kreiramo instancu klase Random
                Random randomOsiguranaSuma = new Random();
                // Granice i korak
                int pocetak = 20000;
                int kraj = 200000;
                int korak = 10000;
                if (premijskaGrupa == "01.02")
                {
                    pocetak = 30000;
                }

                // Izračunavanje broja mogućih vrednosti
                int brojOpcija = (kraj - pocetak) / korak + 1;

                // Slučajan izbor indeksa
                int randomIndeks = randomOsiguranaSuma.Next(0, brojOpcija);
                int OsiguranaSuma = pocetak + randomIndeks * korak;

                await _page.Locator("#idSumeOsiguranjaLs").GetByRole(AriaRole.Textbox).FillAsync(OsiguranaSuma.ToString());

                //Generišem Učešće u šteti
                // Definišemo niz elemenata
                string[] ucesce = { "Bez franšize", "10%", "20%", "30%" };
                // Kreiramo instancu klase Random
                Random randomUcesce = new Random();
                // Generišemo slučajan indeks
                int slucajanIndeks = randomUcesce.Next(ucesce.Length);
                // Biramo element iz niza na osnovu slučajnog indeksa
                string ucesceUsteti = ucesce[slucajanIndeks];
                await _page.Locator("#selFransiza .control-main").ClickAsync();
                await _page.GetByText(ucesceUsteti).ClickAsync();

                //Generišem Rizik krađe
                // Definišemo niz elemenata
                string[] kradja = { "Da", "Ne" };
                // Kreiramo instancu klase Random
                Random randomKradja = new Random();
                // Generišemo slučajan indeks 
                int slucajanIndeks2 = randomKradja.Next(kradja.Length);
                // Biramo element iz niza na osnovu slučajnog indeksa
                string rizikKradje = kradja[slucajanIndeks2];
                await _page.Locator("#selRizikKradje .control-main").ClickAsync();
                await _page.GetByText(rizikKradje, new() { Exact = true }).ClickAsync();

                // Unesi auto nezgodu
                if (taksi != 1 && rentacar != 1 && premijskaGrupa != "01.03" && premijskaGrupa != "01.04" && premijskaGrupa != "01.09")
                {

                    await _page.Locator("#chkAn div").Nth(3).ClickAsync();
                    if (IdLice_ == 1001)
                    {
                        await _page.Locator("#selSumeOsiguranjaAn > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();

                        pocetak = 100000;
                        kraj = 1000000;
                        korak = 50000;
                        brojOpcija = (kraj - pocetak) / korak + 1;


                        Random randomUcesceNezgoda = new Random();
                        // Slučajan izbor indeksa
                        randomIndeks = randomUcesceNezgoda.Next(0, brojOpcija);
                        // Izračunavanje vrednosti
                        int osiguranaSumaSmrt = pocetak + randomIndeks * korak;
                        double doubleOsiguranaSumaSmrt = (double)osiguranaSumaSmrt;
                        // Formatiranje broja
                        string formatiraniBroj = doubleOsiguranaSumaSmrt.ToString("#,##0.00", new CultureInfo("sr-RS"));
                        await _page.GetByText($"Suma smrt {formatiraniBroj} - suma").ClickAsync();
                    }
                    else if (IdLice_ == 1002)
                    {
                        //await _page.Locator("//e-input[@id='idSumeOsiguranjaAN']//input[@class='input']").ClickAsync();
                        //await _page.Locator("#idSumeOsiguranjaAN input[type=\"text\"]").ClickAsync();
                        await _page.GetByText("Osigurana suma smrt").ClickAsync();


                        pocetak = 50000;
                        kraj = 500000;
                        korak = 25000;
                        brojOpcija = (kraj - pocetak) / korak + 1;

                        Random randomUcesceNezgoda = new Random();
                        // Slučajan izbor indeksa
                        randomIndeks = randomUcesceNezgoda.Next(0, brojOpcija);
                        // Izračunavanje vrednosti
                        int osiguranaSumaSmrt = pocetak + randomIndeks * korak;
                        double doubleOsiguranaSumaSmrt = (double)osiguranaSumaSmrt;
                        // Formatiranje broja
                        string formatiraniBroj = doubleOsiguranaSumaSmrt.ToString("#,##0.00", new CultureInfo("sr-RS"));
                        //await _page.GetByText($"Suma smrt {formatiraniBroj} - suma").ClickAsync();
                        //await _page.Locator("#idSumeOsiguranjaAN input[type=\"text\"]").FillAsync(formatiraniBroj);
                        //await _page.GetByText("Osigurana suma invaliditet").ClickAsync();
                        await _page.Locator("#idSumeOsiguranjaAN input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#idSumeOsiguranjaAN input[type=\"text\"]").FillAsync(formatiraniBroj);
                        await _page.GetByText("Osigurana suma invaliditet").ClickAsync();
                    }


                }

                //await _page.PauseAsync();
                //Nalaženje poslednjeg broja dokumenta u Mtpl
                int PoslednjiBrojDokumenta = OdrediBrojDokumenta();

                await _page.Locator("button").Filter(new() { HasText = "Kalkuliši" }).ClickAsync();

                //Ovde proveri URL
                await ProveriURL(_page, PocetnaStrana, $"/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/{PoslednjiBrojDokumenta + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Otkaži izmenu" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

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
        public async Task DK_6_SE_RazduznaLista()
        {

            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, AkorisnickoIme_, Alozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page!.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();

                // Klikni u meniju na Autoodgovornost i provera da li se otvorila odgovarajuća stranica
                await _page.Locator("button").Filter(new() { HasText = "Lom stakla i auto nezgoda" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");

                // Klik na Pregled / Pretraga razdužnih listi i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata/4");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Pregled razdužnih listi za SE";
                await ProveraPostojiGrid(_page, tipGrida);

                // Klik na Nova razdužna lista i provera da li se otvorila odgovarajuća stranica
                await _page.GetByText("Nova razdužna lista (OSK)").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/7/Lom-stakla-auto-nezgoda/Dokument/4/0");






                await _page.Locator("e-calendar input[type=\"text\"]").ClickAsync();
                await _page.GetByLabel(NextDate.ToString("MMMM d")).First.ClickAsync();
                await _page.Locator("e-calendar input[type=\"text\"]").FillAsync(CurrentDate.ToString("dd.mm.yyyy."));

                await Pauziraj(_page!);
                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText(Asaradnik_).ClickAsync();

                //await _page.GetByText("Arhivski magacin Arhivski").ClickAsync();

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
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/7/Lom-stakla-auto-nezgoda/Dokument/4/{PoslednjiDokumentStroga + 1}");


                //await _page.PauseAsync();
                // Definišite XPath za elemente
                //string xpath = "//div[@class='opsezi']/div//button[@class='left primary flat flex-center-center']";
                string xpath = "//div[@class='opsezi']/div//i[@class='ico-check']";
                // Kreirajte locator za sve elemente
                var elementi = _page.Locator(xpath);

                // Dobijte ukupan broj elemenata
                int brojElemenata = await elementi.CountAsync();
                // Ispis rezultata
                Console.WriteLine($"Ukupan broj redova (obrazaca) za razduživanje je: {brojElemenata}");

                for (int i = brojElemenata - 1; i > 1; i--)
                {
                    await elementi.Nth(i).ClickAsync();

                }

                //await Pauziraj(_page!);
                // Ovo je brisanje razdužne liste
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");

                //await Pauziraj(_page!);

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //Ovde treba dodati proveru štampanja

                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                /*
                                                                await _page.Locator(".korisnik").ClickAsync();
                                                                await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();

                                                                await _page.Locator("css = [inner-label='Korisničko ime*']").ClickAsync();
                                                                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync("davor.bulic@eonsystem.com");
                                                                await _page.Locator("css = [type='password']").ClickAsync();
                                                                await _page.Locator("input[type=\"password\"]").FillAsync("Lozinka1!");
                                                                await _page.Locator("a").First.ClickAsync();
                                                                await _page.Locator("button").Filter(new() { HasText = "Prijava" }).ClickAsync();
                                                */

                //await Pauziraj(_page!);
                // Sačekaj na URL posle logovanja
                //await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").ClickAsync();
                //await _page.GetByText($"Imate novi dokument \"Razdužna lista (OSK)\" za verifikaciju").First.HoverAsync();
                //await _page.GetByText($"{oznakaDokumenta}").ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{PoslednjiDokumentStroga + 1}" }).ClickAsync();

                await _page.GotoAsync($"{PocetnaStrana}/Stroga-evidencija/7/Lom-stakla-auto-nezgoda/Dokument/4/{PoslednjiDokumentStroga + 1}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/7/Lom-stakla-auto-nezgoda/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

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
                LogovanjeTesta.LogException(ex, "Greška u testu");
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                throw;
            }
        }


        [Test]
        public async Task BO_1_PutnoZdravstvenoOsiguranje()
        {
            try
            {
                await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme_, BOlozinka_);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Putno zdravstveno
                //await _page!.GetByText("Putno zdravstveno  osiguranje").HoverAsync();
                await _page!.GetByText("Putno zdravstveno  osiguranje").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Webshop backoffice" }).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Backoffice/Backoffice/1/Pregled-dokumenata");
                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Backoffice/Backoffice/1/Pregled-dokumenata");

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Pregled dokumenata za BO";
                await ProveraPostojiGrid(_page, tipGrida);

                // Proveri da li radi filter na gridu
                Trace.Write($"Filter na gridu - ");
                string ukupanBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                await _page.Locator("div:nth-child(13) > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();

                await _page.Locator("div:nth-child(13) > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync("2023.");
                await _page.Locator("div:nth-child(13) > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");

                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string filtriraniBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                if (Convert.ToInt32(ukupanBrojStrana) > Convert.ToInt32(filtriraniBrojStrana))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    //Trace.WriteLine($"Filter na gridu ne radi.");
                    return;
                }
                //await _page.Locator("div:nth-child(13) > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync("");
                //await _page.Locator("div:nth-child(13) > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");

                //await _page.ReloadAsync();
                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Backoffice/Backoffice/1/Pregled-dokumenata");

                //Izbroj koliko ima redova u gridu
                var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");
                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show($"Redova ima:: {rows.Count}", "Informacija", MessageBoxButtons.OK);
                }

                // Traži prvi red koji ima broj ugovora
                int rowIndex = 0; // Definiši brojač
                string brojUgovora = "";
                string brojPonude = "abc";
                foreach (var row in rows)
                {
                    // Inkrementiraj brojač
                    rowIndex++;
                    var valueInColumn1 = await row.QuerySelectorAsync("div.column_1");
                    var valueInColumn2 = await row.QuerySelectorAsync("div.column_2");

                    // Pročitajte vrednost iz ćelije
                    if (valueInColumn1 != null)
                    {
                        brojUgovora = await valueInColumn1.EvaluateAsync<string>("el => el.innerText");
                        brojPonude = await valueInColumn2!.EvaluateAsync<string>("el => el.innerText || ''");
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.Forms.MessageBox.Show($"U redu {rowIndex} kolona \nBroj ugovora ima vrednost:: #{brojUgovora}#, a \nBroj ponude je:: #{brojPonude}#", "Informacija", MessageBoxButtons.OK);
                        }
                        break;
                    }
                    else
                    {
                        if (NacinPokretanjaTesta == "ručno")
                        {
                            System.Windows.Forms.MessageBox.Show($"Proveri grid", "Informacija", MessageBoxButtons.OK);
                        }
                        break;
                    }
                }

                // Pročitaj iz baze idDokument za odgovarajući broj ugovora
                int BrojDokumenta;
                string qBrojDokumenta = $"SELECT * FROM [WebshopDB].[webshop].[Dokument] WHERE [brojPonude] = {brojPonude};";
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

                Console.WriteLine($"ID dokumenta je na okruženju '{Okruzenje}' je: {BrojDokumenta}.\n");


                Trace.Write($"Pregled polise:: ponuda {brojPonude}, ugovor {brojUgovora} - ");
                await _page.GetByText(brojPonude).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                Trace.WriteLine($"OK");

                //Provera štampe postojeće polise
                /*
        if(brojUgovora != "")
                {
                    Trace.Write($"Štampa postojeće polise - ");
                    await ProveriStampuPdf(_page, "Štampaj polisu", "Greška u štampi kreirane polise posle klika na Štampaj polisu.");
                }
                */


                /****************************
                var page1 = await _page.RunAndWaitForPopupAsync(async () =>
                {
                    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                });
                await page1.CloseAsync();
                *******************************/
                Trace.WriteLine($"OK");


                //Provera izmene polise
                Trace.Write($"Izmena polise - ");
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("Izmenaaaaaa");
                await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("IzmenaPrezime");
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.GetByText("Podaci uspešno sačuvani").ClickAsync();
                Trace.WriteLine($"OK");

                //await Pauziraj(_page!);
                //Provera štampe izmenjene polise
                Trace.Write($"Štampa postojeće izmenjene polise - ");
                /*******************
                await ProveriStampu(_page, "Štampaj polisu", "Greška u štampi izmenjene polise posle klika na Štampaj polisu.");
                var newPageTask = _page.Context.WaitForPageAsync();
                await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync(); // Klik na dugme koje generiše PDF
                var newPage = (await newPageTask) as IPage;
                await newPage.WaitForLoadStateAsync(LoadState.Load);

                await newPage.CloseAsync();
                ************************/
                //var response = await _page.GotoAsync("https://primer.com/nepostojeca-stranica");
                //Assert.That(response.Status, Is.EqualTo(404), "Stranica ne vraća očekivani status 404.");

                Trace.WriteLine($"OK");


                //Provera slanja mejla
                await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();
                Trace.WriteLine($"Proveri slanje mejla u [{DateTime.Now}].");

                int grBrojdokumenta = 0;
                //Provera polise bez ugovora
                Trace.WriteLine($"Proveri zašto postoji dugme Štampaj polisu za polisu koja nije kreirana?");
                if (Okruzenje == "Razvoj")
                {
                    grBrojdokumenta = 82;
                }



                //pronađi prvi koji nema broj ugovora
                qBrojDokumenta = $"SELECT  [idDokument], [brojPonude], [brojUgovora] " +
                                 $"FROM [WebshopDB].[webshop].[Dokument] " +
                                 $"WHERE [brojUgovora] = '' " +
                                 $"AND [brojPonude] = (SELECT MIN([brojPonude]) FROM [WebshopDB].[webshop].[Dokument] WHERE [brojUgovora] = '' AND [idDokument] > {grBrojdokumenta});;";
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

                // Otvori polisu bez ugovora

                Console.WriteLine($"Id dokumenta na okruženju '{Okruzenje}' koji nema ugovor je : {BrojDokumenta}.\n");
                await _page.GotoAsync(PocetnaStrana + $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                await ProveriURL(_page, PocetnaStrana, $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                Trace.Write($"Kreiranje polise za dokument {BrojDokumenta} kreirana - ");
                await Pauziraj(_page!);
                //Obradi polisu bez ugovora
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("#inpJmbg input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").FillAsync("bogaman@hotmail.com");
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(1) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(1) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("Kreirano Ime");
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();

                //await _page.PauseAsync();
                await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                //await _page.PauseAsync();
                // Čekanje na promenu sadržaja
                await _page.GetByText("Polisa uspešno kreirana").ClickAsync();
                await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");

                Trace.WriteLine($"OK");
                Trace.WriteLine($"Ovde proveri da li je sve OK sa štampom!");
                Trace.Write($"Štampa novo kreirane polise - ");

                //await ProveriStampuPdf(_page, "Štampaj polisu", "Greška u štampi novo kreirane polise posle klika na Štampaj polisu.");
                /*******************
                var page3 = await _page.RunAndWaitForPopupAsync(async () =>
                {
                    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                });


                //System.Windows.Forms.MessageBox.Show("Ovde proveri zašto postoji dugme Štampaj polisu za polisu koja nije kreirana");

                await page3.CloseAsync();


                var _page2 = await _page.RunAndWaitForPopupAsync(async () =>
                {
                    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                });

                if (NacinPokretanjaTesta == "ručno")
                {
                    System.Windows.Forms.MessageBox.Show("Ovde proveri da li je sve OK sa štampom!");
                }

                await _page2.CloseAsync();
                //Trace.WriteLine($"Ovde proveri da li je sve OK sa štampom!");
                ***************************/
                Trace.WriteLine($"OK");



                await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();
                //System.Windows.Forms.MessageBox.Show("Ovde proveri da li je otišao mejl!");
                Trace.WriteLine($"Proveri da li je otišao mejl u [{DateTime.Now}].");

                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();


                //pronađi poslednji koji ima broj ugovora
                qBrojDokumenta = $"SELECT  [idDokument], [brojPonude], [brojUgovora] " +
                                 $"FROM [WebshopDB].[webshop].[Dokument] " +
                                 $"WHERE [brojUgovora] != '' " +
                                 $"AND [brojPonude] = (SELECT MAX([brojPonude]) FROM [WebshopDB].[webshop].[Dokument] WHERE [brojUgovora] != '');;";
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

                Console.WriteLine($"Id dokumenta na okruženju '{Okruzenje}' koji ima ugovor je : {BrojDokumenta}.\n");
                await _page.GotoAsync(PocetnaStrana + $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                await ProveriURL(_page, PocetnaStrana, $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                //await _page.PauseAsync();


                /****************************************

                //Obradi polisu sa ugovorom
                //ovde uzmi idDokumenta i proveri URL stranice
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator("#inpJmbg input[type=\"text\"]").ClickAsync();
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                await _page.Locator("#inpJmbg input[type=\"text\"]").FillAsync("bogaman@hotmail.com");
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                System.Windows.Forms.MessageBox.Show($"Unesi neku izmenu");
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                //await _page.PauseAsync();

                var _page1 = await _page.RunAndWaitForPopupAsync(async () =>
                {
                    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                });

                await _page1.CloseAsync();


                //await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();
                //var _page2 = await _page.RunAndWaitForPopupAsync(async () =>
                //{
                //    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                //});
                //await _page2.CloseAsync();
                //await _page.Locator("#notify0 button").ClickAsync();

                await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();
                System.Windows.Forms.MessageBox.Show($"Proveri da li je stigao mejl ugovaraču");

                ******************************/
                await _page.Locator(".ico-ams-logo").ClickAsync();

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












        /*
            }



                            [Test]
                            public async Task RegularnaKorisnikFizicko()
                            {

                                #region Priprema
                                var playwright = Playwright;
                                var launchOptions = new BrowserTypeLaunchOptions
                                {
                                    Headless = false,
                                    Args = new List<string> { "--start-maximized" }
                                };
                                await using var browser = await playwright.Chromium.LaunchAsync(launchOptions); // Chrome
                                //await using var browser = await playwright.Firefox.LaunchAsync(launchOptions); // Firefox
                                var context = await browser.NewContextAsync(new BrowserNewContextOptions
                                {
                                    // Set viewport size to null for maximum size
                                    ViewportSize = ViewportSize.NoViewport
                                });
                                var page = await context.NewPageAsync();
                                #endregion Priprema

                                #region Logovanje
                                await page.GotoAsync(Variables.PocetnaStrana);
                                await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/login"); // Da li se otvorila stranica za Logovanje
                                await Expect(page).ToHaveTitleAsync(new Regex("Strana za logovanje")); // Expect a title "Strana za logovanje" a substring.

                                await page.GetByText("Korisničko ime").ClickAsync();
                                await page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                                await page.Locator("#rightBox input[type=\"text\"]").FillAsync(Variables.KorisnikMejl);

                                await page.GetByText("Lozinka").ClickAsync();
                                await page.Locator("input[type=\"password\"]").FillAsync(Variables.KorisnikPassword);

                                await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("PRIJAVA", RegexOptions.IgnoreCase) }).ClickAsync();
                                await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Dashboard"); // Da li se otvorila stranica Dashboard                                                                           
                                await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Strana za logovanje" a substring.
                                #endregion Logovanje

                                #region Autoodgovornost
                                await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Osiguranje vozila", RegexOptions.IgnoreCase) }).ClickAsync();
                                await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Autoodgovornost", RegexOptions.IgnoreCase) }).ClickAsync();
                                await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); // Da li se otvorila stranica Pregled dokumenata
                                await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Dobrodošli u eOsiguranje" a substring.
                                #endregion Autoodgovornost

                                #region Nova polisa
                                await page.Locator("#container div").Filter(new() { HasText = "Nova polisa" }).Nth(4).ClickAsync();
                                //await page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                                await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0"); // Da li se otvorila stranica Nove polise
                                await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje"));
                                #endregion Nova polisa

                                #region OSNOVNI PODACI
                                await page.PauseAsync();
                                #region Serijski broj obrasca polise
                                await page.Locator("id=inpSerijskiBrojAO").ClickAsync();
                                await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync("346611825");
                                //await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync(_serijskiBrojObrasca);
                                #endregion Serijski broj obrasca polise

                                #region Help Serijski broj
                                /*
                                await page.Locator("xpath = //e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                                await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

                                await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                                await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

                                await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                                await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();

                                #endregion Help Serijski broj

                                #region  Mesto izdavanja
                                await page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                                await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']//div[@class='multiselect-dropdown input']").ClickAsync();
                                await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']/div/div/div/div/div/input").FillAsync("beog"); // unos početnih slova
                                await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("beog"); // unos početnih slova
                                await page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
                                #endregion  Mesto izdavanja

                                #region Tip polise - Regularna
                                await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
                                await page.GetByText("Privremeno osiguranje").ClickAsync();
                                await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
                                await page.GetByText("Granično osiguranje").ClickAsync();
                                await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
                                await page.GetByText("Regularna").ClickAsync();
                                #endregion Tip polise - Regularna


                                #region Datum i vreme Regularna
                                await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
                                await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("06.06.2024.");

                                await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").ClickAsync();
                                await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").FillAsync("23:50");
                                #endregion Datum i vreme Regularna

                                #region Datum i vreme Granična
                                /*
                                await page.Locator("id=inpBrDana").ClickAsync();
                                await page.Locator("xpath=//e-input[@id='inpBrDana']//input[@class='input']").FillAsync("10");
                                await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
                                await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");
                                */

        /*
        #endregion Datum i vreme Granična

        #endregion OSNOVNI PODACI

        #region Lica na polisi - Regularna

        #region Help Ugovarač

        //await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        //await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

        //await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        //await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

        //await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        //await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();

        #endregion Help Ugovarač

        #region Oslobođen poreza
        //await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        //await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        #endregion Oslobođen poreza           



        #region Ugovarač je fizičko

        #region Stranac 1
        await page.Locator("#chkStranacU div").Nth(3).ClickAsync();
        await page.Locator("#chkStranacU div").Nth(3).ClickAsync();
        #endregion Stranac 1



        #region Fizičko Unos opštih podataka
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("2612962710096");
        //await page.Locator("e-input").Filter(new() { HasText = "JMBG JMBG nije ispravan" }).GetByRole(AriaRole.Textbox).ClickAsync();
        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("261296271009");

        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).FillAsync("BogdanA");
        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac #inpPrezime").GetByRole(AriaRole.Textbox).FillAsync("MandarićB");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).FillAsync("Kojekude");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).FillAsync("111");

        //Mesto i poštanski broj
        await page.Locator("xpath= //div[@id='ugovarac']/div[@class='row div-mesto']//div[@class='control ']").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).FillAsync("+38111123456789");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).FillAsync("bogaman@hotmail.com");

        #region Očitaj ličnu kartu
        await page.Locator("xpath=//e-button[@id='btnOcitajLKUgovarac']/button[@class='left basic flat flex-center-center']").ClickAsync();
        #endregion Očitaj ličnu kartu
        #endregion Fizičko Unos opštih podataka

        #endregion Ugovarač je fizičko



        #endregion Lica na polisi - Regularna

        #region VOZILO


        #region Vrsta vozila
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Teretno vozilo").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Autobusi").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Vučna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Specijalna motorna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Motocikli").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Priključna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Radna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Putničko vozilo").ClickAsync();
        #endregion Vrsta vozila

        #region Podaci o vozilu


        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("a");
        await page.Locator("xpath=//div[@id='podrucje']/e-select/div/div/div/div/div/div/div[2]").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("442RE");
        //await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync(_registarskiBroj);

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("Opel");

        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).FillAsync("Corsa");

        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync("123SAS321");
        //await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).FillAsync("2021");

        await page.Locator("xpath=//e-input[contains(.,'Zapr. motora [ccm]')]").ClickAsync();
        await page.Locator("xpath=//div[@id='zapremina']//input[@class='input']").FillAsync("1310");

        await page.Locator("xpath=//e-input[contains(.,'Snaga')]").ClickAsync();
        await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync("55");
        //await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync(_snaga);

        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).FillAsync("15");
        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).PressAsync("Tab");

        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).FillAsync("Crvena13");

        //await page.GetByText("Namena", new() { Exact = true }).ClickAsync();
        //await page.Locator("e-input").Filter(new() { HasText = "Namena" }).GetByRole(AriaRole.Textbox).FillAsync("Privremena");
        #endregion Podaci o vozilu


        #region Očitaj Saobraćajnu dozvolu
        await page.Locator("xpath = //text[.='Očitaj saobraćajnu']").ClickAsync();
        #endregion Očitaj Saobraćajnu dozvolu

        #endregion VOZILO

        #region OSTALO
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).FillAsync("Nap-mena u polisi ");


        //await page.PauseAsync();
        /*
                    if (_popusti != "---")
                    {
                        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await page.GetByText(_popusti).ClickAsync();
                    }

        */

        /*
        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Taxi vozilo").ClickAsync();
        #endregion OSTALO




        #region SNIMI, IZMENI I KALKULIŠI
        await page.PauseAsync();
        //await page.GetByRole(AriaRole.Button, new() { Name = "Snimi" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
        await page.PauseAsync();

        #endregion SNIMI, IZMENI I KALKULIŠI
        }
        */
        /*
                [Test]
                public async Task RegularnaKorisnikPravno()
                {

                    #region Priprema
                    var playwright = Playwright;
                    var launchOptions = new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Args = new List<string> { "--start-maximized" }
                    };
                    await using var browser = await playwright.Chromium.LaunchAsync(launchOptions);
                    //await using var browser = await playwright.Firefox.LaunchAsync(launchOptions);
                    var context = await browser.NewContextAsync(new BrowserNewContextOptions
                    {
                        // Set viewport size to null for maximum size
                        ViewportSize = ViewportSize.NoViewport
                    });
                    var page = await context.NewPageAsync();
                    #endregion Priprema

                    #region Logovanje

                    await page.GotoAsync(Variables.PocetnaStrana);
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/login"); // Da li se otvorila stranica za Logovanje
                    await Expect(page).ToHaveTitleAsync(new Regex("Strana za logovanje")); // Expect a title "Strana za logovanje" a substring.

                    await page.GetByText("Korisničko ime").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").FillAsync(Variables.KorisnikMejl);

                    await page.GetByText("Lozinka").ClickAsync();
                    await page.Locator("input[type=\"password\"]").FillAsync(Variables.KorisnikPassword);

                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("PRIJAVA", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Dashboard"); // Da li se otvorila stranica Dashboard                                                                           
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Strana za logovanje" a substring.
                    #endregion Logovanje

                    #region Autoodgovornost
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Osiguranje vozila", RegexOptions.IgnoreCase) }).ClickAsync();
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Autoodgovornost", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); // Da li se otvorila stranica Pregled dokumenata
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Dobrodošli u eOsiguranje" a substring.
                    #endregion Autoodgovornost


                    #region Nova polisa
                    await page.Locator("#container div").Filter(new() { HasText = "Nova polisa" }).Nth(4).ClickAsync();
                    //await page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0"); // Da li se otvorila stranica Nove polise
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje"));
                    #endregion Nova polisa

                    #region OSNOVNI PODACI

                    #region Serijski broj obrasca polise
                    await page.Locator("id=inpSerijskiBrojAO").ClickAsync();
                    await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync("346611825");
                    //await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync(_serijskiBrojObrasca);
                    #endregion Serijski broj obrasca polise

                    #region Help Serijski broj
                    /*
                    await page.Locator("xpath = //e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();
                    */

        /*
        #endregion Help Serijski broj

        #region  Mesto izdavanja
        await page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']/div/div/div/div/div/input").FillAsync("beog"); // unos početnih slova
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("beog"); // unos početnih slova
        await page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
        #endregion  Mesto izdavanja

        #region Tip polise - Regularna
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Privremeno osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Granično osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Regularna").ClickAsync();
        #endregion Tip polise - Regularna


        #region Datum i vreme Regularna
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");

        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").FillAsync("23:50");
        #endregion Datum i vreme Regularna

        #region Datum i vreme Granična
        /*
        await page.Locator("id=inpBrDana").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpBrDana']//input[@class='input']").FillAsync("10");
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");
        */

        /*
        #endregion Datum i vreme Granična

        #endregion OSNOVNI PODACI

        #region Lica na polisi - Regularna

        #region Help Ugovarač

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();

        #endregion Help Ugovarač

        #region Oslobođen poreza
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        #endregion Oslobođen poreza           

        #region Ugovarač je pravno

        await page.Locator("xpath=//e-select[@id='selTipLicaU']//div[@class='control-main']").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();

        #region Unos opštih podataka - Pravno
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).FillAsync("17464272");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).FillAsync("102789279");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).FillAsync("EON - #");

        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[3]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).FillAsync("KojekudePravno");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).FillAsync("111Pravno");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).FillAsync("+38111123456789");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).FillAsync("bogaman@hotmail.com");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).FillAsync("123456789");

        #endregion Unos opštih podataka - Pravno

        #endregion Ugovarač je pravno


        #endregion Lica na polisi - Regularna



        #region VOZILO


        #region Vrsta vozila
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Teretno vozilo").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Autobusi").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Vučna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Specijalna motorna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Motocikli").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Priključna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Radna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Putničko vozilo").ClickAsync();
        #endregion Vrsta vozila

        #region Podaci o vozilu


        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("a");
        await page.Locator("xpath=//div[@id='podrucje']/e-select/div/div/div/div/div/div/div[2]").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("442RE");
        //await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync(_registarskiBroj);

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("Opel");

        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).FillAsync("Corsa");

        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync("123SAS321");
        //await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).FillAsync("2021");

        await page.Locator("xpath=//e-input[contains(.,'Zapr. motora [ccm]')]").ClickAsync();
        await page.Locator("xpath=//div[@id='zapremina']//input[@class='input']").FillAsync("1310");

        await page.Locator("xpath=//e-input[contains(.,'Snaga')]").ClickAsync();
        await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync("55");
        //await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync(_snaga);

        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).FillAsync("15");
        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).PressAsync("Tab");

        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).FillAsync("Crvena13");

        //await page.GetByText("Namena", new() { Exact = true }).ClickAsync();
        //await page.Locator("e-input").Filter(new() { HasText = "Namena" }).GetByRole(AriaRole.Textbox).FillAsync("Privremena");
        #endregion Podaci o vozilu

        /*
        #region Očitaj Saobraćajnu dozvolu
        await page.Locator("xpath = //text[.='Očitaj saobraćajnu']").ClickAsync();
        #endregion Očitaj Saobraćajnu dozvolu
        */

        /*
        #endregion VOZILO

        #region OSTALO
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).FillAsync("Nap-mena u polisi ");


        //await page.PauseAsync();
        /*
                    if (_popusti != "---")
                    {
                        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await page.GetByText(_popusti).ClickAsync();
                    }

        */

        /*
        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Taxi vozilo").ClickAsync();
        #endregion OSTALO




        #region SNIMI, IZMENI I KALKULIŠI
        await page.PauseAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Snimi" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
        await page.PauseAsync();

        #endregion SNIMI, IZMENI I KALKULIŠI
        }
        */
        /*
                [Test]
                public async Task RegularnaLizingFizicko()
                {

                    #region Priprema
                    var playwright = Playwright;
                    var launchOptions = new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Args = new List<string> { "--start-maximized" }
                    };
                    await using var browser = await playwright.Chromium.LaunchAsync(launchOptions);
                    //await using var browser = await playwright.Firefox.LaunchAsync(launchOptions);
                    var context = await browser.NewContextAsync(new BrowserNewContextOptions
                    {
                        // Set viewport size to null for maximum size
                        ViewportSize = ViewportSize.NoViewport
                    });
                    var page = await context.NewPageAsync();
                    #endregion Priprema

                    #region Logovanje

                    await page.GotoAsync(Variables.PocetnaStrana);
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/login"); // Da li se otvorila stranica za Logovanje
                    await Expect(page).ToHaveTitleAsync(new Regex("Strana za logovanje")); // Expect a title "Strana za logovanje" a substring.

                    await page.GetByText("Korisničko ime").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").FillAsync(Variables.KorisnikMejl);

                    await page.GetByText("Lozinka").ClickAsync();
                    await page.Locator("input[type=\"password\"]").FillAsync(Variables.KorisnikPassword);

                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("PRIJAVA", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Dashboard"); // Da li se otvorila stranica Dashboard                                                                           
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Strana za logovanje" a substring.
                    #endregion Logovanje

                    #region Autoodgovornost
                    //await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Osiguranje vozila", RegexOptions.IgnoreCase) }).ClickAsync();
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = MyRegexOsiguranjeVozila() }).ClickAsync();
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Autoodgovornost", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); // Da li se otvorila stranica Pregled dokumenata
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Dobrodošli u eOsiguranje" a substring.
                    #endregion Autoodgovornost


                    #region Nova polisa
                    await page.Locator("#container div").Filter(new() { HasText = "Nova polisa" }).Nth(4).ClickAsync();
                    //await page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0"); // Da li se otvorila stranica Nove polise
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje"));
                    #endregion Nova polisa

                    #region OSNOVNI PODACI

                    #region Serijski broj obrasca polise
                    await page.Locator("id=inpSerijskiBrojAO").ClickAsync();
                    await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync("346611825");
                    //await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync(_serijskiBrojObrasca);
                    #endregion Serijski broj obrasca polise

                    #region Help Serijski broj
                    /*
                    await page.Locator("xpath = //e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();
                    */
        /*
        #endregion Help Serijski broj


        #region  Mesto izdavanja
        await page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']/div/div/div/div/div/input").FillAsync("beog"); // unos početnih slova
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("beog"); // unos početnih slova
        await page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
        #endregion  Mesto izdavanja



        #region Tip polise - Regularna
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Privremeno osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Granično osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Regularna").ClickAsync();
        #endregion Tip polise - Regularna

        #region Datum i vreme Regularna
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");

        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").FillAsync("23:50");
        #endregion Datum i vreme Regularna

        #region Datum i vreme Granična
        /*
        await page.Locator("id=inpBrDana").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpBrDana']//input[@class='input']").FillAsync("10");
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");
        */
        /*
        #endregion Datum i vreme Granična

        #endregion OSNOVNI PODACI



        #region Lica na polisi - Lizing

        #region Help Ugovarač

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();

        #endregion Help Ugovarač



        #region Očitaj Saobraćajnu dozvolu
        await page.Locator("xpath = //text[.='Očitaj saobraćajnu']").ClickAsync();
        #endregion Očitaj Saobraćajnu dozvolu

        #region Ugovarač je lizing kuća
        await page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        //await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač je i korisnik vozila$") }).ClickAsync();
        //await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač nije korisnik vozila$") }).ClickAsync();
        await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač je lizing kuća$") }).ClickAsync();

        #region Oslobođen poreza
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        #endregion Oslobođen poreza     


        #region Unos opštih podataka - Pravno 1

        await page.Locator("xpath=//e-select[@id='selTipLicaU']//div[@class='control-main']").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).FillAsync("17464272");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).FillAsync("102789279");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).FillAsync("EON - #");


        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).FillAsync("KojekudePravno");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).FillAsync("111Pravno");
        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[3]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).FillAsync("+38111123456789");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).FillAsync("bogaman@hotmail.com");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).FillAsync("123456789");

        #endregion Unos opštih podataka - Pravno 1


        #region Unos opštih podataka - Fizičko 2


        #region Očitaj ličnu kartu 2
        await page.Locator("xpath=//e-button[@id='btnOcitajLKKorisnik']/button[@class='left basic flat flex-center-center']").ClickAsync();
        #endregion Očitaj ličnu kartu 2


        await page.Locator("#korisnik div").Filter(new() { HasText = "JMBG" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "JMBG" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("2612962710096");
        //await page.Locator("e-input").Filter(new() { HasText = "JMBG JMBG nije ispravan" }).GetByRole(AriaRole.Textbox).ClickAsync();
        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("261296271009");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ime" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ime" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("Bogdan2");
        await page.Locator("#korisnik div").Filter(new() { HasText = "Prezime" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Prezime" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("Mandarić2");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("Kojekude2");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("2");

        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]/div[1]").ClickAsync();

        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("+3811987654321");

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("bogaman@hotmail.com");



        #endregion Unos opštih podataka - Fizičko 2



        #region Unos opštih podataka - Pravno 2
        await page.Locator("xpath=//e-select[@id='selTipLicaK']//div[@class='control-main']").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("17457284");
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("102624786");
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("RAIFFEISEN #");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("Kojekude22222");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("233333");

        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]/div[1]/span[1]").ClickAsync();

        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("+381100000000000");
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("bogaman26@hotmail.com");

        //await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        //await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("444444444");
        #endregion Unos opštih podataka - Pravno 2



        #endregion Ugovarač je lizing kuća



        #endregion Lica na polisi - Lizing



        #region VOZILO


        #region Vrsta vozila
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Teretno vozilo").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Autobusi").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Vučna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Specijalna motorna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Motocikli").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Priključna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Radna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Putničko vozilo").ClickAsync();
        #endregion Vrsta vozila

        #region Podaci o vozilu


        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("a");
        await page.Locator("xpath=//div[@id='podrucje']/e-select/div/div/div/div/div/div/div[2]").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("442RE");
        //await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync(_registarskiBroj);

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("Opel");

        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).FillAsync("Corsa");

        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync("123SAS321");
        //await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).FillAsync("2021");

        await page.Locator("xpath=//e-input[contains(.,'Zapr. motora [ccm]')]").ClickAsync();
        await page.Locator("xpath=//div[@id='zapremina']//input[@class='input']").FillAsync("1310");

        await page.Locator("xpath=//e-input[contains(.,'Snaga')]").ClickAsync();
        await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync("55");
        //await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync(_snaga);

        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).FillAsync("15");
        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).PressAsync("Tab");

        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).FillAsync("Crvena13");

        //await page.GetByText("Namena", new() { Exact = true }).ClickAsync();
        //await page.Locator("e-input").Filter(new() { HasText = "Namena" }).GetByRole(AriaRole.Textbox).FillAsync("Privremena");
        #endregion Podaci o vozilu


        #endregion VOZILO

        #region OSTALO
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).FillAsync("Nap-mena u polisi ");


        //await page.PauseAsync();
        /*
                    if (_popusti != "---")
                    {
                        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await page.GetByText(_popusti).ClickAsync();
                    }

        */


        /*
        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Taxi vozilo").ClickAsync();
        #endregion OSTALO




        #region SNIMI, IZMENI I KALKULIŠI
        await page.PauseAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Snimi" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
        await page.PauseAsync();

        #endregion SNIMI, IZMENI I KALKULIŠI
        }
        */
        /*
                [Test]
                public async Task RegularnaUgovaracKorisnik()
                {

                    #region Priprema
                    var playwright = Playwright;
                    var launchOptions = new BrowserTypeLaunchOptions
                    {
                        Headless = false,
                        Args = new List<string> { "--start-maximized" }
                    };
                    await using var browser = await playwright.Chromium.LaunchAsync(launchOptions);
                    //await using var browser = await playwright.Firefox.LaunchAsync(launchOptions);
                    var context = await browser.NewContextAsync(new BrowserNewContextOptions
                    {
                        // Set viewport size to null for maximum size
                        ViewportSize = ViewportSize.NoViewport
                    });
                    var page = await context.NewPageAsync();
                    #endregion Priprema

                    #region Logovanje

                    await page.GotoAsync(Variables.PocetnaStrana);
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/login"); // Da li se otvorila stranica za Logovanje
                    await Expect(page).ToHaveTitleAsync(new Regex("Strana za logovanje")); // Expect a title "Strana za logovanje" a substring.

                    await page.GetByText("Korisničko ime").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                    await page.Locator("#rightBox input[type=\"text\"]").FillAsync(Variables.KorisnikMejl);

                    await page.GetByText("Lozinka").ClickAsync();
                    await page.Locator("input[type=\"password\"]").FillAsync(Variables.KorisnikPassword);

                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("PRIJAVA", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Dashboard"); // Da li se otvorila stranica Dashboard                                                                           
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Strana za logovanje" a substring.
                    #endregion Logovanje

                    #region Autoodgovornost
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Osiguranje vozila", RegexOptions.IgnoreCase) }).ClickAsync();
                    await page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Autoodgovornost", RegexOptions.IgnoreCase) }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); // Da li se otvorila stranica Pregled dokumenata
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Dobrodošli u eOsiguranje" a substring.
                    #endregion Autoodgovornost


                    #region Nova polisa
                    await page.Locator("#container div").Filter(new() { HasText = "Nova polisa" }).Nth(4).ClickAsync();
                    //await page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                    await Expect(page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0"); // Da li se otvorila stranica Nove polise
                    await Expect(page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje"));
                    #endregion Nova polisa

                    #region OSNOVNI PODACI

                    #region Serijski broj obrasca polise
                    await page.Locator("id=inpSerijskiBrojAO").ClickAsync();
                    await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync("346611825");
                    //await page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync(_serijskiBrojObrasca);
                    #endregion Serijski broj obrasca polise

                    #region Help Serijski broj
                    /*
                    await page.Locator("xpath = //e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

                    await page.Locator("xpath=//e-help[@id='inpSerijskiBrojAO-help']/e-button[1]").ClickAsync();
                    await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();
                    */
        /*
        #endregion Help Serijski broj


        #region  Mesto izdavanja
        await page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selMestaIzdavanja']/div/div/div/div/div/input").FillAsync("beog"); // unos početnih slova
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//input[@class='multiselect-dropdown-search form-control']").FillAsync("beog"); // unos početnih slova
        await page.Locator("//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='4245'][normalize-space()='11070 - Beograd (Novi Beograd)']").ClickAsync();
        #endregion  Mesto izdavanja



        #region Tip polise - Regularna
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Privremeno osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Granično osiguranje").ClickAsync();
        await page.Locator("xpath=//e-select[@id='selTipPolise']//div[@class='multiselect-dropdown input']").ClickAsync();
        await page.GetByText("Regularna").ClickAsync();
        #endregion Tip polise - Regularna

        #region Datum i vreme Regularna
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");

        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpVremeOd']/div/div/div/input").FillAsync("23:50");
        #endregion Datum i vreme Regularna

        #region Datum i vreme Granična
        /*
        await page.Locator("id=inpBrDana").ClickAsync();
        await page.Locator("xpath=//e-input[@id='inpBrDana']//input[@class='input']").FillAsync("10");
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync("25.05.2024.");
        */
        /*
        #endregion Datum i vreme Granična

        #endregion OSNOVNI PODACI

        await page.PauseAsync();

        #region Lica na polisi - Ugovarač Korisnik
        #region Ugovarač nije korisnik
        await page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        //await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač je i korisnik vozila$") }).ClickAsync();
        await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač nije korisnik vozila$") }).ClickAsync();
        //await page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Ugovarač je lizing kuća$") }).ClickAsync();
        #endregion Ugovarač nije korisnik
        #region Help Ugovarač

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Prethodni')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath = //button[contains(.,'Sledeći')]").ClickAsync();

        await page.Locator("xpath=//e-help[@id='selOsiguranik-help']/e-button[1]").ClickAsync();
        await page.Locator("xpath=//h3[@class='modal-title']//i[@class='ico-xmark']").ClickAsync();

        #endregion Help Ugovarač



        #region Očitaj Saobraćajnu dozvolu
        await page.Locator("xpath = //text[.='Očitaj saobraćajnu']").ClickAsync();
        #endregion Očitaj Saobraćajnu dozvolu



        #region Oslobođen poreza
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        await page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
        #endregion Oslobođen poreza     

        #region Unos opštih podataka - Pravno 1

        await page.Locator("xpath=//e-select[@id='selTipLicaU']//div[@class='control-main']").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "MB" }).GetByRole(AriaRole.Textbox).FillAsync("17464272");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "PIB" }).GetByRole(AriaRole.Textbox).FillAsync("102789279");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Naziv" }).GetByRole(AriaRole.Textbox).FillAsync("EON - #");


        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).FillAsync("KojekudePravno");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).FillAsync("111Pravno");
        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[3]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).FillAsync("+38111123456789");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).FillAsync("bogaman@hotmail.com");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).FillAsync("123456789");

        #endregion Unos opštih podataka - Pravno 1


        #region Unos opštih podataka - Fizičko 2

        await page.Locator("xpath=//e-select[@id='selTipLicaK']//div[@class='control-main']").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();

        #region Očitaj ličnu kartu 2
        await page.Locator("xpath=//e-button[@id='btnOcitajLKKorisnik']/button[@class='left basic flat flex-center-center']").ClickAsync();
        #endregion Očitaj ličnu kartu 2


        await page.Locator("#korisnik div").Filter(new() { HasText = "JMBG" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "JMBG" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("2612962710096");
        //await page.Locator("e-input").Filter(new() { HasText = "JMBG JMBG nije ispravan" }).GetByRole(AriaRole.Textbox).ClickAsync();
        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("261296271009");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ime" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ime" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("Bogdan2");
        await page.Locator("#korisnik div").Filter(new() { HasText = "Prezime" }).Nth(2).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Prezime" }).Nth(2).GetByRole(AriaRole.Textbox).FillAsync("Mandarić2");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("Kojekude2");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("2");

        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]/div[1]").ClickAsync();

        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("+3811987654321");

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("bogaman@hotmail.com");



        #endregion Unos opštih podataka - Fizičko 2

        #region Unos opštih podataka - Fizičko 1
        await page.Locator("xpath=//e-select[@id='selTipLicaU']//div[@class='control-main']").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("2612962710096");
        //await page.Locator("e-input").Filter(new() { HasText = "JMBG JMBG nije ispravan" }).GetByRole(AriaRole.Textbox).ClickAsync();
        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JMBG" }).GetByRole(AriaRole.Textbox).FillAsync("261296271009");

        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).FillAsync("Bogdan1");
        await page.Locator("#ugovarac #inpIme").GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac #inpPrezime").GetByRole(AriaRole.Textbox).FillAsync("Mandarić1");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).FillAsync("Kojekude");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Ulica" }).GetByRole(AriaRole.Textbox).PressAsync("Tab");
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Broj" }).GetByRole(AriaRole.Textbox).FillAsync("111");
        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[3]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Telefon" }).GetByRole(AriaRole.Textbox).FillAsync("+38111123456789");

        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#ugovarac e-input").Filter(new() { HasText = "Email" }).GetByRole(AriaRole.Textbox).FillAsync("bogaman@hotmail.com");

        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).ClickAsync();
        //await page.Locator("#ugovarac e-input").Filter(new() { HasText = "JBKJS" }).GetByRole(AriaRole.Textbox).FillAsync("123456789");
        #endregion Unos opštih podataka - Fizičko 1

        #region Unos opštih podataka - Pravno 2
        await page.Locator("xpath=//e-select[@id='selTipLicaK']//div[@class='control-main']").ClickAsync();
        //await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='1'][contains(text(),'Fizičko')]").ClickAsync();
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2'][normalize-space()='Pravno']").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("17457284");
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("102624786");
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[4]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("RAIFFEISEN #");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Ulica" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("Kojekude22222");

        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).ClickAsync();
        await page.Locator("#korisnik div").Filter(new() { HasText = "Broj" }).Nth(3).GetByRole(AriaRole.Textbox).FillAsync("233333");

        //Mesto i poštanski broj
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[6]/div[1]/e-select[1]/div[1]/div[1]/div[1]/div[1]/span[1]").ClickAsync();

        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("novi");
        await page.Locator("xpath=//div[@class='control-wrapper field info-text-field inner-label-field focus']//div[@value='2372'][normalize-space()='14246 - Belanovica']").ClickAsync();

        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[1]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("+381100000000000");
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("bogaman26@hotmail.com");

        //await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").ClickAsync();
        //await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[4]/div[4]/div[7]/div[3]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("444444444");
        #endregion Unos opštih podataka - Pravno 2






        #endregion Lica na polisi - Ugovarač Korisnik



        #region VOZILO


        #region Vrsta vozila
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Teretno vozilo").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Autobusi").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Vučna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Specijalna motorna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Motocikli").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Priključna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Radna vozila").ClickAsync();
        await page.Locator("#selVrstaVozila > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Putničko vozilo").ClickAsync();
        #endregion Vrsta vozila

        #region Podaci o vozilu


        await page.Locator("xpath=//body/div[@id='container']/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[1]/e-select[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("a");
        await page.Locator("xpath=//div[@id='podrucje']/e-select/div/div/div/div/div/div/div[2]").ClickAsync();

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("442RE");
        //await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[2]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync(_registarskiBroj);

        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]").ClickAsync();
        await page.Locator("xpath=/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[4]/div[2]/div[5]/div[4]/div[5]/e-input[1]/div[1]/div[1]/div[1]/input[1]").FillAsync("Opel");

        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Tip" }).GetByRole(AriaRole.Textbox).FillAsync("Corsa");

        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync("123SAS321");
        //await page.Locator("e-input").Filter(new() { HasText = "Broj šasije" }).GetByRole(AriaRole.Textbox).FillAsync(_brojSasije);

        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("#inpGodiste").GetByRole(AriaRole.Textbox).FillAsync("2021");

        await page.Locator("xpath=//e-input[contains(.,'Zapr. motora [ccm]')]").ClickAsync();
        await page.Locator("xpath=//div[@id='zapremina']//input[@class='input']").FillAsync("1310");

        await page.Locator("xpath=//e-input[contains(.,'Snaga')]").ClickAsync();
        await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync("55");
        //await page.Locator("xpath=//div[@id='snaga']//input[@class='input']").FillAsync(_snaga);

        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).FillAsync("15");
        await page.Locator("#brMesta").GetByRole(AriaRole.Textbox).PressAsync("Tab");

        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).ClickAsync();
        await page.Locator("e-input").Filter(new() { HasText = "Boja" }).GetByRole(AriaRole.Textbox).FillAsync("Crvena13");

        //await page.GetByText("Namena", new() { Exact = true }).ClickAsync();
        //await page.Locator("e-input").Filter(new() { HasText = "Namena" }).GetByRole(AriaRole.Textbox).FillAsync("Privremena");
        #endregion Podaci o vozilu


        #endregion VOZILO

        #region OSTALO
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Napomene" }).FillAsync("Nap-mena u polisi ");


        //await page.PauseAsync();
        /*
                    if (_popusti != "---")
                    {
                        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                        await page.GetByText(_popusti).ClickAsync();
                    }

        */
        /*
        await page.Locator("#selKorekcijePremije > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
        await page.GetByText("Taxi vozilo").ClickAsync();
        #endregion OSTALO




        #region SNIMI, IZMENI I KALKULIŠI
        await page.PauseAsync();
        //await page.GetByRole(AriaRole.Button, new() { Name = "Snimi" }).ClickAsync();
        //await page.GetByRole(AriaRole.Button, new() { Name = "Kalkuliši" }).ClickAsync();
        //await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj polisu" }).ClickAsync();
        await page.PauseAsync();

        #endregion SNIMI, IZMENI I KALKULIŠI

        }
        */









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

