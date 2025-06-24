using System;
using System.IO;
//using System.AppDomain;

class Migracije
{

    /// <summary>
    ///  Glavna funkcija programa koja prihvata dva argumenta i pokreće migraciju.
    /// <para>Argumenti predstavljaju nazive okruženja između kojih se vrši migracija.</para>
    /// <para>Primer korišćenja: <b>dotnet run Razvoj Proba2</b></para>
    /// </summary>
    /// <param name="args"></param>
    /// <remarks>
    /// Ovaj program je dizajniran da migrira kod između različitih okruženja (npr. Razvoj-Proba2, Proba2-UAT, UAT-Produkcija).</remarks>
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("❗Treba proslediti tačno dva argumenta.");
            Console.WriteLine("Parametri mogu biti: Razvoj i Proba2, Proba2 i UAT ili Uat i Produkcija.");
            Console.WriteLine("Primer zadavanja: dotnet run Razvoj Proba2.");
            return; // Prekid programa ako nema dovoljno argumenata
        }

        string prviParametar = args[0];
        string drugiParametar = args[1];

        // Provera da li su argumenti validni
        if ((prviParametar != "Razvoj" && prviParametar != "Proba2" && prviParametar != "UAT" && prviParametar != "Produkcija") ||
            (drugiParametar != "Razvoj" && drugiParametar != "Proba2" && drugiParametar != "UAT" && drugiParametar != "Produkcija"))
        {
            Console.WriteLine("❗Neispravni nazivi parametara. Koristiti: Razvoj, Proba2, UAT ili Produkcija.");
            return; // Prekid programa ako su argumenti nevalidni
        }
        Console.WriteLine("");
        Console.WriteLine($"✓ Prvi parametar: {prviParametar}");
        Console.WriteLine($"✓ Drugi parametar: {drugiParametar}");

        string RadniFolder = AppDomain.CurrentDomain.BaseDirectory;
        string ProjektFolder = Directory.GetParent(RadniFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;
        Console.WriteLine("");
        Console.WriteLine($"✓ RadniFolder: {RadniFolder}");
        Console.WriteLine($"✓ ProjektFolder: {ProjektFolder}");


        string izvorniDir = Path.Combine(ProjektFolder, prviParametar);
        string ciljniDir = Path.Combine(ProjektFolder, drugiParametar);
        Console.WriteLine("");
        Console.WriteLine($"✓ izvorniDir: {izvorniDir}");
        Console.WriteLine($"✓ ciljniDir: {ciljniDir}");


        //string sourceDir = @"C:\_Testovi\AMSOsiguranje\Razvoj";
        string sourceDir = izvorniDir;
        //string targetDir = @"C:\_Testovi\AMSOsiguranje\Proba2";
        string targetDir = ciljniDir;
        //string oldNamespace = "Razvoj";
        string oldNamespace = prviParametar;
        //string newNamespace = "Proba2";
        string newNamespace = drugiParametar;



        KopirajDirektorijum(sourceDir, targetDir, oldNamespace, newNamespace);

        // Kopiraj i preimenuj .csproj fajl
        //string sourceCsproj = Path.Combine(sourceDir, "Razvoj.csproj");
        string sourceCsproj = Path.Combine(sourceDir, $"{prviParametar}.csproj");
        //string targetCsproj = Path.Combine(targetDir, "Proba2.csproj");
        string targetCsproj = Path.Combine(targetDir, $"{drugiParametar}.csproj");
        if (File.Exists(sourceCsproj))
        {
            File.Copy(sourceCsproj, targetCsproj, true);
            Console.WriteLine("✓ .csproj fajl kopiran i preimenovan.");
        }


        //string folder = @"C:\_Testovi\\AMSOsiguranje\Proba2\BatchFiles";
        string folder = Path.Combine(targetDir, "BatchFiles");
        if (!Directory.Exists(folder))
        {
            Console.WriteLine($"❗Folder {folder} ne postoji.");
            return; // Prekid programa ako folder ne postoji
        }

        string stariDeo = $"_{prviParametar}.bat";
        //string noviDeo = "_Proba2.bat";
        string noviDeo = $"_{drugiParametar}.bat";

        string[] fajlovi = Directory.GetFiles(folder, $"*{stariDeo}", SearchOption.TopDirectoryOnly);

        foreach (var fajl in fajlovi)
        {
            string noviNaziv = fajl.Replace(stariDeo, noviDeo);
            File.Delete(Path.Combine(folder, Path.GetFileName(noviNaziv))); // Ukloni ako postoji fajl sa istim imenom
            File.Move(fajl, noviNaziv);
            Console.WriteLine($"✓ Preimenovano: {Path.GetFileName(fajl)} → {Path.GetFileName(noviNaziv)}");
        }

        Console.WriteLine("Završeno preimenovanje fajlova.");

    }

    static void KopirajDirektorijum(string source, string target, string oldNamespace, string newNamespace)
    {
        Directory.CreateDirectory(target);

        foreach (var dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
        {
            // Preskoči Build i Logovi foldere
            if (dirPath.Contains("Build") || dirPath.Contains("Logovi"))
                continue;

            string newDir = dirPath.Replace(source, target);
            Directory.CreateDirectory(newDir);
        }
        foreach (var filePath in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            if (filePath.Contains("Build") || filePath.Contains("Logovi"))
                continue;
            if (filePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                continue;
                if (filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                continue;
            string newPath = filePath.Replace(source, target);

            // Provera ekstenzije
            string extension = Path.GetExtension(filePath).ToLower();

            // Pročitaj sadržaj ako je .cs ili .bat i zameni namespace
            if (extension == ".cs")
            {
                string content = File.ReadAllText(filePath);
                content = content.Replace($"namespace {oldNamespace}", $"namespace {newNamespace}");
                File.WriteAllText(newPath, content);
            }
            else if (extension == ".bat")
            {
                string content = File.ReadAllText(filePath);
                content = content.Replace(oldNamespace, newNamespace);
                File.WriteAllText(newPath, content);
            }
            else if (extension == ".sln")
            {
                string content = File.ReadAllText(filePath);
                content = content.Replace(oldNamespace, newNamespace);
                string newFileName = Path.GetFileName(filePath).Replace(oldNamespace, newNamespace); // Razvoj.sln → Proba2.sln
                string targetFile = Path.Combine(target, newFileName);

                File.WriteAllText(targetFile, content);
            }
            else
            {
                // Ostale fajlove samo kopiraj
                File.Copy(filePath, newPath, true);
            }


        }

        Console.WriteLine("✓ Kopiranje i zamena imena završeni.");
    }
}

