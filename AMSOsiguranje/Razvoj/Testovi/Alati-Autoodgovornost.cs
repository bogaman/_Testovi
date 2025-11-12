
namespace Razvoj
{

    public partial class Osiguranje

    {


        /// <summary>
        /// Određuje se i unosi serijski broj obrasca polise AO
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="korisnickoIme">Na osnovu njega se određuje magacin</param>
        /// <returns></returns>
        protected static async Task UnesiSerijskiBrojPoliseAO(IPage _page, string korisnickoIme)
        {
            // Pronađi prvi slobodan serijski broj za polisu AO

            Server = OdrediServer(Okruzenje);

            string connectionString = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}; Connection Timeout = 120";

            string Lokacija = "(7, 8)";

            if (korisnickoIme == "mario.radomir@eonsystem.com" && Okruzenje == "UAT")
            {
                Lokacija = "(3)";
            }
            else if (korisnickoIme == "mario.radomir@eonsystem.com" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
            {
                Lokacija = "(11)";
            }
            else
            {
                //throw new ArgumentException("Nepoznata uloga: " + korisnickoIme);
            }

            if (NacinPokretanjaTesta == "automatski" && (Okruzenje == "Razvoj" || Okruzenje == "Proba2"))
            {
                Lokacija = "(11)";
            }

            /********************************
            if (OsnovnaUloga == "BackOffice")
            {
                Lokacija = "(4)";
            }
            *********************************/

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
                LogovanjeTesta.LogError($"❌ Određivanje serijskog broja polise AO {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešno određivanje serijskog broja polise AO.", ex);
                throw;
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
            //Assert.Ignore($"Test se preskače jer: {labelaText}.");try


        }

