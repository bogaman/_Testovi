
namespace UAT
{
    public partial class Osiguranje
    {
        /// <summary>
        /// Promenljiva koja definiše koji agent se loguje prilikom ručnog pokretanja testa.
        /// <para>Vrednost se ručno unose pre početka testiranja.</para>
        /// <para>Ako je prazan string, koriste se vrednosti koje zavise od NacinPokretanjaTesta.</para>
        /// </summary> 
        /// <value>"Ne", "Bogdan", "Mario"</value>
        public static string RucnaUloga { get; set; } = "Ne"; // "Ne", "Bogdan", "Mario"
        /// <summary>
        /// Promenljiva koja definiše koji pregledac se koristi za testiranje. 
        /// <para>Vrednost se ručno unosi pre početka testiranja.</para>
        /// <para>Podrazumevano je "Chromium".</para>
        /// </summary>
        /// <value>"Chromium", "Firefox" ili "Webkit"</value>
        public static string Pregledac { get; set; } = "Chromium"; // "Chromium", "Firefox", "Webkit"

        #region Šta se i ko testira
        /// <summary>
        /// Promenjiva koja definiše naziv prostora (namespace) u kojem se nalaze testovi.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para></summary>
        /// <value>"Razvoj", "Proba2", "UAT", "Produkcija"</value>
        public static string NazivNamespace { get; set; } = "";
        /// <summary>
        /// Promenjive Prostor i Okruzenje su sustinski iste kao promenjiva NazivNamespace.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para></summary>
        /// <value>"Razvoj", "Proba2", "UAT", "Produkcija"</value>
        public static string Prostor { get; set; } = "";
        /// <summary>
        /// Promenjive Prostor i Okruzenje su sustinski iste kao promenjiva NazivNamespace.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para>
        /// <para>Može imati vrednosti "Razvoj"/"Proba2"/"UAT"/"Produkcija".</para>
        /// </summary>
        public static string Okruzenje { get; set; } = "UAT";
        /// <summary>
        /// Kako se zove test koji je pokrenut, na osnovu njega definiše se uloga Agent ili BackOffice
        /// </summary>
        /// <value></value>
        public static string NazivTekucegTesta { get; set; } = TestContext.CurrentContext.Test.Name;
        /// <summary>
        /// Zadaje se primarna uloga koju korisnik ima u testiranju.
        /// <para>Primarna uloga zavisi od NazivaTesta.</para>
        /// <para>Ako je uloga Agent, U zavisnosti od NazivTekucegTesta, Okruzenje i NacinPokretanjaTesta</para>
        ///  <para>definisaće se da li je Agent1 ili Agent2.</para>
        /// </summary>
        /// <value>"Agent", "BackOffice"</value>
        public static string OsnovnaUloga { get; set; } = ""; // "Agent", "BackOffice"
        /// <summary>
        /// Koristi se za određivanje načina pokretanja testa, ručno/automatski.
        /// <para>ručno - test se pokreće direktno iz IDE-a ili komandne linije.</para>
        /// <para>automatski - test se pokreće pomoću batch fajla ili CI/CD alata.</para>
        /// </summary>
        /// <remarks>Koristiti se za prilagođavanje ponašanja testa u zavisnosti od načina pokretanja.</remarks>
        /// <value>"ručno", "automatski"</value>
        public static string NacinPokretanjaTesta { get; set; } = Environment.GetEnvironmentVariable("NACIN_POKRETANJA") ?? "ručno";
        /// <summary>
        /// Definiše se pregledac koji će se koristiti za testiranje.
        /// </summary>
        /// <value>"Chromium", "Firefox" ili "Webkit"</value>

        /// <summary>
        /// Definiše se početna stranica za testiranje.
        /// <para>Ova stranica zavisi od klase (OsiguranjeVozila ili WebShop) i okruženja (Razvoj, Proba2, UAT, Produkcija).</para>
        /// </summary>
        /// <value></value>
        public static string PocetnaStrana { get; set; } = "";
        //public static string Tip { get; set; } = "Autoosiguranje"; // "Putno", 
        #endregion Šta se i ko testira

        #region Putanje i folderi

        public static string RadniFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        //public static string RadniFolder2 = AppContext.BaseDirectory; 

