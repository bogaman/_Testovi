

namespace Razvoj
{
    [TestFixture, Order(2)]
    [Parallelizable(ParallelScope.Self)]
    public partial class ZeleniKarton : Osiguranje
    {



        #region Testovi


        [Test, Order(1)]
        public async Task ZK_1_SE_PregledPretragaObrazaca()
        {

            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
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
                    await LogovanjeTesta.LogException($"Greška prilikom pokušaja klika na ćeliju sa vrednošću '{serijskiBrojObrasca}'.", ex);
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
                    await LogovanjeTesta.LogException("Greška prilikom provere tekst boksova sa vrednostima prom1–prom4.", ex);
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


        [Test, Order(3)]
        public async Task ZK_2_SE_PregledPretragaDokumenata()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Osiguranje vozila
                //await _page.GetByText("Osiguranje vozila").HoverAsync();
                // Klikni na tekst Osiguranje vozila
                await _page.GetByText("Osiguranje vozila").ClickAsync();
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

        [Test, Order(5)]
        public async Task ZK_3_SE_UlazPrenosObrazaca()
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
                await _page.GetByText("Osiguranje vozila").ClickAsync();
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
                //await Pauziraj(_page);
                //await _page.Locator("button").Filter(new() { HasText = "Dodaj" }).Nth(2).ClickAsync();
                //await _page.Locator("button").Filter(new() { HasTextString = "^Dodaj$" }).ClickAsync();
                //await _page.Locator("button:text('Dodaj')").ClickAsync();
                await _page.Locator("//e-button[@id='btnDodaj']").ClickAsync();
                //await Pauziraj(_page);
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
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik).ClickAsync();
                }
                else if (Okruzenje == "Proba2")
                {
                    //await _page.GetByText("Centralni magacin 2").ClickAsync(); 
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik).ClickAsync();
                }
                else
                {
                    //await _page.GetByText("90202 - Bogdan Mandarić", new() { Exact = true }).ClickAsync();
                    await _page.Locator("#selZaduzenje").GetByText(Asaradnik).ClickAsync();
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
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                //await _page.PauseAsync();
                // Sačekaj na URL posle logovanja
                await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
                //await Pauziraj(_page);
                //await _page.GetByText($"Dokument možete pogledati klikom na link: {oznakaDokumenta}").ClickAsync();      
                //await _page.GetByRole(AriaRole.Link, new() { Name = $"{oznakaDokumenta}" }).ClickAsync();
                await _page.GotoAsync(PocetnaStrana + $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/{PoslednjiDokumentStroga + 2}");
                await ProveriURL(_page, PocetnaStrana, $"/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/{PoslednjiDokumentStroga + 2}");

                await _page.Locator("button").Filter(new() { HasText = "Verifikuj" }).ClickAsync();
                //await _page.Locator("//e-button[@id='btnVerifikuj']").ClickAsync();
                //await _page.Locator("//button[contains(.,'Verifikuj')]").ClickAsync();

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
        }


        [Test, Order(8)]
        public async Task ZK_4_Polisa()
        {

            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);

                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");



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
                int PoslednjiBrojDokumenta = OdrediBrojDokumentaMtpl();

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

                //await _page.GetByText("Polisa broj 00000569 uspešno").ClickAsync();


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

                            //await Pauziraj(_page);
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


        [Test, Order(10)]
        public async Task ZK_6_RazduznaLista()
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

                //await Pauziraj(_page);

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
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
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



        [Test, Order(15)]
        public async Task ZK_7_Otpis()
        {

            try
            {

                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, AkorisnickoIme, Alozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");


                //await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).HoverAsync();
                await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
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
                await _page.GetByText(Asaradnik).ClickAsync();

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
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
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



        #endregion Testovi
    }

}