        /// <summary>
        /// Unosi se mesto izdavanja polise AO.
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_rb">Redni broj polise AO</param>
        protected static async Task UnesiMestoIzdavanjaPoliseAO(IPage _page, string _rb)
        {

            try
            {
                if (_rb == "01" || _rb == "21" || _rb == "41")
                {
                    await _page.Locator("#selMestaIzdavanja > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("1234");
                    await _page.Locator("#selMestaIzdavanja").GetByText("- Bački Jarak").ClickAsync();
                }

                LogovanjeTesta.LogMessage($"✅ Uneseno mesto izdavanja polise AO: {_rb}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Neuspešan unos Mesta izdavanja polise AO: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan unos Mesta izdavanja polise AO.", ex);
                throw;
            }
        }


        /// <summary>
        /// Unosi se broj dana za polisu AO.
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_tipPolise"></param>
        protected static async Task UnesiBrojDanaZaPolisuAO(IPage _page, string _tipPolise)
        {

            try
            {
                if (_tipPolise != "Regularna")
                {
                    await _page.Locator("#inpBrDana").GetByRole(AriaRole.Textbox).ClickAsync();
                    await _page.Locator("#inpBrDana").GetByRole(AriaRole.Textbox).FillAsync(GenerisiBrojDana());
                }

                LogovanjeTesta.LogMessage($"✅ Unesen broj dana za polisu AO: {GenerisiBrojDana()}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Neuspešan unos broja dana za polisu AO: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan unos broja dana za polisu AO.", ex);
                throw;
            }
        }

        /// <summary>
        /// Unosi se tip ugovarača za polisu AO.
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_tipPolise"></param>
        /// <param name="_tipUgovaraca"></param>
        /// <returns></returns>
        protected static async Task UnesiTipUgovaracaZaPolisuAO(IPage _page, string _tipPolise, string _tipUgovaraca)
        {

            try
            {
                if (_tipPolise != "Granično osiguranje")
                {
                    await _page.Locator("#selOsiguranik > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    //await _page.Locator("#selOsiguranik div").Filter(new() { HasTextRegex = new Regex($"^{_tipUgovaraca}$") }).First.ClickAsync();
                    await _page.GetByText($"{_tipUgovaraca}").First.ClickAsync();
                }

                LogovanjeTesta.LogMessage($"✅ Unesen tip ugovarača za polisu AO: {_tipUgovaraca}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Neuspešan unos tipa ugovarača za polisu AO: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan unos tipa ugovarača za polisu AO.", ex);
                throw;
            }
        }




















        protected static string IzracunajKontrolnuCifruZK(string SerijskiBrojObrasca)
        {
            int intSerijskiBrojObrasca = Convert.ToInt32(SerijskiBrojObrasca);
            int kontrolna = intSerijskiBrojObrasca % 11;
            string kontrolnaCifra;

            if (kontrolna == 10)
            {
                kontrolnaCifra = "X";
            }
            else
            {
                kontrolnaCifra = kontrolna.ToString();
            }

            return kontrolnaCifra;
        }
        protected static int OdrediBrojDokumentaMtpl()
        {

            string qPoslednjiDokumentMtpl = "SELECT MAX([idDokument]) FROM [MtplDB].[mtpl].[Dokument];";
            string qPoslednjiDokumentMtplHistory = "SELECT MAX([idDokument]) FROM [MtplDB].[mtpl].[DokumentHistory];";
            int PoslednjiDokumentMtpl;
            int PoslednjiDokumentMtplHistory;
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
            string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}; Connection Timeout = 120";

            using (SqlConnection konekcija = new(connectionStringStroga))
            {
                konekcija.Open();
                using (SqlCommand command = new(qPoslednjiDokumentMtpl, konekcija))
                {
                    PoslednjiDokumentMtpl = (int)command.ExecuteScalar();
                }
                konekcija.Close();
            }
            Console.WriteLine($"Poslednji broj dokumenta u Mtpl je: {PoslednjiDokumentMtpl}.\n");

            using (SqlConnection konekcija = new(connectionStringStroga))
            {
                konekcija.Open();
                using (SqlCommand command = new(qPoslednjiDokumentMtplHistory, konekcija))
                {
                    PoslednjiDokumentMtplHistory = (int)command.ExecuteScalar();
                }
                konekcija.Close();
            }

            Console.WriteLine($"Poslednji broj dokumenta u Mtpl History je: {PoslednjiDokumentMtplHistory}.\n");

            int vrednost = Math.Max(PoslednjiDokumentMtpl, PoslednjiDokumentMtplHistory);
            Console.WriteLine($"Poslednji broj dokumenta je: {vrednost}.\n");

            return vrednost;
        }

        protected static int OdrediBrojDokumentaKasko()
        {

            string qPoslednjiDokumentKasko = "SELECT MAX([idDokument]) FROM [CascoDB].[casco].[Dokument];";
            string qPoslednjiDokumentKaskoMtplHistory = "SELECT MAX([idDokument]) FROM [CascoDB].[casco].[DokumentHistory];";
            int PoslednjiDokumentKasko;
            int PoslednjiDokumentKaskoHistory;

            Server = OdrediServer(Okruzenje);
            string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}; Connection Timeout = 120";

            using (SqlConnection konekcija = new(connectionStringStroga))
            {
                konekcija.Open();
                using (SqlCommand command = new(qPoslednjiDokumentKasko, konekcija))
                {
                    object rezultat = command.ExecuteScalar();
                    PoslednjiDokumentKasko = rezultat as int? ?? 0;
                    //PoslednjiDokumentKasko = (int)command.ExecuteScalar();
                }
                konekcija.Close();
            }
            Console.WriteLine($"Poslednji broj dokumenta u Mtpl je: {PoslednjiDokumentKasko}.\n");

            using (SqlConnection konekcija = new(connectionStringStroga))
            {
                konekcija.Open();
                using (SqlCommand command = new(qPoslednjiDokumentKaskoMtplHistory, konekcija))
                {
                    object rezultat = command.ExecuteScalar();
                    //PoslednjiDokumentKaskoHistory = (int)command.ExecuteScalar();
                    PoslednjiDokumentKaskoHistory = rezultat as int? ?? 0;
                }
                konekcija.Close();
            }

            Console.WriteLine($"Poslednji broj dokumenta u Mtpl History je: {PoslednjiDokumentKaskoHistory}.\n");

            int vrednost = Math.Max(PoslednjiDokumentKasko, PoslednjiDokumentKaskoHistory);
            Console.WriteLine($"Poslednji broj dokumenta je: {vrednost}.\n");

            return vrednost;
        }




