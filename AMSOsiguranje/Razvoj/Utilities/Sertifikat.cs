using NUnit.Framework;

namespace Razvoj
{

    public partial class Osiguranje

    {
        public static FlaUI.Core.Application? _application;

        public static FlaUI.Core.Application? _application1;
        public static FlaUI.Core.Application? _application2;
        public static AutomationBase? _automation;
        public static AutomationBase? _automation1;
        public static AutomationBase? _automation2;

        public static readonly string AppPath = @"C:\Program Files (x86)\SETCCE\proXSign\bin\proxsign.exe";
        public static readonly string AppPath1 = @"C:\Program Files (x86)\SETCCE\proXSign\bin\proxsign.exe";
        public static readonly string AppPath2 = @"C:\Windows\System32\CredentialUIBroker.exe";
        public static readonly string AppName = "proxsign"; // Bez ekstenzije .exe
        public static readonly string AppName1 = "proxsign"; // Bez ekstenzije .exe
        public static readonly string AppName2 = "CredentialUIBroker"; // Bez ekstenzije .exe
        public static readonly string PartialTitle1 = @"SETCCE";

        protected static async Task Sertifikat(IPage _page, string SertifikatName)
        {


            #region Sertifikat
            try
            {

                // 1. Provera da li je aplikacija već radi
                var pokrenutiProcesi = Process.GetProcessesByName(AppName1);
                if (pokrenutiProcesi.Length != 0)
                {
                    System.Windows.MessageBox.Show($"Aplikacija je već pokrenuta. Zatvaram je...", "Informacija");
                    foreach (var proc in pokrenutiProcesi)
                    {
                        proc.Kill();
                        proc.WaitForExit();
                    }
                    Thread.Sleep(1000);
                }
                // 3. Ponovo pokretanje aplikacije
                //Console.WriteLine("Pokrećem aplikaciju...");
                //System.Windows.MessageBox.Show($"Pokrećem aplikaciju...", "Informacija");


                if (!System.OperatingSystem.IsWindows())
                {
                    throw new PlatformNotSupportedException("FlaUI Application.Launch is supported on Windows only.");
                }

                FlaUI.Core.Application app;
                if (System.OperatingSystem.IsWindowsVersionAtLeast(7))
                {
                    app = FlaUI.Core.Application.Launch(AppPath1);
                }
                else
                {
                    throw new PlatformNotSupportedException("FlaUI Application.Launch is supported on Windows 7.0 and later.");
                }


                Thread.Sleep(1000); // kratko čekanje da se GUI pojavi

                using (var automation = new UIA3Automation())
                {
                    var mainWindow1 = app.GetMainWindow(automation);
                    //Console.WriteLine($"Glavni prozor: {mainWindow1.Title}");
                    //System.Windows.MessageBox.Show($"Glavni prozor: {mainWindow1.Title}", "Informacija");

                    // 3. Čekanje da se UI stabilizuje
                    Thread.Sleep(2000);

                    if (mainWindow1 == null)
                    {
                        //Console.WriteLine("Glavni prozor nije pronađen.");
                        System.Windows.MessageBox.Show($"Glavni prozor nije pronađen.", "Informacija");
                        return;
                    }

                    //Console.WriteLine($"Glavni prozor: {mainWindow1.Title}");
                    System.Windows.MessageBox.Show($"Glavni prozor: {mainWindow1.Title}", "Informacija");





                    // 4. Zatvaranje glavnog prozora klikom na X dugme
                    //var closeButton = mainWindow1.FindFirstDescendant(cf => cf.ByAutomationId("Close"))?.AsButton()
                    //                  ?? mainWindow1.FindFirstDescendant(cf => cf.ByText("Close"))?.AsButton();

                    // 4️⃣ Pokušaj zatvaranja klikom na X
                    var closeButton1 = mainWindow1.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)
                                                                         .And(cf.ByName("Close")));

                    // 4. Zatvaranje glavnog prozora klikom na X dugme
                    var closeButton = mainWindow1.FindFirstDescendant(cf => cf.ByAutomationId("Close"))?.AsButton()
                                      ?? mainWindow1.FindFirstDescendant(cf => cf.ByText("Close"))?.AsButton();
                    //if (closeButton != null)
                    if (closeButton != null)
                    {
                        //Console.WriteLine("Zatvaram prozor klikom na X...");
                        //System.Windows.MessageBox.Show($"Zatvaram prozor klikom na X...", "Informacija");
                        //closeButton.Click();
                        closeButton.AsButton()?.Invoke();
                        //Console.WriteLine("Zatvoren prozor klikom na X.");
                        System.Windows.MessageBox.Show($"Zatvoren prozor klikom na X.", "Informacija");

                    }
                    else
                    {
                        //Console.WriteLine("X dugme nije pronađeno, pokušavam standardno zatvaranje...");
                        //System.Windows.MessageBox.Show($"X dugme nije pronađeno, pokušavam standardno zatvaranje...", "Informacija");
                        mainWindow1.Close();

                        // Ako X nije pronađen, koristi Alt+F4
                        mainWindow1.Focus();
                        FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.LMENU);
                        FlaUI.Core.Input.Keyboard.Press(VirtualKeyShort.F4);
                        FlaUI.Core.Input.Keyboard.Release(VirtualKeyShort.LMENU);
                        FlaUI.Core.Input.Keyboard.Release(VirtualKeyShort.F4);
                        //Console.WriteLine("Zatvoren prozor pomoću Alt+F4.");
                        System.Windows.MessageBox.Show($"Zatvoren prozor pomoću Alt+F4.", "Informacija");
                    }

