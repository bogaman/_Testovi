

namespace Produkcija
{
    [TestFixture, Order(8)]
    [Parallelizable(ParallelScope.Self)]
    public partial class DelimicniKasko : Osiguranje
    {



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








        #endregion Testovi
    }

}
