
namespace UAT
{
    /// <summary>
    /// Klasa koja predstavlja korisnika sa svim potrebnim informacijama.
    /// </summary>
    public class Korisnik
    {
        public string Uloga { get; set; } = "";
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string Email1 { get; set; } = "";
        public string Email2 { get; set; } = "";
        public string KorisnickoIme { get; set; } = "";
        public string Lozinka1 { get; set; } = "";
        public string Lozinka2 { get; set; } = "";
        public string Sertifikat { get; set; } = "";
        public string Pin { get; set; } = "";
        public string IdLica { get; set; } = "";
        public string SaradnickaSifra1 { get; set; } = "";
        public string SaradnickaSifra2 { get; set; } = "";
        public string Saradnik1 { get; set; } = "";
        public string Saradnik2 { get; set; } = "";
    }

    /// <summary>
    /// Klasa koja učitava korisnike iz baze podataka.
    /// </summary>
    public static class KorisnikLoader
    {

        /// <summary>
        /// Vraća iz SQL baze, tabela tTesteri, listu testera koji se loguju i testiraju 
        /// </summary>
        /// <returns>korisnici</returns> <summary>
        /// </summary>
        public static List<Korisnik> UcitajKorisnike()
        {
            var korisnici = new List<Korisnik>();

            using var konekcija = new SqlConnection(LogovanjeTesta.ConnectionStringSQL);
            konekcija.Open();

            string upit = $"SELECT * FROM test.tTesteri";
            using var komanda = new SqlCommand(upit, konekcija);
            using var reader = komanda.ExecuteReader();

            while (reader.Read())
            {
                var korisnik = new Korisnik
                {
                    Uloga = reader["UlogaTester"]?.ToString() ?? string.Empty,
                    Ime = reader["ImeTester"]?.ToString() ?? string.Empty,
                    Prezime = reader["PrezimeTester"]?.ToString() ?? string.Empty,
                    Email1 = reader["Email_1"]?.ToString() ?? string.Empty,
                    Email2 = reader["Email_2"]?.ToString() ?? string.Empty,
                    KorisnickoIme = reader["KorisnickoIme"]?.ToString() ?? string.Empty,
                    Lozinka1 = reader["Lozinka_1"]?.ToString() ?? string.Empty,
                    Lozinka2 = reader["Lozinka_2"]?.ToString() ?? string.Empty,
                    Sertifikat = reader["Sertifikat"]?.ToString() ?? string.Empty,
                    Pin = reader["PIN"]?.ToString() ?? string.Empty,
                    IdLica = reader["IdLica"]?.ToString() ?? string.Empty,
                    SaradnickaSifra1 = reader["SaradnickaSifra_1"]?.ToString() ?? string.Empty,
                    SaradnickaSifra2 = reader["SaradnickaSifra_2"]?.ToString() ?? string.Empty,
                    Saradnik1 = reader["Saradnik_1"]?.ToString() ?? string.Empty,
                    Saradnik2 = reader["Saradnik_2"]?.ToString() ?? string.Empty
                };
                korisnici.Add(korisnik);
            }

            return korisnici;
        }

        public static List<Korisnik> UcitajKorisnikeIzAccessBaze()
        {
            var korisnici = new List<Korisnik>();

            using var konekcija = new OleDbConnection(LogovanjeTesta.ConnectionStringAccess);
            konekcija.Open();

            string upit = $"SELECT * FROM tblKorisnici";
            using var komanda = new OleDbCommand(upit, konekcija);
            using var reader = komanda.ExecuteReader();

            while (reader.Read())
            {
                var korisnik = new Korisnik
                {
                    Uloga = reader["Uloga"]?.ToString() ?? string.Empty,
                    Ime = reader["Ime"]?.ToString() ?? string.Empty,
                    Prezime = reader["Prezime"]?.ToString() ?? string.Empty,
                    Email1 = reader["Email1"]?.ToString() ?? string.Empty,
                    Email2 = reader["Email2"]?.ToString() ?? string.Empty,
                    KorisnickoIme = reader["KorisnickoIme"]?.ToString() ?? string.Empty,
                    Lozinka1 = reader["Lozinka1"]?.ToString() ?? string.Empty,
                    Lozinka2 = reader["Lozinka2"]?.ToString() ?? string.Empty,
                    Sertifikat = reader["Sertifikat"]?.ToString() ?? string.Empty,
                    Pin = reader["Pin"]?.ToString() ?? string.Empty,
                    IdLica = reader["IdLica"]?.ToString() ?? string.Empty,
                    SaradnickaSifra1 = reader["SaradnickaSifra1"]?.ToString() ?? string.Empty,
                    SaradnickaSifra2 = reader["SaradnickaSifra2"]?.ToString() ?? string.Empty,
                    Saradnik1 = reader["Saradnik1"]?.ToString() ?? string.Empty,
                    Saradnik2 = reader["Saradnik2"]?.ToString() ?? string.Empty
                };
                korisnici.Add(korisnik);
            }

            return korisnici;
        }

    }