        public static string ProjektFolder { get; set; } = Directory.GetParent(RadniFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;  // Ide 3 nivoa gore
                                                                                                                                        //public static string projektDir = Path.GetFullPath(Path.Combine(RadniFolder, @"..\..\.."));
                                                                                                                                        //public static string logFolder = Path.Combine(ProjektFolder, "Logovi");
                                                                                                                                        //public static string logFolder2 = Path.GetFullPath(Path.Combine(ProjektFolder, "Logovi"));

        //public static string logFajlOpsti = Path.Combine(logFolder, "logOpsti.txt");
        //public static string logFajlSumarni = logFolder + "\\logSumarni.txt";

        // Putanja do Access baze
        // Početna putanja do izvršne datoteke


        //public static string putanjaDoBaze = Path.Combine(ProjektFolder, @"Podaci\IzlazniPodaci\dbLogovi.accdb");
        //public static string connectionStringLogovi = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={putanjaDoBaze};";

        #endregion Putanje i folderi

        #region Osnovne promenjive

        //public static string BaseUrl { get; set; } = Environment.GetEnvironmentVariable("BASE_URL") ?? "ručno";





        public static string KorisnikMejl { get; set; } = "";
        public static string KorisnikIme { get; set; } = "";
        public static string KorisnikPassword { get; set; } = "";
        //public static string PocetnaStranaWS { get; set; } = "";
        public static long MinSerijskiAO { get; set; } = 34680001;
        public static long MaxSerijskiAO { get; set; } = 34690000;
        #endregion Osnovne promenjive

        #region Početne stranice WebShop-a
        //public static string pocetnaStranaWS { get; set; } = "https://razvojamso-webshop.eonsystem.rs/Index";
        //public static string pocetnaStranaWS { get; set; } = "https://proba2amsowebshop.eonsystem.rs/";
        //public static string pocetnaStranaWS { get; set; } = "https://webshop-test.ams.co.rs/";
        #endregion Početne stranice WebShop-a
        public static string Server { get; set; } = "";

        //public static string Database { get; set; }= "StrictEvidenceDB";
        public static string UserID { get; set; } = "sa";
        public static string PasswordDB { get; set; } = "Pristup#SQL0";
        public static string TrustServerCertificate { get; set; } = "True";

        public static int StanjeKontrole { get; set; } = 0;
        public static DateTime CurrentDate = DateTime.Now.Date; // Današnji dan
        public static DateTime NextDate = CurrentDate.AddDays(1); // Sledeći dan

        // Folder gde se nalaze fajlovi u koje upisujem i/ili ih čitam
        //public static string FileFolder { get; set; } = @"C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/";
        public static string FileFolder { get; set; } = @"C:/_Projekti/AutoMotoSavezSrbije/Podaci";
        // Putanja do CSV fajla
        //public static string FilePath { get; set; } = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/PoliseAutoodgovornost-UAT.csv";
        public static string FilePath { get; set; } = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/PoliseAutoodgovornost-test.csv";

        public static string CurrentUrl { get; set; } = "";
        public static FlaUI.Core.Application? _application;
        public static FlaUI.Core.Application? _application2;
        public static AutomationBase? _automation;
        public static AutomationBase? _automation2;
        public const string AppName = "proxsign"; // Bez ekstenzije .exe
        public const string AppName2 = "CredentialUIBroker"; // Bez ekstenzije .exe
        public const string AppPath = @"C:\Program Files (x86)\SETCCE\proXSign\bin\proxsign.exe";
        public const string AppPath2 = @"C:\Windows\System32\CredentialUIBroker.exe";
        public static string PartialTitle = @"SETCCE";




        /// <summary>
        /// Definiše se početna stranica za testiranje. Ona zavisi od Klase i okruženja
        /// </summary>
        /// <param name="nazivKlase">OsiguranjeVozila ili WebShop.</param>
        /// <param name="okruzenje">Razvoj, Proba2, UAT ili Produkcija.</param>  
        /// <returns>Vraća string PocetnaStrana, to je URL.</returns>    
        /// <exception cref="Exception">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks></remarks>
        private static string DefinisiPocetnuStranu(string nazivKlase, string okruzenje)
        {
            try
            {
                if (nazivKlase == "WebShop" && okruzenje == "Razvoj")
                {
                    PocetnaStrana = "https://razvojamso-webshop.eonsystem.rs";
                }
                else if (nazivKlase == "WebShop" && okruzenje == "Proba2")
                {
                    PocetnaStrana = "https://proba2amsowebshop.eonsystem.rs";
                }
                else if (nazivKlase == "WebShop" && okruzenje == "UAT")
                {
                    PocetnaStrana = "https://webshop-test.ams.co.rs/";
                }
                else if (nazivKlase == "WebShop" && okruzenje == "Produkcija")
                {
                    PocetnaStrana = "https://webshop.ams.co.rs";
                }
                else if (nazivKlase == "OsiguranjeVozila" && okruzenje == "Razvoj")
                {
                    PocetnaStrana = "https://razvojamso-master.eonsystem.rs";
                }
                else if (nazivKlase == "OsiguranjeVozila" && okruzenje == "Proba2")
                {
                    PocetnaStrana = "https://proba2amsomaster.eonsystem.rs";
                }
                else if (nazivKlase == "OsiguranjeVozila" && okruzenje == "UAT")
                {
                    PocetnaStrana = "https://master-test.ams.co.rs";
                }
                else if (nazivKlase == "OsiguranjeVozila" && okruzenje == "Produkcija")
                {
                    PocetnaStrana = "https://eos.ams.co.rs/Osiguranje-vozila";
                }
                else
                {
                    PocetnaStrana = "";
                }
                //return PocetnaStrana;
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"Greška prilikom definisanja početne strane: {ex.Message}");
                PocetnaStrana = "";

            }
            return PocetnaStrana;
        }

