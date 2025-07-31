
using System.Threading.Tasks;

namespace Razvoj
{

    public partial class OsiguranjeVozila : Osiguranje
    {


        private static string OdrediServer(string okruzenje)
        {
            string server;
            server = okruzenje switch
            {
                "Razvoj" => "10.5.41.99",
                "Proba2" => "49.13.25.19",
                "UAT" => "10.41.5.5",
                "Produkcija" => "",
                _ => throw new ArgumentException($"Nepoznato okruženje: {okruzenje}.\nIP adresa servera nije dobro određena."),
            };
            return server;
        }






        private async Task<string> ProcitajCeliju(int red, int kolona)
        {
            try
            {
                // XPath koristi "contains" da bi bio otporan na dodatne klase
                string xpath = $"(//div[contains(@class, 'podaci')]//div[contains(@class, 'grid-row')])[{red}]//div[contains(@class, 'column')][{kolona}]";
                // "(//div[contains(@class, 'podaci')]//div[contains(@class, 'grid-row')])[1]//div[contains(@class, 'column')]"
                //await _page.Locator(xpath).HoverAsync();
                //await _page.Locator(xpath).ClickAsync();
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page instance is null.");
                var celija = _page.Locator(xpath);
                string tekst = await celija.InnerTextAsync();
                //TestLogger.LogMessage($"Pročitano iz [red: {red}, kolona: {kolona}]: {tekst}");
                return tekst;
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException($"Greška pri čitanju ćelije [red: {red}, kolona: {kolona}]", ex);
                return string.Empty;
            }
        }

        //Definišu se podaci potrebni za logovanje - mejl, ime i kozinka
        private static void PodaciZaLogovanje(string uloga, string okruzenje, out string mejl, out string ime, out string lozinka)
        {
            switch (uloga, okruzenje)
            {
                case ("Agent", "Razvoj"):
                    mejl = "bogdan.mandaric@eonsystem.com";
                    ime = "Bogdan Mandarić";
                    lozinka = "Lozinka1!";
                    break;
                case ("Agent", "Proba2"):
                    mejl = "bogdan.mandaric@eonsystem.com";
                    ime = "Bogdan Mandarić";
                    lozinka = "Lozinka1!";
                    break;
                case ("Agent", "UAT"):
                    mejl = "bogdan.mandaric@eonsystem.com";
                    ime = "Bogdan Mandarić";
                    lozinka = "Lozinka1!";
                    break;
                case ("Agent", "Produkcija"):
                    mejl = "bogdan.mandaric@eonsystem.com";
                    ime = "Bogdan Mandarić";
                    lozinka = "Lozinka1!";
                    break;
                case ("BackOffice", "Razvoj"):
                    mejl = "davor.bulic@eonsystem.com";
                    ime = "Davor Bulić";
                    lozinka = "Lozinka1!";
                    break;
                case ("BackOffice", "Proba2"):
                    mejl = "davor.bulic@eonsystem.com";
                    ime = "Davor Bulić";
                    lozinka = "Lozinka1!";
                    break;
                case ("BackOffice", "UAT"):
                    mejl = "davor.bulic@eonsystem.com";
                    ime = "Davor Bulić";
                    lozinka = "Lozinka1!";
                    break;
                case ("BackOffice", "Produkcija"):
                    mejl = "davor.bulic@eonsystem.com";
                    ime = "Davor Bulić";
                    lozinka = "MikicaU8ica?";
                    break;
                default:
                    throw new ArgumentException("Nepoznata uloga: " + uloga);
            }
        }




        private static void Loguj(string poruka)
        {
            string putanja = $"{LogovanjeTesta.LogFolder}\\monitoring_log.txt";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            File.AppendAllText(putanja, $"[{timestamp}] {poruka}{Environment.NewLine}");
        }

        public class ZapisOSlanjuMejla
        {
            public long PoslednjiID { get; set; }
            public long PoslednjiIDMail { get; set; }
            public int Status { get; set; }
            public string Opis { get; set; } = "";
            public DateTime Datum { get; set; }
            public string Subject { get; set; } = "";
        }

        // Čita poslednji zapis o slanju mejla, uoči provere slanja sledećeg mejla
        static async Task<ZapisOSlanjuMejla> ProcitajPoslednjiZapisMejla()
        {
            try
            {
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
                string connectionString = $"Server = {Server}; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                //string qryPoslednjiMejl = "SELECT TOP 1 * FROM [NotificationsDB].[mail].[MailDeliveryStatus] ORDER BY [ID] DESC;";
                string qryPoslednjiMejl = "SELECT TOP 1 [NotificationsDB].[mail].[MailDeliveryStatus].*, [NotificationsDB].[mail].[MailHeaders].[Subject] " +
                                          "FROM [NotificationsDB].[mail].[MailDeliveryStatus] " +
                                          "INNER JOIN [NotificationsDB].[mail].[MailHeaders] ON [NotificationsDB].[mail].[MailDeliveryStatus].[IDMail] = [NotificationsDB].[mail].[MailHeaders].[IDMail] " +
                                          "ORDER BY [ID] DESC;";

                //using (SqlConnection connection = new SqlConnection(connectionString))
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using SqlCommand command = new(qryPoslednjiMejl, connection);
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return await Task.FromResult(new ZapisOSlanjuMejla
                    {
                        PoslednjiID = reader.GetInt64(reader.GetOrdinal("ID")),
                        PoslednjiIDMail = reader.GetInt64(reader.GetOrdinal("IDMail")),
                        Status = Convert.ToInt32(reader["Status"]),
                        Opis = reader.IsDBNull(reader.GetOrdinal("Opis")) ? "-" : reader.GetString(reader.GetOrdinal("Opis")),
                        Datum = reader.GetDateTime(reader.GetOrdinal("Datum")),
                        Subject = reader.IsDBNull(reader.GetOrdinal("Subject")) ? "-" : reader.GetString(reader.GetOrdinal("Subject"))

                    });

                }
                else
                {
                    return await Task.FromResult<ZapisOSlanjuMejla?>(null); // Nema zapisa
                }

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Greška prilikom čitanja poslednjeg zapisa mejla: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Greška prilikom čitanja poslednjeg zapisa mejla", ex);
                return null;
            }
        }



        // Funkcija koja proverava zadati RB i status u petlji do timeout-a
        static async Task<ZapisOSlanjuMejla?> CekajNoviZapis(int noviID, int? status, int timeoutMs)
        {
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
            {
                var zapis = await ProcitajPoslednjiZapisMejla();
                if (zapis.PoslednjiID == noviID && (status == null || zapis.Status == status))
                    return zapis;

                await Task.Delay(500); // Čekaj 500ms pre sledeće provere
            }
            return null;
        }