    /// <summary>
    /// čita JSON fajl sa korisnicima i vraća odgovarajućeg korisnika na osnovu uslova.
    /// </summary>
    public static class KorisnikLoader5
    {
        public static Korisnik? Korisnik1 { get; private set; }
        public static Korisnik? Korisnik2 { get; private set; }
        public static Korisnik? Korisnik3 { get; private set; }
        public static List<Korisnik> UcitajKorisnike5()
        {
            string pathDoJsonFajla = Path.Combine(Osiguranje.ProjektFolder, "korisnici.json");
            if (!File.Exists(pathDoJsonFajla))
            {
                Console.WriteLine($"Fajl nije pronađen: {pathDoJsonFajla}");
                return [];
            }

            string json = File.ReadAllText(pathDoJsonFajla);

            try
            {
                var korisnici = JsonSerializer.Deserialize<List<Korisnik>>(json);

                Korisnik1 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "BackOffice");
                Korisnik2 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "Agent1");
                Korisnik3 = (korisnici ?? []).FirstOrDefault(k => k.Uloga == "Agent2");
                // Primer upotrebe:
                Console.WriteLine($"+Korisnik1: {Korisnik1?.Ime} {Korisnik1?.Prezime}");
                Console.WriteLine($"+Korisnik2: {Korisnik2?.Ime} {Korisnik2?.Prezime}");
                Console.WriteLine($"+Korisnik3: {Korisnik3?.Ime} {Korisnik3?.Prezime}");
                return korisnici ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom parsiranja JSON-a: {ex.Message}");
                return [];
            }
        }
    }


    /*
                public static Korisnik VratiKorisnika(string nacinPokretanja, string nazivTesta)
                {
                    if (nacinPokretanja == "rucno")
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "Bogdan");
                    if (nazivTesta.Contains("GrupaA"))
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "korisnik2");
                    if (nazivTesta.Contains("GrupaB"))
                        return _korisnici.FirstOrDefault(k => k.KorisnickoIme == "korisnik3");

                    return null;
                }

                public class Root
                {
                    public List<Korisnik>? Korisnici { get; set; }
                }
    */
    // Ova metoda može biti u nekoj servisnoj klasi

    /*
            public static void UcitajKorisnike5()
            {
                //string path = Path.Combine(ProjektFolder, "korisnici.json");
                //string path = "C:\\_Testovi\\AMSOsiguranje\\Razvoj\\korisnici.json";
                //string json = File.ReadAllText(path);
                var korisnici = KorisnikLoader5.UcitajKorisnike5();


                // Filtriranje po korisničkom imenu
                var korisnik1 = korisnici.FirstOrDefault(k => k.Uloga == "BackOffice");
                var korisnik2 = korisnici.FirstOrDefault(k => k.Uloga == "Agent1");
                var korisnik3 = korisnici.FirstOrDefault(k => k.Uloga == "Agent2");

                // Primer upotrebe:
                Console.WriteLine($"Korisnik1: {korisnik1?.Ime} {korisnik1?.Prezime}");
                Console.WriteLine($"Korisnik2: {korisnik2?.Ime} {korisnik2?.Prezime}");
                Console.WriteLine($"Korisnik3: {korisnik3?.Ime} {korisnik3?.Prezime}");
            }
            */
}