        ///<summary>
        ///Čeka se na učitavanje određenog URL-a i proverava link
        ///</summary>
        ///<param name="_page">Instanca stranice na kojoj se vrši provera URL-a.</param>
        ///<param name="osnovnaStrana">Osnovna stranica koja se koristi za formiranje URL-a.</param>
        ///<param name="dodatak">Dodatak koji se dodaje na osnovnu stranu, npr. "/Login".</param>
        ///<returns>Ne vraća vrednost</returns>
        ///<exception cref="Exception">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>
        ///<remarks>Ova metoda čeka da se stranica učita i proverava da li je trenutni URL jednak očekivanom URL-u.</remarks>
        public async Task ProveriURL(IPage _page, string osnovnaStrana, string dodatak)
        {
            try
            {
                await _page.WaitForURLAsync(new Regex($"{osnovnaStrana + dodatak}", RegexOptions.IgnoreCase));
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čekaj dok se DOM ne učita
                await _page.WaitForLoadStateAsync(LoadState.Load); // čekaj da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čekaj dok mrežni zahtevi ne prestanu

                string currentUrl = _page.Url;
                if (dodatak == "/Login")
                {
                    currentUrl = currentUrl.Split('?')[0];
                }
                string ocekivanaStrana = (osnovnaStrana + dodatak).ToLower();
                Assert.That(currentUrl.ToLower, Is.EqualTo(ocekivanaStrana));
                LogovanjeTesta.LogMessage($"✅ Učitana je strana: {osnovnaStrana + dodatak}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ Greška prilikom učitavanja stranice: - {osnovnaStrana + dodatak}\n{ex.Message}");
                throw;
            }
        }

    }
    public static class Funkcije
    {

        public static bool IsProcessRunning(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string? lpszClass, string? lpszWindow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static bool IsWindowOpen(string windowTitle)
        {
            //IntPtr windowPtr = FindWindow(null, windowTitle);
            IntPtr windowPtr = FindWindow(null, windowTitle);
            return windowPtr != IntPtr.Zero;
        }

        public static bool IsWindowOpenWithPartialTitle(string partialTitle)
        {
            IntPtr hwnd = IntPtr.Zero;
            while ((hwnd = FindWindowEx(IntPtr.Zero, hwnd, null, null)) != IntPtr.Zero)
            {
                StringBuilder windowText = new StringBuilder(256);
                GetWindowText(hwnd, windowText, 256);
                if (windowText.ToString().Contains(partialTitle))
                {
                    return true;
                }
            }
            return false;
        }





        /*******************************************
        public static AutomationElement FindWindowWithPartialTitle(string partialTitle1)
        {
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window);
            var rootElement = AutomationElement.RootElement;
            foreach (AutomationElement window in rootElement.FindAll(TreeScope.Children, condition))
            {
                if (window.Current.Name.Contains(partialTitle1))
                {
                    return window;
                }
            }
            return null;
        }
        *************************************************/

        /***************************************************
                public static T GetValueOrDefault<T>(this SqlDataReader reader, string columnName, T defaultValue)
                {
                    var ordinal = reader.GetOrdinal(columnName);
                    if (reader.IsDBNull(ordinal))
                    {
                        return defaultValue;
                    }

                    object value = reader.GetValue(ordinal);

                    // Ako čitaš string i on je prazan, vrati default
                    if (typeof(T) == typeof(string))
                    {
                        string strValue = value as string;
                        if (string.IsNullOrWhiteSpace(strValue))
                        {
                            return defaultValue;
                        }
                    }

                    return (T)value;
                }
        ***********************************************/
    }

