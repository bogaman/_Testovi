

namespace Razvoj
{
    [TestFixture, Order(5)]
    [Parallelizable(ParallelScope.Self)]
    public partial class JavniSaobracaj : Osiguranje
    {



        #region Testovi





        [Test, Order(501)]
        public async Task JS_1_SE_PregledPretragaRazduznihListi()
        {

            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
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


        [Test, Order(502)]
        public async Task JS_2_Polisa()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");

                await NovaPolisa(_page, "Nova polisa JS");
                await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Dokument/0");

                // Pronađi odgovarajuću polisu AO koja nema ZK
                Server = OdrediServer(Okruzenje);

                string connectionStringMtpl = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}; Connection Timeout = 60";

                string istekla = DateTime.Now.ToString("yyyy-MM-dd");
                Console.WriteLine(istekla);
                int GranicniBrojdokumenta = 0;
                if (Okruzenje == "Razvoj")
                {
                    GranicniBrojdokumenta = 5410;
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




                await _page.Locator("//e-input[@id='inpRegistarskiBrojAO']").ClickAsync();
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
                           //await Pauziraj(_page);

                           await UnesiRegistarskiBrojAO(_page, brojPolise);
                           await _page.Locator("#selVrstaVozilaJS > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                           await _page.GetByText("Za putnike u taksi vozilima i").ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Snimi i kalkuliši" }).ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                           await _page.Locator("button").Filter(new() { HasText = "Obriši dokument" }).ClickAsync();
                           //await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                           await _page.Locator("#confirmBoxCancelButton button").ClickAsync();
                           //await Pauziraj(_page);
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

        [Test, Order(503)]
        public async Task JS_6_RazduznaLista()
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

                string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}; Connection Timeout = 60";

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

                await Pauziraj(_page);

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
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
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













        #endregion Testovi
    }

}

