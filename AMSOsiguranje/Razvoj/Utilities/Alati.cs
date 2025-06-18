namespace Razvoj
{

    public class Alati : Osiguranje
    {
        /// <summary>
        /// Briše sve .txt fajlove u zadatom folderu.
        /// </summary>
        /// <param name="folderPath">Putanja do foldera</param>
        public static void ObrisiTxtFajlove(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine($"Folder ne postoji: {folderPath}");
                    return;
                }

                string[] txtFajlovi = Directory.GetFiles(folderPath, "*1.txt");

                foreach (string fajl in txtFajlovi)
                {
                    File.Delete(fajl);
                    Console.WriteLine($"Obrisan fajl: {fajl}");
                }

                Console.WriteLine("Brisanje .txt fajlova je završeno.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom brisanja fajlova: {ex.Message}");
            }
        }
    }


}