
namespace Razvoj
{
    public class LogovanjeTesta : Osiguranje
    {
        #region Parametri testova

        /// <summary>
        /// ID testiranja, koristi se za povezivanje sa bazom podataka. To je primarni ključ u bazi.
        /// </summary>
        /// <value></value>
        public static int IDTestiranje { get; set; } = 0; // ID testiranja, koristi se za povezivanje sa bazom podataka

        /// <summary>
        /// ID pojedinačnog testa, koristi se za povezivanje sa bazom podataka. To je primarni ključ u bazi.
        /// </summary>
        /// <value></value>

        public static int IDTestaSQL { get; set; } = 0; // ID pojedinačnog testa, koristi se za povezivanje sa bazom podataka


        #endregion Parametri testova
        public static string LogFolder { get; set; } = Path.Combine(ProjektFolder, "Logovi");
        public static string LogFajlSumarni { get; set; } = LogFolder + "\\logSumarni.txt";

        public static string LogFajlTrace { get; set; } = Path.Combine(LogFolder, "logTrace.txt");

        public static string LogFajlOpsti { get; set; } = Path.Combine(LogFolder, "logOpsti.txt");
        public static string PutanjaDoBazeIzvestaja { get; set; } = Path.Combine(LogFolder, @"dbRezultatiTestiranja.accdb");

        public static string ConnectionStringLogovi { get; set; } = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={PutanjaDoBazeIzvestaja};";
        public static string PutanjaDoBazeAcces { get; set; } = Path.Combine(OsnovniFolder, @"dbLogoviTestova.accdb");
        public static string ConnectionStringAccess { get; set; } = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={PutanjaDoBazeAcces};";
        public static string ConnectionStringSQL { get; set; } = $"Server = 10.5.41.99; Database = TestLogDB; User ID = {UserID}; Password = {PasswordDB}; TrustServerCertificate = {TrustServerCertificate}";
        public static int FailedTests => TestContext.CurrentContext.Result.FailCount;
        public static int PassTests => TestContext.CurrentContext.Result.PassCount;
        public static int SkippedTests => TestContext.CurrentContext.Result.SkipCount;
        public static int UkupnoTests => FailedTests + PassTests + SkippedTests;


        #region Vremena
        /// <summary>
        /// Određuje se vreme početka testiranja.
        /// </summary>
        public static DateTime PocetakTestiranja { get; set; } = DateTime.MinValue; // Vreme kada su pokrenuti svi testovi
        /// <summary>
        /// Određuje se vreme završetka testiranja.
        /// </summary>
        public static DateTime KrajTestiranja { get; set; } = DateTime.MinValue; // Vreme kada su svi testovi završeni
        ///<summary>
        /// Vreme kada je pokrenut pojedinačni test.</summary>
        ///<remarks>Ovo vreme se koristi za praćenje trajanja pojedinačnih testova.</remarks>
        public static DateTime PocetakTesta { get; set; } = DateTime.MinValue; // Vreme kada je pokrenut pojedinačni test
        /// <summary>
        /// Vreme kada je završen pojedinačni test. 
        /// <para>Koristi se za praćenje trajanja pojedinačnih testova i unosi se u bazu podataka.</para>
        /// </summary>
        public static DateTime KrajTesta { get; set; } = DateTime.MinValue; // Vreme kada je pojedinačni test završen
        #endregion Vremena


        /// <summary>
        /// Unosi se vreme početka testiranja u bazu podataka i vraća ID unetog zapisa.
        /// </summary>
        /// <param name="pocetakTestiranja">Vreme početka testiranja.</param>  
        /// <param name="nazivNamespace">Naziv namespace-a koji se koristi za testiranje.</param>
        /// <param name="nacinPokretanjaTesta">Kako se pokrece test, ručno/automatski.</param>
        /// <returns>Vraća ID unetog zapisa u tabeli tblSumarniIzvestajTestiranja.</returns>    
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka testiranja i čuva informacije u bazi podataka.</remarks>
        /// <example>
        /// <code>
        /// DateTime pocetak = DateTime.Now;    
        /// int id = LogovanjeTesta.UnesiPocetakTestiranja(pocetak);
        /// Console.WriteLine($"ID unetog zapisa: {id}");
        /// </code>
        /// </example>
        public static int UnesiPocetakTestiranja(DateTime pocetakTestiranja, string nazivNamespace, string nacinPokretanjaTesta)
        {

            string insertCommand = @"INSERT INTO test.tReportSumary (PocetakTestiranja, Okruzenje, NacinTestiranja ) 
                                     VALUES (@pocetakTestiranja, @nazivNamespace, @nacinPokretanjaTesta)
                                     SELECT SCOPE_IDENTITY();"; // Vraća ID poslednje unete vrednosti u okviru iste sesije i scope-a

            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (SqlConnection connection = new(ConnectionStringSQL))
            {
                try
                {
                    connection.Open();

                    using SqlCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@pocetakTestiranja", pocetakTestiranja);
                    command.Parameters.AddWithValue("@nazivNamespace", nazivNamespace);
                    command.Parameters.AddWithValue("@nacinPokretanjaTesta", nacinPokretanjaTesta);
                    object? result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        newRecordId = Convert.ToInt32(result);
                    }

                    Console.WriteLine($"Novi ID unetog zapisa (redni broj testiranja) je: {newRecordId}");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"❌ Greška prilikom upisa u bazu: {ex.Message}");
                    LogError($"❌ Greška prilikom upisa u bazu: {ex.Message}");
                    throw;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return newRecordId;
        }


