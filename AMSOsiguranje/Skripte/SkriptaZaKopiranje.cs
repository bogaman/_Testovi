using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string source = @"Razvoj\TestLogin.cs";
        string destination = @"Test\TestLogin.cs";
        string oldNamespace = "Razvoj";
        string newNamespace = "Test";

        if (!File.Exists(source))
        {
            Console.WriteLine($"Fajl {source} ne postoji.");
            return;
        }

        string content = File.ReadAllText(source);
        content = content.Replace($"namespace {oldNamespace}", $"namespace {newNamespace}");

        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        File.WriteAllText(destination, content);

        Console.WriteLine("Fajl kopiran i namespace zamenjen.");
    }
}
