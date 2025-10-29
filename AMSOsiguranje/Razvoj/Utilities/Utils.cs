
namespace Razvoj
{
    public partial class Osiguranje
    {


        /// <summary>
        /// Promenljiva koja definiše koji agent se loguje prilikom ručnog pokretanja testa.
        /// <para>Vrednost se ručno unose pre početka testiranja.</para>
        /// </summary> 
        /// <value>"Bogdan", "Mario"</value>
        public static string RucnaUloga { get; set; } = "Bogdan"; // "Ne", "Bogdan", "Mario"

        /// <summary>
        /// Promenljiva koja definiše koji pregledac se koristi za testiranje. 
        /// <para>Vrednost se ručno unosi pre početka testiranja.</para>
        /// <para>Podrazumevano je "Chromium".</para>
        /// </summary>
        /// <value>"Chromium", "Firefox" ili "Webkit"</value>
        public static string Pregledac { get; set; } = "Chromium"; // "Chromium", "Firefox", "Webkit"

        //public static string BOlozinka = KorisnikLoader5.Korisnik1 != null ? KorisnikLoader5.Korisnik1.Lozinka1 : string.Empty;

        //public static string AkorisnickoIme = KorisnikLoader5.Korisnik2 != null ? KorisnikLoader5.Korisnik2.KorisnickoIme : string.Empty;
        //public static string Alozinka = KorisnikLoader5.Korisnik2 != null ? KorisnikLoader5.Korisnik2.Lozinka1 : string.Empty;
        //public static string Asaradnik = KorisnikLoader5.Korisnik2 != null ?
        //                   $"{KorisnikLoader5.Korisnik2.SaradnickaSifra1} - {KorisnikLoader5.Korisnik2?.Ime} {KorisnikLoader5.Korisnik2?.Prezime}" : string.Empty;

        #region Šta se i ko testira

        /// <summary>
        /// Promenjiva koja definiše naziv prostora (namespace) u kojem se nalaze testovi.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para></summary>
        /// <value>"Razvoj", "Proba2", "UAT", "Produkcija"</value>
        public static string? NazivNamespace { get; set; }

        /// <summary>
        /// Promenjive Prostor i Okruzenje su sustinski iste kao promenjiva NazivNamespace.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para></summary>
        /// <value>"Razvoj", "Proba2", "UAT", "Produkcija"</value>
        public static string? Prostor { get; set; }

        /// <summary>
        /// Promenjive Prostor i Okruzenje su sustinski iste kao promenjiva NazivNamespace.
        /// <para>Koristi se za organizaciju i identifikaciju testova unutar projekta.</para>
        /// <para>Može imati vrednosti "Razvoj"/"Proba2"/"UAT"/"Produkcija".</para>
        /// </summary>
        public static string Okruzenje { get; set; } = "";

        /// <summary>
        /// Kako se zove test koji je pokrenut, na osnovu njega definiše se uloga Agent ili BackOffice
        /// </summary>
        /// <value></value>
        public static string NazivTekucegTesta { get; set; } = TestContext.CurrentContext.Test.Name;

        /// <summary>
        /// Naziv kompjutera na kom se izvršava testiranje.
        /// </summary>
        /// <value></value>
        public static string DeviceName { get; set; } = Environment.MachineName;
        /// <summary>
        /// Zadaje se primarna uloga koju korisnik ima u testiranju.
        /// <para>Primarna uloga zavisi od NazivaTesta.</para>
        /// <para>Ako je uloga Agent, U zavisnosti od NazivTekucegTesta, Okruzenje i NacinPokretanjaTesta</para>
        /// <para>definisaće se da li je Agent1 ili Agent2.</para>
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
        /// Definiše se početna stranica za testiranje.
        /// <para>Ova stranica zavisi od klase (OsiguranjeVozila ili WebShop) i okruženja (Razvoj, Proba2, UAT, Produkcija).</para>
        /// </summary>
        /// <value></value>
        public static string PocetnaStrana { get; set; } = "";
        //public static string Tip { get; set; } = "Autoosiguranje"; // "Putno", 
        #endregion Šta se i ko testira

        #region Putanje i folderi

        /// <summary>
        /// Dodaje se na URL osnovne stranice.
        /// Ova putanja se koristi za navigaciju na specifične stranice
        /// </summary>
        /// <value></value>
        public static string DodatakNaURL { get; set; } = "";
        public static string RadniFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        //public static string RadniFolder2 = AppContext.BaseDirectory; 