        /// <summary>
        /// Unosi se vreme početka pojedinačnog testa u bazu podataka i vraća ID unetog zapisa. 
        /// </summary>
        /// <param name="IDTestiranje">ID testiranja (redni broj testiranja).</param>
        /// <param name="NazivTekucegTesta">Naziv trenutnog testa.</param>
        /// <param name="pocetakTesta">Vreme početka testa.</param> 
        /// <returns>Vraća ID unetog zapisa u tabeli tblPojedinacniIzvestajTestova. To je redni broj pojedinačnog testa</returns>
        /// <exception cref="SqlException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka pojedinačnog testa i čuva informacije u bazi podataka.</remarks>
        public static int UnesiPocetakTesta(int IDTestiranje, string NazivTekucegTesta, DateTime pocetakTesta)
        {
            string insertCommand = @"INSERT INTO test.tReportIndividual (IDTestiranje, NazivTesta, PocetakTesta) 
                                     VALUES (@IDTestiranje, @nazivTekucegTesta, @pocetakTesta) 
                                     SELECT SCOPE_IDENTITY();";
            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (SqlConnection connection = new(ConnectionStringSQL))
            {
                try
                {
                    connection.Open();

                    using SqlCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@IDTestiranje", IDTestiranje);
                    command.Parameters.AddWithValue("@nazivTekucegTesta", NazivTekucegTesta);
                    command.Parameters.AddWithValue("@pocetakTesta", pocetakTesta);
                    object? result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        newRecordId = Convert.ToInt32(result);
                        Console.WriteLine($"Novi ID unetog zapisa (redni broj testa) je: {newRecordId}");
                    }
                }

                catch (SqlException ex)
                {
                    Console.WriteLine($"❌ Greška prilikom unosa podataka u MSSQL: {ex.Message}");
                    throw;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }

            return newRecordId;
        }

        /// <summary>
        /// Unosi se rezultat svih testiranja u bazu podataka.
        /// </summary>  
        /// <param name="newRecordId">ID testiranja (redni broj testiranja).</param>
        /// <param name="krajTesta">Vreme završetka testa.</param>        
        /// <param name="StatusTesta">Status testa.</param>
        /// <param name="errorMessage">Poruka o grešci.</param>    
        /// <param name="stackTrace"></param>
        /// <param name="agent">Ko je testirao.</param>    
        /// <param name="backOffice">Ko je testirao.</param> 
        /// <returns>Ne vraća ništa, samo ažurira postojeći zapis u tabeli test.tReportIndividual.</returns>
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>
        /// <remarks>Ova metoda se koristi za ažuriranje rezultata testiranja u bazi podataka.</remarks>
        public static void UnesiRezultatTesta(int newRecordId, DateTime krajTesta, TestStatus StatusTesta, string errorMessage, string stackTrace, string agent, string backOffice)
        {
            string updateCommand = @"UPDATE test.tReportIndividual 
                                     SET KrajTesta = @krajTesta, 
                                         Rezultat = @rezultat, 
                                         OpisGreske = @opisGreske, 
                                         StackTrace = @stackTrace, 
                                         Agent = @agent, 
                                         BackOffice = @backOffice 
                                      WHERE IDTest = @IDTest;";

            using SqlConnection connection = new(ConnectionStringSQL);


            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("@krajTesta", krajTesta);
                    command.Parameters.AddWithValue("@rezultat", StatusTesta.ToString());
                    command.Parameters.AddWithValue("@opisGreske", errorMessage ?? string.Empty);
                    command.Parameters.AddWithValue("@stackTrace", stackTrace ?? string.Empty);
                    command.Parameters.AddWithValue("@agent", agent ?? string.Empty);
                    command.Parameters.AddWithValue("@backOffice", backOffice ?? string.Empty);
                    command.Parameters.AddWithValue("@IDTest", newRecordId);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} red je ažuriran.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ Greška prilikom ažuriranja rezultata testa: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }


        /// <summary>
        /// Unosi se rezultat svih testiranja u bazu podataka.
        /// </summary>  
        /// <param name="newRecordId">ID testiranja (redni broj testiranja).</param>
        /// <param name="paliTestovi">Broj testova koji su pali.</param>        
        /// <param name="prosliTestovi">Broj testova koji su prošli.</param>
        /// <param name="preskoceniTestovi">Broj testova koji su preskočeni.</param>    
        /// <param name="UkupnoTestova">Ukupan broj testova.</param>
        /// <param name="krajTestiranja">Vreme završetka testiranja.</param>    
        /// <returns>Ne vraća ništa, samo ažurira postojeći zapis u tabeli tblSumarniIzvestajTestiranja.</returns>
        /// <exception cref="SqlException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>
        /// <remarks>Ova metoda se koristi za ažuriranje rezultata testiranja u bazi podataka.</remarks>
        public static void UnesiRezultatTestiranja(int newRecordId, int paliTestovi, int prosliTestovi, int preskoceniTestovi, int UkupnoTestova, DateTime krajTestiranja)
        {
            string updateCommand = @"UPDATE test.tReportSumary 
                                     SET NeuspesnihTestova = @failedTests, 
                                         UspesnihTestova = @passTests, 
                                         PreskocenihTestova = @skippedTests, 
                                         UkupnoTestova = @ukupnoTests, 
                                         KrajTestiranja = @krajTestiranja 
                                     WHERE IDTestiranje = @IDTestiranje;";

            using SqlConnection connection = new(ConnectionStringSQL);
            try
            {
                connection.Open();

                using SqlCommand command = new(updateCommand, connection);
                command.Parameters.AddWithValue("@failedTests", paliTestovi);
                command.Parameters.AddWithValue("@passTests", prosliTestovi);
                command.Parameters.AddWithValue("@skippedTests", preskoceniTestovi);
                command.Parameters.AddWithValue("@ukupnoTests", UkupnoTestova);
                command.Parameters.AddWithValue("@krajTestiranja", krajTestiranja);
                command.Parameters.AddWithValue("@IDTestiranje", newRecordId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} red je uspešno unet.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
                LogError($"❌ Greška prilikom ažuriranja podataka: {ex.Message}");
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void LogException(string lokacija, Exception ex)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionStringSQL);
                connection.Open();

                string query = @"INSERT INTO test.tErrorLog (IdTestiranje, IdTest, DatumVremeGreske, Lokacija, Poruka, StackTrace)
                                 VALUES (@IdTestiranje, @IdTest, @DatumVremeGreske, @Lokacija, @Poruka, @StackTrace)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdTest", IDTestaSQL);
                command.Parameters.AddWithValue("@IdTestiranje", IDTestiranje);
                command.Parameters.AddWithValue("@DatumVremeGreske", DateTime.Now);
                command.Parameters.AddWithValue("@Lokacija", lokacija ?? "Nepoznata");
                command.Parameters.AddWithValue("@Poruka", ex.Message);
                command.Parameters.AddWithValue("@StackTrace", ex.StackTrace ?? "");

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Greška pri logovanju u bazu: " + e.Message);
            }
        }
        static void ProveriLogFolder()
        {
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }
        }

        static LogovanjeTesta()
        {
            ProveriLogFolder();

            TextWriterTraceListener listener = new TextWriterTraceListener(LogFajlTrace);
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
        }

        // Ova funkcija upisuje u fajl logTrace.txt i prikazuje poruke na konzoli
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
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
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


            File.AppendAllText(LogFajlSumarni, $"[{DateTime.Now}] Sumarni izveštaj:{Environment.NewLine}");
            File.AppendAllText(LogFajlSumarni, $"Pali:".PadRight(6) + $"{FailedTests}".PadRight(5));
            File.AppendAllText(LogFajlSumarni, $"Prošli:".PadRight(8) + $"{PassTests}".PadRight(5));
            File.AppendAllText(LogFajlSumarni, $"Preskočeni:".PadRight(12) + $"{SkippedTests}".PadRight(5));
            File.AppendAllText(LogFajlSumarni, $"Ukupno:".PadRight(8) + $"{UkupnoTests}".PadRight(5));
            File.AppendAllText(LogFajlSumarni, $"{Environment.NewLine}");
            File.AppendAllText(LogFajlSumarni, $"{Environment.NewLine}");
        }


        // Upisivanje početka testa u fajl logOpsti.txt
        public static void UnesiPocetakTesta_1()
        {
            File.AppendAllText($"{LogFajlOpsti}", $"[INFO] Test: {TestContext.CurrentContext.Test.Name}, Okruženje: {Okruzenje} ({PocetnaStrana}), {DateTime.Now.ToString("dd.MM.yyy. u hh:mm:ss")}\n");
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
            File.AppendAllText($"{LogFajlOpsti}", $"[REZULTAT] {status}, {DateTime.Now.ToString("dd.MM.yyyy. u hh:mm:ss")}\n");
            File.AppendAllText($"{LogFajlOpsti}", $"\n");
        }








    }


}