                    // 5️⃣ Provera da li je aplikacija i dalje aktivna (u tray-u)
                    Thread.Sleep(1000);
                    var procesiPosleZatvaranja = Process.GetProcessesByName(AppName1);
                    if (procesiPosleZatvaranja.Any())
                    {
                        //Console.WriteLine($"Aplikacija '{AppName}' je i dalje aktivna u pozadini (tray). OK ✅");
                        System.Windows.MessageBox.Show($"Aplikacija '{AppName1}' je i dalje aktivna u pozadini (tray). OK ✅", "Informacija");
                    }
                    else
                    {
                        //Console.WriteLine($"Aplikacija '{AppName}' nije pronađena posle zatvaranja ❌");
                        System.Windows.MessageBox.Show($"Aplikacija '{AppName1}' nije pronađena posle zatvaranja ❌", "Informacija");
                    }

                }

                var process = Process.GetProcessesByName(AppName1).FirstOrDefault();
                _application1 = FlaUI.Core.Application.Attach(process);
                /*
                if (process != null)
                {
                    //Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                    _application1 = FlaUI.Core.Application.Attach(process);
                }
                else
                {
                    // Ako aplikacija nije pokrenuta, pokreni je
                    _application1 = FlaUI.Core.Application.Launch(AppPath1);
                }

                */

                // Inicijalizacija FlaUI
                _automation1 = new UIA3Automation();
                // Dohvatanje glavnog prozora aplikacije
                //var mainWindow = _application1.GetMainWindow(_automation1);
                var mainWindow = Retry.WhileNull(
                    () => _application1.GetMainWindow(_automation1),
                    TimeSpan.FromSeconds(10)).Result;
                // Provera da li je mainWindow null
                if (mainWindow == null)
                {
                    throw new Exception("Main window of the application was not found.");
                }