        private static async Task ProveriStatusSlanjaMejla(ZapisOSlanjuMejla PrethodniZapisMejla)
        {
            try
            {
                // 2. Čekaj da se pojavi novi zapis sa RB + 1 i Status = 1
                var noviID1 = PrethodniZapisMejla.PoslednjiID + 1;
                var zapis1 = await CekajNoviZapis((int)noviID1, status: 1, timeoutMs: 100000);
                if (zapis1 != null)
                {
                    //Loguj($"1 Poslat mejl -> ID: {zapis1.PoslednjiID}, IDMail: {zapis1.PoslednjiIDMail}, Status: {zapis1.Status}, Opis: {zapis1.Opis},  Datum: {zapis1.Datum}, Subject: : {zapis1.Subject}");
                    LogovanjeTesta.LogMessage($"✅ 1 Poslat mejl -> ID: {zapis1.PoslednjiID}, IDMail: {zapis1.PoslednjiIDMail}, Status: {zapis1.Status}, Opis: {zapis1.Opis},  Datum: {zapis1.Datum}, Subject: : {zapis1.Subject}", false);
                }

                else
                {
                    //Loguj("Timeout: Nije stigao mejl sa Status = 1");
                    LogovanjeTesta.LogError("❌ Timeout: Nije stigao mejl sa Status = 1");
                }

                // 2. Čekaj da se pojavi novi zapis sa RB + 1 i Status = 1
                var noviID2 = PrethodniZapisMejla.PoslednjiID + 2;
                var zapis2 = await CekajNoviZapis((int)noviID2, status: 2, timeoutMs: 100000);
                if (zapis2 != null)
                {
                    Loguj($" ✅2 Poslat mejl -> ID: {zapis2.PoslednjiID}, IDMail: {zapis2.PoslednjiIDMail}, Status: {zapis2.Status}, Opis: {zapis2.Opis},  Datum: {zapis2.Datum}, Subject: : {zapis2.Subject}");
                    LogovanjeTesta.LogMessage($"✅ 2 Poslat mejl -> ID: {zapis2.PoslednjiID}, IDMail: {zapis2.PoslednjiIDMail}, Status: {zapis2.Status}, Opis: {zapis2.Opis},  Datum: {zapis2.Datum}, Subject: : {zapis2.Subject}", false);
                }
                else
                {
                    Loguj("Timeout: Nije stigao mejl sa Status = 2");
                    LogovanjeTesta.LogError("❌ Timeout: Nije stigao mejl sa Status = 2");

                }

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Greška prilikom provere statusa slanja mejla: {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Greška prilikom provere statusa slanja mejla", ex);
                Loguj($"Greška: {ex.Message}");
            }

        }
        private ZapisOSlanjuMejla ProcitajNoviZapisMejla(long poslednjiID, long poslednjiIDMail, int status, string opis, DateTime datum)
        {
            return new ZapisOSlanjuMejla
            {
                PoslednjiID = poslednjiID,
                PoslednjiIDMail = poslednjiIDMail,
                Status = status,
                Opis = opis,
                Datum = datum
            };
        }



        private static (long poslednjiID, long poslednjiIDMail, int status, string opis, DateTime Datum) ProcitajPoslednjiZapis()
        {
            #region Pročitaj poslednji mejl
            long poslednjiID = -1;
            long poslednjiIDMail = -1;
            int status = -1;
            string opis = "greška";
            DateTime datum = DateTime.Now;
            //int noviStatus = -1;
            //int maxPokusaja = 10;
            //int intervalCekanjaMs = 1000; // 1 sekunda
            string logFile1 = @"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\log_mejl.txt";
            /*
            Server = Okruzenje switch
            {
                "Razvoj" => "10.5.41.99",
                Proba2 => "49.13.25.19",
                "UAT" => "10.41.5.5",
                "Produkcija" => "",
                _ => throw new ArgumentException("Nepoznata uloga: " + Okruzenje),
            };
            */
            Server = OdrediServer(Okruzenje);
            string connectionString = $"Server = {Server}; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
            string qryPoslednjiMejl = "SELECT TOP 1 * FROM [NotificationsDB].[mail].[MailDeliveryStatus] ORDER BY ID DESC;";

            /*  
              string qPoslednjiIDNotifikacija = "SELECT MAX ([ID]) FROM [NotificationsDB].[mail].[MailDeliveryStatus];";
              string qPoslednjiIDMail = "SELECT MAX ([IDMail]) FROM [NotificationsDB].[mail].[MailDeliveryStatus];";

              using (SqlConnection konekcija = new(connectionStringIDNotifikacija))
              {
                  konekcija.Open();
                  using (SqlCommand command = new(qPoslednjiIDNotifikacija, konekcija))
                  {
                      poslednjiID = (Int64)command.ExecuteScalar();
                  }
                  using (SqlCommand command = new(qPoslednjiIDMail, konekcija))
                  {
                      poslednjiIDMail = (Int64)command.ExecuteScalar();
                  }

                  konekcija.Close();
              }
  */

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(qryPoslednjiMejl, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            poslednjiID = reader.GetInt64(reader.GetOrdinal("ID"));
                            poslednjiIDMail = reader.GetInt64(reader.GetOrdinal("IDMail"));
                            status = Convert.ToInt32(reader["Status"]);
                            opis = reader.IsDBNull(reader.GetOrdinal("Opis")) ? "-" : reader.GetString(reader.GetOrdinal("Opis"));
                            datum = reader.GetDateTime(reader.GetOrdinal("Datum"));

                            string logLine = $"ID: {poslednjiID}, IDMail: {poslednjiIDMail}, Status: {status}, Opis: {opis}, Datum: {datum}";
                            //Console.WriteLine(logLine);
                            File.AppendAllText(logFile1, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} {logLine}{Environment.NewLine}");
                        }
                        else
                        {
                            File.AppendAllText(logFile1, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} Nema rezultata za dati upit.{Environment.NewLine}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile1, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} Greška: {ex.Message}{Environment.NewLine}");
            }
            return (poslednjiID, poslednjiIDMail, status, opis, datum);

            //return Task.CompletedTask;
            #endregion Pročitaj poslednji mejl
        }

        private static async Task ProveriStampu404(IPage _page, string Dugme, string Poruka)
        {
            var pageStampa = await _page.RunAndWaitForPopupAsync(async () =>
                        {
                            await _page.Locator("button").Filter(new() { HasText = Dugme }).ClickAsync();
                        });

            var errorElement = pageStampa.Locator("text=HTTP ERROR 404");
            try
            {

                if (await errorElement.IsVisibleAsync())
                {
                    //Assert.That(await errorElement.IsHiddenAsync(), Is.True, $"{Poruka} 'HTTP ERROR 404' je vidljiv na stranici.");

                    File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"+++++++++++++++++++++{DateTime.Now:dd.MM.yyyy HH:mm:ss} {Environment.NewLine}");
                    LogovanjeTesta.LogMessage($"✅ {Poruka} ima 'HTTP ERROR 404'", false);
                    Exception ex = new PlaywrightException();
                    await LogovanjeTesta.LogException($"✅ {Poruka} ima 'HTTP ERROR 404'", ex);
                }
                else
                {
                    //Exception ex = new PlaywrightException();
                    //await LogovanjeTesta.LogException($"✅ {Poruka} nema 'HTTP ERROR 404'", ex);
                }

            }

            //catch (AssertionException ex)
            catch (PlaywrightException ex)
            {
                // Upisivanje u fajl
                File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"--------------------------{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {ex.Message}{Environment.NewLine}");
                LogovanjeTesta.LogError($"❌ {Poruka} ima 'HTTP ERROR 404'");