        public static string ProjektFolder { get; set; } = Directory.GetParent(RadniFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;  // Ide 3 nivoa gore
                                                                                                                                        //public static string projektDir = Path.GetFullPath(Path.Combine(RadniFolder, @"..\..\.."));
                                                                                                                                        //public static string logFolder = Path.Combine(ProjektFolder, "Logovi");
                                                                                                                                        //public static string logFolder2 = Path.GetFullPath(Path.Combine(ProjektFolder, "Logovi"));
        public static string OsnovniFolder { get; set; } = Directory.GetParent(ProjektFolder)!.FullName;  // Ide 1 nivo gore
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



        /// <summary>
        /// Pauzira samo ako test nije pokrenut automatski.
        /// </summary>
        protected static async Task Pauziraj(IPage _page)
        {
            if (Environment.GetEnvironmentVariable("NACIN_POKRETANJA") != "automatski")
            {
                await _page.PauseAsync();
            }
        }

        /// <summary>
        /// Definiše se početna stranica za testiranje. Ona zavisi od klase i okruženja
        /// </summary>
        /// <param name="nazivKlase">OsiguranjeVozila ili WebShop.</param>
        /// <param name="okruzenje">Razvoj, Proba2, UAT ili Produkcija.</param>  
        /// <returns>Vraća string PocetnaStrana, to je URL.</returns>    
        /// <exception cref="Exception">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks></remarks>
        private static async Task<string> DefinisiPocetnuStranu(string nazivKlase, string? okruzenje)
        {
            var currentMethodName = MethodBase.GetCurrentMethod() != null ? MethodBase.GetCurrentMethod()!.Name : "UnknownMethod";
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
                    PocetnaStrana = "https://webshop-test.ams.co.rs";
                }
                else if (nazivKlase == "WebShop" && okruzenje == "Produkcija")
                {
                    PocetnaStrana = "https://webshop.ams.co.rs";
                }
                else if (nazivKlase != "WebShop" && okruzenje == "Razvoj")
                {
                    PocetnaStrana = "https://razvojamso-master.eonsystem.rs";
                }
                else if (nazivKlase != "WebShop" && okruzenje == "Proba2")
                {
                    PocetnaStrana = "https://proba2amsomaster.eonsystem.rs";
                }
                else if (nazivKlase != "WebShop" && okruzenje == "UAT")
                {
                    PocetnaStrana = "https://master-test.ams.co.rs";
                }
                else if (nazivKlase != "WebShop" && okruzenje == "Produkcija")
                {
                    PocetnaStrana = "https://eos.ams.co.rs/Osiguranje-vozila";
                }

                if (!string.IsNullOrEmpty(PocetnaStrana))
                { return PocetnaStrana; }
                else
                {
                    await LogovanjeTesta.LogException(currentMethodName, new Exception("PocetnaStrana nije određena."));
                    throw new Exception($"PocetnaStrana nije određena u proceduri {currentMethodName}.");
                }
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"Greška prilikom definisanja početne strane: {ex.Message}");
                await LogovanjeTesta.LogException(currentMethodName, ex);
                throw;
            }
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
        protected static async Task ProveriURL(IPage _page, string osnovnaStrana, string dodatak)
        {
            try
            {
                await _page.WaitForURLAsync(new Regex($"{osnovnaStrana + dodatak}", RegexOptions.IgnoreCase), new() { Timeout = 120000 });
                //System.Windows.MessageBox.Show($"Učitana: {osnovnaStrana + dodatak}...", "Proveri URL", MessageBoxButton.OK, MessageBoxImage.Information);
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čekaj dok se DOM ne učita
                await _page.WaitForLoadStateAsync(LoadState.Load); // čekaj da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čekaj dok mrežni zahtevi ne prestanu

                string currentUrl = _page.Url;
                //System.Windows.MessageBox.Show($"Učitana stranica: {currentUrl}", "Proveri URL", MessageBoxButton.OK, MessageBoxImage.Information);
                if (dodatak == "/Login")
                {
                    currentUrl = currentUrl.Split('?')[0];
                }
                string ocekivanaStrana = (osnovnaStrana + dodatak).ToLower();
                //System.Windows.MessageBox.Show($"Učitana stranica: {currentUrl}", "Proveri URL", MessageBoxButton.OK, MessageBoxImage.Information);
                Assert.That(currentUrl.ToLower, Is.EqualTo(ocekivanaStrana));
                LogovanjeTesta.LogMessage($"✅ Učitana je strana: {osnovnaStrana + dodatak}.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogError($"❌ 1Greška prilikom učitavanja stranice: - {osnovnaStrana + dodatak}\n{ex.Message}");
                await LogovanjeTesta.LogException($"❌ Greška prilikom učitavanja stranice: - {osnovnaStrana + dodatak}", ex);
                throw;
            }
        }





        private async Task ApplyZoomWithObserverAsync(IPage page)
        {
            string js = @"
        (function() {
            const applyZoom = () => {
                if (document.body) {
                    document.body.style.zoom = '75%';
                }
            };

            const observer = new MutationObserver(() => {
                applyZoom();
            });

            applyZoom();

            observer.observe(document.documentElement || document.body, {
                childList: true,
                subtree: true
            });
        })();
    ";

            await page.EvaluateAsync(js);
        }

        private static async Task ForceZoomAsync(IPage page)
        {
            await page.SetViewportSizeAsync(1280, 720);
            //await page.SetViewportSizeAsync(1920, 1080);
            //await page.SetViewportSizeAsync(1919, 1079);
            await page.EvaluateAsync(@"() => {
        document.documentElement.style.transform = 'scale(0.75)';
        document.documentElement.style.transformOrigin = '0 0';
        document.documentElement.style.width = '133.33%';
    }");
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


}



