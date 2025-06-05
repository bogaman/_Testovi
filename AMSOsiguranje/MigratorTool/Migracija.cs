using System;
using System.IO;

class Program
{
    static void Main()
    {
        string sourceDir = @"C:\_Testovi\AMSOsiguranje\Razvoj";
        string targetDir = @"C:\_Testovi\AMSOsiguranje\Proba2";
        string sourceBatDir = @"C:\_Testovi\AMSOsiguranje\Razvoj\BatchFiles";
        string targetBatDir = @"C:\_Testovi\AMSOsiguranje\Proba2\BatchFiles";
        string oldNamespace = "Razvoj";
        string newNamespace = "Proba2";

        KopirajDirektorijum(sourceDir, targetDir, oldNamespace, newNamespace);

        // Kopiraj i preimenuj .csproj fajl
        string sourceCsproj = Path.Combine(sourceDir, "Razvoj.csproj");
        string targetCsproj = Path.Combine(targetDir, "Proba2.csproj");
        if (File.Exists(sourceCsproj))
        {
            File.Copy(sourceCsproj, targetCsproj, true);
            Console.WriteLine("✓ .csproj fajl kopiran i preimenovan.");
        }

        
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
            if (filePath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) )
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