                //throw; // Sa throw se test prekida, bez throw se test nastavlja.
            }


            await pageStampa.CloseAsync();
        }


        private static async Task ProveriStampuPdf(IPage _page, string Dugme, string Poruka)
        {
            var pageStampa = await _page.RunAndWaitForPopupAsync(async () =>
                            {
                                await _page.Locator("button").Filter(new() { HasText = Dugme }).ClickAsync();
                            });

            try
            {
                Assert.That(pageStampa.Url, Does.EndWith(".pdf"), $"{Poruka} 'završava sa pdf'.");
                File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"+++++++++++++++++++++{DateTime.Now:dd.MM.yyyy HH:mm:ss} {Environment.NewLine}");
                LogovanjeTesta.LogMessage($"✅ {Poruka} završava sa pdf", false);
            }
            catch (AssertionException ex)
            {
                // Upisivanje u fajl
                File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"--------------------------{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {ex.Message}{Environment.NewLine}");
                LogovanjeTesta.LogError($"❌ {Poruka} ne završava sa pdf");
                await LogovanjeTesta.LogException($"❌ {Poruka} ne završava sa pdf", ex);
                throw; // Sa throw se test prekida, bez throw se test nastavlja.
            }

            /*********************
                                        if (pageStampa.Url.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                                        {
                                            Console.WriteLine("Otvoren je PDF dokument.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nije otvoren PDF dokument.");
                                            Trace.WriteLine($"nije OK");
                                            return;
                                        }

                                        ************************/
            await pageStampa.CloseAsync();



        }






        public static void PorukaKrajTesta()
        {
            string NazivTekucegTesta = TestContext.CurrentContext.Test.Name;
            System.Windows.MessageBox.Show($"Test #{NazivTekucegTesta}# je završen.", "Informacija", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Information);
        }







        private static void WriteToCsv(string filePath, List<(string podatak1, string podatak2, string podatak3)> data)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Upis zaglavlja
                writer.WriteLine("SerijskiBroj,IdTipObrasca,IDLokacijaZaduzena");

                // Upis podataka
                foreach (var (podatak1, podatak2, podatak3) in data)
                {
                    writer.WriteLine($"{podatak1},{podatak2},{podatak3}");
                }
            }
        }


        private static string IzvrsiUpit(string connectionString, string upit)
        {
            using SqlConnection konekcija = new(connectionString);
            Console.WriteLine($"Initial State: {konekcija.State}");
            konekcija.Open();
            Console.WriteLine($"State after opening: {konekcija.State}");
            Console.WriteLine("Uspostavljena je veza sa bazom StrictEvidenceDB!");
            SqlCommand command = new(upit, konekcija);
            int rezultat = (int)command.ExecuteScalar();

            konekcija.Close();
            Console.WriteLine($"State after closing: {konekcija.State}.\n");
            return rezultat.ToString();
        }


        private static string IzracunajKontrolnuCifruZK(string SerijskiBrojObrasca)
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
        private static int OdrediBrojDokumenta()
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
            string connectionStringStroga = $"Server = {Server}; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";

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
        private static string DefinisiBrojSasije()
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
        private static string DefinisiRegistarskuOznaku()
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

        /*
        private static string DefinisiPocetnuStranu(string okruzenje)
        {
            string pocetnaStrana = okruzenje switch
            {
                "Razvoj" => "https://razvojamso-master.eonsystem.rs",
                "Proba2" => "https://proba2amsomaster.eonsystem.rs",
                "UAT" => "https://master-test.ams.co.rs",
                "Produkcija" => "https://eos.ams.co.rs",
                _ => throw new ArgumentException("Nepoznata stranica: " + PocetnaStrana)
            };
            return pocetnaStrana;
        }
        */







        public static async Task NovaPolisa(IPage _page, string tipOsiguranja)
        {
            await _page.GetByRole(AriaRole.Link, new() { Name = tipOsiguranja }).ClickAsync();
        }

        public static async Task ProveriSadrzajNaStrani(IPage _page, string[] tekstZaProveru)
        {
            foreach (var text in tekstZaProveru)
            {
                var element = _page.Locator($"text={text}");
                bool isVisible = await element.IsVisibleAsync();

                if (!isVisible)
                {
                    System.Windows.Forms.MessageBox.Show($"Element sa natpisom '{text}' nije pronađen na stranici.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public static async Task ProveriVidljivostKontrole(IPage _page, string kontrola, string tip)
        {
            bool isVisible = await _page.IsVisibleAsync(kontrola);
            if (isVisible)
            {
                //MessageBox.Show($"Element {kontrola} je vidljiv: {isVisible}\n\nProverava se:: {tip}.", "Informacija", MessageBoxButtons.OK);
                await _page.Locator(kontrola).ClickAsync();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show($"Element:\n{kontrola} nije vidljiv: {isVisible}.\n\nProverava se:: {tip}.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        public static async Task ProveriKontrolu(IPage _page, string kontrola)
        {
            var elementPostoji = await _page.QuerySelectorAsync(kontrola);

            string kontrolaSkracena1944 = kontrola[19..^44];
            string kontrolaSkracena042 = kontrola[0..^42];

            if (elementPostoji != null)
            {
                bool isVisible = await _page.IsVisibleAsync(kontrola);

                string className2 = await _page.EvalOnSelectorAsync<string>(kontrolaSkracena042, "el => el.className");
                string onemoguceno2 = await _page.EvalOnSelectorAsync<string>(kontrolaSkracena042, "el => el.disabled");
                if (isVisible)
                {
                    System.Windows.MessageBox.Show($"isVisible = {isVisible} - vidljiv je.\n",
                                    $"Element je {kontrolaSkracena1944}", (MessageBoxButton)MessageBoxButtons.OK);

                    if (onemoguceno2 == "False")
                    {
                        System.Windows.MessageBox.Show($"disabledAttr = '{onemoguceno2}' - moguće editovanje", $"Element je {kontrolaSkracena1944}", (MessageBoxButton)MessageBoxButtons.OK);

                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"disabledAttr = '{onemoguceno2}' - nije moguće editovanje", $"Element je {kontrolaSkracena1944}", (MessageBoxButton)MessageBoxButtons.OK);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show($"isVisible = {isVisible} - nije vidljiv.\n",
                                    $"Element je {kontrolaSkracena1944}", (MessageBoxButton)MessageBoxButtons.OK);
                }

            }
            else
            {
                System.Windows.MessageBox.Show($"Element NE postoji", $"Element je {kontrolaSkracena1944}", (MessageBoxButton)MessageBoxButtons.OK);
            }
        }
        public static async Task ProveriStanjeKontrole(IPage _page, string kontrola)
        {
            bool isVisible = await _page.IsVisibleAsync(kontrola);
            string pattern = @"'sel[^']*'";
            Match match = Regex.Match(kontrola, pattern);
            string foundText = match.Value;
            //string cleanedText = foundText.TrimEnd('\''); // briše poslednji znak
            // Izdvaja se string bez prva 4 i poslednjeg znaka
            string cleanedText = foundText.Substring(4, foundText.Length - 5);

            string? disabledAttr = await _page.Locator(kontrola).GetAttributeAsync("disabled");
            //MessageBox.Show($"Atribut 'Disabled'  {disabledAttr}", "Informacija", MessageBoxButtons.OK);

            //MessageBox.Show($"Vidljivost elementa {cleanedText}: {isVisible}. \n", "Informacija", MessageBoxButtons.OK);
            if (isVisible)
            {
                //MessageBox.Show($"Element:: {cleanedText} je vidljiv.\nVidljivost je:: {isVisible}", "Informacija", MessageBoxButtons.OK);


                //if (disabledAttr != null)
                if (disabledAttr == "")
                {
                    //MessageBox.Show($"Element:: {cleanedText} je disejblovan:: {disabledAttr} .\nNije moguće editovanje.", "Informacija", MessageBoxButtons.OK);
                    StanjeKontrole = 1;
                }
                else
                {
                    //MessageBox.Show($"Element:: {cleanedText} je enejblovan:: {disabledAttr} .\nMoguće je editovanje.", "Informacija", MessageBoxButtons.OK);
                    StanjeKontrole = 0;
                }
            }
            else
            {
                //MessageBox.Show($"Element:: {cleanedText} nije vidljiv.\nVidljivost je:: {isVisible}", "Informacija", MessageBoxButtons.OK);
                StanjeKontrole = 2;
            }
        }




        private static async Task UnesiTipPolise(IPage _page, string _tipPolise)
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

        private static async Task DatumOd(IPage _page)
        {
            //unos datuma početka
            await _page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).ClickAsync();//await _page.Locator("#cal_calDatumOd").GetByPlaceholder("dd.mm.yyyy.").ClickAsync();
            await _page.Locator("#cal_calDatumOd").GetByRole(AriaRole.Textbox).FillAsync(NextDate.ToString("dd.MM.yyyy."));//await _page.GetByLabel(Variables.NextDate.ToString("MMMM d")).First.ClickAsync();

            //await _page.GetByLabel(NextDate.ToString("MMMM d")).Nth(2).ClickAsync();
            //await page.GetByLabel("Avgust 28,").Nth(2).ClickAsync();
        }

        private static async Task OcitajDokument(IPage _page, string tipDokumenta)
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

        private static async Task ObradiPomoc(IPage _page, string tipPomoci)
        {
            //await _page.Locator(tipPomoci).GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
            await _page.Locator(tipPomoci).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = " Pomoć" }).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = " Pomoć" }).GetByRole(AriaRole.Button).ClickAsync();
            await _page.Locator("h3 button").ClickAsync();
        }

        private static async Task ProveriTarifu(IPage _page)
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
        private static async Task ProveriPartnera(IPage _page)
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

        private static async Task ProveriPadajucuListu(IPage _page, string kontrola)
        {

            var elementPostoji = await _page.QuerySelectorAsync(kontrola);

            string kontrolaSkracena1944 = kontrola[19..^44];
            string kontrolaSkracena042 = kontrola[0..^42];


            //string className = await _page.EvalOnSelectorAsync<string>("//e-select[@id='selTarife']", "el => el.className");
            if (elementPostoji != null)
            {
                //MessageBox.Show($"Element {kontrola[19..^44]} {elementPostoji} - postoji", "Informacija", MessageBoxButtons.OK);
            }
            else
            {
                //MessageBox.Show($"Element {kontrola[19..^44]} {elementPostoji} - ne postoji", "Informacija", MessageBoxButtons.OK);
            }

            bool isVisible = await _page.IsVisibleAsync(kontrola);


            //string pattern = @"'sel[^']*'";
            //Match match = Regex.Match(kontrola, pattern);
            //string foundText = match.Value;
            //string cleanedText = foundText.TrimEnd('\''); // briše poslednji znak
            // Izdvaja se string bez prva 4 i poslednjeg znaka
            //string cleanedText = foundText[4..^1];

            string? disabledAttr = await _page.Locator(kontrola).GetAttributeAsync("disabled");
            if (disabledAttr != null)
            {
                //MessageBox.Show($"Diseblovanost kontrole {kontrola[19..^44]} je  {disabledAttr} - DISABLE", "Informacija", MessageBoxButtons.OK);
            }
            else
            {
                //MessageBox.Show($"Diseblovanost kontrole {kontrola[19..^44]} je  {disabledAttr} - ENABLE", "Informacija", MessageBoxButtons.OK);
            }

            //MessageBox.Show($"Atribut 'Disabled'  {disabledAttr}", "Informacija", MessageBoxButtons.OK);

            //MessageBox.Show($"Vidljivost elementa {cleanedText}: {isVisible}. \n", "Informacija", MessageBoxButtons.OK);

            string className = await _page.EvalOnSelectorAsync<string>(kontrola[0..^42], "el => el.className");
            string onemoguceno = await _page.EvalOnSelectorAsync<string>(kontrola[0..^42], "el => el.disabled");
            //MessageBox.Show($"Class name: {className}.", "Informacija", MessageBoxButtons.OK);
            //MessageBox.Show($"Kontrola je onemogućena: {onemoguceno}.", "Informacija", MessageBoxButtons.OK);
            if (className == "invisible" || onemoguceno == "True")
                if (isVisible)
                {
                    //MessageBox.Show($"Element:: {cleanedText} je vidljiv.\nVidljivost je:: {isVisible}", "Informacija", MessageBoxButtons.OK);


                    //if (disabledAttr != null)
                    if (disabledAttr == "")
                    {
                        //MessageBox.Show($"Element:: {cleanedText} je disejblovan:: {disabledAttr} .\nNije moguće editovanje.", "Informacija", MessageBoxButtons.OK);
                        StanjeKontrole = 1;
                    }
                    else
                    {
                        //MessageBox.Show($"Element:: {cleanedText} je enejblovan:: {disabledAttr} .\nMoguće je editovanje.", "Informacija", MessageBoxButtons.OK);
                        StanjeKontrole = 0;
                    }
                }
                else
                {
                    //MessageBox.Show($"Element:: {cleanedText} nije vidljiv.\nVidljivost je:: {isVisible}", "Informacija", MessageBoxButtons.OK);
                    StanjeKontrole = 2;
                }

        }


        private static async Task UnesiPorez(IPage _page, string _oslobodjenPoreza)
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



        }

        private static async Task ProveriPrethodnuPolisu(IPage _page)
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

        private static async Task UnesiRegistarskiBrojAO(IPage _page, string _registarskiBrojAO)
        {
            await _page.GetByText("Registarski broj AO").ClickAsync();
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).FillAsync(_registarskiBrojAO);
            await _page.Locator("#inpRegistarskiBrojAO").GetByRole(AriaRole.Textbox).PressAsync("Tab");
        }


        /// <summary>
        /// Uloguj se koristeći korisničko ime i lozinku.
        /// <para>Korisničko ime i lozinka određuju se na osnovu NacinPokretanjaTesta i RucnaUloga </para></summary>
        /// <param name="_page"></param>
        /// <param name="korisnickoIme">Može biti BackOffice ili Agent</param>
        /// <param name="lozinka">Može biti BackOffice ili Agent</param>
        public static async Task UlogujSe(IPage _page, string korisnickoIme, string lozinka)
        {
            try
            {
                // Unesi korisničko ime
                await _page.GetByText("Korisničko ime").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(korisnickoIme);
                // Unesi lozinku
                await _page.GetByText("Lozinka").ClickAsync();
                await _page.Locator("input[type=\"password\"]").FillAsync(lozinka);
                // Klik na Prijava
                await _page.Locator("//text[.='Prijava']").ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Ulogovan je korisnik: {KorisnikMejl}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Logovanje korisnika {KorisnikMejl} neuspešno. {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Logovanje korisnika {KorisnikMejl} neuspešno.", ex);
                throw;
            }
        }

        /// <summary>
        /// Uloguj se koristeći korisničko ime i lozinku.
        /// <para>Korisničko ime i lozinka se određuju na osnovu NacinPokretanjaTesta i RucnaUloga </para>
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="Uloga">Može biti BackOffice ili Agent</param>
        /// <returns></returns>
        public static async Task UlogujSe_2(IPage _page, string Uloga)
        {
            try
            {
                string korisnickoIme = KorisnikLoader5.Korisnik3?.KorisnickoIme ?? string.Empty;
                string korisnikPassword = KorisnikLoader5.Korisnik3?.Lozinka1 ?? string.Empty;
                if (Uloga == "BackOffice")
                {
                    korisnickoIme = KorisnikLoader5.Korisnik1?.KorisnickoIme ?? string.Empty;
                    korisnikPassword = KorisnikLoader5.Korisnik1?.Lozinka1 ?? string.Empty;
                    if (Okruzenje == "Produkcija")
                    {
                        korisnikPassword = KorisnikLoader5.Korisnik1?.Lozinka2 ?? string.Empty;
                    }
                }
                else if (OsnovnaUloga == "Agent")
                {
                    if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Bogdan")
                    {
                        korisnickoIme = KorisnikLoader5.Korisnik2?.KorisnickoIme ?? string.Empty;
                        korisnikPassword = KorisnikLoader5.Korisnik2?.Lozinka1 ?? string.Empty;
                    }
                    else if (NacinPokretanjaTesta == "ručno" && RucnaUloga == "Mario")
                    {
                        korisnickoIme = KorisnikLoader5.Korisnik3?.KorisnickoIme ?? string.Empty;
                        korisnikPassword = KorisnikLoader5.Korisnik3?.Lozinka1 ?? string.Empty;
                    }

                }
                else
                {
                    throw new ArgumentException("Nepoznat korisnik. Molimo proverite unos.");
                }
                // Ako nije BackOffice, dodeli podrazumevane vrednosti ili podesi prema potrebi
                // korisnickoIme = ...; korisnikPassword = ...;


                // Unesi korisničko ime
                await _page.GetByText("Korisničko ime").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(korisnickoIme);
                // Unesi lozinku
                await _page.GetByText("Lozinka").ClickAsync();
                await _page.Locator("input[type=\"password\"]").FillAsync(korisnikPassword);
                // Klik na Prijava
                await _page.Locator("//text[.='Prijava']").ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Ulogovan je korisnik: {KorisnikMejl}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Logovanje korisnika {KorisnikMejl} neuspešno. {ex.Message}");
                throw;
            }
        }


        // Logovanje na početnoj stranici
        /// <summary>
        /// Uloguj se na aplikaciju koristeći korisničko ime i lozinku.
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="OsnovnaUloga">Može biti BackOffice ili Agent</param>
        /// <param name="RucnaUloga">Može biti "Ne" ili ime korisnika (npr. Bogdan, Mario)</param>
        /// <returns></returns>
        public static async Task UlogujSe_1(IPage _page, string OsnovnaUloga, string RucnaUloga)
        {
            try
            {
                string korisnickoIme = string.Empty;
                string korisnikPassword = string.Empty;
                if (OsnovnaUloga == "BackOffice")
                {
                    korisnickoIme = KorisnikLoader5.Korisnik1?.KorisnickoIme ?? string.Empty;
                    korisnikPassword = KorisnikLoader5.Korisnik1?.Lozinka1 ?? string.Empty;
                    if (Okruzenje == "Produkcija")
                    {
                        korisnikPassword = KorisnikLoader5.Korisnik1?.Lozinka2 ?? string.Empty;
                    }
                }
                else if (OsnovnaUloga == "Agent")
                {
                    if ((RucnaUloga == "Ne" && NacinPokretanjaTesta == "ručno") || RucnaUloga == "Bogdan")
                    {
                        korisnickoIme = KorisnikLoader5.Korisnik2?.KorisnickoIme ?? string.Empty;
                        // Unesi lozinku
                        korisnikPassword = KorisnikLoader5.Korisnik2?.Lozinka1 ?? string.Empty;
                    }
                    else if ((RucnaUloga == "Ne" && NacinPokretanjaTesta == "automatski") || RucnaUloga == "Mario")
                    {
                        korisnickoIme = KorisnikLoader5.Korisnik3?.KorisnickoIme ?? string.Empty;
                        // Unesi lozinku
                        korisnikPassword = KorisnikLoader5.Korisnik3?.Lozinka1 ?? string.Empty;
                    }

                }


                else
                {
                    throw new ArgumentException("Nepoznat korisnik. Molimo proverite unos.");
                }
                // Ako nije BackOffice, dodeli podrazumevane vrednosti ili podesi prema potrebi
                // korisnickoIme = ...; korisnikPassword = ...;


                // Unesi korisničko ime
                await _page.GetByText("Korisničko ime").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(korisnickoIme);
                // Unesi lozinku
                await _page.GetByText("Lozinka").ClickAsync();
                await _page.Locator("input[type=\"password\"]").FillAsync(korisnikPassword);
                // Klik na Prijava
                await _page.Locator("//text[.='Prijava']").ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Ulogovan je korisnik: {KorisnikMejl}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Logovanje korisnika {KorisnikMejl} neuspešno. {ex.Message}");
                throw;
            }
        }






        // Logovanje na početnoj stranici
        public static async Task UlogujSe_6(IPage _page, string KorisnikMejl, string KorisnikPassword)
        {
            try
            {
                // Unesi korisničko ime
                await _page.GetByText("Korisničko ime").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").ClickAsync();
                await _page.Locator("#rightBox input[type=\"text\"]").FillAsync(KorisnikMejl);
                // Unesi lozinku
                await _page.GetByText("Lozinka").ClickAsync();
                await _page.Locator("input[type=\"password\"]").FillAsync(KorisnikPassword);
                // Klik na Prijava
                await _page.Locator("//text[.='Prijava']").ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Ulogovan je korisnik: {KorisnikMejl}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Logovanje korisnika {KorisnikMejl} neuspešno. {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Odjavi se iz aplikacije.
        /// </summary>
        /// <param name="_page"></param>
        /// <returns></returns>
        public static async Task IzlogujSe(IPage _page)
        {
            try
            {
                //Trace.Write($"Odjavljivanje - ");
                await _page.Locator(".korisnik").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Odjavljivanje" }).ClickAsync();
                //await _page.GotoAsync(PocetnaStrana + "/Login");
                //await ProveriURL(_page, PocetnaStrana, "/Login");
                LogovanjeTesta.LogMessage($"✅ Korisnik izlogovan.", false);
                //Trace.WriteLine($"OK");
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Odjavljivanje korisnika. {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Odjavljivanje korisnika {KorisnikMejl} neuspešno.", ex);
                throw;
            }
        }


        private static async Task SnimiDokument(IPage _page, int brDokument, string staSeSnima)
        {
            try
            {
                await _page.Locator("button").Filter(new() { HasText = "Snimi" }).ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Snimanje: {staSeSnima}, dokument {brDokument + 1}", false);
                //Trace.WriteLine($"OK");

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Snimanje: {staSeSnima}, dokument {brDokument + 1} {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Snimanje: {staSeSnima}, dokument {brDokument + 1} neuspešno.", ex);
                throw;
            }

        }

        /// <summary>
        /// Bira se opcija iz padajuće liste ili &lt;select&gt; elementa.
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="selektorListe">Selektor (lokator) padajuće liste</param>
        /// <param name="vrednostOpcije">Opcija koju treba izabrati</param>
        /// <param name="koristiSelectOption"></param>
        /// <returns></returns>
        public static async Task IzaberiOpcijuIzListe(IPage _page, string selektorListe, string vrednostOpcije, bool koristiSelectOption = true)
        {
            try
            {
                var lista = _page.Locator(selektorListe);

                // 1. Proveri da li je opcija već selektovana (korisno ako lista prikazuje selektovani tekst)
                string trenutnoSelektovano = await lista.InnerTextAsync();
                if (trenutnoSelektovano.Trim() == vrednostOpcije.Trim())
                {
                    LogovanjeTesta.LogMessage($"⚠️ Opcija '{vrednostOpcije}' je već selektovana u '{selektorListe}'.", false);
                    return;
                }

                // 2. Ako je HTML <select> element, koristi SelectOptionAsync
                if (koristiSelectOption)
                {
                    var selectElement = _page.Locator(selektorListe);
                    var tagName = await selectElement.EvaluateAsync<string>("e => e.tagName");
                    if (tagName.ToLower() == "select")
                    {
                        await selectElement.SelectOptionAsync(new SelectOptionValue { Label = vrednostOpcije });
                        LogovanjeTesta.LogMessage($"✅ Opcija '{vrednostOpcije}' izabrana korišćenjem SelectOptionAsync na '{selektorListe}'.", false);
                        return;
                    }
                }

                // 3. Fallback na klik-based selekciju
                await lista.ClickAsync();
                await _page.Locator(selektorListe).GetByText(vrednostOpcije, new() { Exact = true }).ClickAsync();
                LogovanjeTesta.LogMessage($"✅ Opcija '{vrednostOpcije}' selektovana klikom u '{selektorListe}'.", false);
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException($"Greška prilikom izbora opcije '{vrednostOpcije}' u listi '{selektorListe}'", ex);
                throw;
            }
        }

        private static async Task UnesiMagacin(IPage _page, string selektor)
        {
            try
            {
                await _page.Locator(selektor).ClickAsync();
                if (Okruzenje == "UAT")
                {
                    await _page.GetByText("Centralni magacin", new() { Exact = true }).ClickAsync();
                }
                else
                {
                    await _page.GetByText("Centralni magacin 1", new() { Exact = true }).First.ClickAsync();
                }

                LogovanjeTesta.LogMessage($"✅ Izbor magacina", false);
                //Trace.WriteLine($"OK");

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Izbor magacina.\n{ex.Message}");
                throw;
            }

        }
        private static async Task ObrisiDokument(IPage _page, int brDokument)
        {
            try
            {
                await _page.Locator("#btnObrisiDokument button").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();
                LogovanjeTesta.LogMessage($"✅ Dokument obrisan: {brDokument + 1}", false);
                //Trace.WriteLine($"OK");

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Brisanje dokumenta {brDokument + 1} {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Brisanje dokumenta {brDokument + 1} neuspešno.", ex);
                throw;
            }

        }


        //Provera da li postoji grid
        private static async Task ProveraPostojiGrid(IPage _page, string tipGrida)
        {
            string lokatorGrid = string.Empty;
            try
            {

                lokatorGrid = tipGrida switch
                {
                    "Polise AO" => "//e-grid[@id='grid_dokumenti']",
                    "Obrasci polisa AO" => "//e-grid[@id='grid_obrasci']",
                    "Dokumenta stroge evidencije za polise AO" => "//e-grid[@id='grid_dokumenti']",
                    "Zahtevi za izmenu polisa AO" => "//e-grid[@id='grid_zahtevi_za_izmenu']",
                    "Razdužne liste AO" => "//e-grid[@id='grid_dokumenti']",
                    "Obrasci polisa ZK" => "//e-grid[@id='grid_obrasci']",
                    "Dokumenti Stroge evidencije za polise ZK" => "//e-grid[@id='grid_dokumenti']",
                    "Polise ZK" => "//e-grid[@id='grid_dokumenti']",
                    "grid Dokumenti" => "//e-grid[@id='grid_dokumenti']",
                    "Pregled razdužnih listi za JS" => "//e-grid[@id='grid_dokumenti']",
                    "grid Obrasci" => "//e-grid[@id='grid_dokumenti']",
                    "Pregled razdužnih listi za DK" => "//e-grid[@id='grid_dokumenti']",
                    "Pregled razdužnih listi za SE" => "//e-grid[@id='grid_dokumenti']",
                    "Pregled dokumenata za BO" => "//e-grid[@id='grid_dokumenti']",
                    "Polise Kasko" => "//e-grid[@id='grid_dokumenti']",
                    "Razdužne liste Kasko" => "//e-grid[@id='grid_dokumenti']",
                    "Produkcija" => "",
                    _ => throw new ArgumentException($"Nepoznato okruženje: {tipGrida}.\nIP adresa servera nije dobro određena."),
                };

                await _page.Locator(lokatorGrid).ClickAsync(new LocatorClickOptions
                {
                    Position = new Position { X = 99, Y = 1 }  // 99% širine, 1% visine
                });
                LogovanjeTesta.LogMessage($"✅ Grid: {tipGrida} - {lokatorGrid} pronađena.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Grid nije pronađen: {tipGrida} - {lokatorGrid} \\n {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Grid nije pronađen: {tipGrida} - {lokatorGrid}", ex);
                throw;
            }
        }

        //Proveri postojanje kontrole
        public static async Task ProveriPostojanjeKontrole(IPage _page, string kontrola, string tip)
        {
            try
            {
                await _page.Locator(kontrola).ClickAsync(new LocatorClickOptions
                {
                    Position = new Position { X = 99, Y = 1 }  // 99% širine, 1% visine
                });

                LogovanjeTesta.LogMessage($"✅ Kontrola: {tip} - {kontrola} pronađena.", false);


                /*******************
                var kontrolaPostoji = await _page.QuerySelectorAsync(kontrola);
                if (kontrolaPostoji == null)
                {
                    throw new Exception($"❌ Grid kontrola {tip} nije pronađena na stranici.");
                }

                LogovanjeTesta.LogMessage("222222222222222222", false);
                
                //else
                //{
                    //Trace.WriteLine($"Pronađena je kontrola: {kontrola} - OK");
                //    LogovanjeTesta.LogMessage($"✅ Postoji kontrola: {tip} -  {kontrola}.", false);
                //}
*********************/
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Kontrola nije pronađena: {tip} - {kontrola} \\n {ex.Message}");
                throw;
            }
            // Prekid testiranja
            //Trace.WriteLine($"Nije pronađena kontrola: {kontrola} - Nije OK");
            //LogovanjeTesta.LogError($"❌ 1111111111111Nije pronađena kontrola: - {tip} - {kontrola}");
            //throw new Exception($"22222222222222222Testiranje prekinuto: Kontrola '{tip}' nije pronađena.");



            //MessageBox.Show($"Pronađena je kontrola:\n{kontrola}.\n\nProverava se:: {tip}.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Console.WriteLine($"Proverava se:: {tip}. Pronađena je kontrola:\n{kontrola}.");





            /*
            var kontrolaPostoji = await _page.QuerySelectorAsync(kontrola);
            if (kontrolaPostoji != null)
            {
                //MessageBox.Show($"Pronađena je kontrola:\n{kontrola}.\n\nProverava se:: {tip}.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Console.WriteLine($"Proverava se:: {tip}. Pronađena je kontrola:\n{kontrola}.");
            }
            else
            {
                //MessageBox.Show($"Nije pronađena kontrola:\n{kontrola}.\n\nProverava se:: {tip}.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine($"Proverava se:: {tip}. Nije pronađena kontrola:\n{kontrola}.");
                return;
            }
            */
        }


        //Proveri da li radi filter na gridu
        private static async Task ProveriFilterGrida(IPage _page, string kriterijum, string tipGrida, int kolona)
        {
            try
            {
                //Trace.Write($"Filter na gridu obrazaca AO za admina - ");
                string ukupanBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                if (kolona == 5)
                {
                    await _page.Locator($"div:nth-child({kolona}) > .filterItem > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Na verifikacijiU izradiVerifikovan$") }).Locator("div").Nth(2).ClickAsync();
                    await _page.Locator($"div:nth-child({kolona + 1}) > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                }






                else if (kolona == 9 || kolona == 10)
                {
                    await _page.Locator($"div:nth-child({kolona}) > .filterItem > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
                    await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^KreiranRaskinutStorniranU izradi$") }).Locator("div").Nth(0).ClickAsync();
                    await _page.Locator($"div:nth-child({kolona + 1}) > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                }

                else
                {
                    await _page.Locator($"div:nth-child({kolona}) > .filterItem > .control-wrapper > .control > .control-main > .input").ClickAsync();
                    await _page.Locator($"div:nth-child({kolona}) > .filterItem > .control-wrapper > .control > .control-main > .input").FillAsync(kriterijum);
                    await _page.Locator($"div:nth-child({kolona}) > .filterItem > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");
                }
                await Task.Delay(3000); // Pauza od 3 sekunde (3000 ms) da se filter učita

                string filtriraniBrojStrana = await _page.Locator("//e-button[@class='btn-page-num num-max']").InnerTextAsync();
                if ((Convert.ToInt32(ukupanBrojStrana) >= Convert.ToInt32(filtriraniBrojStrana)) && (Convert.ToInt32(filtriraniBrojStrana) > 0))
                {
                    LogovanjeTesta.LogMessage($"✅ Filter na gridu {tipGrida} radi OK", false);
                }
                else
                {
                    throw new Exception($"❌Filter na gridu {tipGrida} ne radi.");
                }
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Filter na gridu {tipGrida} ne radi. {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Filter na gridu {tipGrida} ne radi.", ex);
                throw;
            }

        }

        public async Task ProveraVestPostoji_old(IPage _page, string ocekivaniTekst)
        {
            try
            {
                string sadrzajStranice = await _page.InnerTextAsync("body");
                bool sadrziTekst = sadrzajStranice.Contains(ocekivaniTekst);
                // Loguj rezultat
                LogovanjeTesta.LogMessage($"✅ Stranica sadrži tekst: '{ocekivaniTekst}'", false);
                // Assertuj rezultat
                Assert.That(sadrziTekst, Is.True, $"Tekst '{ocekivaniTekst}' NIJE pronađen na stranici.");
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException(NazivTekucegTesta, ex);
                LogovanjeTesta.LogError(NazivTekucegTesta);
                throw;
            }
        }
        public async Task ProveraVestPostoji(IPage _page, string ocekivaniTekst)
        {
            try
            {
                // Čekaj da se tekst pojavi na stranici, ali maksimalno 5 sekundi
                var jsHandle = await _page.WaitForFunctionAsync(
                    @"(tekst) => document.body && document.body.innerText.includes(tekst)",
                    ocekivaniTekst,
                    new PageWaitForFunctionOptions { Timeout = 5000 });
                bool tekstPronadjen = await jsHandle.JsonValueAsync<bool>();

                // Loguj rezultat
                LogovanjeTesta.LogMessage($"✅ Tekst '{ocekivaniTekst}' je pronađen na stranici.", false);

                // Assertuj rezultat
                Assert.That(tekstPronadjen, Is.True, $"Tekst '{ocekivaniTekst}' NIJE pronađen na stranici.");
            }
            catch (TimeoutException)
            {
                // Ako se tekst nije pojavio u zadatom roku, pročitaj stranicu i uradi dodatnu proveru
                string sadrzajStranice = await _page.InnerTextAsync("body");
                bool sadrziTekst = sadrzajStranice.Contains(ocekivaniTekst);

                if (!sadrziTekst)
                {
                    string poruka = $"❌ Tekst '{ocekivaniTekst}' NIJE pronađen na stranici ni nakon čekanja.";
                    LogovanjeTesta.LogError(poruka);
                    LogovanjeTesta.LogTestResult(NazivTekucegTesta, false);
                    Assert.Fail(poruka);
                }
                else
                {
                    LogovanjeTesta.LogMessage($"✅ Tekst '{ocekivaniTekst}' je pronađen u fallback proveri.", false);
                }
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException(NazivTekucegTesta, ex);
                LogovanjeTesta.LogTestResult(NazivTekucegTesta, false);
                throw;
            }
        }


        public static async Task ProveraVestJeObrisana(IPage _page, string ocekivaniTekst)
        {
            try
            {
                string sadrzajStranice = await _page.InnerTextAsync("body");
                bool sadrziTekst = sadrzajStranice.Contains(ocekivaniTekst);
                // Loguj rezultat
                LogovanjeTesta.LogMessage($"✅ Vest je obrisana: '{ocekivaniTekst}'", false);
                // Assertuj rezultat
                Assert.That(sadrziTekst, Is.False, $"Tekst '{ocekivaniTekst}' JE pronađen na stranici.");
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException(NazivTekucegTesta, ex);
                LogovanjeTesta.LogTestResult(NazivTekucegTesta, false);
                //throw;
            }
        }

        public async Task ProveraVestNijeObrisana(IPage _page, string ocekivaniTekst)
        {
            try
            {
                //string sadrzajStranice = await _page.InnerTextAsync("body");
                string sadrzajStranice = await _page.InnerTextAsync("podaci");
                bool sadrziTekst = sadrzajStranice.Contains(ocekivaniTekst);
                // Loguj rezultat
                LogovanjeTesta.LogMessage($"❌ Stranica sadrži tekst: '{ocekivaniTekst}'", false);
                // Assertuj rezultat
                Assert.That(sadrziTekst, Is.True, $"Tekst '{ocekivaniTekst}' JE pronađen na stranici.");
            }
            catch (Exception ex)
            {
                await LogovanjeTesta.LogException(NazivTekucegTesta, ex);
                LogovanjeTesta.LogTestResult(NazivTekucegTesta, false);
                //throw;
            }
        }
        //Proveri postojanje automatski generisane vesti
        private static async Task VestPostoji(IPage _page, string ocekivaniTekst, string oznakaDokumenta)
        {
            await _page.Locator(".ico-ams-logo").ClickAsync();
            string tekst = ocekivaniTekst;
            try
            {
                await _page.GetByText(tekst).First.ClickAsync();
                LogovanjeTesta.LogMessage($"✅ Vest pronađena: {tekst}: {oznakaDokumenta}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Vest nije pronađena: {tekst}: {oznakaDokumenta}\n {ex.Message}");
                throw;
            }
        }



        private static async Task VestNePostoji(IPage _page, string ocekivaniTekst, string oznakaDokumenta)
        {
            await _page.Locator(".ico-ams-logo").ClickAsync();
            string tekst = ocekivaniTekst;
            try
            {
                await _page.GetByText(tekst).First.ClickAsync();
                LogovanjeTesta.LogMessage($"❌ Vest je pronađena: {tekst}: {oznakaDokumenta}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogMessage($"✅ Vest nije pronađena: {tekst}: {oznakaDokumenta}\n {ex.Message}");
                //throw;
            }
        }


        //Arhiviranje vesti
        private static async Task ArhivirajVest(IPage _page, string ocekivaniTekst, string oznakaDokumenta)
        {
            try
            {
                // Hover nad elementom koji sadrži oznaku dokumenta
                var oznakaElement = _page.GetByText(oznakaDokumenta).First;
                await oznakaElement.HoverAsync();

                await oznakaElement.HoverAsync(new LocatorHoverOptions
                {
                    Position = new Position { X = 5, Y = 5 }
                });

                //await _page.GetByText("A K T U E L N A").First.HoverAsync();

                //await _page.GetByText("A K T U E L N A").First.HoverAsync(new LocatorHoverOptions
                //{
                //Position = new Position { X = 40, Y = 20 }
                //});

                // Sačekaj da dugme postane vidljivo
                var dugmeArhiviraj = _page.Locator(".btnArhiviraj > .left").First;
                await dugmeArhiviraj.HoverAsync();
                await dugmeArhiviraj.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 3000 // ms
                });


                // Klikni na dugme "Arhiviraj"
                await dugmeArhiviraj.ClickAsync();

                // Klik na potvrdu
                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                // Potvrda da je vest arhivirana
                await _page.GetByText("Vest je arhivirana.").ClickAsync();

                LogovanjeTesta.LogMessage($"✅ Vest uspešno arhivirana - {oznakaDokumenta}", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Nema vesti za arhiviranje");
                LogovanjeTesta.LogError($"❌ Arhiviranje vesti: {oznakaDokumenta} \n {ex.Message}");
                throw;
            }
        }

        private static async Task ArhivirajVest_old(IPage _page, string ocekivaniTekst, string oznakaDokumenta)
        {

            try
            {

                await _page.GetByText(oznakaDokumenta).First.HoverAsync();
                //await _page.GetByText(ocekivaniTekst).First.ClickAsync();
                // Sačekaj da se podmeni pojavi
                await _page.WaitForSelectorAsync(".btnArhiviraj > .left");
                //await _page.GetByText("A K T U E L N A").First.HoverAsync();
                //await _page.GetByText("A K T U E L N A").First.ClickAsync();
                /*
                await _page.GetByText("A K T U E L N A").First.HoverAsync(new LocatorHoverOptions
                {
                    Position = new Position { X = 30, Y = 10 }
                });
                await _page.GetByText("A K T U E L N A").First.HoverAsync(new LocatorHoverOptions
                {
                    Position = new Position { X = 40, Y = 20 }
                });
                */
                await _page.Locator(".btnArhiviraj > .left").First.ClickAsync();
                await _page.GetByText("Da li ste sigurni da želite").ClickAsync();
                await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();

                await _page.GetByText("Vest je arhivirana.").ClickAsync();
                LogovanjeTesta.LogMessage($"✅ Vest uspešno arhivirana - {oznakaDokumenta}", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Nema vesti za arhiviranje");
                LogovanjeTesta.LogError($"❌ Arhiviranje vesti: {oznakaDokumenta} \\n {ex.Message}");
                throw;
            }
        }


        //Ovde se ispituje pojavljivanje pop-up elementa, ovo probaj da razradiš
        /***************************
                        try
                        {
                            var popupElement = _page.Locator("div").Filter(new() { HasText = "Vest je arhivirana." });

                            await popupElement.WaitForAsync(new LocatorWaitForOptions
                            {
                                State = WaitForSelectorState.Visible,
                                Timeout = 2000
                            });

                            string popupText = await popupElement.InnerTextAsync();
                            Console.WriteLine($"✅ Pop-up pronađen: {popupText}");
                        }
                        catch (PlaywrightException)
                        {
                            Console.WriteLine("⚠️ Pop-up nije pronađen ili je prebrzo nestao.");
                        }

        ***********************/








        //Nalaženje poslednjeg serijskog broja obrasca polise koji je u sistemu (tipObrasca = 1 - AO, 4 - ZK)
        private static async Task<long> PoslednjiSerijskiStroga(int tipObrasca, string dodatniUslov)
        {
            try
            {
                //Nalaženje poslednjeg serijskog broja obrasca AO koji je u sistemu
                string qMaksimalniSerijski = $"SELECT MAX (SerijskiBroj) FROM [StrictEvidenceDB].[strictevidence].[tObrasci] " +
                                               $"WHERE [IdTipObrasca] = {tipObrasca} {dodatniUslov};";

                // Konekcija sa bazom StrictEvidenceDB - Razvoj
                long MaksimalniSerijskiRazvoj;
                long MaksimalniSerijskiTest;
                long MaksimalniSerijskiUAT;
                string connectionStringStroga;
                connectionStringStroga = $"Server = 10.5.41.99; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qMaksimalniSerijski, konekcija))
                    {
                        MaksimalniSerijskiRazvoj = (long)(command.ExecuteScalar() ?? 0); ;
                    }
                    konekcija.Close();
                }

                // Konekcija sa bazom StrictEvidenceDB - Test
                connectionStringStroga = $"Server = 49.13.25.19; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qMaksimalniSerijski, konekcija))
                    {
                        object MaksimalniSerijskiTest1 = command.ExecuteScalar();
                        MaksimalniSerijskiTest = MaksimalniSerijskiTest1 != DBNull.Value ? Convert.ToInt64(MaksimalniSerijskiTest1) : 0;
                    }
                    konekcija.Close();
                }

                // Konekcija sa bazom StrictEvidenceDB - UAT
                connectionStringStroga = $"Server = 10.41.5.5; Database = StrictEvidenceDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
                using (SqlConnection konekcija = new(connectionStringStroga))
                {
                    konekcija.Open();
                    using (SqlCommand command = new(qMaksimalniSerijski, konekcija))
                    {
                        MaksimalniSerijskiUAT = (long)(command.ExecuteScalar() ?? 0);
                    }
                    konekcija.Close();
                }

                Console.WriteLine($"Maksimalni serijski broj obrasca polise AO na Razvoju je: {MaksimalniSerijskiRazvoj}.");
                Console.WriteLine($"Maksimalni serijski broj obrasca polise AO na Testu je:   {MaksimalniSerijskiTest}.");
                Console.WriteLine($"Maksimalni serijski broj obrasca polise AO na UAT-u je:   {MaksimalniSerijskiUAT}.\n");

                long MaksimalniSerijski = Math.Max(MaksimalniSerijskiRazvoj, Math.Max(MaksimalniSerijskiUAT, MaksimalniSerijskiTest));
                //Console.WriteLine($"Maksimalni serijski broj obrasca polise AO na svim okruženjima je: {MaksimalniSerijski}.\n");
                LogovanjeTesta.LogMessage($"✅ Poslednji serijski broj obrasca polise AO na svim okruženjima je: {MaksimalniSerijski}.", false);
                return MaksimalniSerijski;

            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Greška prilikom nalaženja poslednjeg serijskog broja obrasca poliseAO.\n{ex.Message}");
                await LogovanjeTesta.LogException($"❌ Greška prilikom nalaženja poslednjeg serijskog broja obrasca [IdTipObrasca] = {tipObrasca}.", ex);
                throw;
            }


        }

        private static async Task<int> PoslednjiDokumentStroga()
        {
            try
            {
                int PoslednjiDokument = 0;
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
                        PoslednjiDokument = (int)command.ExecuteScalar();
                    }
                    konekcija.Close();
                }
                LogovanjeTesta.LogMessage($"✅ Poslednji broj dokumenta u strogoj evidenciji je: {PoslednjiDokument}.", false);
                //Console.WriteLine($"Poslednji broj dokumenta u strogoj evidenciji je: {PoslednjiDokument}.\n");
                return PoslednjiDokument;
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Greška prilikom nalaženja broja poslednjeg dokumenta u strogoj evidenciji.\n{ex.Message}");
                await LogovanjeTesta.LogException($"❌ Greška prilikom nalaženja broja poslednjeg dokumenta u strogoj evidenciji.", ex);
                throw;
            }

        }



        private static string IzracunajKontrolnuCifru(string SerijskiBrojObrasca)
        {
            char KonChar1 = SerijskiBrojObrasca[7];
            char KonChar2 = SerijskiBrojObrasca[6];
            char KonChar3 = SerijskiBrojObrasca[5];
            char KonChar4 = SerijskiBrojObrasca[4];
            char KonChar5 = SerijskiBrojObrasca[3];
            char KonChar6 = SerijskiBrojObrasca[2];
            char KonChar7 = SerijskiBrojObrasca[1];
            char KonChar8 = SerijskiBrojObrasca[0];

            int extractedValue1 = 2 * int.Parse(KonChar1.ToString());
            int extractedValue2 = int.Parse(KonChar2.ToString());
            int extractedValue3 = 2 * int.Parse(KonChar3.ToString());
            int extractedValue4 = int.Parse(KonChar4.ToString());
            int extractedValue5 = 2 * int.Parse(KonChar5.ToString());
            int extractedValue6 = int.Parse(KonChar6.ToString());
            int extractedValue7 = 2 * int.Parse(KonChar7.ToString());
            int extractedValue8 = int.Parse(KonChar8.ToString());

            int rezultat1 = extractedValue1.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat2 = extractedValue2.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat3 = extractedValue3.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat4 = extractedValue4.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat5 = extractedValue5.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat6 = extractedValue6.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat7 = extractedValue7.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();
            int rezultat8 = extractedValue8.ToString()
                                           .Select(c => 1 * (c - '0'))
                                           .Sum();

            int zbir = rezultat1 + rezultat2 + rezultat3 + rezultat4 + rezultat5 + rezultat6 + rezultat7 + rezultat8;
            int ostatak = zbir % 10;
            int kontrolna = 0;
            if (ostatak == 0)
            {
                kontrolna = 0;
            }
            else
            {
                kontrolna = 10 - ostatak;
            }
            /*
            return (extractedValue1.ToString() + " " +
                       extractedValue2.ToString() + " " +
                    extractedValue3.ToString() + " " +
                       extractedValue4.ToString() + " " +
                    extractedValue5.ToString() + " " +
                       extractedValue6.ToString() + " " +
                    extractedValue7.ToString() + " " +
                       extractedValue8.ToString() + " | " +
                       rezultat1.ToString() + " " +
                       rezultat2.ToString() + " " +
                       rezultat3.ToString() + " " +
                       rezultat4.ToString() + " " +
                       rezultat5.ToString() + " " +
                       rezultat6.ToString() + " " +
                       rezultat7.ToString() + " " +
                       rezultat8.ToString() + " | " +
                       $"Zbir je {zbir} | mod: {ostatak} | kontrolna: {kontrolna}"
                    );
                    */
            return kontrolna.ToString();
        }



    }

}