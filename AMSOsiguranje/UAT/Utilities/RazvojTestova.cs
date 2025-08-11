namespace UAT
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TestDevelopment : Osiguranje
    {



        [Test]
        public async Task _AO_4_ZamenaSerijskihBrojeva()
        {
            await _page!.PauseAsync();

            await _page!.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
            await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
            await _page.GetByText("Zamena serijskih brojeva").ClickAsync();
            await _page.GetByText("---Zamena serijskih brojevaPogrešno izdata polisa ---").ClickAsync();
            await _page.Locator("#razlogZamene").GetByText("Zamena serijskih brojeva").ClickAsync();

            //Uzmi serijske brojeve dveju kreiranih polisa 
            await _page.Locator("#unos1 input[type=\"text\"]").ClickAsync();
            await _page.GetByText("Serijski broj polise 2").ClickAsync();
            await _page.Locator(".lista").ClickAsync();
            await _page.Locator("#unos1").GetByText("Niste uneli validan serijski").ClickAsync();
            await _page.Locator("#unos2").GetByText("Niste uneli validan serijski").ClickAsync();
            await _page.GetByText("---Zamena serijskih brojevaPogrešno izdata polisa Zamena serijskih brojeva").ClickAsync();

            // uzmi serijski broj polise koja je izdata i serijski broj polise koja je odštampana
            await _page.Locator("#razlogZamene").GetByText("Pogrešno izdata polisa").ClickAsync();
            await _page.GetByText("Serijski broj obrasca unetog").ClickAsync();
            await _page.GetByText("Serijski broj odštampanog").ClickAsync();
            await _page.GetByText("Pogrešno izdata polisa se").ClickAsync();


            await _page.Locator("button").Filter(new() { HasText = "Zameni brojeve" }).ClickAsync();

            OsiguranjeVozila.PorukaKrajTesta();
        }

        [Test]
        public async Task _ProveraDashboard()
        {
            //await _page.PauseAsync();
            string[] tekstZaProveru; // Definiše šta se proverava na stranici

            #region Dashboard
            await _page!.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");

            #region Glavni i meni
            tekstZaProveru = ["Osiguranje vozila", "Putno zdravstveno osiguranje", "Administracija"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            tekstZaProveru = ["Davor Buli", "Korisnik broj: 1000"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            // Da li je ikona korisnika vidljiva
            await OsiguranjeVozila.ProveriVidljivostKontrole(_page, "//i[@class='ico-user-secret']", "ikona korisnika");
            #endregion Glavni i meni

            #region Administracija
            // Pređi mišem preko teksta Administracija
            await _page.GetByText("Administracija").HoverAsync();
            await _page.GetByText("Administracija").ClickAsync();
            // Klikni u meniju na Pregled korisnika
            await _page.GetByRole(AriaRole.Button, new() { Name = "Pregled korisnika" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Pregled-korisnika");
            //await _page.PauseAsync();
            await _page.GetByText("Opšti podaci Id Korisničko").ClickAsync();
            await _page.Locator(".col-12 > div:nth-child(2)").ClickAsync();
            await _page.Locator(".col-12 > div:nth-child(2)").ClickAsync();
            await _page.GetByText("Id lica - Korisnik - Korisničko ime - ").ClickAsync();

            //await _page.PauseAsync();
            await _page.Locator("button").Filter(new() { HasText = "Administracija" }).ClickAsync();
            await _page.Locator("button").Filter(new() { HasText = "Pregled tendera" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Pregled-tendera");
            await _page.GetByText("Naziv -").ClickAsync();
            await _page.GetByText("Ugovarač -").ClickAsync();
            await _page.GetByText("Naziv tendera").ClickAsync();
            await _page.Locator("#aktivan div").Nth(3).ClickAsync();
            await _page.Locator("#aktivan i").ClickAsync();
            await _page.GetByText("Valuta plaćanja [dana]").ClickAsync();

            await _page.GetByText("Opšti podaci Id Naziv tendera").ClickAsync();
            await _page.GetByText("Opšti podaci Id Naziv tendera").ClickAsync();
            await _page.Locator("div").Filter(new() { HasText = "idTender - Naziv - Ugovara" }).Nth(3).ClickAsync();
            //await _page.GetByText("Naziv tendera").Nth(2).ClickAsync();
            //await _page.GetByText("Naziv tendera").Nth(3).ClickAsync();

            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");
            #endregion Administracija




            #region Brzi linkovi
            tekstZaProveru = ["Brzi linkovi", "Nova polisa AO", "Novi zeleni karton", "Nova Polisa JS", "Nova polisa - lom stakla, auto nezgoda"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.Locator("//a[contains(.,'Nova polisa AO')]").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");
            //await _page.PauseAsync();

            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");

            await _page.Locator("//a[contains(.,'Novi zeleni karton')]").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Dokument/0");

            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");

            await _page.Locator("//a[contains(.,'Nova polisa JS')]").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Dokument/0");

            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");

            await _page.Locator("//a[contains(.,'Nova polisa - lom')]").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");

            await _page.Locator(".ico-ams-logo").ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Dashboard");
            #endregion Brzi linkovi

            #region Vesti i obaveštenja
            //tekstZaProveru = ["Vesti i obaveštenja", "Pretraga vesti", "Nova vest"];
            tekstZaProveru = ["Vesti i obaveštenja", "Pretraga vesti"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            // Proveri da li stranica sadrži dugme Nova vest
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//e-button[@id='btnNovaVest']", "dugme Nova vest");

            // Proveri da li stranica sadrži grid Vesti
            //await OsiguranjeVozila.ProveraPostojiGrid(_page, "//e-grid[@id='grid_vesti']", "grid Vesti");

            // Provera postojanja barem jedne vesti
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//div[starts-with(@class, 'vestiBox id_')]", "barem jedna vest");

            // Provera postojanja brojaca
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//button[@class='left primary flat btn-page-num num-1 flex-center-center']", "levi brojač");
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//button[@class='left primary flat flex-center-center']", "znak -");
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//div[@class='control']//input[@class='input']", "aktuelna strana vesti");
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//button[@class='left primary flat right flex-center-center']", "znak +");
            await OsiguranjeVozila.ProveriPostojanjeKontrole(_page, "//button[@class='left primary flat btn-page-num num-max flex-center-center']", "desni brojač");

            #endregion Vesti i obaveštenja

            // Provera budućih funkcionalnosti
            await _page.Locator("h3").Filter(new() { HasText = "Funkcionalnost će uskoro biti" }).ClickAsync();
            //await _page.Locator("h2").Filter(new() { HasText = "Funkcionalnost će uskoro biti" }).ClickAsync();


            #region Pregled prodaje 
            /*************************
                        if (Okruzenje == "Razvoj")
                        {
                            //await _page.Locator("h2").Filter(new() { HasText = "Pregled prodaje u izabranom periodu" }).ClickAsync();


                            await _page.Locator("div").Filter(new() { HasText = "Pregled prodaje u izabranom" }).Nth(4).ClickAsync();
                            await _page.GetByText("Pregled prodaje u izabranom").ClickAsync();
                            await _page.Locator("#cal_calendarVreme1 input[type=\"text\"]").ClickAsync();
                            await _page.Locator("#cal_calendarVreme2 input[type=\"text\"]").ClickAsync();
                            await _page.Locator("#selektRezolucija > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();

                            //await _page.GetByText("Meseci").ClickAsync();
                            //await _page.Locator("//div[@etitle='Meseci']").ClickAsync();
                            //await _page.Locator("#canvas").ClickAsync(new LocatorClickOptions{Position = new Position{X = 144, Y = 11,},});
                            await _page.Locator("//canvas[@class='platno']").ClickAsync(new LocatorClickOptions { Position = new Position { X = 144, Y = 11, }, });

                            await _page.Locator("div").Filter(new() { HasText = "Pregled prodaje u izabranom" }).Nth(4).ClickAsync();

                            System.Windows.MessageBox.Show("Doradi proveru #Pregled prodaje#", "Info", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Warning);

                        }
                        else
                        {
                            await _page.Locator("h2").Filter(new() { HasText = "Funkcionalnost će uskoro biti" }).ClickAsync();
                        }
            *****************/
            #endregion Pregled prodaje


            // Provera futera
            tekstZaProveru = ["Status mreže: Online", "Sesija aktivna još:"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            #endregion Dashboard


            #region Autoodgovornost

            // Pređi mišem preko teksta Osiguranje vozila
            await _page.GetByText("Osiguranje vozila").HoverAsync();
            await _page.GetByText("Osiguranje vozila").ClickAsync();
            // Klikni u meniju na Autoodgovornost
            await _page.GetByRole(AriaRole.Button, new() { Name = "Autoodgovornost" }).ClickAsync();


            /***************************************************************************************
            Ovo je isto kao gornje dve naredbe, ali se klikće na Autoodgovornost
            await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
            await _page.Locator("button").Filter(new() { HasText = "Autoodgovornost" }).ClickAsync();
            ***************************************************************************************/

            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Heading, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Autoodgovornost" }).ClickAsync();

            tekstZaProveru = ["Informativno kalkulisanje", "Polise autoodgovornosti", "Nova polisa",
                              "Pregled / Pretraga polisa", "Zamena serijskih brojeva", "Pregled / Pretraga zahteva za izmenom polisa",
                              "Razdužne liste", "Nova razdužna lista", "Pregled / Pretraga razdužnih listi", "Pregled tokova sa UOS",
                              "Pregled / Pretraga zahteva ka UOS", "Pregled / Pretraga paketa za kompletiranje", "Stroga evidencija",
                              "Pregled / Pretraga obrazaca", "Pregled / Pretraga dokumenata", "Novi ulaz u centralni magacin",
                              "Novi prenos", "Novi otpis"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            // Proveri da li stranica sadrži grid Autoodgovornost
            //await OsiguranjeVozila.ProveraPostojiGrid(_page, "//e-grid[@id='grid_dokumenti']", "grid Dokumenti - polise AO");

            //Informativno kalkulisanje
            await _page.GetByRole(AriaRole.Link, new() { Name = "Informativno kalkulisanje" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Informativno-kalkulisanje");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);
            tekstZaProveru = ["Dodaj", "Obriši poslednjeg", "Procena cene", "Vrsta osiguranja", "Broj vozila"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            // Polise Autoodgovornosti
            await _page.GetByRole(AriaRole.Link, new() { Name = "Polise autoodgovornosti" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");
            tekstZaProveru = ["Informativno kalkulisanje", "Polise autoodgovornosti", "Nova polisa", "Pregled / Pretraga polisa", "Zamena serijskih brojeva", "Pregled / Pretraga zahteva za izmenom polisa", "Razdužne liste", "Nova razdužna lista", "Pregled / Pretraga razdužnih listi", "Pregled tokova sa UOS", "Pregled / Pretraga zahteva ka UOS", "Pregled / Pretraga paketa za kompletiranje", "Stroga evidencija", "Pregled / Pretraga obrazaca", "Pregled / Pretraga dokumenata", "Novi ulaz u centralni magacin", "Novi prenos", "Novi otpis"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-dokumenata");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Zamena serijskih brojeva" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Zamena-serijskih-brojeva");
            //await ProveriSadrzajNaStrani(_page, textLevoAutoodgovornost);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zahteva za izmenom polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-zahteva/Izmene-polisa");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            //Razdužne liste
            await _page.GetByRole(AriaRole.Link, new() { Name = "Razdužne liste" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Nova razdužna lista" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/4/0");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga razduž" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata/4");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            //Pregled tokova ka UOS
            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled tokova sa UOS" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zahteva ka UOS" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-zahteva/Izmene-UOS");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga paketa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Pregled-kompletiranja");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            //Stroga evidencija
            await _page.GetByRole(AriaRole.Link, new() { Name = "Stroga evidencija" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga obrazaca" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-obrazaca");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga dokumenata" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Pregled-dokumenata");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi ulaz u centralni" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/1/0");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi prenos" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/2/0");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi otpis" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/1/Autoodgovornost/Dokument/3/0");
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);

            #endregion Autoodgovornost

            #region Zeleni karton

            await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "Zeleni karton" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Heading, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Zeleni karton" }).ClickAsync();


            await _page.GetByRole(AriaRole.Link, new() { Name = "Zeleni kartoni" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi zeleni karton" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Dokument/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zelenih kartona" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zahteva" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/4/Zeleni-karton/Pregled-zahteva/Izmene-polisa");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Razdužne liste" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Nova razdužna lista" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/4/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga razduž" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-dokumenata/4");


            await _page.GetByRole(AriaRole.Link, new() { Name = "Stroga evidencija" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga obrazaca" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-obrazaca");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga dokumenata" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Pregled-dokumenata");


            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi ulaz u centralni" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/1/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi prenos" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/2/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Novi otpis" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Stroga-Evidencija/4/Zeleni-karton/Dokument/3/0");

            #endregion Zeleni karton

            #region Javni saobraćaj

            await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje putnika u javnom" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-dokumenata");
            tekstZaProveru = ["Nova polisa",
                              "Pregled / Pretraga polisa", "Pregled / Pretraga zahteva za izmenom polisa",
                              "Razdužne liste (OSK)", "Nova razdužna lista (OSK)", "Pregled / Pretraga razdužnih listi (OSK)"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "OSiguranje putnika" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Osiguranje putnika u javnom" }).ClickAsync();

            await _page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Dokument/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zahteva" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/6/Osiguranje-putnika/Pregled-zahteva/Izmene-polisa");


            #endregion Javni saobraćaj


            #region Delimični kasko

            await _page.GetByRole(AriaRole.Button, new() { Name = "Osiguranje vozila" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "Lom stakla i auto nezgoda" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");
            tekstZaProveru = ["Nova polisa",
                              "Pregled / Pretraga polisa", "Pregled / Pretraga zahteva za izmenom polisa",
                              "Razdužne liste (OSK)", "Nova razdužna lista (OSK)", "Pregled / Pretraga razdužnih listi (OSK)"];
            await OsiguranjeVozila.ProveriSadrzajNaStrani(_page, tekstZaProveru);
            await _page.GetByRole(AriaRole.Link, new() { Name = "Lom stakla i auto nezgoda" }).ClickAsync();
            await _page.GetByRole(AriaRole.Link, new() { Name = "Nova polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga polisa" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Link, new() { Name = "Pregled / Pretraga zahteva" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-zahteva/Izmene-polisa");

            #endregion Delimični kasko


            /********************************************
                        #region Kasko
                        // Pređi mišem preko teksta Osiguranje vozila
                        //await _page.GetByText("Osiguranje vozila").HoverAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Osiguranje vozila" }).ClickAsync();
                        await _page.Locator("button").Filter(new() { HasText = "Kasko" }).ClickAsync();

                        // Klikni u meniju na Autoodgovornost
                        //await _page.GetByRole(AriaRole.Button, new() { Name = "Kasko" }).ClickAsync();
                        await ProveriURL(_page, PocetnaStrana, "/Kasko-osiguranje-vozila/9/Kasko/Dokument/0");

                        await _page.GetByRole(AriaRole.Heading, new() { Name = "Osiguranje vozila" }).ClickAsync();
                        await _page.GetByRole(AriaRole.Heading, new() { Name = "Kasko osiguranje" }).ClickAsync();

                        tekstZaProveru = ["Informativno kalkulisanje", "Polise kasko osiguranja", "Nova polisa",
                                          "Pregled / Pretraga polisa", "Pregled / Pretraga zahteva za izmenom polisa"];
                        await ProveriSadrzajNaStrani(_page, tekstZaProveru);

                        await _page.PauseAsync();
                        //Informativno kalkulisanje
                        await _page.GetByRole(AriaRole.Link, new() { Name = "Informativno kalkulisanje" }).ClickAsync();
                        await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/1/Autoodgovornost/Informativno-kalkulisanje");
                        await ProveriSadrzajNaStrani(_page, tekstZaProveru);
                        tekstZaProveru = ["Dodaj", "Obriši poslednjeg", "Procena cene", "Vrsta osiguranja", "Broj vozila"];
                        await ProveriSadrzajNaStrani(_page, tekstZaProveru);

                        // Proveri da li stranica sadrži grid Autoodgovornost
                        //await ProveraPostojiGrid(_page, "//e-grid[@id='grid_dokumenti']", "grid Dokumenti - polise AO");

                        System.Windows.Forms.MessageBox.Show("Kasko treba doraditi kada bude završen", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        #endregion Kasko

            *************************************************/
            #region Putno zdravstveno
            // Pređi mišem preko teksta Putno zdravstveno osiguranje
            await _page.GetByText("Putno zdravstveno osiguranje").HoverAsync();
            await _page.GetByText("Putno zdravstveno osiguranje").ClickAsync();
            // Klikni u meniju na Autoodgovornost
            await _page.GetByRole(AriaRole.Button, new() { Name = "Webshop backoffice" }).ClickAsync();
            await ProveriURL(_page, PocetnaStrana, "/Backoffice/Backoffice/1/Pregled-dokumenata");

            await _page.GetByRole(AriaRole.Heading, new() { Name = "WEBSHOP BACKOFFICE" }).ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Putno zdravstveno" }).ClickAsync();

            //tekstZaProveru = ["Informativno kalkulisanje", "Polise autoodgovornosti", "Nova polisa", "Pregled / Pretraga polisa", "Zamena serijskih brojeva", "Pregled / Pretraga zahteva za izmenom polisa", "Razdužne liste", "Nova razdužna lista", "Pregled / Pretraga razdužnih listi", "Pregled tokova sa UOS", "Pregled / Pretraga zahteva ka UOS", "Pregled / Pretraga paketa za kompletiranje", "Stroga evidencija", "Pregled / Pretraga obrazaca", "Pregled / Pretraga dokumenata", "Novi ulaz u centralni magacin", "Novi prenos", "Novi otpis"];
            //await ProveriSadrzajNaStrani(_page, tekstZaProveru);
            // Proveri da li stranica sadrži grid Dokumenti
            //await OsiguranjeVozila.ProveraPostojiGrid(_page, "//e-grid[@id='grid_dokumenti']", "grid Dokumenti - polise putnog osiguranja");

            #endregion Putno zdravstveno

            //await _page.PauseAsync();


            if (KorisnikMejl == "davor.bulic@eonsystem.com")
            {
                await _page.GetByRole(AriaRole.Button, new() { Name = "Putno zdravstveno" }).ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new Regex("Putno zdravstveno") .ClickAsync();
                await _page.GetByRole(AriaRole.Button, new() { Name = "Webshop backoffice" }).ClickAsync();
            }

            if (_page.Url == PocetnaStrana + "/Error")
            {
                await _page.GotoAsync(PocetnaStrana + "/Dashboard");
            }

            //await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();
            await _page.Locator(".ico-ams-logo").ClickAsync();
            await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
            CurrentUrl = _page.Url;
            Assert.That(PocetnaStrana + "/Dashboard", Is.EqualTo(CurrentUrl));

            //Klik na korisnika
            await _page.GetByText(KorisnikIme[..5]).ClickAsync();

            await _page.GetByRole(AriaRole.Button, new() { Name = "Podešavanja" }).ClickAsync();

            await _page.GetByRole(AriaRole.Button, new() { Name = "Promena lozinke" }).ClickAsync();
            await _page.WaitForURLAsync(PocetnaStrana + "/Nova-lozinka");
            CurrentUrl = _page.Url;
            Assert.That(PocetnaStrana + "/Nova-lozinka", Is.EqualTo(CurrentUrl));
            await _page.Locator("e-input").Filter(new() { HasText = "Unesite novu lozinku" }).GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("e-input").Filter(new() { HasText = "Ponovite novu lozinku" }).GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("e-input").Filter(new() { HasText = "Unesite novu lozinku Obavezno" }).GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("e-input").Filter(new() { HasText = "Unesite staru lozinku" }).GetByRole(AriaRole.Textbox).ClickAsync();

            //await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();
            await _page.Locator(".ico-ams-logo").ClickAsync();
            await _page.WaitForURLAsync(PocetnaStrana + "/Dashboard");
            CurrentUrl = _page.Url;
            Assert.That(PocetnaStrana + "/Dashboard", Is.EqualTo(CurrentUrl));

            await _page.Locator(".korisnik").ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "Odjavljivanje" }).ClickAsync();
            await _page.WaitForURLAsync(PocetnaStrana + "/Login");
            CurrentUrl = _page.Url;
            Assert.That(PocetnaStrana + "/Login", Is.EqualTo(CurrentUrl));

            if (NacinPokretanjaTesta == "ručno")
            {
                OsiguranjeVozila.PorukaKrajTesta();
            }
            //await _page.ScreenshotAsync(new PageScreenshotOptions { Path = $"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\screenshot_{TestContext.CurrentContext.Test.Name}_{DateTime.Now.ToString("yyyy-MM-dd")}.png" });

            Assert.Pass();

        }


        [Test]
        public async Task UcitavanjeStranice()
        {
            //await _page!.PauseAsync();



            await OsiguranjeVozila.NovaPolisa(_page!, "Nova polisa - lom");
            await _page!.PauseAsync();
            await ProveriURL(_page, PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");

            // Proveri trenutni URL
            var currentUrl = _page.Url;

            if (currentUrl == PocetnaStrana + "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0")
            {
                Console.WriteLine("URL je ispravan.");
            }
            else
            {
                Console.WriteLine($"URL nije ispravan. Trenutni URL: {currentUrl}");
            }

            // Sačekaj da se stranica učita
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            /*********************************
                        await _page.WaitForNavigationAsync(new PageWaitForNavigationOptions
                        {
                            WaitUntil = WaitUntilState.DOMContentLoaded
                        });
            ******************************************************************/
            // Sačekaj da DOM bude učitan
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // Opcionalno: Proveri da li je stranica vidljiva ili da sadrži neki ključni element
            var element = await _page.QuerySelectorAsync("neki lokator");

            if (element != null)
            {
                Console.WriteLine("Stranica se učitala i element je pronađen.");
            }
            else
            {
                Console.WriteLine("Stranica se učitala, ali element nije pronađen.");
            }
            //await _page.PauseAsync();
            //await NovaPolisa(_page, "Nova polisa - lom");
            //await ProveriURL(_page, Variables.PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");



        }


        [Test]
        public async Task CitajAtribute()
        {
            //await NovaPolisa(_page, "Nova polisa OP");
            await OsiguranjeVozila.NovaPolisa(_page!, "Novi zeleni karton");
            //await NovaPolisa(_page, "Nova polisa - lom");
            //await ProveriURL(_page, Variables.PocetnaStrana, "/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/0");
            await _page!.PauseAsync();
            string mojElement = "//e-select[@id='selPartner']";


            // Pronađi element koristeći XPath
            var element = await _page.QuerySelectorAsync(mojElement);

            if (element != null)
            {
                Console.WriteLine("Element je pronađen.");

                // Izvrši JavaScript kod da prikaže sve atribute elementa
                var attributeNames = await element.EvaluateAsync<string[]>(@"(el) => Array.from(el.attributes).map(attr => attr.name)");

                // Proveri da li se imena atributa vraćaju
                if (attributeNames != null && attributeNames.Length > 0)
                {
                    Console.WriteLine($"Broj atributa: {attributeNames.Length}");

                    // Sada možemo da dobijemo vrednosti tih atributa
                    foreach (var attributeName in attributeNames)
                    {
                        var attributeValue = await element.EvaluateAsync<string>($@"(el, attrName) => el.getAttribute(attrName)", attributeName);
                        Console.WriteLine($"Atribut: {attributeName}, Vrednost: {attributeValue}");
                    }
                }
                else
                {
                    Console.WriteLine("Nema atributa ili selektor nije tačan.");
                }
            }
            else
            {
                Console.WriteLine("Element nije pronađen.");
            }


            if (element != null)
            {
                Console.WriteLine("Element je pronađen.");

                // Izvrši JavaScript kod da prikaže sve atribute elementa
                var attributeNames = await element.EvaluateAsync<string[]>(@"(el) => Array.from(el.attributes).map(attr => attr.name)");

                // Proveri da li se imena atributa vraćaju
                if (attributeNames != null && attributeNames.Length > 0)
                {
                    Console.WriteLine($"Broj atributa: {attributeNames.Length}");

                    // Sada možemo da dobijemo vrednosti tih atributa i dodamo redni broj
                    for (int i = 0; i < attributeNames.Length; i++)
                    {
                        var attributeValue = await element.EvaluateAsync<string>($@"(el, attrName) => el.getAttribute(attrName)", attributeNames[i]);
                        Console.WriteLine($"Redni broj: {i + 1}, Atribut: {attributeNames[i]}, Vrednost: {attributeValue}");
                    }
                }
                else
                {
                    Console.WriteLine("Nema atributa ili selektor nije tačan.");
                }
            }
            else
            {
                Console.WriteLine("Element nije pronađen.");
            }

            if (element != null)
            {
                // Proveri da li je element onemogućen
                var isDisabled = await element.EvaluateAsync<bool>("el => el.disabled");

                if (isDisabled)
                {
                    Console.WriteLine($"Element je onemogućen (disabled). IsDisabled ima vrednost:: {isDisabled}.");
                }
                else
                {
                    Console.WriteLine($"Element je omogućen (enabled). IsDisabled ima vrednost::  {isDisabled}.");
                }
            }
            else
            {
                Console.WriteLine("Element nije pronađen.");
            }

            if (element != null)
            {
                // Proveri da li je element vidljiv
                var isVisible = await element.EvaluateAsync<bool>("el => !!(el.offsetWidth || el.offsetHeight || el.getClientRects().length)");

                if (isVisible)
                {
                    Console.WriteLine("Element je vidljiv.");
                }
                else
                {
                    Console.WriteLine("Element nije vidljiv.");
                }
            }
            else
            {
                Console.WriteLine("Element nije pronađen.");
            }




            // Evaluiraj JavaScript kod koji čita sve atribute
            /*  var attributes = await element.EvaluateAsync<Dictionary<string, string>>(@"(el) => {
        const attrs = {};
        for (let i = 0; i < el.attributes.length; i++) {
          const attr = el.attributes[i];
          attrs[attr.name] = attr.value;
        }
        return attrs;
        }");*/
            /* 
             var attributes = await element.EvaluateAsync<Dictionary<string, string>>(@"(el) => {
                 const attrs = {};
                 for (let attr of el.getAttributeNames()) {
                     attrs[attr] = el.getAttribute(attr);
                 }
                 return attrs;
             }");

             //
         Console.WriteLine($"Broj atributa: {attributes.Count}");
                         //Proveri da li ima atributa
                         if (attributes != null && attributes.Count > 0)
                         {
                             // Ispiši sve atribute
                             foreach (var attribute in attributes)
                             {
                                 Console.WriteLine($"Atributi su: {attributes}");
                                 Console.WriteLine("Element jeste pronađen.");
                                 Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                             }

                         }
                         else
                         {
                             Console.WriteLine("Element nije pronađen.");
                         }
                     }
                     //Console.WriteLine("Element nije pronađen.");


                     /*
                     // Pronađi element koristeći XPath selektor
                     var elementAttributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
                         mojElement, 
                         @"(element) => {
                             const attributes = {};
                             for (let i = 0; i < element.attributes.length; i++) {
                                 const attr = element.attributes[i];
                                 attributes[attr.name] = attr.value;
                             }
                             return attributes;
                         }"
                     );

                     // Proveri da li element ima atribute
                     if (elementAttributes != null && elementAttributes.Count > 0)
                     {
                         // Iteriraj kroz sve atribute i ispiši ih
                         foreach (var attribute in elementAttributes)
                         {
                             Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                         }
                     }
                     else
                     {
                         Console.WriteLine("Nema atributa ili selektor nije tačan.");
                     }
                     */
            /*
                        // Pronađi element koristeći XPath
                        var elementAttributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
                            mojElement,
                            @"el => {
                    const attrs = {};
                    for (let attr of el.attributes) {
                        attrs[attr.name] = attr.value;
                    }
                    return attrs;
                }"
                        );

                        // Proveri da li je element pronađen i da li ima atribute
                        if (elementAttributes != null && elementAttributes.Count > 0)
                        {
                            // Iteriraj kroz sve atribute i ispiši ih
                            foreach (var attribute in elementAttributes)
                            {
                                Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nema atributa ili selektor nije tačan.");
                        }

            */

            /* Čitanje celog HTML sadržaja
            // Dohvati HTML sadržaj elementa koristeći XPath
            var elementHtml = await _page.EvalOnSelectorAsync<string>(
                mojElement,
                "el => el.outerHTML"
                );
            // Proveri da li je element pronađen
            if (!string.IsNullOrEmpty(elementHtml))
                {
                            // Definiši putanju do fajla gde ćeš sačuvati podatke
                            string filePath1 = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/element-data.txt";

                            // Zapiši HTML sadržaj u fajl
                            await File.WriteAllTextAsync(filePath1, elementHtml);

                            Console.WriteLine("Podaci su uspešno zapisani u fajl.");
                        }
                        else
                        {
                            Console.WriteLine("Element nije pronađen.");
                        }

                        // Pročitaj specifične atribute
                        var idAttr = await _page.GetAttributeAsync(mojElement, "id");
                        var dataSourceAttr = await _page.GetAttributeAsync(mojElement, "data-source-id");
                        var valueAttr = await _page.GetAttributeAsync(mojElement, "value");

                        // Pročitaj unutrašnji tekst (npr. "Novi Grad")
                        var innerText = await _page.EvalOnSelectorAsync<string>(
                            mojElement + "//span[@class='seltext']",
                            "el => el.textContent"
                        );

                        // Zapiši atribute i tekst u fajl
                        string filePath = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/element-data-neki.txt";
                        string content = $"ID: {idAttr}\nData-Source-ID: {dataSourceAttr}\nValue: {valueAttr}\nInner Text: {innerText}";
                        await File.WriteAllTextAsync(filePath, content);

                        Console.WriteLine("Atributi i tekst su uspešno zapisani u fajl.");


            */
            /*
                        // Pročitaj sve atribute elementa koristeći XPath selektor
                        var elementAttributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
                                            mojElement,

                            @"el => {
                    const attrs = {};
                    for (let attr of el.attributes) {
                        attrs[attr.name] = attr.value;
                    }
                    return attrs;
                }"
                        );
                        // Proveri da li je element pronađen i da li ima atribute
                        if (elementAttributes != null && elementAttributes.Count > 0)
                        {
                            // Iteriraj kroz sve atribute i ispiši ih
                            foreach (var attribute in elementAttributes)
                            {
                                Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                            }

                            // Opcionalno: zapiši atribute u fajl
                            string filePath3 = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/element-data-svi.txt";
                            using (StreamWriter writer = new StreamWriter(filePath3))
                            {
                                foreach (var attribute in elementAttributes)
                                {
                                    writer.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                                }
                            }

                            Console.WriteLine("Atributi su uspešno zapisani u fajl.");
                        }
                        else
                        {
                            Console.WriteLine("Nema atributa ili selektor nije tačan.");
                        }

            */
            /*
            // Pročitaj sve atribute elementa koristeći XPath selektor
            var elementAttributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
                mojElement, 
                @"el => {
                    const attrs = {};
                    // Prolazi kroz sve atribute elementa
                    for (let i = 0; i < el.attributes.length; i++) {
                        attrs[el.attributes[i].name] = el.attributes[i].value;
                    }
                    return attrs;
                }"
            );

            // Proveri da li je element pronađen i da li ima atribute
            if (elementAttributes != null && elementAttributes.Count > 0)
            {
                // Iteriraj kroz sve atribute i ispiši ih
                foreach (var attribute in elementAttributes)
                {
                    Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                }

                // Opcionalno: zapiši atribute u fajl
                string filePath5 = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/element-data-noviiii.txt";
                using (StreamWriter writer = new StreamWriter(filePath5))
                {
                    foreach (var attribute in elementAttributes)
                    {
                        writer.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                    }
                }

                Console.WriteLine("Atributi su uspešno zapisani u fajl.");
            }
            else
            {
                Console.WriteLine("Nema atributa ili selektor nije tačan.");
            }

            */

            /*    
                  //await _page.Locator(mojElement).ClickAsync();
                  var elementPostoji = await _page.QuerySelectorAsync(mojElement);


                  if (elementPostoji != null)
                  {
                      MessageBox.Show($"Element postoji: {mojElement}.\n{elementPostoji}", "Informacija", MessageBoxButtons.OK);
                  }



                  // Prvo, pročitaj vrednost klase elementa
                  var elementClass = await _page.EvalOnSelectorAsync<string>(
                      mojElement,
                      "el => el.className"
                  );
                  //Console.WriteLine($"Klasa elementa: {elementClass}");
                  MessageBox.Show($"Klasa elementa je: -{elementClass}-.", "Informacija", MessageBoxButtons.OK);
                  await _page.PauseAsync();
                  // Zatim, pročitaj id elementa
                  var elementId = await _page.EvalOnSelectorAsync<string>(
                      mojElement,
                      "el => el.id"
                  );
                  //Console.WriteLine($"Visina elementa: {elementId}px");
                  MessageBox.Show($"Id elementa je: -{elementId}-.", "Informacija", MessageBoxButtons.OK);




                  // Pročitaj sva CSS svojstva elementa koristeći getComputedStyle
                  var elementStyles = await _page.EvalOnSelectorAsync<Dictionary<string, string>>
                  (
                      mojElement,
                      @"el => {
                                  const styles = window.getComputedStyle(el);
                                  const result = {};
                                  for (let i = 0; i < styles.length; i++) 
                                  {
                                  const propertyName = styles[i];
                                  result[propertyName] = styles.getPropertyValue(propertyName);
                                  }
                                  return result;
                              }"
                  );

                  // Iteriraj kroz sva svojstva i ispiši ih
                  if (elementStyles != null && elementStyles.Count > 0)
                  {
                      foreach (var style in elementStyles)
                      {
                          Console.WriteLine($"Svojstvo: {style.Key}, Vrednost: {style.Value}");
                      }
                  }
                  else
                  {
                      Console.WriteLine("Nema stilova ili selektor nije tačan.");
                  }




        */
            //var pageContent = await _page.ContentAsync();
            //Console.WriteLine(pageContent);
            /*
             var attributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
        "id=selPartner",
        "el => Array.from(el.attributes).reduce((acc, attr) => { acc[attr.name] = attr.value; return acc; }, {})"
        );
             await _page.PauseAsync();
             // Iteriraj kroz sve atribute i ispiši ih
             foreach (var attribute in attributes)
             {
                 //Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                 //MessageBox.Show($"Atribut: {attribute.Key}. Vrednost: {attribute.Value}", "Informacija", MessageBoxButtons.OK);
                 MessageBox.Show($"Atribut:. Vrednost:", "Informacija", MessageBoxButtons.OK);
             }

             */



            /* 
                     var elementExists = await _page.QuerySelectorAsync(mojElement);

                     if (elementExists == null)
                     {
                         //Console.WriteLine("Element nije pronađen na stranici. Proveri selektor.");
                         MessageBox.Show($"Element {mojElement} \nnije pronađen na stranici. Proveri selektor.", "Informacija", MessageBoxButtons.OK);
                     }
                     else
                     {
                         MessageBox.Show($"Element {mojElement} \nje pronađen na stranici.", "Informacija", MessageBoxButtons.OK);
                         // Ako element postoji, dohvati atribute
                         var attributes = await _page.EvalOnSelectorAsync<Dictionary<string, string>>(
                             mojElement,
                             @"el => {
                     const attrs = el.attributes;
                     const result = {};
                     for (let i = 0; i < attrs.length; i++) {
                         result[attrs[i].name] = attrs[i].value;
                     }
                     return result;
                 }"
                         );
                         MessageBox.Show($"{attributes}\n{attributes.Count}", "Informacija", MessageBoxButtons.OK);
                         // Proveri da li su atribute vraćeni i iteriraj kroz njih
                         if (attributes != null && attributes.Count > 0)
                         {
                             foreach (var attribute in attributes)
                             {
                                 //Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");

                             }
                         }
                         else
                         {
                             Console.WriteLine("Element nema atribute ili selektor nije tačan.");
                         }
                     }
                   */
        }


        [Test]
        public async Task ProveriStampu()
        {
            // Otvorite stranicu sa gridom
            await _page!.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Pregled-dokumenata");

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu
            string column3Text = "";
            int rowIndex = 0; // Kreće od 1, ako želiš da redni broj počinje od 1
                              // Izaberi (pronađi) sve redove u tabeli
            var rows = await _page.QuerySelectorAllAsync("div.podaci div.row.grid-row.row-click");

            System.Windows.Forms.MessageBox.Show($"Redova ima:: {rows.Count}", "Informacija", MessageBoxButtons.OK);

            foreach (var row in rows)
            {
                // Inkrementiraj brojač
                rowIndex++;
                // Definiši brojač

                var valueInColumn12 = await row.QuerySelectorAsync("div.column_12");
                //MessageBox.Show($"Kolona 12 Value:: {valueInColumn12}", "Informacija", MessageBoxButtons.OK);
                string column12Text;
                if (valueInColumn12 != null)
                {
                    column12Text = await valueInColumn12.EvaluateAsync<string>("el => el.innerText");
                }
                else
                {
                    column12Text = "";
                }

                System.Windows.Forms.MessageBox.Show($"U redu {rowIndex} Kolona 12 Status:: {column12Text}", "Informacija", MessageBoxButtons.OK);



                if (column12Text == "Kreiran")
                {

                    // Pročitaj vrednost iz kolone 1
                    var valueInColumn1 = await row.QuerySelectorAsync("div.column_1");
                    string column1Text;
                    if (valueInColumn1 != null)
                    {
                        column1Text = await valueInColumn1.EvaluateAsync<string>("el => el.innerText");
                    }
                    else
                    {
                        column1Text = "";
                    }
                    System.Windows.Forms.MessageBox.Show($"U redu {rowIndex} Kolona 1 Tekst:: {column1Text}", "Informacija", MessageBoxButtons.OK);

                    //Pročitaj vrednost iz kolone 3
                    var valueInColumn3 = await row.QuerySelectorAsync("div.column_3");
                    //string column3Text;
                    if (valueInColumn3 != null)
                    {
                        column3Text = await valueInColumn3.EvaluateAsync<string>("el => el.innerText");
                    }
                    else
                    {
                        column3Text = "";
                    }
                    //MessageBox.Show($":: {column3Text}", "Informacija", MessageBoxButtons.OK);
                    System.Windows.Forms.MessageBox.Show($"U redu {rowIndex} Kolona 3 Tekst:: {column3Text}", "Informacija", MessageBoxButtons.OK);



                    break; // Prekinite petlju nakon pronalaska prvog reda

                }
            }

            //await _page.GetByText(column3Text).ClickAsync();
            //await _page.GetByText("48001").ClickAsync();
            //await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/5153");// ne otvara stranicu
            //await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/5256");// ima grešku, ne kreira štampu
            await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/7/Lom-stakla-auto-nezgoda/Dokument/5336");// dobra štampa
                                                                                                                                      // Postavi maksimalno vreme čekanja (33 sekunde)

            await _page.PauseAsync();

            var timeout = TimeSpan.FromSeconds(10);

            // Izmeri početak vremena
            var startTime = DateTime.Now;

            try
            {
                // Pokreni radnju klika i čekaj na otvaranje novog taba (popup prozora)
                var popupTask = _page.RunAndWaitForPopupAsync(async () =>
                {
                    await _page.Locator("#btnStampajPolisu button").ClickAsync();
                });

                // Koristi Task.WhenAny da čekaš ili otvaranje taba ili istek vremena
                var completedTask = await Task.WhenAny(popupTask, Task.Delay(timeout));

                if (completedTask == popupTask)
                {
                    // Tab je otvoren
                    var page1 = await popupTask;

                    // Proveri koliko je vremena prošlo
                    var elapsedTime = DateTime.Now - startTime;

                    // Ispiši poruku o vremenu otvaranja taba
                    System.Windows.Forms.MessageBox.Show($"Tab je otvoren za {elapsedTime.TotalSeconds} sekundi.", "Informacija", MessageBoxButtons.OK);
                    //Console.WriteLine($"Tab je otvoren za {elapsedTime.TotalSeconds} sekundi.");

                    var pageContent = await page1.ContentAsync();

                    // Proveri da li stranica sadrži određeni tekst
                    if (pageContent.Contains("HTTP ERROR"))
                    {
                        //Console.WriteLine("Tekst je pronađen na stranici.");
                        System.Windows.Forms.MessageBox.Show($"Stranica ima GREŠKU!.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //Console.WriteLine("Tekst nije pronađen na stranici.");
                        System.Windows.Forms.MessageBox.Show($"Stranica nema grešku.", "Informacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // Zatvori tab
                    await page1.CloseAsync();
                }
                else
                {
                    // Proveri koliko je vremena prošlo
                    var elapsedTime = DateTime.Now - startTime;
                    // Tab nije otvoren unutar 33 sekunde
                    //Console.WriteLine("Tab nije otvoren u zadatom vremenu (33 sekunde).");
                    System.Windows.Forms.MessageBox.Show($"Tab nije otvoren za {elapsedTime.TotalSeconds} sekundi.", "Informacija", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Došlo je do greške: {ex.Message}");
            }
            /*       
                   var page1 = await _page.RunAndWaitForPopupAsync(async () =>
                       {
                           await _page.Locator("#btnStampajPolisu button").ClickAsync();
                       });
                   await _page.PauseAsync();
                   await page1.CloseAsync();
                   var page2 = await _page.RunAndWaitForPopupAsync(async () =>
                       {
                           await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu (matr. štam.)" }).ClickAsync();
                       });
                   await _page.PauseAsync();
                   await page2.CloseAsync();
                   var page3 = await _page.RunAndWaitForPopupAsync(async () =>
                       {
                           await _page.Locator("button").Filter(new() { HasText = "Štampaj uplatnicu/fakturu" }).ClickAsync();
                       });
                   await _page.PauseAsync();
                   await page3.CloseAsync();
                   await _page.PauseAsync();
                   */
        }


        [Test]
        public async Task _TestTestova()
        {
            DbProviderFactories.RegisterFactory("System.Data.OleDb", System.Data.OleDb.OleDbFactory.Instance);

            var factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            foreach (var name in System.Data.OleDb.OleDbEnumerator.GetRootEnumerator())
            {
                Console.WriteLine(name);
            }
            try
            {
                await _page!.PauseAsync();
                //await OsiguranjeVozila.UlogujSe(_page, AkorisnickoIme, Alozinka);
                await OsiguranjeVozila.IzlogujSe(_page!);
                await _page!.PauseAsync();


                await OsiguranjeVozila.UlogujSe_2(_page, "BackOffice");
                await _page!.PauseAsync();
                System.Windows.MessageBox.Show("", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //Trace.Write($"////////Provera da li ima 404 ");
                await _page.GotoAsync("https:master-test.ams.co.rs/BackOffice/BackOffice/1/Putno-osiguranje/3894");

                //await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/BackOffice/BackOffice/1/Putno-osiguranje/4053");

                //await ProveriStampu404(_page, "Štampaj polisu", "Štampa polse DZO: ");
                //await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/Osiguranje-vozila/1/Autoodgovornost/Dokument/6978");
                //await _page.PauseAsync();
                //await ProveriStampuPdf(_page, "Štampaj polisu", "Štampa polse AO: ");
                //await ProveriStampuPdf(_page, "Štampaj uplatnicu/fakturu", "Štampa uplatnice AO: ");
                //await _page.Locator("button").Filter(new() { HasText = "Dodatne opcije" }).ClickAsync();
                //await ProveriStampuPdf(_page, "Štampaj prepis polise", "Štampa prepisa polise AO: ");

                /*
                                var pageStampa = await _page.RunAndWaitForPopupAsync(async () =>
                                {
                                    await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                                });

                                if (pageStampa.Url.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine("Otvoren je PDF dokument.");
                                }
                                else
                                {
                                    Console.WriteLine("Nije otvoren PDF dokument.");
                                    Trace.WriteLine($"nije OK");
                                    return;
                                }
                                await pageStampa.CloseAsync();
                */
                //Trace.WriteLine($"OK");

                //string NazivTekucegTesta = TestContext.CurrentContext.Test.Name;


                Console.WriteLine($"******************** RadniFolder:            {RadniFolder}");
                //Console.WriteLine($"******************** RadniFolder 2:          {RadniFolder2}");
                Console.WriteLine($"******************** ProjektFolder:          {ProjektFolder}");
                //Console.WriteLine($"******************** projektDir:             {projektDir}");    
                Console.WriteLine($"******************** logFolder:              {LogovanjeTesta.LogFolder}");
                //Console.WriteLine($"******************** logFolder2:             {logFolder2}");
                Console.WriteLine($"******************** logFajlSumarni:         {LogovanjeTesta.LogFajlSumarni}");
                Console.WriteLine($"******************** logTrace:               {LogovanjeTesta.LogFajlTrace}");
                Console.WriteLine($"******************** logFajlOpsti:           {LogovanjeTesta.LogFajlOpsti}");
                Console.WriteLine($"******************** putanjaDoBazeIzvestaja: {LogovanjeTesta.PutanjaDoBazeIzvestaja}");
                Console.WriteLine($"******************** Test:                   {NazivTekucegTesta}");

            }
            catch (Exception ex)
            {
                string kontekst = $"Greška u testu {NazivTekucegTesta}.";

                LogovanjeTesta.LogException(ex, kontekst);
                LogovanjeTesta.LogTestResult(NazivTekucegTesta, false);
                //Assert.Fail("Došlo je do greške: " + ex.Message);
                throw;
            }

            await OsiguranjeVozila.IzlogujSe(_page);
            await ProveriURL(_page, PocetnaStrana, "/Login");
            await _page.PauseAsync();
            // Vrednosti koje unosimo
            string Prom1 = DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss.fffffff");
            string Prom2 = NazivTekucegTesta;
            int Prom3 = 423;

            // SQL upit
            string sqlUpit = "INSERT INTO Tabela1 (DatumVreme, NazivTesta, Kolona3) VALUES (?, ?, ?)";

            try
            {
                using (OleDbConnection konekcija = new OleDbConnection(LogovanjeTesta.ConnectionStringLogovi))
                {
                    konekcija.Open();

                    using (OleDbCommand komanda = new OleDbCommand(sqlUpit, konekcija))
                    {
                        // Dodajemo parametre u SQL upit
                        komanda.Parameters.AddWithValue("?", Prom1);
                        komanda.Parameters.AddWithValue("?", Prom2);
                        komanda.Parameters.AddWithValue("?", Prom3);

                        // Izvršavamo upit
                        int brojRedova = komanda.ExecuteNonQuery();

                        Console.WriteLine($"✅ Uspešno ubačeno {brojRedova} redova u bazu.");
                        LogovanjeTesta.LogMessage($"✅ Uspešno ubačeno {brojRedova} redova u bazu.", false);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                await LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }

            Console.WriteLine($"{PocetnaStrana} - {PocetnaStrana}");





            //await _page.GotoAsync("https://razvojamso-master.eonsystem.rs/BackOffice/BackOffice/1/Putno-osiguranje/3984");

            //await ProveriStampu(_page, "Štampaj polisu", "");
            /********************************         
                     var pageStampa = await _page.RunAndWaitForPopupAsync(async () =>
                     {
                         await _page.Locator("button").Filter(new() { HasText = "Štampaj polisu" }).ClickAsync();
                     });

                     try
                     {
                         Assert.That(pageStampa.Url.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase), Is.True, "Greška posle klika na Štampaj. Nije otvoren pdf dokument.");
                     }
                     catch (Exception ex)
                     {
                         // Upisivanje u fajl
                         File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {ex.Message}{Environment.NewLine}");

                         // Sa throw se test prekida, bez throw se test nastavlja.
                         //throw;
                     }


           ******************************/
            //await pageStampa.CloseAsync();
            //await page1.GetByText("HTTP ERROR").ClickAsync();
            //await page1.Locator("text=HTTP ERROR 404").ClickAsync();

            //await _page.PauseAsync();
            //await _page.GotoAsync("https://razvojamso-backoffice.eonsystem.rs/Printouts/59e680b2-8675-4343-b26d-a847614da6b3_2/Isprava%20o%20zaklju%C4%8Denom%20ugovoru%20dobrovoljnog%20zdravstvenog%20osiguranja.pdf?rnd=0.19541805081592645");

            /******************************************           
                       var errorElement = pageStampa.Locator("text=HTTP ERROR 404");
                       try
                       {
                           Assert.That(await errorElement.IsHiddenAsync(), Is.True, "Štampa ne radi. 'HTTP ERROR 404' je vidljiv na stranici.");
                       }
                       catch (AssertionException ex)
                       {
                           // Upisivanje u fajl
                           File.AppendAllText($"C:\\_Projekti\\AutoMotoSavezSrbije\\Logovi\\test_log_AO1.txt", $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} - {ex.Message}{Environment.NewLine}");

                           // Sa throw se test prekida, bez throw se test nastavlja.
                           throw;
                       }

           **************************/
            await _page.PauseAsync();
            //await _page.GotoAsync("https://razvojamso-backoffice.eonsystem.rs/Printouts/59e680b2-8675-4343-b26d-a847614da6b3_2/Isprava%20o%20zaklju%C4%8Denom%20ugovoru%20dobrovoljnog%20zdravstvenog%20osiguranja.pdf?rnd=0.19541805081592645");           
            //var errorElement = _page.Locator("text=HTTP ERROR 404");
            //Assert.That(await errorElement.IsVisibleAsync(), Is.False, "Tekst 'HTTP ERROR 404' nije vidljiv na stranici.");

            //var response = await _page.GotoAsync("your-url-here");
            //Assert.That(response.Status, Is.EqualTo(404), "Stranica ne vraća očekivani status 404.");
            //string path = AppContext.BaseDirectory;


            await OsiguranjeVozila.IzlogujSe(_page);
            await ProveriURL(_page, PocetnaStrana, "/Login");
            if (NacinPokretanjaTesta == "ručno")
            {
                OsiguranjeVozila.PorukaKrajTesta();
            }

        }


        public static IEnumerable<string> GetTestUrls()
        {
            string urls = TestContext.Parameters.Get("BaseUrls", "https://razvojamso-master.eonsystem.rs");
            return urls.Split(',').Select(url => url.Trim());
        }


        [Test, TestCaseSource(nameof(GetTestUrls))]
        public async Task _TestTestova_radni_2(string NacinPokretanjaTesta)
        {
            await _page!.PauseAsync();
            Console.WriteLine("BaseUrls iz TestContext.Parameters: " + TestContext.Parameters.Get("BaseUrls", "Nema podataka"));
            await _page!.GotoAsync(NacinPokretanjaTesta);
            //Trace.WriteLine("Recording a trace: TestMethod1 started");
            await _page.PauseAsync();
            await OsiguranjeVozila.IzlogujSe(_page);
            await ProveriURL(_page, PocetnaStrana, "/Login");
            //Trace.WriteLine("Recording a trace: TestMethod1 completed successfully");
            OsiguranjeVozila.PorukaKrajTesta();
            //await _page.PauseAsync();
            //return;
        }





        [Test]
        public async Task _0TestTestova()
        {

            try
            {

                //await Pauziraj(_page!);
                if (_page == null)
                    throw new ArgumentNullException(nameof(_page), "_page cannot be null before calling UlogujSe.");
                //await OsiguranjeVozila.UlogujSe(_page, AkorisnickoIme, Alozinka);




                System.Windows.MessageBox.Show("", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                string PocetnaStrana = "https://razvojamso-master.eonsystem.rs/BackOffice/BackOffice/1/Dashboard";
                await ProveriURL(_page, PocetnaStrana, "/Dashboard");



                await OsiguranjeVozila.IzlogujSe(_page!);
                await _page!.PauseAsync();
                await OsiguranjeVozila.UlogujSe_2(_page, "Agent");

                await _page!.PauseAsync();
                System.Windows.MessageBox.Show("", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                DbProviderFactories.RegisterFactory("System.Data.OleDb", System.Data.OleDb.OleDbFactory.Instance);

                var factory = DbProviderFactories.GetFactory("System.Data.OleDb");
                foreach (var name in System.Data.OleDb.OleDbEnumerator.GetRootEnumerator())
                {
                    Console.WriteLine(name);
                }




                Console.WriteLine($"******************** RadniFolder:            {RadniFolder}");
                //Console.WriteLine($"******************** RadniFolder 2:          {RadniFolder2}");
                Console.WriteLine($"******************** ProjektFolder:          {ProjektFolder}");
                //Console.WriteLine($"******************** projektDir:             {projektDir}");    
                Console.WriteLine($"******************** logFolder:              {LogovanjeTesta.LogFolder}");
                //Console.WriteLine($"******************** logFolder2:             {logFolder2}");
                Console.WriteLine($"******************** logFajlSumarni:         {LogovanjeTesta.LogFajlSumarni}");
                Console.WriteLine($"******************** logTrace:               {LogovanjeTesta.LogFajlTrace}");
                Console.WriteLine($"******************** logFajlOpsti:           {LogovanjeTesta.LogFajlOpsti}");
                Console.WriteLine($"******************** putanjaDoBazeIzvestaja: {LogovanjeTesta.PutanjaDoBazeIzvestaja}");
                Console.WriteLine($"******************** Test:                   {NazivTekucegTesta}");






                #region Sertifikat

                var process = Process.GetProcessesByName(AppName).FirstOrDefault();
                if (process != null)
                {
                    //Ako je aplikacija pokrenuta, pridruži se postojećem procesu
                    _application = FlaUI.Core.Application.Attach(process);
                    Console.WriteLine($"Aplikacija '{AppName}' je već pokrenuta.");
                }
                else
                {
                    // Ako aplikacija nije pokrenuta, pokreni je
                    Console.WriteLine($"Aplikacija '{AppName}' nije pokrenuta.");
                    _application = FlaUI.Core.Application.Launch(AppPath);
                }

                await _page!.PauseAsync();

                // Inicijalizacija FlaUI
                _automation = new UIA3Automation();
                // Dohvatanje glavnog prozora aplikacije
                var mainWindow = _application.GetMainWindow(_automation);

                // Provera da li je mainWindow null
                if (mainWindow == null)
                {
                    throw new Exception("Main window of the application was not found.");
                }

                //Pronalazak TreeView elementa
                var treeView = mainWindow.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Tree))?.AsTree();
                //Assert.IsNotNull(treeView, "TreeView not found");

                // Pronalazak TreeItem sa tekstom "Petrović Petar"
                //var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName("Bogdan Mandarić 200035233"))).AsTreeItem();
                var SertifikatName_ = KorisnikLoader5.Korisnik3?.Sertifikat ?? string.Empty;
                var treeItem = treeView?.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.TreeItem).And(cf.ByName(SertifikatName_))).AsTreeItem();
                //Assert.IsNotNull(treeItem, "TreeItem 'Bogdan Mandarić' not found");

                // Klik na TreeItem
                if (treeItem != null)
                {
                    treeItem.Click();
                }
                else
                {
                    throw new Exception($"TreeItem '{SertifikatName_}' not found.");
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
                if (okButton != null)
                {
                    okButton.Click();
                }
                else
                {
                    throw new Exception("OK button not found in the application window.");
                }

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

                #endregion Sertifikat
                await _page!.PauseAsync();
                //return;

                await OsiguranjeVozila.IzlogujSe(_page);

                await ProveriURL(_page, PocetnaStrana, "/Login");
                await _page.PauseAsync();
                //return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }
        }



        [Test]
        public async Task _TestSvegaISvacega()
        {
            System.Windows.Forms.MessageBox.Show("Test je počeo!");


            //Nova polisa
            await _page!.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");
            await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");

            //Kreirana
            //await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5416");
            //await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5416");

            //U izradi
            //await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5425");
            //await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5425");

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čeka dok se DOM ne učita
            await _page.WaitForLoadStateAsync(LoadState.Load); // Čeka da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu

            //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();

            string kontrola = "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']";
            //string kontrola = "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']";
            await OsiguranjeVozila.ProveriKontrolu(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");
            await OsiguranjeVozila.ProveriKontrolu(_page, "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']");

            await _page.PauseAsync();

            var elementPostoji = await _page.QuerySelectorAsync(kontrola);

            if (elementPostoji != null)
            {
                string kontrolaSkracena1944 = kontrola[19..^44];
                string kontrolaSkracena042 = kontrola[0..^42];


                bool isVisible = await _page.IsVisibleAsync(kontrola);


                string disabledAttr = await _page.Locator(kontrola).GetAttributeAsync("disabled");


                string className1 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.className");

                string onemoguceno1 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.disabled");


                string className2 = await _page.EvalOnSelectorAsync<string>(kontrolaSkracena042, "el => el.className");

                string onemoguceno2 = await _page.EvalOnSelectorAsync<string>(kontrolaSkracena042, "el => el.disabled");

                var innerText = await elementPostoji.EvaluateAsync<string>("el => el.innerText");
                System.Windows.Forms.MessageBox.Show($"elementPostoji = {elementPostoji}\n" +
                                $"isVisible = {isVisible}.\n" +
                                $"disabledAttr = '{disabledAttr}'", $"Element {kontrolaSkracena1944}", MessageBoxButtons.OK);
                //MessageBox.Show($"isVisible = {isVisible}", $"Element {kontrolaSkracena1944}", MessageBoxButtons.OK);
                //MessageBox.Show($"disabledAttr = '{disabledAttr}'", $"Element {kontrolaSkracena1944}", MessageBoxButtons.OK);
                System.Windows.Forms.MessageBox.Show($"Class name1 = {className1}\n" +
                                $"Onemoguceno1 = '{onemoguceno1}'.", $"Element {kontrola}", MessageBoxButtons.OK);
                //MessageBox.Show($"Onemoguceno1 = '{onemoguceno1}'.", $"Element {kontrola}", MessageBoxButtons.OK);
                System.Windows.Forms.MessageBox.Show($"Class name2 = {className2}\n" +
                                $"Onemoguceno2 = '{onemoguceno2}'.", $"Element {kontrolaSkracena042}", MessageBoxButtons.OK);
                //MessageBox.Show($"Onemoguceno2 = '{onemoguceno2}'.", $"Element {kontrolaSkracena042}", MessageBoxButtons.OK);





                //var innerText = await elementPostoji.EvaluateAsync<string>("el => el.innerText");
                Console.WriteLine("innerText: " + innerText);

                var outerHtml = await elementPostoji.EvaluateAsync<string>("el => el.outerHTML");
                Console.WriteLine("outerHTML: " + outerHtml);
                // Ispis svih Properties elementa
                var properties = await elementPostoji.EvaluateAsync<Dictionary<string, object>>("el => Object.entries(el).reduce((props, [key, value]) => { props[key] = value; return props; }, {})");
                // Proveri da li element ima određene property-je koje možeš da pročitaš
                //var properties = await element.EvaluateAsync<Dictionary<string, string>>("el => { let props = {}; for (let key in el) { props[key] = el[key]; } return props; }");
                //string pera = "Pera";

                if (properties != null && properties.Count > 0)
                {

                    foreach (var prop in properties)
                    {
                        System.Windows.Forms.MessageBox.Show($"{prop.Key}: {prop.Value}");
                        //Console.WriteLine($"{prop.Key}: {prop.Value}");
                    }
                    //string mika = "Moka";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Element ne postoji.");
            }
            //await ProveriPadajucuListu(_page, "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']");
            //await ProveriPadajucuListu(_page, "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']");

            //await _page.PauseAsync();
            var id = await elementPostoji.EvaluateAsync<string>("el => el.id");
            Console.WriteLine("ID: " + id);

            var className = await elementPostoji.EvaluateAsync<string>("el => el.className");
            Console.WriteLine("Class Name: " + className);

            var attributes = await elementPostoji.EvaluateAsync<Dictionary<string, string>>(
                "el => Array.from(el.attributes).reduce((acc, attr) => { acc[attr.name] = attr.value; return acc; }, {})"
            );

            if (attributes != null && attributes.Count > 0)
            {
                foreach (var attribute in attributes)
                {
                    Console.WriteLine($"{attribute.Key}: {attribute.Value}");
                }
            }
            else
            {
                Console.WriteLine("Nema atributa za ispisivanje.");
            }

            var properties1 = await elementPostoji.EvaluateAsync<string[]>("el => Object.keys(el)");
            Console.WriteLine("Properties:");
            foreach (var property in properties1)
            {
                var value = await elementPostoji.EvaluateAsync<string>($"el => el['{property}']");
                Console.WriteLine($"{property}: {value}");
                //Console.WriteLine(property);
            }
            var classList = await elementPostoji.EvaluateAsync<string>("el => el.classList.toString()");
            Console.WriteLine("Class List: " + classList);
        }




        [Test]
        public async Task TestKontrolePadajucaLista()
        {
            System.Windows.Forms.MessageBox.Show("Test je počeo!");


            //Nova polisa
            await _page!.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");
            await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/0");

            //Kreirana
            //await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5416");
            //await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5416");

            //U izradi
            //await _page.GotoAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5425");
            //await _page.WaitForURLAsync(PocetnaStrana + "/Osiguranje-vozila/1/Autoodgovornost/Dokument/5425");

            //Čeka učitavanje
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // čeka dok se DOM ne učita
            await _page.WaitForLoadStateAsync(LoadState.Load); // Čeka da se završi celo učitavanje stranice, uključujući sve resurse poput slika i stilova.
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // čeka dok mrežni zahtevi ne prestanu

            //await _page.Locator("button").Filter(new() { HasText = "Izmeni" }).ClickAsync();


            //string kontrola = "//e-select[@id='selTarife']//div[@class='multiselect-dropdown input']";
            //string kontrola = "#selDrustvo";
            //string kontrola = "#selDrustvo1";
            //string kontrola = "#inpSerijskiBrojAO";
            //string kontrola = "#chkStranacU";
            //string kontrola = "//div[@class='checkbox col checked']";
            //string kontrola = "//e-checkbox[@id='chkStranacU']//div[@class='checkbox col']";
            //string kontrola = "//e-checkbox[@id='chkStranacU']";
            //e-checkbox[@id='chkStranacU']
            string kontrola = "#selPartner";
            //string kontrola = "id=selPartner";
            //string kontrola = "//e-select[@id='selPartner']";
            //string kontrola = "//e-select[@id='selPartner']//div[@class='multiselect-dropdown input']";
            //string kontrola = "//div[@class='commonBox font09 div-korisnik']//div[@class='col-4 column']";
            //string kontrola = "//e-select[@id='selPartner']/div[@class='control-wrapper field info-text-field inner-label-field']";
            //string kontrola = "//div[@id='ugovarac']//e-input[contains(.,'JMBG')]";

            var lokator = _page.Locator(kontrola);

            //string nazivKontrole = kontrola[16..^2];
            string nazivKontrole = kontrola[4..^0];

            // Da li postoji element
            var element = await _page.QuerySelectorAsync(kontrola);
            if (element != null)
            {
                Console.WriteLine($"Element {kontrola} postoji.");
            }
            else
            {
                Console.WriteLine($"Element {kontrola} nije pronađen."); return;
            }

            await _page.WaitForSelectorAsync(kontrola);

            await lokator.ClickAsync();
            await lokator.PressAsync("Escape");
            Console.WriteLine($"\nČitanje pojedinačnih atributa elementa: {kontrola}");
            Console.WriteLine("------------------------------------------------------");

            // Pročitaj specifične atribute
            var idAttr = await _page.GetAttributeAsync(kontrola, "id");
            Console.WriteLine($"idAttr: {idAttr}");
            string idKontrole = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.id");
            Console.WriteLine($"idKontrole: {idKontrole}");

            // Dohvati ime čvora
            string nodeIme = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.nodeName");
            Console.WriteLine($"nodeName: {nodeIme}");
            // Dohvati ime HTML taga
            string tagName = await element.EvaluateAsync<string>("el => el.tagName");
            Console.WriteLine($"Tip elementa (Tag): {tagName}");

            // Da li je element vidljiv
            bool isVisible = await _page.IsVisibleAsync(kontrola);
            if (isVisible)
            {
                Console.WriteLine($"Element je vidljiv: {isVisible}");
            }
            else
            {
                Console.WriteLine($"Element nije vidljiv: {isVisible}");
            }

            //Da li je element enable ili disable
            string onemoguceno = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.disabled");
            Console.WriteLine($"Element nije omogućen: {onemoguceno}");

            bool isEnabled = await _page.IsEnabledAsync(kontrola);
            if (isEnabled)
            {
                Console.WriteLine($"Element je omogućen: {isEnabled}");
            }
            else
            {
                Console.WriteLine($"Element je omogućen: {isEnabled}");
            }

            bool isDisabled = await _page.IsDisabledAsync(kontrola);
            if (isDisabled)
            {
                Console.WriteLine($"Element je onemogućen: {isDisabled}");
            }
            else
            {
                Console.WriteLine($"Element je onemogućen: {isDisabled}");
            }

            // Čitanje klase
            string className = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.className");
            if (string.IsNullOrEmpty(className)) // ili if (className.Contains("active")) - da li element ima klasu "active"
            {
                Console.WriteLine($"Element: nema klasu.");
            }
            else
            {
                Console.WriteLine($"Element ima klasu {className}.");
            }

            //Čitanje inner-label na dva načina
            var innerLabel1 = await _page.QuerySelectorAsync(kontrola);
            string dataValue = await innerLabel1.GetAttributeAsync("inner-label");
            Console.WriteLine($"Data atribut inner-label (1. način): " + dataValue);
            string innerLabela2 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.getAttribute('inner-label')");
            Console.WriteLine($"Data atribut inner-label (2. način): " + innerLabela2);

            // Čitanje vrednosti specifičnog `data-*` atributa pomoću `dataset`
            string dataSourceId1 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.dataset.sourceId");
            Console.WriteLine($"data-source-id (1. način) elementa je: {dataSourceId1}");

            var dataSourceId2 = await _page.GetAttributeAsync(kontrola, "data-source-id");
            Console.WriteLine($"data-source-id (2. način) elementa je: {dataSourceId2}");

            string dataSourceId3 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.getAttribute('data-source-id')");
            Console.WriteLine($"data-source-id (3. način) elementa je: {dataSourceId3}");

            var innerText1 = await element.EvaluateAsync<string>("el => el.innerText");
            Console.WriteLine($"innerText (1. način) elementa je: {innerText1}");
            string innerText2 = await _page.InnerTextAsync(kontrola);
            Console.WriteLine($"innerText (2. način) elementa je: {innerText2}");

            var valueAttr = await _page.GetAttributeAsync(kontrola, "value");
            Console.WriteLine($"Value elementa je: {valueAttr}");
            string value1 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.getAttribute('value')");
            Console.WriteLine($"Vrednost atributa 'value': {value1}");

            // Pročitaj unutrašnji tekst (npr. "Novi Grad")
            var textContent = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.textContent");
            Console.WriteLine($"textContent elementa je: postoji");

            string textContent1 = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.textContent.trim()");
            Console.WriteLine($"Prikazana vrednost: {textContent1}");

            // Čitanje vrednosti href:
            string href = await _page.GetAttributeAsync(kontrola, "href");

            // Dohvatanje vrednosti iz unutrašnjeg <input> ili <span>:
            string innerValue = await _page.EvalOnSelectorAsync<string>($"{kontrola} .multiselect-dropdown .selected", "el => el.textContent.trim()");
            Console.WriteLine($"Izabrana vrednost: {innerValue}");

            // Pročitaj visinu elementa
            var elementHeight = await _page.EvalOnSelectorAsync<int>(kontrola, "el => el.offsetHeight");
            Console.WriteLine($"Visina elementa: {elementHeight}px");

            // Zatim, pročitaj širinu elementa
            int elementWidth = await _page.EvalOnSelectorAsync<int>(kontrola, "el => el.offsetWidth");
            Console.WriteLine($"Širina elementa: {elementWidth}px");

            // Provera veličine i pozicije elementa
            var boundingBox = await _page.Locator(kontrola).BoundingBoxAsync();
            if (boundingBox != null)
            {
                Console.WriteLine($"Kontrola ima Width: {boundingBox.Width}, Height: {boundingBox.Height}");
                Console.WriteLine($"Kontrola ima poziciju X = {boundingBox.X}, Y = {boundingBox.Y}");
            }

            string trazi = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.getAttribute('multiselect-search')");
            Console.WriteLine($"multiselect-search: {trazi}");

            bool isFocused = await _page.EvaluateAsync<bool>($@"
                                        (selector) => {{
                                        const el = document.querySelector(selector);
                                        return el === document.activeElement;
                                        }}
                                        ", kontrola);
            Console.WriteLine(isFocused ? "Element je u fokusu." : "Element nije u fokusu.");

            /******************************************************************************
            bool isChecked = await _page.IsCheckedAsync(kontrola);
            if (isChecked)
            {
                Console.WriteLine($"Element {kontrola} je čekirana: {isChecked}");
            }
            else
            {
                Console.WriteLine($"Element {kontrola} nije čekirana: {isChecked}");
            }
            *******************************************************************************/

            // Da li je animiran
            bool isAnimating = await _page.EvalOnSelectorAsync<bool>(kontrola, "el => getComputedStyle(el).animationName !== 'none'");
            Console.WriteLine($"isAnimating je: {isAnimating}");


            // Dohvati HTML sadržaj elementa koristeći XPath
            var elementHtml = await _page.EvalOnSelectorAsync<string>(kontrola, "el => el.outerHTML");
            var outerHtml = await element.EvaluateAsync<string>("el => el.outerHTML");

            if (elementHtml == outerHtml)
            {
                Console.WriteLine("Stringovi elementHtml i outerHtml SU jednaki.");
            }
            else
            {
                Console.WriteLine("Stringovi elementHtml i outerHtml NISU jednaki.");
            }

            //Unutrašnji tag elementa
            string innerHTML = await _page.InnerHTMLAsync(kontrola);
            //Console.WriteLine($"innerHTML: {innerHTML}");
            if (innerHTML == outerHtml)
            {
                Console.WriteLine("Stringovi innerHTML i outerHtml SU jednaki.");
            }
            else
            {
                Console.WriteLine("Stringovi innerHTML i outerHtml NISU jednaki.");
            }
            //ovo je alternatibva
            var nekiHtml = await element.EvaluateAsync<string>("el => el.innerHTML");

            /*******************************************************
            // Proveri da li je element pronađen
            if (!string.IsNullOrEmpty(elementHtml))
            {
                // Definiši putanju do fajla gde ćeš sačuvati podatke
                //string filePath = "C:/putanja/do/fajla/element-data.txt";

                // Zapiši HTML sadržaj u fajl
                //await File.WriteAllTextAsync(filePath, elementHtml);

                //Console.WriteLine("Podaci su uspešno zapisani u fajl.");
                Console.WriteLine($"\n\nOuterHTML elementa: {nazivKontrole} je {elementHtml}");
                Console.WriteLine($"\n\nOuterHTML elementa: {kontrola} je {outerHtml}");

            }
            else
            {
                Console.WriteLine("Element nije pronađen.");
            }
            **********************************************************/

            //string inputValue = await _page.InputValueAsync(kontrola);
            //Console.WriteLine($"inputValue: {inputValue}");

            // Kombinovane informacije
            if (await _page.IsVisibleAsync(kontrola) && await _page.IsEnabledAsync(kontrola))
            {
                Console.WriteLine("Element je vidljiv i omogućen za interakciju.");
            }
            else
            {
                Console.WriteLine("Element nije dostupan za interakciju.");
            }

            Console.WriteLine("\n****************************************************");

            var attributeCount = await element.EvaluateAsync<int>("el => el.attributes.length");
            if (attributeCount == 0)
            {
                Console.WriteLine("Element nema atribute.");
                return;
            }
            else
            {
                Console.WriteLine($"attributeCount: {attributeCount}");
            }

            var brojAtributa = await element.EvaluateAsync<int>(@"(el) => {return el && el.attributes ? el.attributes.length : 0;}");
            if (brojAtributa == 0)
            {
                Console.WriteLine("Element nema atribute.");
                return;
            }
            else
            {
                Console.WriteLine($"brojAtributa: {brojAtributa}");
            }



            // Evaluiraj atribute i vrati ih kao JSON string
            var rawAttributes = await element.EvaluateAsync<string>(@"(el) => {
                                if (!el || !el.attributes) return null;
                                const attrs = {};
                                for (let i = 0; i < el.attributes.length; i++) {
                                const attr = el.attributes[i];
                                attrs[attr.name] = attr.value || ''; // Postavi praznu vrednost ako nema value
                                                                                }
                                return JSON.stringify(attrs); // Vrati kao JSON string
                                                                                }
                                                                                ");

            if (!string.IsNullOrEmpty(rawAttributes))
            {
                Console.WriteLine("\nAtributi elementa:");
                Console.WriteLine("------------------------------------------------------");
                // Parsiranje JSON stringa u Dictionary
                var attributes = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(rawAttributes);

                foreach (var attribute in attributes)
                {
                    Console.WriteLine($"Atribut: {attribute.Key}, Vrednost: {attribute.Value}");
                }
            }
            else
            {
                Console.WriteLine("Nema atributa za prikaz.");
            }


            Console.WriteLine("\nRaw JSON atributa: " + rawAttributes);



            /******************************************************************************/
            Console.WriteLine("\n****************************************************");
            Console.WriteLine("879 Do ovde je dobro!\n");

            /******************************************************************************/



            // Ako je element INPUT, proveri njegov atribut 'type'
            if (tagName == "E-SELECT")
            {
                string dataSource = await element.EvaluateAsync<string>("el => el.getAttribute('data-source-id')");
                string value = await element.EvaluateAsync<string>("el => el.getAttribute('value')");

                Console.WriteLine($"Atribut data-source-id: {dataSource}");
                Console.WriteLine($"Atribut value: {value}");
                string inputType = await element.EvaluateAsync<string>("el => el.type");
                Console.WriteLine($"Tip INPUT elementa: {inputType}");
            }


            if (tagName == "DIV")
            {
                // Provera da li je čekiran (na osnovu atributa)
                bool isChecked = await element.EvaluateAsync<bool>("el => el.hasAttribute('checked')");

                if (isChecked)
                {
                    Console.WriteLine("E-CHECKBOX je označen (čekiran).");
                }
                else
                {
                    Console.WriteLine("E-CHECKBOX nije označen (nije čekiran).");
                }
            }





            // Korišćenje shadowRoot (ako je prisutan):

            string shadowValue = await _page.EvaluateAsync<string>($@"
    () => {{
        const el = document.querySelector('{kontrola}');
        const shadow = el.shadowRoot;
        if (shadow) {{
            const inner = shadow.querySelector('.some-class'); // Prilagodi selektor
            return inner ? inner.textContent.trim() : '';
        }}
        return '';
    }}
");
            Console.WriteLine($"Vrednost iz Shadow DOM-a: {shadowValue}");


            //Provera kroz value property JavaScript-a:
            string jsValue = await _page.EvalOnSelectorAsync<string>(
                kontrola,
                "el => el.value"
            );
            Console.WriteLine($"Vrednost (JavaScript): {jsValue}");






            var options = await _page.Locator($"{kontrola} .option").AllTextContentsAsync();
            Console.WriteLine("Opcije unutar liste:");
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }

            var optionValues = await _page.Locator($"{kontrola} .option").EvaluateAllAsync<string[]>(@"
                            elements => elements.map(el => el.getAttribute('value'))
                        ");
            Console.WriteLine("Vrednosti opcija:");
            foreach (var value in optionValues)
            {
                Console.WriteLine(value);
            }

            var element1 = await _page.QuerySelectorAsync($"xpath=//e-select[@id='selDrustvo']");
            if (element1 == null)
            {
                Console.WriteLine("Element nije pronađen sa XPath-om.");
            }
            else
            {
                Console.WriteLine("Element pronađen sa XPath-om.");
            }

            string[] attributeNames = { "id", "data-source-id", "inner-label", "unset", "multiselect-search",
                                                                "data-attr", "data-level","value", "intid", "tabindex","disabled" };
            foreach (var attributeName in attributeNames)
            {
                var attributeValue = await _page.GetAttributeAsync(kontrola, attributeName);
                Console.WriteLine($"{attributeName}: {attributeValue}");
            }



            var match = Regex.Matches(outerHtml, @"(\w+)=[""'](.*?)[""']");
            foreach (Match m in match)
            {
                Console.WriteLine($"{m.Groups[1].Value}: {m.Groups[2].Value}");
            }




            var shadowRoot = await _page.EvalOnSelectorAsync<IJSHandle>(kontrola, "el => el.shadowRoot");
            if (shadowRoot != null)
            {
                Console.WriteLine("Shadow DOM je prisutan.");
                var shadowAttributes = await _page.EvaluateAsync<Dictionary<string, string>>(@"
                                (kontrola) => {
                                    const el = document.querySelector(kontrola).shadowRoot;
                                    const attributes = {};
                                    Array.from(el.host.attributes).forEach(attr => {
                                        attributes[attr.name] = attr.value;
                                    });
                                    return attributes;
                                }
                            ");

                foreach (var attribute in shadowAttributes)
                {
                    Console.WriteLine($"{attribute.Key}: {attribute.Value}");
                }
            }
            else
            {
                Console.WriteLine("Element ne koristi Shadow DOM.");
            }




            /******************************************************************************/
            Console.WriteLine("\n");
            Console.WriteLine("Ovo se proverava!\n");
            /**********************************************************************************************/





            /******************************************************************************/
            Console.WriteLine("\n");
            Console.WriteLine("Ovo ne daje rezultate!\n");
            /**********************************************************************************************/
            // Parsiranje outerHTML-a
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(outerHtml);

            var node = doc.DocumentNode;
            foreach (var attr in node.Attributes)
            {
                Console.WriteLine($"{attr.Name}: {attr.Value}");
            }


        }



        [Test]
        public async Task _ObrisiLogove()
        {
            await Task.Delay(1);
            string putanja = LogovanjeTesta.LogFolder;  // Ovde unesi svoj folder
            Alati.ObrisiTxtFajlove(putanja);
        }



        [Test]
        [TestCase("https://razvojamso-master.eonsystem.rs")]
        //[TestCase("https://proba2amsomaster.eonsystem.rs")]
        [TestCase("https://master-test.ams.co.rs")]
        //[TestCase("https://eos.ams.co.rs")]
        public async Task _TestTestova_radni_1(string NacinPokretanjaTesta)
        {
            await _page!.PauseAsync();
            await _page.GotoAsync(NacinPokretanjaTesta);
            //Trace.WriteLine("Recording a trace: TestMethod1 started");
            await _page.PauseAsync();
            await OsiguranjeVozila.IzlogujSe(_page);
            await ProveriURL(_page, PocetnaStrana, "/Login");
            //Trace.WriteLine("Recording a trace: TestMethod1 completed successfully");
            OsiguranjeVozila.PorukaKrajTesta();
            //await _page.PauseAsync();
            //return;
        }



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


        [Test]
        public async Task CuvanjeSesije()
        {
            await _page!.PauseAsync();
            //Console.WriteLine("Unesi vrednost za testiranje:");

            // Koristi konzolu da simuliraš unos vrednosti
            //string userInput = "";
            //string userInput = Console.ReadLine();

            // Ispis unete vrednosti
            //Console.WriteLine($"Uneta vrednost je: {userInput}");

            // Dalji testovi sa unetom vrednošću
            //Assert.IsNotNull(userInput, "Vrednost ne sme biti null.");
            //Assert.IsNotEmpty(userInput, "Vrednost ne sme biti prazna.");

            // Čekaj da se učita glavna stranica nakon logovanja
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // Sačuvaj stanje sesije
            var storageState = await _page.Context.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                //Path = "auth_state.json" // Sačuvaj sesiju u fajl
                Path = "C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/auth_state.json"
            });

            //MessageBox.Show("Izaberi", "Info", MessageBoxButtons.YesNoCancel);

        }


        /*        
                [Test]
                public Task ProcitajJson()
                {
                    // Putanja do auth_state.json fajla
                    string filePath = @"C:/_Projekti/AutoMotoSavezSrbije/ulazniPodaci/auth_state.json";

                    // Proveravamo da li fajl postoji
                    if (File.Exists(filePath))
                    {
                        // Učitavamo sadržaj fajla kao string
                        string jsonContent = File.ReadAllText(filePath);

                        // Parsiramo JSON sadržaj u Dictionary
                        var authState = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                        // Iteriramo kroz sve ključeve i vrednosti u auth_state.json
                        foreach (var kvp in authState)
                        {
                            Console.WriteLine($"Ključ: {kvp.Key}, Vrednost: {kvp.Value}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("auth_state.json fajl ne postoji na zadatoj putanji.");
                    }

                    return Task.CompletedTask;
                }

                */


    }
}


