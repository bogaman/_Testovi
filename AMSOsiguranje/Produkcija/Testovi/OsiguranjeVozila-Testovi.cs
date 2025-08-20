

namespace Produkcija
{
    [TestFixture, Order(1)]
    [Parallelizable(ParallelScope.Self)]
    public partial class OsiguranjeVozila : Osiguranje
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




        [Test]
        public async Task DK_1_SE_PregledPretragaRazduznihListi()
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
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{oznakaDokumenta}'.", ex);
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
                    await LogovanjeTesta.LogException($"Greška prilikom provere elemenata sa vrednostima: {expectedValues}.", ex);
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

                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                await _page.GetByText("Osiguranje vozila").ClickAsync();
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

                //await Pauziraj(_page);
                await DatumOd(_page, "#cal_calDatumOd");
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
                    if (IdLice == 1001)
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
                    else if (IdLice == 1002)
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
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaMtpl();

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
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
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

                await Pauziraj(_page);
                await _page.Locator("#selRazduzenje > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                await _page.GetByText(Asaradnik).ClickAsync();

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

                //await Pauziraj(_page);
                // Ovo je brisanje razdužne liste
                //await _page.Locator("#btnObrisiDokument button").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");

                //await Pauziraj(_page);

                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Pošalji na verifikaciju" }).ClickAsync();
                string oznakaDokumenta = await _page.Locator("//div[@class='obrazac-container commonBox']//div[@class='col-3']//input[@class='input']").InputValueAsync();
                //Ovde treba dodati proveru štampanja

                await _page.GetByText("Pregled / Pretraga razdužnih listi (OSK)").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
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

                //await Pauziraj(_page);
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
                //await LogovanjeTesta.LogException("Greška u testu", ex);
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
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
                                var _page = await context.NewPageAsync();
                                #endregion Priprema

                                #region Logovanje
                                await _page.GotoAsync(Variables.PocetnaStrana);
                                await Expect(_page).ToHaveURLAsync(Variables.PocetnaStrana + "/login"); // Da li se otvorila stranica za Logovanje
                                await Expect(_page).ToHaveTitleAsync(new Regex("Strana za logovanje")); // Expect a title "Strana za logovanje" a substring.

                                await _page.GetByText("Korisničko ime").ClickAsync();
                                await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(Variables.KorisnikMejl);

                                await _page.GetByText("Lozinka").ClickAsync();
                                await _page.Locator("input[type=\"password\"]").FillAsync(Variables.KorisnikPassword);

                                await _page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("PRIJAVA", RegexOptions.IgnoreCase) }).ClickAsync();
                                await Expect(_page).ToHaveURLAsync(Variables.PocetnaStrana + "/Dashboard"); // Da li se otvorila stranica Dashboard                                                                           
                                await Expect(_page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Strana za logovanje" a substring.
                                #endregion Logovanje

                                #region Autoodgovornost
                                await _page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Osiguranje vozila", RegexOptions.IgnoreCase) }).ClickAsync();
                                await _page.GetByRole(AriaRole.Button, new() { NameRegex = new Regex("Autoodgovornost", RegexOptions.IgnoreCase) }).ClickAsync();
                                await Expect(_page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata"); // Da li se otvorila stranica Pregled dokumenata
                                await Expect(_page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje")); // Expect a title "Dobrodošli u eOsiguranje" a substring.
                                #endregion Autoodgovornost

                                #region Nova polisa
                                await _page.Locator("#container div").Filter(new() { HasText = "Nova polisa" }).Nth(4).ClickAsync();
                                //await _page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
                                await Expect(_page).ToHaveURLAsync(Variables.PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0"); // Da li se otvorila stranica Nove polise
                                await Expect(_page).ToHaveTitleAsync(new Regex("Dobrodošli u eOsiguranje"));
                                #endregion Nova polisa

                                #region OSNOVNI PODACI
                                await _page.PauseAsync();
                                #region Serijski broj obrasca polise
                                await _page.Locator("id=inpSerijskiBrojAO").ClickAsync();
                                await _page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync("346611825");
                                //await _page.Locator("xpath=//e-input[@id='inpSerijskiBrojAO']//input[@class='input']").FillAsync(_serijskiBrojObrasca);
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







        [Test]
        public async Task KA_08_SE_RazduznaLista()
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
                //await _page.GetByText("90202 - Bogdan Mandarić").ClickAsync();
                await _page.GetByText(Asaradnik).ClickAsync();

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
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");
                //Ovde treba dodati proveru štampanja
                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije KA:");
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();

                //await _page.PauseAsync();

                await _page.Locator(".ico-ams-logo").ClickAsync();

                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                //await _page.PauseAsync();
                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/9/Kasko/dokument/4/{PoslednjiDokumentStroga + 1}");
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
                //await _page.GotoAsync(PocetnaStrana + $"/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/{PoslednjiDokumentStroga + 1}");
                //await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/1/Autoodgovornost/Dokument/4/{PoslednjiDokumentStroga + 1}");

                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "------------------");

                //Proveri štampu neverifikovanog dokumenta
                await ProveriStampuPdf(_page, "Štampaj dokument", "Kreiran neverifikovan dokument stroge evidencije AO:");
                await _page.GetByText("Pregled / Pretraga razdužnih listi").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/9/Kasko/Pregled-dokumenata/4");


                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

                //await _page.Locator(".ico-ams-logo").ClickAsync();

                //await _page.Locator(".korisnik").ClickAsync();
                //await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                // kada se Davor odjavi, uloguj se kao Bogdan i proveri vesti

                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");

                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await _page.GetByText($"{oznakaDokumenta}").First.ClickAsync();
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-evidencija/9/Kasko/Dokument/4/{PoslednjiDokumentStroga + 1}");

                await ProveriStampuPdf(_page, "Štampaj dokument", "Verifikovan dokument stroge evidencije KA:");

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