        protected static string DefinisiBrojSasije()
        {
            // Definišite skup karaktera (25 slova i 10 cifara)
            const string skupKaraktera = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Duzina kombinacije
            const int duzinaKombinacije = 17;

            // Instanca klase Random
            Random random = new Random();

            // StringBuilder za efikasno kreiranje stringa
            StringBuilder kombinacija = new StringBuilder();

            // Generisanje kombinacije
            for (int i = 0; i < duzinaKombinacije; i++)
            {
                // Izbor slučajnog indeksa iz skupa
                int randomIndex = random.Next(0, skupKaraktera.Length);

                // Dodavanje slučajnog karaktera u kombinaciju
                kombinacija.Append(skupKaraktera[randomIndex]);
            }
            string BrojSasije = kombinacija.ToString();
            return BrojSasije;
        }
        protected static string DefinisiRegistarskuOznaku()
        {
            // Definišite skup karaktera (25 slova i 10 cifara)
            const string skupCifara = "0123456789";
            const string skupSlova = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // Duzina kombinacije
            const int duzinaCifara = 4;
            const int duzinaSlova = 2;

            // Instanca klase Random za cifre
            Random randomCifre = new Random();

            // StringBuilder za efikasno kreiranje stringa
            StringBuilder kombinacijaCifara = new StringBuilder();

            // Generisanje kombinacije
            for (int i = 0; i < duzinaCifara; i++)
            {
                // Izbor slučajnog indeksa iz skupa
                int randomIndex = randomCifre.Next(0, skupCifara.Length);

                // Dodavanje slučajnog karaktera u kombinaciju
                kombinacijaCifara.Append(skupCifara[randomIndex]);
            }

            // Instanca klase Random za slova
            Random randomSlova = new Random();

            // StringBuilder za efikasno kreiranje stringa
            StringBuilder kombinacijaSlova = new StringBuilder();

            // Generisanje kombinacije
            for (int i = 0; i < duzinaSlova; i++)
            {
                // Izbor slučajnog indeksa iz skupa
                int randomIndex = randomSlova.Next(0, skupSlova.Length);

                // Dodavanje slučajnog karaktera u kombinaciju
                kombinacijaSlova.Append(skupSlova[randomIndex]);
            }

            // Ispis rezultata

            string RegistarskaOznaka = kombinacijaCifara.ToString() + "-" + kombinacijaSlova.ToString();
            //Console.WriteLine($"Proizvoljna kombinacija od {duzinaKombinacije} znakova je: {kombinacija}");
            return RegistarskaOznaka;
        }







        /// <summary>
        /// Unosi tip polise za Autoodgovornost
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_tipPolise"></param>
        /// <returns></returns>
        protected static async Task UnesiTipPoliseAO(IPage _page, string _tipPolise)
        {
            await _page.GetByText("Tip polise").ClickAsync();

            await _page.GetByText("---RegularnaPrivremeno osiguranjeGranično osiguranje ---").ClickAsync();
            await _page.GetByText("Regularna").ClickAsync();

            await _page.GetByText("---RegularnaPrivremeno osiguranjeGranično osiguranje Regularna").ClickAsync();
            await _page.GetByText("Privremeno osiguranje").ClickAsync();

            await _page.GetByText("---RegularnaPrivremeno osiguranjeGranično osiguranje Privremeno osiguranje").ClickAsync();
            await _page.GetByText("Granično osiguranje").ClickAsync();

            await _page.GetByText("---RegularnaPrivremeno osiguranjeGranično osiguranje Granično osiguranje").ClickAsync();
            await _page.Locator("#selTipPolise").GetByText("---").ClickAsync();

            await _page.GetByText("---RegularnaPrivremeno osiguranjeGranično osiguranje ---").ClickAsync();

            //await _page.Locator("#selTipPolise").GetByText(_tipPolise).ClickAsync();
            await _page.GetByText(_tipPolise).ClickAsync();
        }

        /// <summary>
        /// Unosi datum od
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="Lokator"></param>
        /// <returns></returns>
        protected static async Task DatumOd(IPage _page, string Lokator)
        {
            //unos datuma početka
            await _page.Locator(Lokator).GetByRole(AriaRole.Textbox).ClickAsync();//await _page.Locator("#cal_calDatumOd").GetByPlaceholder("dd.mm.yyyy.").ClickAsync();
            await _page.Locator(Lokator).GetByRole(AriaRole.Textbox).FillAsync(NextDate.ToString("dd.MM.yyyy."));//await _page.GetByLabel(Variables.NextDate.ToString("MMMM d")).First.ClickAsync();

            //await _page.GetByLabel(NextDate.ToString("MMMM d")).Nth(2).ClickAsync();
            //await _page.GetByLabel("Avgust 28,").Nth(2).ClickAsync();
        }

        protected static async Task VremeOd(IPage _page, string Lokator)
        {
            //unos datuma početka
            await _page.Locator(Lokator).GetByRole(AriaRole.Textbox).ClickAsync();
            //await _page.Locator(Lokator).GetByRole(AriaRole.Textbox).FillAsync("23:50");
            await _page.Locator(Lokator).GetByRole(AriaRole.Textbox).FillAsync(DateTime.Now.AddHours(1).ToString("HH:mm"));

        }