                //Pronalazak TreeView elementa
                //var treeView = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree))?.AsTree();
                var treeView = Retry.WhileNull(
                    () => mainWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Tree))?.AsTree(),
                    TimeSpan.FromSeconds(5)).Result;


                //Assert.IsNotNull(treeView, "TreeView not found");

                // Pronalazak TreeItem sa tekstom "Petrović Petar"
                //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName("Bogdan Mandarić 200035233"))).AsTreeItem();
                //var SertifikatName = KorisnikLoader5.Korisnik3?.Sertifikat ?? string.Empty;
                //var SertifikatName = AKorisnik_?.Sertifikat ?? string.Empty;
                //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName(SertifikatName))).AsTreeItem();


                var treeItem = Retry.WhileNull(
                    () => treeView?.FindFirstDescendant(cf =>
                        cf.ByControlType(ControlType.TreeItem).And(cf.ByName(SertifikatName)))?.AsTreeItem(),
                    TimeSpan.FromSeconds(5)).Result;


                //Assert.IsNotNull(treeItem, "TreeItem 'Bogdan Mandarić' not found");

                // Klik na TreeItem
                if (treeItem != null)
                {
                    treeItem.Click();
                }
                else
                {
                    throw new Exception($"TreeItem '{SertifikatName}' not found.");
                }

                // Pronalazak dugmeta "Cancel"
                //var cancelButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("Cancel"))).AsButton();
                //Assert.IsNotNull(quitButton, "Quit button not found");
                // Klik na dugme Quit
                //cancelButton.Click();

                // Pronalazak dugmeta "OK"
                var okButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK Enter"))).AsButton();
                //Assert.IsNotNull(okButton, "OK button not found");
                // Klik na dugme OK
                //if (okButton != null)
                //{
                //okButton.Click();
                //}
                //else
                //{
                //throw new Exception("OK button not found in the application window.");
                //}
                if (okButton == null)
                    throw new Exception("OK button not found in the application window.");
                okButton.WaitUntilClickable(TimeSpan.FromSeconds(5));
                okButton.Click();


                var process2 = Process.GetProcessesByName(AppName2).FirstOrDefault();
                if (process2 != null)
                {
                    // Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                    _application2 = FlaUI.Core.Application.Attach(process2);
                    // Inicijalizacija FlaUI
                    _automation2 = new UIA3Automation();
                    // Dohvatanje glavnog prozora aplikacije
                    var mainWindow2 = _application2.GetMainWindow(_automation2);

                    // Pronalazak TextBox elementa
                    if (mainWindow2 == null)
                    {
                        throw new Exception("Main window of the second application was not found.");
                    }
                    var treeElement = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree));
                    var textBox = treeElement?.AsTextBox();
                    //Assert.IsNotNull(textBox, "textBox not found");
                    // Unos teksta u TextBox
                    if (textBox != null)
                    {
                        textBox.Enter("73523");
                    }
                    else
                    {
                        throw new Exception("TextBox not found in the second application window.");
                    }
                    // Pronalazak dugmeta "OK"
                    var okButton2 = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK"))).AsButton();
                    //Assert.IsNotNull(okButton2, "OK button not found");
                    // Klik na dugme OK
                    if (okButton2 != null)
                    {
                        okButton2.Click();
                    }
                    else
                    {
                        throw new Exception("OK button not found in the second application window.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                await LogovanjeTesta.LogException("FlaUI Sertifikat Selekcija", ex);
                throw; // ili možeš odlučiti da NE baciš grešku dalje
            }


            #endregion Sertifikat





        }

        protected static async Task KreirajPolisuAO(IPage _page, string SertifikatName)
        {
            await _page.Locator("button").Filter(new() { HasText = "Kreiraj polisu" }).ClickAsync();

            await _page.Locator("button").Filter(new() { HasText = "Da!" }).ClickAsync();


            try
            {
                // Inicijalizacija FlaUI
                _automation = new UIA3Automation();
                //var mainWindow = _application.GetMainWindow(_automation);
                var process = Process.GetProcessesByName(AppName).FirstOrDefault();
                if (process != null)
                {
                    //Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                    _application = FlaUI.Core.Application.Attach(process);



                }
                else
                {
                    // Ako aplikacija nije pokrenuta, pokreni je
                    //_application = FlaUI.Core.Application.Launch(AppPath);
                    //await Sertifikat(_page, SertifikatName);
                }


                await _page.PauseAsync();

                // Dohvatanje glavnog prozora aplikacije
                var mainWindow = _application.GetMainWindow(_automation);
                mainWindow = Retry.WhileNull(
                    () => _application.GetMainWindow(_automation),
                    TimeSpan.FromSeconds(10)).Result;
                // Provera da li je mainWindow null
                if (mainWindow == null)
                {
                    throw new Exception("Main window of the application was not found.");
                }

                //Pronalazak TreeView elementa
                //var treeView = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree))?.AsTree();
                var treeView = Retry.WhileNull(
                    () => mainWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.Tree))?.AsTree(),
                    TimeSpan.FromSeconds(5)).Result;


                //Assert.IsNotNull(treeView, "TreeView not found");

                // Pronalazak TreeItem sa tekstom "Petrović Petar"
                //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName("Bogdan Mandarić 200035233"))).AsTreeItem();
                //var SertifikatName = KorisnikLoader5.Korisnik3?.Sertifikat ?? string.Empty;
                //var SertifikatName = AKorisnik_?.Sertifikat ?? string.Empty;
                //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName(SertifikatName))).AsTreeItem();


                var treeItem = Retry.WhileNull(
                    () => treeView?.FindFirstDescendant(cf =>
                        cf.ByControlType(ControlType.TreeItem).And(cf.ByName(SertifikatName)))?.AsTreeItem(),
                    TimeSpan.FromSeconds(5)).Result;


                //Assert.IsNotNull(treeItem, "TreeItem 'Bogdan Mandarić' not found");

                // Klik na TreeItem
                if (treeItem != null)
                {
                    treeItem.Click();
                }
                else
                {
                    throw new Exception($"TreeItem '{SertifikatName}' not found.");
                }

                // Pronalazak dugmeta "Cancel"
                //var cancelButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("Cancel"))).AsButton();
                //Assert.IsNotNull(quitButton, "Quit button not found");
                // Klik na dugme Quit
                //cancelButton.Click();

                // Pronalazak dugmeta "OK"
                var okButton = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK Enter"))).AsButton();
                //Assert.IsNotNull(okButton, "OK button not found");
                // Klik na dugme OK
                //if (okButton != null)
                //{
                //okButton.Click();
                //}
                //else
                //{
                //throw new Exception("OK button not found in the application window.");
                //}
                if (okButton == null)
                    throw new Exception("OK button not found in the application window.");
                okButton.WaitUntilClickable(TimeSpan.FromSeconds(5));
                okButton.Click();


                var process2 = Process.GetProcessesByName(AppName2).FirstOrDefault();
                if (process2 != null)
                {
                    // Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                    _application2 = FlaUI.Core.Application.Attach(process2);
                    // Inicijalizacija FlaUI
                    _automation2 = new UIA3Automation();
                    // Dohvatanje glavnog prozora aplikacije
                    var mainWindow2 = _application2.GetMainWindow(_automation2);

                    // Pronalazak TextBox elementa
                    if (mainWindow2 == null)
                    {
                        throw new Exception("Main window of the second application was not found.");
                    }
                    var treeElement = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree));
                    var textBox = treeElement?.AsTextBox();
                    //Assert.IsNotNull(textBox, "textBox not found");
                    // Unos teksta u TextBox
                    if (textBox != null)
                    {
                        textBox.Enter("73523");
                    }
                    else
                    {
                        throw new Exception("TextBox not found in the second application window.");
                    }
                    // Pronalazak dugmeta "OK"
                    var okButton2 = mainWindow2.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button).And(cf.ByName("OK"))).AsButton();
                    //Assert.IsNotNull(okButton2, "OK button not found");
                    // Klik na dugme OK
                    if (okButton2 != null)
                    {
                        okButton2.Click();
                    }
                    else
                    {
                        throw new Exception("OK button not found in the second application window.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                await LogovanjeTesta.LogException("FlaUI Sertifikat Selekcija", ex);
                throw; // ili možeš odlučiti da NE baciš grešku dalje
            }



        }



        [TearDown]
        public void TearDownApplications()
        {
            // Dispose automations first
            try
            {
                _automation?.Dispose();
            }
            catch { }
            finally
            {
                _automation = null;
            }
            try
            {
                _automation1?.Dispose();
            }
            catch { }
            finally
            {
                _automation1 = null;
            }

            try
            {
                _automation2?.Dispose();
            }
            catch { }
            finally
            {
                _automation2 = null;
            }

            // Close and dispose applications
            try
            {
                if (_application != null)
                {
                    try { _application.Close(); } catch { }
                    try { _application.Dispose(); } catch { }
                    _application = null;
                }
            }
            catch { }


            try
            {
                if (_application1 != null)
                {
                    try { _application1.Close(); } catch { }
                    try { _application1.Dispose(); } catch { }
                    _application1 = null;
                }
            }
            catch { }

            try
            {
                if (_application2 != null)
                {
                    try { _application2.Close(); } catch { }
                    try { _application2.Dispose(); } catch { }
                    _application2 = null;
                }
            }
            catch { }
        }


    }
}