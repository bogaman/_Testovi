
namespace Razvoj
{

    public class LogovanjeTesta : Osiguranje
    {
        //private static readonly new string logFajlSumarni = logFolder + "\\logFajlSumarni.txt";
        //private static readonly string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logovi");
        private static readonly string logTrace = Path.Combine(logFolder, "logTrace.txt");
public static readonly string logFajlOpsti = Path.Combine(logFolder, "logOpsti.txt");
        public static readonly string logFajlSumarni = logFolder + "\\logSumarni.txt";



        static LogovanjeTesta()
        {
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            TextWriterTraceListener listener = new TextWriterTraceListener(logTrace);
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
        }

        public static void LogMessage(string message, bool useTimestamp = true)
        {
            try
            {
                //string timestamp = DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss");
                //string logEntry = $"[{timestamp}] {message}";
                string logEntry = useTimestamp
                ? $"[{DateTime.Now:dd.MM.yyyy. HH:mm:ss}] {message}"
                : message;
                Trace.WriteLine(logEntry);
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri upisu u log: {ex.Message}");
            }
        }

        public static void LogError(string errorMessage)
        {
            LogMessage("ERROR: " + errorMessage);
        }

        public static void LogTestResult(string testName, bool isSuccess)
        {
            string result = isSuccess ? "USPESNO" : "NEUSPESNO";
            LogMessage($"Test '{testName}' je završen sa statusom: {result}");
        }
        public static void LogException(Exception ex, string context = "")
        {
            try
            {
                var stackTrace = new StackTrace(ex, true); // true omogućava info o liniji
                var frame = stackTrace.GetFrame(0); // uzimamo prvi frame
                int line = frame?.GetFileLineNumber() ?? 0;
                string fileName = frame?.GetFileName() ?? "Nepoznata datoteka";

                string errorDetails = $"Exception: {ex.Message}\n" +
                                      $"Lokacija: {fileName} - linija: {line}\n" +
                                      $"StackTrace: {ex.StackTrace}";

                if (!string.IsNullOrEmpty(context))
                {
                    errorDetails = $"Kontekst: {context}\n" + errorDetails;
                }

                LogError(errorDetails);
            }
            catch (Exception loggingEx)
            {
                Console.WriteLine("Greška prilikom logovanja izuzetka: " + loggingEx.Message);
            }
        }
        public static void LogException1(Exception ex, string context = "")
        {
            string errorDetails = $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}";
            if (!string.IsNullOrEmpty(context))
            {
                errorDetails = $"Kontekst: {context}\n" + errorDetails;
            }
            LogError(errorDetails);
        }

        /// Metoda za kreiranje sumarnog izveštaja, poziva se iz OneTimeTearDown()
        public static void FormirajSumarniIzvestaj()
        {
            var failedTests = TestContext.CurrentContext.Result.FailCount;
            var passTests = TestContext.CurrentContext.Result.PassCount;
            var skippedTests = TestContext.CurrentContext.Result.SkipCount;
            var ukupnoTests = failedTests + passTests + skippedTests;

            File.AppendAllText(logFajlSumarni, $"[{DateTime.Now}] Sumarni izveštaj:{Environment.NewLine}");
            File.AppendAllText(logFajlSumarni, $"Pali:".PadRight(6) + $"{failedTests}".PadRight(5));
            File.AppendAllText(logFajlSumarni, $"Prošli:".PadRight(8) + $"{passTests}".PadRight(5));
            File.AppendAllText(logFajlSumarni, $"Preskočeni:".PadRight(12) + $"{skippedTests}".PadRight(5));
            File.AppendAllText(logFajlSumarni, $"Ukupno:".PadRight(8) + $"{ukupnoTests}".PadRight(5));
            File.AppendAllText(logFajlSumarni, $"{Environment.NewLine}");
            File.AppendAllText(logFajlSumarni, $"{Environment.NewLine}");
        }

        //public static string logFajlOpsti = Path.Combine(logFolder, "logOpsti.txt");
        // Upisivanje početka testa u fajl logOpsti.txt
        public static void UnesiPocetakTesta()
        {
            File.AppendAllText($"{logFajlOpsti}", $"[INFO] Test: {TestContext.CurrentContext.Test.Name}, Okruženje: {Okruzenje} ({PocetnaStrana}), {DateTime.Now.ToString("dd.MM.yyy. u hh:mm:ss")}\n");
        }


        // Upisivanje opšteg rezultata testa u logOpsti.txt
        public static void UnesiKrajTesta()
        {
            string status = TestContext.CurrentContext.Result.Outcome.Status.ToString();
            if (status == "Passed")
            {
                status = "✅ USPESNO";
            }
            else if (status == "Failed")
            {
                status = "❌ NEUSPESNO";
            }
            else if (status == "Skipped")
            {
                status = "❌ PRESKOCENO";
            }
            else
            {
                status = "❌ NEPOZNATO";
            }
            File.AppendAllText($"{logFajlOpsti}", $"[REZULTAT] {status}, {DateTime.Now.ToString("dd.MM.yyy. u hh:mm:ss")}\n");
            File.AppendAllText($"{logFajlOpsti}", $"\n");
        }

    }
/*****************

    [TestFixture]
    public class TestSuite
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [SetUp]
        public async Task SetUp()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _page = await _browser.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task TestExample()
        {
            string testName = "TestExample";
            try
            {
                await _page.GotoAsync("https://example.com");
                bool containsText = await _page.InnerTextAsync("body").ContinueWith(t => t.Result.Contains("Example Domain"));
                Assert.That(containsText, Is.True, "Očekivani tekst nije pronađen.");
                LogovanjeTesta.LogTestResult(testName, true);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogException(ex, testName);
                LogovanjeTesta.LogTestResult(testName, false);
                throw;
            }
        }
    }
    ******************/
}