        protected static async Task OcitajDokument(IPage _page, string tipDokumenta)
        {

            string nazivProcesa = "CitacEdoc";

            // 1. Provera da li je proces vec pokrenut
            // GetProcesses() vraca sve aktivne procese na sistemu.
            bool aplikacijaAktivna = Process.GetProcesses()
                                           .Any(p => p.ProcessName.Equals(nazivProcesa, System.StringComparison.OrdinalIgnoreCase));
            if (aplikacijaAktivna)
            {
                Console.WriteLine($"Aplikacija '{nazivProcesa}' je već aktivna.");
            }
            else
            {
                try
                {
                    // 2. Ako aplikacija nije aktivna, pokreni je
                    Process.Start(@"C:\Users\bogdan.mandaric\AppData\Local\Apps\2.0\2LRG08ZX.N6J\JO5A516J.KMK\cita..tion_2fca89819740be51_0001.0000_6c66621e7342d7e9\CitacEdoc.exe");
                    //Console.WriteLine($"Aplikacija '{nazivAplikacije}' je uspešno pokrenuta sa putanje: {putanjaDoExe}");
                }
                catch (Exception ex)

                {
                    Console.WriteLine($"Greška prilikom pokretanja aplikacije '{nazivProcesa}': {ex.Message}");
                    // U slučaju greške (npr. pogrešna putanja), možete ovde baciti izuzetak
                    // throw;
                }
            }

            if (tipDokumenta == "Saobracajna")
            {
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Očitaj saobraćajnu" }).ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Očitaj saobraćajnu" }).ClickAsync();
            }
            else if (tipDokumenta == "Licna1")
            {
                await _page.Locator("#btnOcitajLKUgovarac").GetByRole(AriaRole.Button, new() { Name = "Očitaj ličnu kartu" }).ClickAsync();
            }
            else if (tipDokumenta == "Licna2")
            {
                await _page.Locator("#btnOcitajLKKorisnik").GetByRole(AriaRole.Button, new() { Name = "Očitaj ličnu kartu" }).ClickAsync();
            }



