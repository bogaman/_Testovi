
namespace Razvoj
{
    [TestFixture, Order(2)]
    [Parallelizable(ParallelScope.Self)]
    public partial class PutnePoliseBO : Osiguranje
    {
        #region Testovi

        [Test, Order(101)]
        public async Task BO_01_PretragaPolisaPZO()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Putno zdravstveno  osiguranje
                //await _page.GetByText("Putno zdravstveno  osiguranje").HoverAsync();
                // Klikni na tekst Putno zdravstveno  osiguranje
                await _page.GetByText("Putno zdravstveno osiguranje").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Webshop backoffice" }).ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Pregle polisa
                string tipGrida = "Pregled polisa PZO";
                await ProveraPostojiGrid(_page, tipGrida);

                // Filtriraj grid da datum isteka bude 2023. godina
                await FiltrirajGrid(_page, ".2023.", tipGrida, 13, "TekstBoks", 0);

                //Sortiraj po datumu isteka - Rastuće
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Datum isteka -$") }).Locator("div").Nth(1).ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string datumIstekMin = await ProcitajCeliju(1, 13);
                string BrojPonude = await ProcitajCeliju(1, 2);

                //Sortiraj po datumu isteka - Opadajuće
                await _page.Locator("//div[@class='sort active']//div[@class='sortDir']").ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                BrojPonude = await ProcitajCeliju(1, 2);
                //Pročitaj datum isteka MAX
                string datumIstekMax = await ProcitajCeliju(1, 13);

