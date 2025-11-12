
namespace Razvoj
{

    public partial class Osiguranje
    {

        public static (string, string) IzaberiPodatkeZaZahtev()
        {
            // 1️⃣ Generišemo slučajan broj 0, 1 ili 2
            Random rnd = new Random();
            int broj = rnd.Next(0, 3);

            string naslovZahteva = string.Empty;
            string tekstZahteva = string.Empty;

            // 2️⃣ U zavisnosti od slučajnog broja, dodeljujemo A/B/C i odgovarajući P/Q/R
            switch (broj)
            {
                case 0:
                    naslovZahteva = "Promeni ime";
                    tekstZahteva = "Dragan";
                    break;
                case 1:
                    naslovZahteva = "Promeni prezime";
                    tekstZahteva = "Petrović";
                    break;
                case 2:
                    naslovZahteva = "Promeni adresu";
                    tekstZahteva = "Kojekude";
                    break;
            }

            // 3️⃣ Vraćamo obe vrednosti kao tuple
            return (naslovZahteva, tekstZahteva);
        }



        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public static void UploadFile(string filePath)
        {
            if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                throw new PlatformNotSupportedException("UI automation is supported only on Windows.");

            using var automation = new UIA3Automation();
            // 1️⃣ Sačekaj da se pojavi prozor "Open"
            var openWindow = Retry.WhileNull(
                () => automation.GetDesktop()
                    .FindFirstDescendant(cf => cf.ByName("Open")),
                TimeSpan.FromSeconds(5)
            ).Result?.AsWindow();

            if (openWindow == null)
            {
                throw new Exception("Open dijalog nije pronađen!");
            }

            openWindow.WaitUntilClickable();
            FlaUI.Core.AutomationElements.TextBox? fileNameBox = null;
            // 2️⃣ Pronađi polje "File name" (Edit kontrola)
            // strategija C: edit unutar comboBox (često File name je ComboBox sa edit delom)
            if (fileNameBox == null)
            {
                var combo = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.ComboBox));
                if (combo != null)
                {
                    var innerEdit = combo.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit));
                    if (innerEdit != null)
                        fileNameBox = innerEdit.AsTextBox();
                }
            }
            // Fokusiraj, očisti i unesi
            if (fileNameBox == null)
            {
                // Pokušaj direktno pronaći Edit kontrolu u prozoru ako prethodna strategija nije uspela
                var editDirect = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit));
                if (editDirect != null)
                    fileNameBox = editDirect.AsTextBox();
            }

            if (fileNameBox == null)
                throw new Exception("Polje za naziv fajla nije pronađeno u Open dijalogu.");

            fileNameBox.Focus();
            // 3️⃣ Očisti i unesi putanju do fajla
            fileNameBox.Text = filePath;



            // 4️⃣ Pronađi i klikni dugme "Open"
            var openButton = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByName("Open"))).AsButton();
            if (openButton == null)
                throw new Exception("Dugme Open nije pronađeno!");

            openButton.Invoke();
        }

        /// <summary>
        /// Pokušava da pronađe "Open" (ili lokalizovano "Otvori") dijalog i unese filePath u polje "File name",
        /// pa klikne dugme Open. Vraća true ako je uspešno, ili baca exception sa detaljima.
        /// </summary>
        public static void UploadFileRobust(string filePath, UIA3Automation automation, int timeoutSeconds = 10)
        {
            if (automation == null) throw new ArgumentNullException(nameof(automation));
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("filePath ne sme biti prazan");

            // 1) pronađi Window tipičnog klasnog imena za open dijalog (#32770) i moguće nazive prozora
            var openWindow = Retry.WhileNull(() =>
            {
                // desktop kontekst
                var desktop = automation.GetDesktop();

                // Pokušaj da pronađeš standardni Open dijalog (class #32770)
                var w = desktop.FindFirstDescendant(cf =>
                        cf.ByClassName("#32770")
                          .And(
                              // imena koja se često koriste; lokalizacija: "Open", "Otvori", "Choose File", "File Upload"...
                              cf.ByName("Open").Or(cf.ByName("Otvori"))
                                .Or(cf.ByName("Choose File"))
                                .Or(cf.ByName("File Upload"))
                                .Or(cf.ByName("Select file to upload"))
                          )
                    )?.AsWindow();

                // Ako nije pronađen po imenu, uzmi bilo koji #32770 koji sadrži Edit ili ComboBox
                if (w == null)
                {
                    w = desktop.FindAllChildren()
                               .Select(x => x.AsWindow())
                               .Where(x => x != null && x.ClassName == "#32770")
                               .FirstOrDefault(x => x.FindFirstDescendant(d => d.ByControlType(ControlType.Edit).Or(d.ByControlType(ControlType.ComboBox))) != null);
                }
                return w;
            }, TimeSpan.FromSeconds(timeoutSeconds)).Result;

            if (openWindow == null)
                throw new InvalidOperationException($"Open dialog nije pronađen (čekano ~{timeoutSeconds}s). Pokreni test i proveri lokalizaciju/naslov dijaloga.");

            openWindow.WaitUntilClickable();

            // 2) pokušaj nekoliko strategija da pronađeš "file name" edit kontrolu
            FlaUI.Core.AutomationElements.TextBox? fileNameBox = null;

            // strategija A: po poznatom AutomationId (često "1148" u standardnim Windows dialog)
            var byId = openWindow.FindFirstDescendant(cf => cf.ByAutomationId("1148").And(cf.ByControlType(ControlType.Edit)));
            if (byId != null)
                fileNameBox = byId.AsTextBox();

            // strategija B: pronađi EDIT direktno (prvi Edit u hijerarhiji)
            if (fileNameBox == null)
            {
                var edit = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit));
                if (edit != null)
                    fileNameBox = edit.AsTextBox();
            }

            // strategija C: edit unutar comboBox (često File name je ComboBox sa edit delom)
            if (fileNameBox == null)
            {
                var combo = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.ComboBox));
                if (combo != null)
                {
                    var innerEdit = combo.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit));
                    if (innerEdit != null)
                        fileNameBox = innerEdit.AsTextBox();
                }
            }

            // strategija D: ako i dalje null, probaj da nabrojis sve Edit kontrole i izaberes onaj koji ima fokusibilan ili vidljiv status
            if (fileNameBox == null)
            {
                var edits = openWindow.FindAllDescendants(cf => cf.ByControlType(ControlType.Edit)).Select(e => e.AsTextBox()).ToArray();
                if (edits.Length == 1)
                {
                    fileNameBox = edits[0];
                }
                else if (edits.Length > 1)
                {
                    // pokuša da nađe edit koji ima hint/name "File name" ili lokalizovano ime
                    fileNameBox = edits.FirstOrDefault(e =>
                        !string.IsNullOrEmpty(e.Properties.Name.Value) &&
                        (e.Properties.Name.Value.IndexOf("File name", StringComparison.OrdinalIgnoreCase) >= 0
                         || e.Properties.Name.Value.IndexOf("Naziv fajla", StringComparison.OrdinalIgnoreCase) >= 0
                         || e.Properties.Name.Value.IndexOf("Ime fajla", StringComparison.OrdinalIgnoreCase) >= 0));
                    // fallback: prva koja je Enabled i Visible (koristi IsOffscreen iz FlaUI; vidljivo ako nije offscreen)
                    if (fileNameBox == null)
                        fileNameBox = edits.FirstOrDefault(e => e.IsEnabled && !e.IsOffscreen);
                }
            }

            if (fileNameBox == null)
            {
                // DODATNA POMOĆ: izlistaj sve kontrole za debugging
                var all = openWindow.FindAllDescendants();
                var summary = string.Join(Environment.NewLine, all.Select(a => $"{a.ControlType}:{a.AutomationId}:{a.Name}"));
                throw new InvalidOperationException("Ne mogu da pronađem polje za unos imena fajla u Open dijalogu. " +
                                                    "Moguće rešenje: proveri lokalizaciju ili hijerarhiju. " +
                                                    $"Evo svih descendants:\n{summary}");
            }

            // 3) Unesi putanju / naziv fajla
            // Fokusiraj, očisti i unesi
            fileNameBox.Focus();
            // Neki editovi zahtevaju SelectAll pre upisa
            try
            {
                fileNameBox.Text = filePath;
            }
            catch
            {
                // fallback: koristimo keyboard
                fileNameBox.Click();
                FlaUI.Core.Input.Keyboard.Type(filePath);
            }

            Thread.Sleep(200); // mali delay da UI obradi tekst

            // 4) Pronađi Open dugme (lokalizacija -> "Open", "Otvori", "Choose", "Open File")
            var openButton = openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByName("Open")))?.AsButton()
                         ?? openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByName("Otvori")))?.AsButton()
                         ?? openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByName("Open file")))?.AsButton()
                         ?? openWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Button).And(cf.ByAutomationId("1")))?.AsButton(); // ponekad "1"

            if (openButton == null)
                throw new InvalidOperationException("Dugme 'Open' nije pronađeno u dijalogu. Proveri jezik / automation id.");

            openButton.Invoke();
        }

    }

}