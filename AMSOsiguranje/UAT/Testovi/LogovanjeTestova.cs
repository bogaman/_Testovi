
namespace UAT
{
    public class LogovanjeTesta : Osiguranje
    {
        #region Parametri testova
        /// <summary>
        /// ID testiranja, koristi se za povezivanje sa bazom podataka. To je primarni ključ u bazi.
        /// </summary>
        /// <value></value>
        public static int IDTestiranja { get; set; } = 0; // ID testiranja, koristi se za povezivanje sa bazom podataka
        /// <summary>
        /// ID testiranja, koristi se za povezivanje sa bazom podataka. To je primarni ključ u bazi.
        /// </summary>
        /// <value></value>
        public static int IDTestiranja1 { get; set; } = 0; // ID testiranja, koristi se za povezivanje sa bazom podataka
        /// <summary>
        /// ID pojedinačnog testa, koristi se za povezivanje sa bazom podataka. To je primarni ključ u bazi.
        /// </summary>
        /// <value></value>
        public static int IDTesta { get; set; } = 0; // ID pojedinačnog testa, koristi se za povezivanje sa bazom podataka
        public static int IDTesta1 { get; set; } = 0; // ID pojedinačnog testa, koristi se za povezivanje sa bazom podataka

        #endregion Parametri testova
        public static string LogFolder { get; set; } = Path.Combine(ProjektFolder, "Logovi");
        public static string LogFajlSumarni { get; set; } = LogFolder + "\\logSumarni.txt";

        public static string LogFajlTrace { get; set; } = Path.Combine(LogFolder, "logTrace.txt");

        public static string LogFajlOpsti { get; set; } = Path.Combine(LogFolder, "logOpsti.txt");
        public static string PutanjaDoBazeIzvestaja { get; set; } = Path.Combine(LogFolder, @"dbRezultatiTestiranja.accdb");

        public static string ConnectionStringLogovi { get; set; } = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={PutanjaDoBazeIzvestaja};";
        public static string PutanjaDoBaze { get; set; } = Path.Combine(OsnovniFolder, @"dbLogoviTestova.accdb");
        public static string KonekcijaLogovi { get; set; } = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={PutanjaDoBaze};";
        public static int FailedTests => TestContext.CurrentContext.Result.FailCount;
        public static int PassTests => TestContext.CurrentContext.Result.PassCount;
        public static int SkippedTests => TestContext.CurrentContext.Result.SkipCount;
        public static int UkupnoTests => FailedTests + PassTests + SkippedTests;


        #region Vremena
        /// <summary>
        /// Zadaje se vreme početka svih testiranja.
        /// </summary>
        public static DateTime PocetakTestiranja { get; set; } = DateTime.MinValue; // Vreme kada su pokrenuti svi testovi
        /// <summary>
        /// Zadaje se vreme završetka svih testiranja.
        /// </summary>
        public static DateTime KrajTestiranja { get; set; } = DateTime.MinValue; // Vreme kada su svi testovi završeni
        ///<summary>
        /// Vreme kada je pokrenut pojedinačni test.
        /// </summary>
        ///<remarks>Ovo vreme se koristi za praćenje trajanja pojedinačnih testova.</remarks>
        public static DateTime PocetakTesta { get; set; } = DateTime.MinValue; // Vreme kada je pokrenut pojedinačni test
        /// <summary>
        /// Vreme kada je pojedinačni test završen. 
        /// <para>Koristi se za praćenje trajanja pojedinačnih testova i unosi se u bazu podataka.</para>
        /// </summary>
        /// <value></value>
        public static DateTime KrajTesta { get; set; } = DateTime.MinValue; // Vreme kada je pojedinačni test završen
        #endregion Vremena