    /// <summary>
    /// Klasa koja predstavlja korisnika sa svim potrebnim informacijama.
    /// </summary>
    public class Korisnik
    {
        public string Uloga { get; set; } = "";
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string Email { get; set; } = "";
        public string Email1 { get; set; } = "";
        public string KorisnickoIme { get; set; } = "";
        public string Lozinka1 { get; set; } = "";
        public string Lozinka2 { get; set; } = "";
        public string Sertifikat { get; set; } = "";
        public string Pin { get; set; } = "";
        public string IdLica { get; set; } = "";
        public string SaradnickaSifra1 { get; set; } = "";
        public string SaradnickaSifra2 { get; set; } = "";

    }

    /// <summary>
    /// čita JSON fajl sa korisnicima i vraća odgovarajućeg korisnika na osnovu uslova.
    /// </summary>
    public static class KorisnikLoader
    {
        public static Korisnik? Korisnik1 { get; private set; }
        public static Korisnik? Korisnik2 { get; private set; }
        public static Korisnik? Korisnik3 { get; private set; }
        public static List<Korisnik> UcitajKorisnike()
        {
            string pathDoJsonFajla = Path.Combine(Osiguranje.ProjektFolder, "korisnici.json");
            if (!File.Exists(pathDoJsonFajla))
            {
                Console.WriteLine($"Fajl nije pronađen: {pathDoJsonFajla}");
                return [];
            }

            string json = File.ReadAllText(pathDoJsonFajla);

            try
            {
                var korisnici = JsonSerializer.Deserialize<List<Korisnik>>(json);

                Korisnik1 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "BackOffice");
                Korisnik2 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "Agent1");
                Korisnik3 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "Agent2");
                // Primer upotrebe:
                Console.WriteLine($"+Korisnik1: {Korisnik1?.Ime} {Korisnik1?.Prezime}");
                Console.WriteLine($"+Korisnik2: {Korisnik2?.Ime} {Korisnik2?.Prezime}");
                Console.WriteLine($"+Korisnik3: {Korisnik3?.Ime} {Korisnik3?.Prezime}");
                return korisnici ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom parsiranja JSON-a: {ex.Message}");
                return [];
            }
        }
    }
    /*
                public static Korisnik VratiKorisnika(string nacinPokretanja, string nazivTesta)
                {
                    if (nacinPokretanja == "rucno")
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "Bogdan");
                    if (nazivTesta.Contains("GrupaA"))
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "korisnik2");
                    if (nazivTesta.Contains("GrupaB"))
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "korisnik3");

                    return null;
                }

                public class Root
                {
                    public List<Korisnik>? Korisnici { get; set; }
                }
    */
    // Ova metoda može biti u nekoj servisnoj klasi

    /*
            public static void UcitajKorisnike()
            {
                //string path = Path.Combine(ProjektFolder, "korisnici.json");
                //string path = "C:\\_Testovi\\AMSOsiguranje\\Razvoj\\korisnici.json";
                //string json = File.ReadAllText(path);
                var korisnici = KorisnikLoader.UcitajKorisnike();


                // Filtriranje po korisničkom imenu
                var korisnik1 = korisnici.FirstOrDefault(k => k.Uloga == "BackOffice");
                var korisnik2 = korisnici.FirstOrDefault(k => k.Uloga == "Agent1");
                var korisnik3 = korisnici.FirstOrDefault(k => k.Uloga == "Agent2");

                // Primer upotrebe:
                Console.WriteLine($"Korisnik1: {korisnik1?.Ime} {korisnik1?.Prezime}");
                Console.WriteLine($"Korisnik2: {korisnik2?.Ime} {korisnik2?.Prezime}");
                Console.WriteLine($"Korisnik3: {korisnik3?.Ime} {korisnik3?.Prezime}");
            }
            */
}



