namespace Razvoj
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public partial class WebShop : Osiguranje
    {
        [Test]
        public async Task IndividualnoPutno()
        {
            try
            {
                //izbor Individualnog osiguranja
                await _page!.Locator("a").Filter(new() { HasText = "INDIVIDUALNO PUTNO OSIGURANJE" }).First.ClickAsync();

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno1");
                await ProveriTitlStranice(_page, "Osnovni podaci");

                //unos broja putnika
                await UnosBrojaOdraslih(_page, "1");
                await UnosBrojaDece(_page, "0");
                await UnosBrojaSeniora(_page, "0");

                //unos datuma početka
                await DatumPocetka(_page);

                //unos trajanja
                await UnosTrajanja(_page, "5");

                //Svrha putovanja
                await IzaberiOpcijuIzListe(_page, "#selSvrha", "Doplatak za privremeni rad u inostranstvu", false);

                // Covid 19
                await Covid(_page, "Da");

                // Teritorija Srbije
                await TeritorijaSrbije(_page);

                // Dalje
                await Dalje(_page, "Dalje");

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno2");
                //Provera da li se otvorila stranica za izbor Paketa pokrića
                await ProveriTitlStranice(_page, "Paketi pokrića");

                //Provera stranice Paketi pokrića
                await ProveraNaStraniciPaketiPokrica(_page);

                // Izbor paketa pokrića
                await IzborPaketaPokrica(_page, "Paket1");

                // Dalje
                await Dalje(_page, "Dalje"); //ili Nazad

                //Stranica za unos putnika
                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno3");

                await ProveriTitlStranice(_page, "Osigurana lica");

                await _page.GetByRole(AriaRole.Heading, new() { Name = "INDIVIDUALNO PUTNO OSIGURANJE" }).ClickAsync();
                await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();

                // Ugovarač je jedan odrasli
                await UnesiUgovaraca(_page,
                                     "Petar-DaCovid", "IndividualniPetrović",
                                     "2612962710096", "Japanska", "442",
                                     "111", "- Rušanj", "Pasoš br. 1",
                                     "+381123456789", "bogaman@hotmail.com");

                //Ugovarač je osigurano lice i prekopiraj podatke
                await UgovaracJeOsiguranoLice(_page);

                await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();

                // Dalje
                await Dalje(_page, "Dalje"); //ili "Nazad"

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno4");

                await ProveriTitlStranice(_page, "Rekapitulacija");

                //Potvrde na stranici rekapitulacija
                await PotvrdaRekapitulacije(_page);

                // Klik na Plaćanje
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                // Na stranici Plaćanje
                await Placanje(_page, "5342230500001234", "08", "2025");

                await ProveriURL(_page, PocetnaStrana, "/Uspesno");
                await ProveriTitlStranice(_page, "Uspešna kupovina");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                //await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
                //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();
                Assert.Pass();
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
        public async Task PorodicnoPutno()
        {
            try
            {
                //izbor Porodičnog osiguranja
                await _page!.Locator("a").Filter(new() { HasText = "PORODIČNO PUTNO OSIGURANJE" }).First.ClickAsync();
                //await _page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("PORODIČNO PUTNO OSIGURANJE"), Exact = true }).First.ClickAsync();

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno1");
                await ProveriTitlStranice(_page, "Osnovni podaci");

                //unos broja putnika
                await UnosBrojaOdraslih(_page, "1");
                await UnosBrojaDece(_page, "2");
                //await UnosBrojaSeniora(_page, "0");                                                                                                                  //await page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA"), Exact = true }).First.ClickAsync(); // izbor Individualnog sa više ulazaka

                //unos datuma početka
                await DatumPocetka(_page);

                //unos trajanja
                await UnosTrajanja(_page, "10");

                //Svrha putovanja
                await IzaberiOpcijuIzListe(_page, "#selSvrha", "Turistički", false);

                // Covid 19
                await Covid(_page, "Ne");

                // Teritorija Srbije
                await TeritorijaSrbije(_page);

                // Dalje
                await Dalje(_page, "Dalje");

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno2");
                //Provera da li se otvorila stranica za izbor Paketa pokrića
                await ProveriTitlStranice(_page, "Paketi pokrića");

                //Provera stranice Paketi pokrića
                await ProveraNaStraniciPaketiPokrica(_page);

                // Izbor paketa pokrića
                await IzborPaketaPokrica(_page, "Paket2");

                // Dalje
                await Dalje(_page, "Dalje"); //ili Nazad

                //Stranica za unos putnika
                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno3");

                await ProveriTitlStranice(_page, "Osigurana lica");

                await _page.GetByRole(AriaRole.Heading, new() { Name = "PORODIČNO PUTNO OSIGURANJE" }).ClickAsync();
                await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();

                // Ugovarač je Jedan odrasli
                await UnesiUgovaraca(_page,
                                     "Mitar-NoCovid", "PorodičnoMirić",
                                     "2612962710096", "Kojekude", "100",
                                     "111", "- Kaluđerica", "Pasoš br. 123",
                                     "+3819876543216789", "bogaman@hotmail.com");

                //Ugovarač je osigurano lice i prekopiraj podatke
                await UgovaracJeOsiguranoLice(_page);

                await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();
                await _page.GetByText("OSIGURANIK 1 - ODRASLA OSOBA").ClickAsync();

                await UnesiDetePrvo(_page, "Prvo", "Dete-Prvo", "3109009780168", "Pasoš dete 1");

                await UnesiDeteDrugo(_page, "Drugo", "Dete-Drugo", "3111014710210", "Pasoš dete 2");

                // Dalje
                await Dalje(_page, "Dalje"); //ili Nazad

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno4");

                await ProveriTitlStranice(_page, "Rekapitulacija");

                //Potvrde na stranici rekapitulacija
                await PotvrdaRekapitulacije(_page);

                // Klik na Plaćanje
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                // Na stranici Plaćanje
                await Placanje(_page, "5342230500001234", "08", "2025");

                await ProveriURL(_page, PocetnaStrana, "/Uspesno");
                await ProveriTitlStranice(_page, "Uspešna kupovina");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                //await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
                //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();
                //Assert.Pass();
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
        public async Task SaViseUlazaka()
        {
            try
            {
                //await _page.PauseAsync();
                //await _page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA"), Exact = true }).First.ClickAsync(); // izbor Individualnog sa više ulazaka
                await _page!.Locator("a").Filter(new() { HasText = "INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA" }).First.ClickAsync();

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno1");
                await ProveriTitlStranice(_page, "Osnovni podaci");

                //unos broja putnika
                await UnosBrojaOdraslih(_page, "1");
                await UnosBrojaDece(_page, "0");
                await UnosBrojaSeniora(_page, "1");

                //unos datuma početka
                await DatumPocetka(_page);

                //izbor trajanja
                await IzborTrajanja(_page, "do 30 dana");

                //Period pokrića
                await IzborPeriodaPokrica(_page, "5");

                //Svrha putovanja
                await IzaberiOpcijuIzListe(_page, "#selSvrha", "Doplatak za ekstremne sportove", false);

                // Covid 19
                await Covid(_page, "Da");

                // Teritorija Srbije
                await TeritorijaSrbije(_page);

                // Dalje
                await Dalje(_page, "Dalje");

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno2");
                //Provera da li se otvorila stranica za izbor Paketa pokrića
                await ProveriTitlStranice(_page, "Paketi pokrića");

                //Provera stranice Paketi pokrića
                await ProveraNaStraniciPaketiPokrica(_page);

                // Izbor paketa pokrića
                await IzborPaketaPokrica(_page, "Paket3");

                // Dalje
                await Dalje(_page, "Dalje"); //ili Nazad

                //Stranica za unos putnika
                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno3");

                await ProveriTitlStranice(_page, "Osigurana lica");

                await _page.GetByRole(AriaRole.Heading, new() { Name = "INDIVIDUALNO PUTNO OSIGURANJE VIŠE ULAZAKA" }).ClickAsync();
                await _page.GetByText("PODACI O UGOVARAČU").ClickAsync();

                // Ugovarač je Jedan odrasli
                await UnesiUgovaraca(_page,
                                     "Zoki-DaCovid", "VišeUlazaka Zorić",
                                     "2612962710096", "Kineska", "bb",
                                     "111", "- Rušanj", "Pasoš br. 333",
                                     "+381123456789", "bogaman@hotmail.com");

                //Ugovarač je osigurano lice i prekopiraj podatke
                await UgovaracJeOsiguranoLice(_page);

                await _page.GetByText("PODACI O OSIGURANICIMA").ClickAsync();
                await _page.GetByText("OSIGURANIK 1 - ODRASLA OSOBA").ClickAsync();


                // Unos seniora
                await UnesiSeniora(_page,
                                   "Senior", "Više ulazaka",
                                   "3112952710293", "Pasoš Seniorski");

                // Dalje
                await Dalje(_page, "Dalje"); //ili Nazad

                //sačekaj da se stranica učita
                await ProveriURL(_page, PocetnaStrana, "/Putno4");

                await ProveriTitlStranice(_page, "Rekapitulacija");

                //Potvrde na stranici rekapitulacija
                await PotvrdaRekapitulacije(_page);

                // Klik na Plaćanje
                await _page.GetByRole(AriaRole.Button, new() { Name = "Plaćanje" }).ClickAsync();
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Nazad" }).ClickAsync();

                // Na stranici Plaćanje
                await Placanje(_page, "5342230500001234", "08", "2025");

                await ProveriURL(_page, PocetnaStrana, "/Uspesno");
                await ProveriTitlStranice(_page, "Uspešna kupovina");

                await _page.GetByRole(AriaRole.Button, new() { Name = "Početna" }).ClickAsync();

                //await _page.GetByText("Webshop AMS Osiguranja").ClickAsync(); // Klik na pop-up prozor
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync(); // potvrda kolačića
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne" }).ClickAsync(); // odbijanje kolačića
                //await Expect(_page).ToHaveTitleAsync(RegexAMSOsiguranjeWebshop());
                //await _page.GetByRole(AriaRole.Button, new() { Name = "Da" }).ClickAsync();
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogovanjeTesta.LogError($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}");
                LogovanjeTesta.LogException($"❌ Neuspešan test {NazivTekucegTesta} - {ex.Message}", ex);
                throw;
            }

        }

    }
}