            /*
                        string popupText = await _page.InnerTextAsync("body");

                        if (popupText.Contains("U čitaču/ima nije pronađena "))
                        {
                            // Akcija 1 ako je prozor otvoren i sadrži tekst
                            Console.WriteLine("Prozor je otvoren i sadrži očekivani tekst.");

                            //await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

                        }

                        // Akcija 1 ako je prozor otvoren i sadrži tekst
                        Console.WriteLine("Prozor je otvoren i sadrži očekivani tekst.");
            
            await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
*/

        }


        protected static async Task<string> OcitajDokument_1(IPage _page, string tipDokumenta)
        {
            string tekstNotifikacije = "";
            int maxPokusaja = 5; // Osiguraj da se test ne zaglavi u petlji
            int trenutniPokusaj = 0;

            // Lokatori za pop-upove i krstiće. 
            // Koristi `HasText` ili `Has` za pouzdanost, a ne samo ID.
            var notifyPopup = _page.Locator("//div[@class='notify greska']");


            //var notifikacija2Locator = _page.Locator("//div[@class='notify greska']");
            //var krstic1Locator = notifyPopup.Locator(".krstic");
            //var krstic2Locator = notifikacija2Locator.Locator(".krstic");


            // Playwright ne blokira izvršavanje ako element nije vidljiv.
            // Zbog toga je ovo idealno za if-else grananje
            bool notifikacijaVidljiva = false;
            //bool notifikacija2Vidljiva = false;
            while (true)
            {
                if (tipDokumenta == "Saobracajna")
                {
                    //await _page.GetByRole(AriaRole.Button, new() { Name = "Očitaj saobraćajnu" }).ClickAsync();
                    await _page.Locator("button").Filter(new() { HasText = "Očitaj saobraćajnu" }).ClickAsync();
                }
                else if (tipDokumenta == "Licna1")
                {
                    await _page.Locator("#btnOcitajLKUgovarac").GetByRole(AriaRole.Button, new() { Name = "Očitaj ličnu kartu" }).ClickAsync();
                }
                else if (tipDokumenta == "Licna2")
                {
                    await _page.Locator("#btnOcitajLKKorisnik").GetByRole(AriaRole.Button, new() { Name = "Očitaj ličnu kartu" }).ClickAsync();
                }

                trenutniPokusaj++;
                if (trenutniPokusaj > maxPokusaja)
                {
                    Assert.Fail("Prekoračen maksimalan broj pokušaja. Nijedna notifikacija nije uspela da se zatvori.");
                }

                try
                {
                    await notifyPopup.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 3000 });
                    notifikacijaVidljiva = true;

                    //Pojavila se Notifikacija
                    Console.WriteLine("Pojavila se Notifikacija1. Pokrećem WindowsProgram1.exe i ponavljam akciju.");
                    // Uzimamo tekst notifikacije
                    tekstNotifikacije = await notifyPopup.InnerTextAsync();

                    if (tekstNotifikacije.Contains("Servis nije pokrenut"))
                    {

                        Console.WriteLine("Servis nije pokrenut. Pokrećem CitacEdoc.exe.");
                        // Pokreće eksterni program
                        if (DeviceName == "BOGDANM")

                        {
                            Process.Start(@"C:\Users\bogdan.mandaric\AppData\Local\Apps\2.0\2LRG08ZX.N6J\JO5A516J.KMK\cita..tion_2fca89819740be51_0001.0000_6c66621e7342d7e9\CitacEdoc.exe");
                        }
                        else if (DeviceName == "LT-TESTER")
                        {
                            Process.Start(@"C:\Users\amsotest\AppData\Local\Apps\2.0\REDBD7PZ.NY1\OG72MP2Z.EJX\cita..tion_2fca89819740be51_0001.0000_6c66621e7342d7e9\CitacEdoc.exe");
                        }

                        // Zatvori pop-up
                        await _page.GetByText("Servis nije pokrenut!").ClickAsync();
                        //await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
                        await _page.Locator("//div[@id='notify0']//e-button[@type='icon']").ClickAsync();
                    }
                    else if (tekstNotifikacije.Contains("U čitaču/ima nije pronađena "))
                    {
                        // Akcija 1 ako je prozor otvoren i sadrži tekst
                        Console.WriteLine("Prozor je otvoren i sadrži očekivani tekst.");

                        // Zatvori pop-up
                        await _page.GetByText("U čitaču/ima nije pronađena ").ClickAsync();
                        //await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync(); 
                        //await _page.Locator("//div[@id='notify0']//e-button[@type='icon']").ClickAsync();
                        await _page.Locator("//div[@id='notify0']//i[@class='ico-xmark']").ClickAsync();

                    }
                    else
                    {
                        Console.WriteLine("Prozor je otvoren, ali ne sadrži očekivani tekst.");
                    }


                }
                catch (TimeoutException)
                {
                    notifikacijaVidljiva = false;
                }

                /*
                try
                {
                    await notifikacija2Locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 3000 });
                    notifikacija2Vidljiva = true;
                }
                catch (TimeoutException)
                {
                    notifikacija2Vidljiva = false;
                }
                */
                /*
                if (notifikacijaVidljiva)
                {
                    // Situacija 1: Pojavila se Notifikacija1
                    Console.WriteLine("Pojavila se Notifikacija1. Pokrećem WindowsProgram1.exe i ponavljam akciju.");
                    // Uzimamo tekst notifikacije
                    tekstNotifikacije = await notifyPopup.InnerTextAsync();

                    // Pokreće eksterni program
                    Process.Start(@"C:\Users\bogdan.mandaric\AppData\Local\Apps\2.0\2LRG08ZX.N6J\JO5A516J.KMK\cita..tion_2fca89819740be51_0001.0000_6c66621e7342d7e9\CitacEdoc.exe");
                    // Zatvori pop-up
                    await krstic1Locator.ClickAsync();

                    // Petlja će se ponovo pokrenuti i ponoviti klik na Dugme1
                }
                else if (notifikacija2Vidljiva)
                {
                    // Situacija 2: Pojavila se Notifikacija2
                    Console.WriteLine("Pojavila se Notifikacija2. Završavam scenario.");

                    // Zatvori pop-up
                    await krstic2Locator.ClickAsync();

                    // Izlazi iz petlje jer je scenario završen
                    //break;
                }
                else
                {
                    // Situacija 3: Nijedan pop-up se nije pojavio
                    Console.WriteLine("Nijedan pop-up nije detektovan. Scenario se nastavlja.");
                    //break; // Izlazi iz petlje da bi nastavio sa daljim testom
                }

                // Opcija: Dodati kratko čekanje pre sledećeg pokusaja
                await Task.Delay(500);


                // Mesto za dalji kod, npr. asercije
                //Assert.That(await _page.Locator("#dashboard").IsVisibleAsync(), Is.True);

*/
                /*
                            string popupText = await _page.InnerTextAsync("body");

                            if (popupText.Contains("U čitaču/ima nije pronađena "))
                            {
                                // Akcija 1 ako je prozor otvoren i sadrži tekst
                                Console.WriteLine("Prozor je otvoren i sadrži očekivani tekst.");

                                //await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

                            }

                            // Akcija 1 ako je prozor otvoren i sadrži tekst
                            Console.WriteLine("Prozor je otvoren i sadrži očekivani tekst.");

                await _page.Locator("#notify0").GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
            */

                return tekstNotifikacije;
            }
        }


        protected static async Task ObradiPomoc(IPage _page, string tipPomoci)
        {
            //await _page.Locator(tipPomoci).GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
            await _page.Locator(tipPomoci).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = " Pomoć" }).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = " Pomoć" }).GetByRole(AriaRole.Button).ClickAsync();
            await _page.Locator("h3 button").ClickAsync();
        }

        protected static async Task ProveriTarifu(IPage _page)
        {
            string className = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selTarife']", "el => el.className");
            string onemoguceno = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selTarife']", "el => el.disabled");
            //MessageBox.Show($"Class name:: {className}.", "Informacija", MessageBoxButtons.OK);
            //MessageBox.Show($"Kontrola je onemogućena: {onemoguceno}..", "Informacija", MessageBoxButtons.OK);
            if (className == "invisible" || onemoguceno == "True")
            {
                //MessageBox.Show($"Class name:: {className}. \nTarifa je vidljiva", "Informacija", MessageBoxButtons.OK);
                //MessageBox.Show($"Kontrola je disejblovana:: {omoguceno}. \nNije moguće editovanje.", "Informacija", MessageBoxButtons.OK);
            }
            else
            {
                //MessageBox.Show($"Kontrola je disejblovana:: {omoguceno}. \nEditovanje je moguće.", "Informacija", MessageBoxButtons.OK);
                await _page.Locator("//e-select[@id='selTarife']").ClickAsync();
                await _page.GetByText("nepostojeća test").ClickAsync();
                await _page.Locator("//e-select[@id='selTarife']").ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
                await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("Tarifa");
                await _page.GetByText("Tarifa").First.ClickAsync();
                //await _page.PauseAsync();
            }
        }
        protected static async Task ProveriPartnera(IPage _page)
        {
            //await _page.PauseAsync();
            //string mojElement = "//div[@class='commonBox font09 div-korisnik']/div[@class='row']";

            string mojElement = "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']";
            //e-select[@id='selPartner']//div[@class='multiselect-dropdown input']
            var elementPostoji = await _page.QuerySelectorAsync(mojElement);

            //MessageBox.Show($"{elementPostoji}", "Informacija", MessageBoxButtons.OK);

            if (elementPostoji != null)
            {
                //MessageBox.Show($"Element Partner postoji: {mojElement}.\nStatus je: {elementPostoji}", "Informacija", MessageBoxButtons.OK);
                string listaPartner = "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']";
                var listaPostoji = await _page.QuerySelectorAsync(listaPartner);
                if (listaPostoji != null)
                {
                    //MessageBox.Show($"Lista Partner postoji:{listaPostoji}", "Informacija", MessageBoxButtons.OK);
                }
                //await _page.Locator("#selPartner").GetByText("Partner").ClickAsync();
                await _page.Locator("#selPartner").ClickAsync();
                await _page.GetByText("AUTO CENTAR PETROVIC ML DOO").ClickAsync();
                //await _page.Locator("#selPartner").GetByText("Partner").ClickAsync();

                await _page.Locator("#selPartner").ClickAsync();
                //await _page.GetByText("AMS OSIGURANJE ADAUTO CENTAR PETROVIC ML DOO AMS OSIGURANJE AD").ClickAsync();
                await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^AMS OSIGURANJE AD$") }).ClickAsync();

            }

            else
            {
                //MessageBox.Show($"Element Partner ne postoji: {mojElement}.\nStatus je: {elementPostoji}\nPreskače se provera Partnera.", "Informacija", MessageBoxButtons.OK);
            }
            //string className = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']", "el => el.className");
            //string omoguceno = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']", "el => el.disabled");
            //string visina = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selPartner']//div[@class='control ']", "el => el.scrollHeight");

            //if (className != "invisible" && omoguceno != "False")
            //{
            //MessageBox.Show($"Class name:: {className}. \nSaradnik je vidljiv", "Informacija", MessageBoxButtons.OK);
            //MessageBox.Show($"Kontrola je disejblovana:: {omoguceno}. \nNije moguće editovanje.", "Informacija", MessageBoxButtons.OK);
            //MessageBox.Show($"Visina kontrole je:: {visina}.", "Informacija", MessageBoxButtons.OK);
            //}
            //else
            //{
            //    MessageBox.Show($"Kontrola je disejblovana:: {omoguceno}. \nEditovanje je moguće.", "Informacija", MessageBoxButtons.OK);
            //    await _page.Locator("//e-select[@id='selTarife']").ClickAsync();
            //    await _page.GetByText("nepostojeća test").ClickAsync();
            //    await _page.Locator("//e-select[@id='selTarife']").ClickAsync();
            //    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
            //    await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync("Tarifa");
            //    await _page.GetByText("Tarifa").First.ClickAsync();
            //    await _page.PauseAsync();
            //}


        }

        /// <summary>
        /// Unosi porez na osnovu stanja kontrole i vrednosti iz baze
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_oslobodjenPoreza"></param>
        /// <returns></returns>
        protected static async Task UnesiPorez(IPage _page, string _oslobodjenPoreza)
        {
            try
            {
                var elementPorez = _page.Locator("//e-checkbox[@id='chkOslobodjenPoreza']//div[@class='control ']");
                //var elementPorezId = await elementPorez.EvaluateAsync<string>("el => el.id");
                //MessageBox.Show($"ID elementa je: {elementPorezId}", "Informacija", MessageBoxButtons.OK);

                // Čitanje className svojstva
                //var elementPorezClass = await elementPorez.EvaluateAsync<string>("el => el.className");
                //MessageBox.Show($"Class elementa je: {elementPorezClass}", "Informacija", MessageBoxButtons.OK);

                // Čitanje hidden svojstva
                string elementPorezHidden = await elementPorez.EvaluateAsync<string>("el => el.hidden");
                //MessageBox.Show($"Hidden elementa je: {elementPorezHidden}", "Informacija", MessageBoxButtons.OK);

                // Čitanje disabled svojstva
                //var elementPorezDisabled = await elementPorez.EvaluateAsync<string>("el => el.disabled");
                //MessageBox.Show($"Disable elementa je: {elementPorezDisabled}", "Informacija", MessageBoxButtons.OK);

                if (elementPorezHidden != "False")
                {
                    await _page.GetByText("Oslobođen poreza").ClickAsync();
                    await _page.Locator("#chkOslobodjenPoreza i").ClickAsync();
                    await _page.Locator("#chkOslobodjenPoreza div").Nth(3).ClickAsync();
                    await _page.GetByText("Oslobođen poreza").ClickAsync();
                }

                if (_oslobodjenPoreza == "Da" && elementPorezHidden != "False")
                {
                    await _page.GetByText("Oslobođen poreza").ClickAsync();
                }



                LogovanjeTesta.LogMessage($"✅ Unos poreza na osnovu stanja kontrole: {_oslobodjenPoreza}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Neuspešan unos poreza na osnovu stanja kontrole: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan unos poreza na osnovu stanja kontrole.", ex);
                throw;
            }


        }

        protected static async Task ProveriPrethodnuPolisu(IPage _page)
        {
            //provera Društva
            await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("13 - AMS OSIGURANJE A.D.O.").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("02 - DDOR NOVI SAD A.D.O.").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("15 - GENERALI OSIGURANJE").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("24 - GLOBOS OSIGURANJE A.D.O. BEOGRAD").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("23 - GRAWE OSIGURANJE A.D.O. BEOGRAD").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("06 - MILENIJUM OSIGURANJE A.D").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("04 - TRIGLAV OSIGURANJE A.D.O. BEOGRAD").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("14 - UNIQA NEŽIVOTNO").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("01 - DUNAV OSIGURANJE A.D.O. BEOGRAD").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("17 - SAVA NEŽIVOTNO").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("19 - WIENER STÄDTISCHE").ClickAsync();
            //await _page.Locator("#selDrustvo > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).ClickAsync();
            await _page.Locator("#selDrustvo").GetByText("---").ClickAsync();

            //Provera premijskog stepena
            await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            await _page.Locator("#selPremijskiStepen").GetByText("1", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("2", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("3", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("4", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("5", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("6", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("7", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("8", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("9", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("10", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("11", new() { Exact = true }).ClickAsync();
            //await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            //await _page.Locator("#selPremijskiStepen").GetByText("12", new() { Exact = true }).ClickAsync();
            await _page.Locator("//e-select[@id='selPremijskiStepen']//div[@class='multiselect-dropdown input']").ClickAsync();
            await _page.Locator("#selPremijskiStepen").GetByText("---").ClickAsync();

            //Provera registarskog broja polise
            await _page.GetByText("Registarski broj polise", new() { Exact = true }).ClickAsync();


            //Klik na Pretraži UOS
            await _page.Locator("#btnPretraziUos button").ClickAsync();

            var labela = _page.Locator("//e-input[@id='inpRegistarskiBroj']//label[@class='info-text ']");
            await labela.ClickAsync();
            // Čitanje textContent svojstva
            string labelaText = await labela.EvaluateAsync<string>("el => el.textContent");

            /*******************************
            if (labelaText != "")
            {
                MessageBox.Show($"Text Content elementa je: {labelaText}", "Informacija", MessageBoxButtons.OK);

            }
            *****************************/
            Assume.That(labelaText, Is.Not.EqualTo(""), $"Test se preskače jer: {labelaText}. \n");
        }

        protected static async Task UnesiRegistarskiBrojAO(IPage _page, string _registarskiBrojAO)
        {
            await _page.GetByText("Registarski broj AO").ClickAsync();
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).FillAsync(_registarskiBrojAO);
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).PressAsync("Tab");
        }



        /// <summary>
        /// Unosi tip lica za polisu AO na osnovu vrednosti iz baze
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_tipPolise"></param>
        /// <param name="_tipUgovaraca"></param>
        /// <param name="_tipLica1">Fizičko ili Pravno </param>
        /// <param name="_tipLica2">Fizičko, Pravno ili 0</param>
        /// <param name="_platilac"></param>
        /// <returns></returns>
        protected static async Task UnesiTipLicaZaPolisuAO(IPage _page, string _tipPolise, string _tipUgovaraca, string _tipLica1, string _tipLica2, string _platilac)
        {
            try
            {

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
                    if (_platilac == "Levi")
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



                LogovanjeTesta.LogMessage($"✅ Unos tipa lica.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Neuspešan unos tipa lica: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan unos tipa lica.", ex);
                throw;
            }


        }










        protected static string GenerisiSnagu(string _premijskaGrupa)
        {
            Random rnd = new();

            if (_premijskaGrupa == "Putničko vozilo")
                return rnd.Next(10, 200).ToString();
            else if (_premijskaGrupa == "Teretno vozilo")
                return rnd.Next(50, 250).ToString();
            else if (_premijskaGrupa == "Autobusi")
                return rnd.Next(100, 500).ToString();
            else if (_premijskaGrupa == "Vučna vozila")
                return rnd.Next(10, 200).ToString();
            else if (_premijskaGrupa == "Specijalna motorna vozila")
                return rnd.Next(200, 1001).ToString();
            else if (_premijskaGrupa == "Motocikli")
                return rnd.Next(10, 150).ToString();
            else if (_premijskaGrupa == "Priključna vozila")
                return rnd.Next(10, 150).ToString();
            else if (_premijskaGrupa == "Radna vozila")
                return rnd.Next(10, 150).ToString();
            else
                return rnd.Next(0, 1401).ToString();
        }
        protected static string GenerisiZapreminu(string _premijskaGrupa)
        {
            Random rnd = new();

            if (_premijskaGrupa == "Putničko vozilo")
                return rnd.Next(500, 3000).ToString();
            else if (_premijskaGrupa == "Teretno vozilo")
                return rnd.Next(1000, 5000).ToString();
            else if (_premijskaGrupa == "Autobusi")
                return rnd.Next(1000, 5000).ToString();
            else if (_premijskaGrupa == "Vučna vozila")
                return rnd.Next(1000, 5000).ToString();
            else if (_premijskaGrupa == "Specijalna motorna vozila")
                return rnd.Next(20, 5001).ToString();
            else if (_premijskaGrupa == "Motocikli")
                return rnd.Next(10, 1150).ToString();
            else if (_premijskaGrupa == "Priključna vozila")
                return rnd.Next(10, 150).ToString();
            else if (_premijskaGrupa == "Radna vozila")
                return rnd.Next(10, 150).ToString();
            else
                return rnd.Next(0, 1401).ToString();
        }

        protected static string GenerisiNosivost(string _premijskaGrupa)
        {
            Random rnd = new();

            if (_premijskaGrupa == "Putničko vozilo")
                return rnd.Next(500, 3000).ToString();
            else if (_premijskaGrupa == "Teretno vozilo")
                return rnd.Next(100, 15000).ToString();
            else if (_premijskaGrupa == "Autobusi")
                return rnd.Next(1000, 5000).ToString();
            else if (_premijskaGrupa == "Vučna vozila")
                return rnd.Next(1000, 5000).ToString();
            else if (_premijskaGrupa == "Specijalna motorna vozila")
                return rnd.Next(20, 5001).ToString();
            else if (_premijskaGrupa == "Motocikli")
                return rnd.Next(10, 1150).ToString();
            else if (_premijskaGrupa == "Priključna vozila")
                return rnd.Next(100, 25000).ToString();
            else if (_premijskaGrupa == "Radna vozila")
                return rnd.Next(10, 150).ToString();
            else
                return rnd.Next(0, 1401).ToString();
        }
        protected static string GenerisiBrojMesta(string _premijskaGrupa)
        {
            Random rnd = new();

            if (_premijskaGrupa == "Putničko vozilo")
                return rnd.Next(1, 9).ToString();
            else if (_premijskaGrupa == "Teretno vozilo")
                return rnd.Next(1, 9).ToString();
            else if (_premijskaGrupa == "Autobusi")
                return rnd.Next(50, 200).ToString();
            else if (_premijskaGrupa == "Motocikli")
                return rnd.Next(1, 3).ToString();
            else
                return rnd.Next(1, 9).ToString();
        }

        protected static string GenerisiBrojDana()
        {
            Random rnd = new();


            return rnd.Next(1, 300).ToString();
        }



    }
}