        /// <summary>
        /// Unosi se vreme početka testiranja u bazu podataka i vraća ID unetog zapisa.
        /// </summary>
        /// <param name="pocetakTestiranja">Vreme početka testiranja.</param>  
        /// <returns>Vraća ID unetog zapisa u tabeli tblSumarniIzvestajTestiranja.</returns>    
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka testiranja i čuva informacije u bazi podataka.</remarks>
        /// 
        /// <example>
        /// <code>
        /// DateTime pocetak = DateTime.Now;    
        /// int id = LogovanjeTesta.UnesiPocetakTestiranja(pocetak);
        /// Console.WriteLine($"ID unetog zapisa: {id}");
        /// </code>
        /// </example>
        public static int UnesiPocetakTestiranja(DateTime pocetakTestiranja)
        {

            string insertCommand = $"INSERT INTO tblSumarniIzvestajTestiranja (pocetakTestiranja) " +
                                   $"VALUES (@pocetakTestiranja)";

            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (OleDbConnection connection = new(ConnectionStringLogovi))
            {
                try
                {
                    connection.Open();

                    using OleDbCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@pocetakTestiranja", pocetakTestiranja);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} red je uspešno unet u tabelu tblSumarniIzvestajTestiranja.");

                    using (OleDbCommand getIdCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                    {
                        object? result = getIdCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newRecordId = Convert.ToInt32(result);
                        }
                    }
                    Console.WriteLine($"Novi ID unetog zapisa (redni broj testiranja) je: {newRecordId}");
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
                    LogError($"❌ Greška prilikom upisa u bazu: {ex.Message}");
                    throw; // Ponovo baca grešku, da NUnit zna da je test neuspešan
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
        /// <param name="IDTestiranja">ID testiranja (redni broj testiranja).</param>
        /// <param name="NazivTekucegTesta">Naziv trenutnog testa.</param>
        /// <param name="pocetakTesta">Vreme početka testa.</param> 
        /// <returns>Vraća ID unetog zapisa u tabeli tblPojedinacniIzvestajTestova. To je redni broj pojedinačnog testa</returns>
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka pojedinačnog testa i čuva informacije u bazi podataka.</remarks>
        public static int UnesiPocetakTesta(int IDTestiranja, string NazivTekucegTesta, DateTime pocetakTesta)
        {
            string insertCommand = $"INSERT INTO tblPojedinacniIzvestajTestova (IDTestiranja, NazivTesta, PocetakTesta) " +
                                   @$"VALUES (@IDTestiranja, @nazivTekucegTesta, @pocetakTesta)";
            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (OleDbConnection connection = new(ConnectionStringLogovi))
            {
                try
                {
                    connection.Open();

                    using OleDbCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@IDTestiranja", IDTestiranja);
                    command.Parameters.AddWithValue("@nazivTekucegTesta", NazivTekucegTesta);
                    command.Parameters.AddWithValue("@pocetakTesta", pocetakTesta);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} red je uspešno unet.");

                    using (OleDbCommand getIdCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                    {
                        object? result = getIdCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newRecordId = Convert.ToInt32(result);
                        }
                    }
                    Console.WriteLine($"Novi ID unetog zapisa (redni broj testa) je: {newRecordId}");
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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
        /// <param name="IDTestiranja">ID testiranja (redni broj testiranja).</param>
        /// <param name="NazivTekucegTesta">Naziv trenutnog testa.</param>
        /// <param name="pocetakTesta">Vreme početka testa.</param> 
        /// <returns>Vraća ID unetog zapisa u tabeli tblPojedinacniIzvestajTestova. To je redni broj pojedinačnog testa</returns>
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka pojedinačnog testa i čuva informacije u bazi podataka.</remarks>
        public static int UnesiPocetakTesta1(int IDTestiranja, string NazivTekucegTesta, DateTime pocetakTesta)
        {
            string insertCommand = $"INSERT INTO tblPojedinacniIzvestajTestova (IDTestiranja, NazivTesta, PocetakTesta) " +
                                   @$"VALUES (@IDTestiranja, @nazivTekucegTesta, @pocetakTesta)";
            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (OleDbConnection connection = new(KonekcijaLogovi))
            {
                try
                {
                    connection.Open();

                    using OleDbCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@IDTestiranja", IDTestiranja);
                    command.Parameters.AddWithValue("@nazivTekucegTesta", NazivTekucegTesta);
                    command.Parameters.AddWithValue("@pocetakTesta", pocetakTesta);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} red je uspešno unet.");

                    using (OleDbCommand getIdCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                    {
                        object? result = getIdCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newRecordId = Convert.ToInt32(result);
                        }
                    }
                    Console.WriteLine($"Novi ID unetog zapisa (redni broj testa) je: {newRecordId}");
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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
        /// Unosi se rezultat svih testiranja u bazu podataka.
        /// </summary>  
        /// <param name="newRecordId">ID testiranja (redni broj testiranja).</param>
        /// <param name="paliTestovi">Broj testova koji su pali.</param>        
        /// <param name="prosliTestovi">Broj testova koji su prošli.</param>
        /// <param name="preskoceniTestovi">Broj testova koji su preskočeni.</param>    
        /// <param name="UkupnoTestova">Ukupan broj testova.</param>
        /// <param name="krajTestiranja">Vreme završetka testiranja.</param>    
        /// <returns>Ne vraća ništa, samo ažurira postojeći zapis u tabeli tblSumarniIzvestajTestiranja.</returns>
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>
        /// <remarks>Ova metoda se koristi za ažuriranje rezultata testiranja u bazi podataka.</remarks>
        public static void UnesiRezultatTestiranja(int newRecordId, int paliTestovi, int prosliTestovi, int preskoceniTestovi, int UkupnoTestova, DateTime krajTestiranja)
        {
            string updateCommand = $"UPDATE tblSumarniIzvestajTestiranja SET " +
                                   $"failedTests = @failedTests, " +
                                   $"passTests = @passTests, " +
                                   $"skippedTests = @skippedTests, " +
                                   $"ukupnoTests = @ukupnoTests, " +
                                   $"krajTestiranja = @krajTestiranja " +
                                   $"WHERE IDTestiranja = @IDTestiranja;";

            using OleDbConnection connection = new(ConnectionStringLogovi);
            try
            {
                connection.Open();

                using OleDbCommand command = new(updateCommand, connection);
                command.Parameters.AddWithValue("@failedTests", paliTestovi);
                command.Parameters.AddWithValue("@passTests", prosliTestovi);
                command.Parameters.AddWithValue("@skippedTests", preskoceniTestovi);
                command.Parameters.AddWithValue("@ukupnoTests", UkupnoTestova);
                command.Parameters.AddWithValue("@krajTestiranja", krajTestiranja);
                command.Parameters.AddWithValue("@IDTestiranja", newRecordId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} red je uspešno unet.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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

        public static void UnesiRezultatTesta(int newRecordId, DateTime krajTesta, TestStatus StatusTesta, string errorMessage, string stackTrace)
        {
            string updateCommand = $"UPDATE tblPojedinacniIzvestajTestova SET " +
                                   $"KrajTesta = @krajTesta, " +
                                   $"Rezultat = @rezultat, " +
                                   $"OpisGreske = @opisGreske, " +
                                   $"StackTrace = @stackTrace " +
                                   $"WHERE IDTesta = @IDTesta;";

            using OleDbConnection connection = new(ConnectionStringLogovi);
            try
            {
                connection.Open();
                using OleDbCommand command = new(updateCommand, connection);
                command.Parameters.AddWithValue("@krajTesta", krajTesta);
                command.Parameters.AddWithValue("@rezultat", StatusTesta.ToString()); // Pretvori enum u string
                command.Parameters.AddWithValue("@opisGreske", errorMessage);
                command.Parameters.AddWithValue("@stackTrace", stackTrace);
                command.Parameters.AddWithValue("@IDTesta", newRecordId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} red je ažuriran.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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
        public static void UnesiRezultatTesta1(int newRecordId, DateTime krajTesta, TestStatus StatusTesta, string errorMessage, string stackTrace)
        {
            string updateCommand = $"UPDATE tblPojedinacniIzvestajTestova SET " +
                                   $"KrajTesta = @krajTesta, " +
                                   $"Rezultat = @rezultat, " +
                                   $"OpisGreske = @opisGreske, " +
                                   $"StackTrace = @stackTrace " +
                                   $"WHERE IDTesta = @IDTesta;";

            using OleDbConnection connection = new(KonekcijaLogovi);
            try
            {
                connection.Open();
                using OleDbCommand command = new(updateCommand, connection);
                command.Parameters.AddWithValue("@krajTesta", krajTesta);
                command.Parameters.AddWithValue("@rezultat", StatusTesta.ToString()); // Pretvori enum u string
                command.Parameters.AddWithValue("@opisGreske", errorMessage);
                command.Parameters.AddWithValue("@stackTrace", stackTrace);
                command.Parameters.AddWithValue("@IDTesta", newRecordId);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} red je ažuriran.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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

        /// <summary>
        /// Unosi se vreme početka testiranja u bazu podataka i vraća ID unetog zapisa.
        /// </summary>
        /// <param name="pocetakTestiranja">Vreme početka testiranja.</param>  
        /// <param name="nazivNamespace">Naziv namespace-a koji se koristi za testiranje.</param>
        /// <returns>Vraća ID unetog zapisa u tabeli tblSumarniIzvestajTestiranja.</returns>    
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>  
        /// <remarks>Ova metoda se koristi za praćenje početka testiranja i čuva informacije u bazi podataka.</remarks>
        /// 
        /// <example>
        /// <code>
        /// DateTime pocetak = DateTime.Now;    
        /// int id = LogovanjeTesta.UnesiPocetakTestiranja(pocetak);
        /// Console.WriteLine($"ID unetog zapisa: {id}");
        /// </code>
        /// </example>
        public static int UnesiPocetakTestiranja1(DateTime pocetakTestiranja, string nazivNamespace)
        {

            string insertCommand = $"INSERT INTO tblSumarniIzvestajTestiranja (pocetakTestiranja, Okruzenje ) " +
                                   $"VALUES (@pocetakTestiranja, @nazivNamespace)";

            int newRecordId = -1; // Pretpostavljamo da je primarni ključ numerički i auto-inkrement

            using (OleDbConnection connection = new(KonekcijaLogovi))
            {
                try
                {
                    connection.Open();

                    using OleDbCommand command = new(insertCommand, connection);
                    command.Parameters.AddWithValue("@pocetakTestiranja", pocetakTestiranja);
                    command.Parameters.AddWithValue("@nazivNamespace", nazivNamespace);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} red je uspešno unet u tabelu tblSumarniIzvestajTestiranja.");

                    using (OleDbCommand getIdCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                    {
                        object? result = getIdCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newRecordId = Convert.ToInt32(result);
                        }
                    }
                    Console.WriteLine($"Novi ID unetog zapisa (redni broj testiranja) je: {newRecordId}");
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
                    LogError($"❌ Greška prilikom upisa u bazu: {ex.Message}");
                    throw; // Ponovo baca grešku, da NUnit zna da je test neuspešan
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
        /// Unosi se rezultat svih testiranja u bazu podataka.
        /// </summary>  
        /// <param name="newRecordId">ID testiranja (redni broj testiranja).</param>
        /// <param name="paliTestovi">Broj testova koji su pali.</param>        
        /// <param name="prosliTestovi">Broj testova koji su prošli.</param>
        /// <param name="preskoceniTestovi">Broj testova koji su preskočeni.</param>    
        /// <param name="UkupnoTestova">Ukupan broj testova.</param>
        /// <param name="krajTestiranja">Vreme završetka testiranja.</param>    
        /// <returns>Ne vraća ništa, samo ažurira postojeći zapis u tabeli tblSumarniIzvestajTestiranja.</returns>
        /// <exception cref="OleDbException">Baca grešku ako dođe do problema prilikom unosa podataka.</exception>
        /// <remarks>Ova metoda se koristi za ažuriranje rezultata testiranja u bazi podataka.</remarks>
        public static void UnesiRezultatTestiranja2(int newRecordId, int paliTestovi, int prosliTestovi, int preskoceniTestovi, int UkupnoTestova, DateTime krajTestiranja)
        {
            string updateCommand = $"UPDATE tblSumarniIzvestajTestiranja SET " +
                                   $"failedTests = @failedTests, " +
                                   $"passTests = @passTests, " +
                                   $"skippedTests = @skippedTests, " +
                                   $"ukupnoTests = @ukupnoTests, " +
                                   $"krajTestiranja = @krajTestiranja " +
                                   $"WHERE IDTestiranja = @IDTestiranja;";

            using OleDbConnection connection = new(KonekcijaLogovi);
            try
            {
                connection.Open();

                using OleDbCommand command = new(updateCommand, connection);
                command.Parameters.AddWithValue("@failedTests", paliTestovi);
                command.Parameters.AddWithValue("@passTests", prosliTestovi);
                command.Parameters.AddWithValue("@skippedTests", preskoceniTestovi);
                command.Parameters.AddWithValue("@ukupnoTests", UkupnoTestova);
                command.Parameters.AddWithValue("@krajTestiranja", krajTestiranja);
                command.Parameters.AddWithValue("@IDTestiranja", newRecordId);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} red je uspešno unet.");
            }
            catch (OleDbException ex)
            {
                Console.WriteLine($"Greška prilikom unosa podataka: {ex.Message}");
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



    }


    [TestFixture]
    public class TestSuite : Osiguranje
    {

        [Test]
        public async Task TestExample()
        {
            string testName = "TestExample";
            try
            {
                await _page!.GotoAsync("https://example.com");
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
}