                if (DateTime.Parse(datumIstekMin) < DateTime.Parse(datumIstekMax))
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"Sortiranje po datumu isteka ne radi.");
                    await LogovanjeTesta.LogException($"Sortiranje po datumu isteka ne radi.", new Exception("Sortiranje po datumu isteka ne radi."));
                    throw new Exception("Sortiranje po datumu isteka ne radi.");
                }

                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                await _page.Locator(".ico-ams-logo").ClickAsync();

                await IzlogujSe(_page);
                await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
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
        public async Task BO_2_PregledIzmenaPolisePZO()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Putno zdravstveno
                //await _page.GetByText("Putno zdravstveno  osiguranje").HoverAsync();
                await _page.GetByText("Putno zdravstveno  osiguranje").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Webshop backoffice" }).ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Pregled polisa PZO";
                await ProveraPostojiGrid(_page, tipGrida);

                //Sortiraj po Broju Ugovora - Rastuće
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Broj ugovora -$") }).Locator("div").Nth(1).ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)
                string brojUgovoraMin = await ProcitajCeliju(1, 1);
                int brojUgovoraMinInt = int.TryParse(brojUgovoraMin, out var temp) ? temp : 0;

                //Sortiraj po Broju Ugovora - Opadajuće
                await _page.Locator("//div[@class='sort active']//div[@class='sortDir']").ClickAsync();
                await Task.Delay(2000); // Pauza od 2 sekunde (2000 ms)

                //Pročitaj BrojUgovora MAX
                string brojUgovoraMax = await ProcitajCeliju(1, 1);
                int brojUgovoraMaxInt = int.TryParse(brojUgovoraMax, out temp) ? temp : 0;

                if (brojUgovoraMinInt < brojUgovoraMaxInt)
                {
                    Trace.WriteLine($"OK");
                }
                else
                {
                    Trace.WriteLine($"Sortiranje po Broju ugovora ne radi.");
                    await LogovanjeTesta.LogException($"Sortiranje po Broju ugovora ne radi.", new Exception("Sortiranje po Broju ugovora ne radi."));
                    throw new Exception("Sortiranje po Broju ugovora ne radi.");
                }
                string BrojPonude = await ProcitajCeliju(1, 2);

                DodatakNaURL = "/BackOffice" + await ProcitajCeliju(1, 14);

                await _page.GetByText(BrojPonude).ClickAsync();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri štampu postojeće polise
                await ProveriStampu404(_page, "Štampaj polisu", "Greška u štampi kreirane polise posle klika na Štampaj polisu.");
                Match idDokument = Regex.Match(DodatakNaURL, @"(\d+)$");

                //Zapis poslednjeg mejla (Pre izmene polise)
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                // Proveri izmene polise
                try
                {
                    await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("Izmenaaaaaa");
                    await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                    await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("IzmenaPrezime" + DateTime.Now.ToString());
                    await _page.Locator("body > div:nth-child(3) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > e-input:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)").First.ClickAsync();
                    await _page.Locator("body > div:nth-child(3) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > e-input:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)").First.FillAsync("amso.bogdan@mail.eonsystem.com");
                    await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                    await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                    await _page.GetByText("Podaci uspešno sačuvani").ClickAsync();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Greška prilikom izmene polise: {ex.Message}");
                    await LogovanjeTesta.LogException($"Greška prilikom izmene polise {idDokument}: {ex.Message}", ex);
                    throw;
                }

                //Provera da li je poslat mejl ka BO da je polisa izmenjena
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl za BO sa informacijom da je polisa izmenjena nije poslat.");

                await ProveriStampu404(_page, "Štampaj polisu", "Greška u štampi izmenjene polise posle klika na Štampaj polisu.");

                //Provera slanja mejla za kupca
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                //Pošalji mejl
                await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();

                //Provera da li je kupcu poslat mejl izmenjene polise
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl sa izmenjenom polisom koji ide ka kupcu nije poslat.");

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
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
        public async Task BO_3_KreiranjePolisePZO()
        {
            try
            {
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), $"_page cannot be null when calling test {NazivTekucegTesta}.");
                await Pauziraj(_page);
                await UlogujSe(_page, BOkorisnickoIme, BOlozinka);
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");
                // Pređi mišem preko teksta Putno zdravstveno
                //await _page.GetByText("Putno zdravstveno  osiguranje").HoverAsync();
                await _page.GetByText("Putno zdravstveno osiguranje").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Webshop backoffice" }).ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);
                await _page.GetByText("Putno zdravstveno osiguranje - backoffice").ClickAsync();
                DodatakNaURL = "/Backoffice/Backoffice/1/Pregled-dokumenata";
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                // Proveri da li stranica sadrži grid Obrasci
                string tipGrida = "Pregled polisa PZO";
                await ProveraPostojiGrid(_page, tipGrida);

                //pronađi prvi koji nema broj ugovora
                int BrojDokumenta = 0;
                int BrojPonude = 0;
                Server = OdrediServer(Okruzenje);
                int grBrojdokumenta = 0;
                //Provera polise bez ugovora
                Trace.WriteLine($"Proveri zašto postoji dugme Štampaj polisu za polisu koja nije kreirana?");
                if (Okruzenje == "Razvoj")
                {
                    grBrojdokumenta = 1566;
                }

                // Konekcija sa bazom
                string connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                string qBrojDokumenta = $"SELECT  [idDokument], [brojPonude], [brojUgovora] " +
                                        $"FROM [WebshopDB].[webshop].[Dokument] " +
                                        $"WHERE [brojUgovora] = '' " +
                                        $"AND [brojPonude] = (SELECT MAX([brojPonude]) FROM [WebshopDB].[webshop].[Dokument] WHERE [brojUgovora] = '' AND [idDokument] > {grBrojdokumenta});;";

                using (SqlConnection konekcija = new(connectionString))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qBrojDokumenta, konekcija))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            BrojDokumenta = reader["idDokument"] != DBNull.Value
                                ? Convert.ToInt32(reader["idDokument"])
                                : 0;

                            BrojPonude = reader["brojPonude"] != DBNull.Value
                                ? Convert.ToInt32(reader["brojPonude"])
                                : 0;
                        }
                    }
                    konekcija.Close();
                }

                //System.Windows.Forms.MessageBox.Show($"BrojDokumenta je: {BrojDokumenta}, BrojPonude je: {BrojPonude}", "Informacija", MessageBoxButtons.OK);
                //await _page.PauseAsync();
                // Filtriraj grid po broju ponude
                await FiltrirajGrid(_page, BrojPonude.ToString(), tipGrida, 2, "TekstBoks", 0);

                await _page.GetByText(BrojPonude.ToString()).ClickAsync();
                DodatakNaURL = "/BackOffice/BackOffice/1/Putno-osiguranje/" + BrojDokumenta.ToString();
                await ProveriURL(_page, PocetnaStrana, DodatakNaURL);

                /*
                //await _page.Locator('#cal_calPocetak').getByRole('textbox', { name: 'dd.mm.yyyy.' }).click();
                await DatumOd(_page, "#cal_calPocetak");
                //await _page.getByLabel('Avgust 14,').click();
                //await _page.Locator('#cal_calPocetak').getByRole('textbox', { name: 'dd.mm.yyyy.' }).fill('14.08.2025.');
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First().ClickAsync();
                await _page.Locator('.col-2 > .editabilno > .control-wrapper > .control > .control-main > .input').first().click();
                await _page.Locator('.col-2 > .editabilno > .control-wrapper > .control > .control-main > .input').first().click();
                await _page.Locator('.col-2 > .editabilno > .control-wrapper > .control > .control-main > .input').first().dblclick();
                await _page.Locator('.col-2 > .editabilno > .control-wrapper > .control > .control-main > .input').first().click();
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').press('ArrowRight');
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').fill('26129627100');
                await _page.Locator('.commonBox.font09.div-osiguranici > div:nth-child(2) > div > div:nth-child(3) > .editabilno > .control-wrapper > .control > .control-main > .input').first().click();
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').press('ArrowRight');
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').press('ArrowRight');
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').press('ArrowRight');
                await _page.Locator('.control-wrapper.field.inner-label-field.info-text-field.focus > .control > .control-main > .input').fill('26129627100');
                await _page.Locator('div:nth-child(6) > .editabilno > .control-wrapper > .control > .control-main > .input').first().click();
                await _page.getByRole('button', { name: ' Izmeni' }).click();
                await _page.getByRole('button', { name: ' Snimi' }).click();
                await _page.getByRole('button', { name: ' Kreiraj polisu' }).click();
                const page1Promise = _page.waitForEvent('popup');
                await _page.getByRole('button', { name: ' Štampaj polisu' }).click();
                const page1 = await page1Promise;
                await _page.getByRole('button', { name: ' Pošalji mejl' }).click();
*/

                //Zapis poslednjeg mejla (Pre izmene polise)
                var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

                //await _page.PauseAsync();

                //Kreiraj polisu bez ugovora
                try
                {
                    await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                    await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                    await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                    await DatumOd(_page, "#cal_calPocetak");


                    await _page.Locator("body > div:nth-child(3) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > e-input:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)").First.ClickAsync();
                    await _page.Locator("body > div:nth-child(3) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(4) > div:nth-child(2) > e-input:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > input:nth-child(1)").First.FillAsync("amso.bogdan@mail.eonsystem.com");
                    //await _page.Locator("#inpEmail input[type=\"text\"]").FillAsync("amso.bogdan@mail.eonsystem.com");
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(1) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                    await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(1) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("Kreirano Novo Ime");
                    await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                    await _page.GetByText("Podaci uspešno sačuvani").WaitForAsync(new() { Timeout = 4000 });
                    //await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");
                    await _page.GetByText("Podaci uspešno sačuvani").ClickAsync();


                    //Provera da li je poslat mejl ka BO da je polisa izmenjena
                    await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl za BO sa informacijom da je polisa izmenjena nije poslat.");



                    //provera da li je poslat mejl BO da je polisa kreirana
                    PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                    //Kreiraj polisu
                    var porukaLocator = _page.Locator("//div[contains(., 'Polisa uspešno kreirana') or contains(., 'INSERT INTO putno_polise')]");
                    //await _page.PauseAsync();
                    await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).First.ClickAsync();
                    //await _page.Locator("//e-button[@id='createPolicy']//button[@class='left primary flat flex-center-center']").First.ClickAsync();

                    // Čekanje na promenu sadržaja
                    await porukaLocator.WaitForAsync(new() { Timeout = 14000 });
                    //await _page.WaitForFunctionAsync("() => document.readyState === 'complete'");



                    string tekstPoruke = await porukaLocator.InnerTextAsync();
                    if (tekstPoruke.Contains("Polisa uspešno kreirana"))
                    {
                        //LogovanjeTesta.LogMessage("✅ Kalkulisanje je uspešno.");
                        //kalkulisanjeUspesno = true;
                    }
                    else if (tekstPoruke.Contains("INSERT INTO putno_polise"))
                    {
                        LogovanjeTesta.LogMessage("⚠️ Neuspešno kalkulisanje. Poruka: " + tekstPoruke);

                        // Pokušaj da zatvoriš poruku ako postoji dugme
                        var dugmeZatvori = _page.Locator("#notify0 button, .modal button:has-text('×'), .modal-close");
                        if (await dugmeZatvori.IsVisibleAsync())
                        {
                            await dugmeZatvori.ClickAsync();
                            LogovanjeTesta.LogMessage("❎ Poruka zatvorena.");
                        }
                        tekstPoruke = $"Povećaj Granični broj dokumenta. Sada je {grBrojdokumenta}.";
                        await LogovanjeTesta.LogException($"Greška prilikom kreiranja polise {BrojDokumenta}: {tekstPoruke}", new Exception(tekstPoruke));
                        throw new Exception(tekstPoruke);
                    }
                    else
                    {
                        LogovanjeTesta.LogMessage("⚠️ Nepoznata poruka: " + tekstPoruke);
                        await LogovanjeTesta.LogException($"Greška prilikom kreiranja polise : Nepoznata greška", new Exception("Nepoznata greška"));
                        //await _page.WaitForTimeoutAsync(1000);
                        throw (new Exception("Nepoznata greška"));
                    }
                    //await _page.PauseAsync();
                    //await _page.GetByText("Polisa uspešno kreirana").ClickAsync();

                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Greška prilikom kreiranja polise: {ex.Message}");
                    await LogovanjeTesta.LogException($"Greška prilikom kreiranja polise {BrojDokumenta}: {ex.Message}", ex);
                    throw;
                }



                //Provera da li je poslat mejl ka BO da je polisa kreirana
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl za BO sa informacijom da je polisa kreirana nije poslat.");
                //await _page.PauseAsync();
                await ProveriStampu404(_page, "Štampaj polisu", "Greška u štampi kreirane polise posle klika na Štampaj polisu.");

                //Provera slanja mejla za kupca
                PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();
                //await _page.PauseAsync();
                //Pošalji mejl
                await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();

                //Provera da li je kupcu poslat mejl izmenjene polise
                await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl sa izmenjenom polisom koji ide ka kupcu nije poslat.");

                await _page.Locator(".ico-ams-logo").ClickAsync();
                await IzlogujSe(_page);
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                if (NacinPokretanjaTesta == "ručno")
                {
                    PorukaKrajTesta();
                }
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }


            //await _page.PauseAsync();
            //return;


            // Proveri štampu postojeće polise
            //await ProveriStampu404(_page, "Štampaj polisu", "Greška u štampi kreirane polise posle klika na Štampaj polisu.");
            //Match idDokument = Regex.Match(DodatakNaURL, @"(\d+)$");

            //System.Windows.Forms.MessageBox.Show($"IdDokumenta je: {idDokument}", "Informacija", MessageBoxButtons.OK);

            //Zapis poslednjeg mejla (Pre izmene polise)
            //var PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

            // Proveri izmene polise
            /*
            try
            {
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("Izmenaaaaaa");
                await _page.Locator("button").Filter(new() { HasText = "Otkaži" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
                await _page.Locator(".commonBox > div:nth-child(2) > div > div:nth-child(2) > .editabilno > .control-wrapper > .control > .control-main > .input").First.FillAsync("IzmenaPrezime" + DateTime.Now.ToString("yyyy-MM-dd"));
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();
                await _page.GetByText("Podaci uspešno sačuvani").ClickAsync();

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Greška prilikom izmene polise: {ex.Message}");
                await LogovanjeTesta.LogException($"Greška prilikom izmene polise {idDokument}: {ex.Message}", ex);
                throw;
            }
            //Provera da li je poslat mejl ka BO da je polisa izmenjena
            //await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl za BO sa informacijom da je polisa izmenjena nije poslat.");
            //await ProveriStampu404(_page, "Štampaj polisu", "Greška u štampi izmenjene polise posle klika na Štampaj polisu.");

            //Provera slanja mejla za kupca
            //PrethodniZapisMejla = await ProcitajPoslednjiZapisMejla();

            //Pošalji mejl
            await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();



            //Provera da li je kupcu poslat mejl izmenjene polise
            //await ProveriStatusSlanjaMejla(PrethodniZapisMejla, "Mejl sa izmenjenom polisom koji ide ka kupcu nije poslat.");

            await _page.PauseAsync();
            return;



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

            //qBrojDokumenta = $"SELECT * FROM [WebshopDB].[webshop].[Dokument] WHERE [brojPonude] = {brojPonude};";
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
            //Server = OdrediServer(Okruzenje);

            // Konekcija sa bazom
            //string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
            //connectionString = $"Server = {Server}; Database = '' ; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
            //using (SqlConnection konekcija = new(connectionString))
            //{
            //konekcija.Open();
            //using (SqlCommand command = new(qBrojDokumenta, konekcija))
            //{
            //object BrojDokumenta1 = command.ExecuteScalar();
            //BrojDokumenta = BrojDokumenta1 != DBNull.Value ? Convert.ToInt32(BrojDokumenta1) : 0;
            //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
            //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
            //}
            //konekcija.Close();
            //}

            //Console.WriteLine($"ID dokumenta je na okruženju '{Okruzenje}' je: {BrojDokumenta}.\n");


            //Trace.Write($"Pregled polise:: ponuda {brojPonude}, ugovor {brojUgovora} - ");
            //await _page.GetByText(brojPonude).ClickAsync();
            //await ProveriURL(_page, PocetnaStrana, $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
            //Trace.WriteLine($"OK");

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
            /*******************************
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

            //await Pauziraj(_page);
            //Provera štampe izmenjene polise
            Trace.Write($"Štampa postojeće izmenjene polise - ");
            ******************************/
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

            /*********************************************
            Trace.WriteLine($"OK");

            //Provera slanja mejla
            await _page.Locator("button").Filter(new() { HasText = "Pošalji mejl" }).ClickAsync();
            Trace.WriteLine($"Proveri slanje mejla u [{DateTime.Now}].");

            // grBrojdokumenta = 0;
            //Provera polise bez ugovora
            Trace.WriteLine($"Proveri zašto postoji dugme Štampaj polisu za polisu koja nije kreirana?");
            //if (Okruzenje == "Razvoj")
            //{
            //    grBrojdokumenta = 82;
            //}



            //pronađi prvi koji nema broj ugovora
            //qBrojDokumenta = $"SELECT  [idDokument], [brojPonude], [brojUgovora] " +
            //$"FROM [WebshopDB].[webshop].[Dokument] " +
            //$"WHERE [brojUgovora] = '' " +
            //$"AND [brojPonude] = (SELECT MIN([brojPonude]) FROM [WebshopDB].[webshop].[Dokument] WHERE [brojUgovora] = '' AND [idDokument] > {grBrojdokumenta});;";
            //using (SqlConnection konekcija = new(connectionString))
            //{
            //konekcija.Open();
            //using (SqlCommand command = new(qBrojDokumenta, konekcija))
            //{
            //    object BrojDokumenta1 = command.ExecuteScalar();
            //    BrojDokumenta = BrojDokumenta1 != DBNull.Value ? Convert.ToInt32(BrojDokumenta1) : 0;

            //MaksimalniSerijskiTest = (long)(command.ExecuteScalar() ?? 0);
            //MaksimalniSerijski = (long)(command.ExecuteScalar() ?? 0);
            //}
            //konekcija.Close();
            //}
            */
            // Otvori polisu bez ugovora
            /*
                        Console.WriteLine($"Id dokumenta na okruženju '{Okruzenje}' koji nema ugovor je : {BrojDokumenta}.\n");
                        await _page.GotoAsync(PocetnaStrana + $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                        await ProveriURL(_page, PocetnaStrana, $"/BackOffice/BackOffice/1/Putno-osiguranje/{BrojDokumenta}");
                        Trace.Write($"Kreiranje polise za dokument {BrojDokumenta} kreirana - ");
                        await Pauziraj(_page);
                        //Obradi polisu bez ugovora
                        await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();
                        await _page.Locator("#inpJmbg input[type=\"text\"]").ClickAsync();
                        await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                        await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                        await _page.Locator("#inpJmbg input[type=\"text\"]").PressAsync("ArrowRight");
                        await _page.Locator("#inpJmbg input[type=\"text\"]").FillAsync("amso.bogdan@mail.eonsystem.com");
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
            /*
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
                        await _page.Locator("#inpJmbg input[type=\"text\"]").FillAsync("amso.bogdan@mail.eonsystem.com");
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
            /*
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
            */

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

