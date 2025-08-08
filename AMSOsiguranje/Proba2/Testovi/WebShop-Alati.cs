namespace Proba2
{
    public partial class WebShop
    {




        //Bira se početna strana Web shopa na osnovu zadatog okruženja - Razvoj, test, ...
        private static string DefinisiPocetnuStranuWS(string okruzenje)
        {
            string pocetnaStranaWS = okruzenje switch
            {
                "Razvoj" => "https://razvojamso-webshop.eonsystem.rs",
                "Proba2" => "https://proba2amsowebshop.eonsystem.rs",
                "UAT" => "https://webshop-test.ams.co.rs",
                "Produkcija" => "https://webshop.ams.co.rs",
                _ => throw new ArgumentException("Nepoznata stranica: " + PocetnaStrana)
            };
            return pocetnaStranaWS;
        }
        /*******************************************************************************
        private static async Task SacekajUcitavanjestranice(IPage _page, string dodatak)
        {
            await _page.WaitForURLAsync(PocetnaStranaWS + dodatak);
            // Sačekaj da DOM bude učitan
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
        *******************************************************************************/
        private static async Task ProveriTitlStranice(IPage _page, string naslov) //proverava da li se stvarni rezultat (title) poklapa sa očekivanim (naslov)
        {
            var title = await _page.TitleAsync();
            //await Task.Delay(1000); // Ova pauza može pomoći kod testiranja

            try
            {
                Assert.That(title, Is.EqualTo(naslov));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Greška: {ex.Message}\nTrenutna vrednost title: {title}");
                throw;
            }
        }

        private async Task UnosBrojaOdraslih(IPage _page, string brojOdraslih)
        {
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Button, new() { Name = "-" }).ClickAsync();
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Textbox).ClearAsync();
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Textbox).FillAsync("5");
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Textbox).PressAsync("Backspace");
            await _page.Locator("#spinOdrasli").GetByRole(AriaRole.Textbox).FillAsync(brojOdraslih);
        }

        private async Task UnosBrojaDece(IPage _page, string brojDece)
        {
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Button, new() { Name = "-" }).ClickAsync();
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Textbox).ClearAsync();
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Textbox).FillAsync("4");
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Textbox).PressAsync("Backspace");
            await _page.Locator("#spbDeca").GetByRole(AriaRole.Textbox).FillAsync(brojDece);
        }

        private async Task UnosBrojaSeniora(IPage _page, string brojDece)
        {
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Button, new() { Name = "+" }).ClickAsync();
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Button, new() { Name = "-" }).ClickAsync();
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Textbox).ClearAsync();
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Textbox).FillAsync("4");
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Textbox).PressAsync("Backspace");
            await _page.Locator("#spinSeniori").GetByRole(AriaRole.Textbox).FillAsync(brojDece);
        }

        private async Task DatumPocetka(IPage _page)
        {
            //unos datuma početka
            await _page.Locator("#cal_calPocetak").GetByPlaceholder("dd.mm.yyyy.").ClickAsync();
            await _page.GetByLabel(NextDate.ToString("MMMM d")).First.ClickAsync();
        }

        private async Task UnosTrajanja(IPage _page, string Trajanje)
        {
            //unos trajanja
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "od 1 do 90" }).ClickAsync();
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "od 1 do 90" }).FillAsync(Trajanje);
        }

        private async Task IzborTrajanja(IPage _page, string TrajanjeDo)
        {
            await _page.Locator(".multiselect-dropdown").First.ClickAsync();
            await _page.GetByText("do 30 dana").ClickAsync();
            await _page.GetByText("do 30 danado 60 danado 90 danado 180 danado 365 dana do 30 dana").ClickAsync();
            await _page.GetByText("do 60 dana").ClickAsync();
            await _page.GetByText("do 30 danado 60 danado 90 danado 180 danado 365 dana do 60 dana").ClickAsync();
            await _page.GetByText("do 90 dana").ClickAsync();
            await _page.GetByText("do 30 danado 60 danado 90 danado 180 danado 365 dana do 90 dana").ClickAsync();
            await _page.GetByText("do 180 dana").ClickAsync();
            await _page.GetByText("do 30 danado 60 danado 90 danado 180 danado 365 dana do 180 dana").ClickAsync();
            await _page.GetByText("do 365 dana").ClickAsync();
            await _page.GetByText("do 30 danado 60 danado 90 danado 180 danado 365 dana do 365 dana").ClickAsync();
            await _page.GetByText(TrajanjeDo).ClickAsync();
        }

        private async Task IzborPeriodaPokrica(IPage _page, string PeriodPokrica)
        {
            await _page.Locator("#selPeriodPokrica > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.Locator("#selPeriodPokrica").GetByText("5", new() { Exact = true }).ClickAsync();
            await _page.Locator("#selPeriodPokrica span").Filter(new() { HasText = "5" }).ClickAsync();
            await _page.Locator("#selPeriodPokrica").GetByText("10").ClickAsync();
            await _page.Locator("#selPeriodPokrica span").Filter(new() { HasText = "10" }).ClickAsync();
            await _page.Locator("#selPeriodPokrica").GetByText("15").ClickAsync();
            await _page.Locator("#selPeriodPokrica span").Filter(new() { HasText = "15" }).ClickAsync();
            await _page.Locator("#selPeriodPokrica").GetByText("20").ClickAsync();
            await _page.Locator("#selPeriodPokrica span").Filter(new() { HasText = "20" }).ClickAsync();
            await _page.Locator("#selPeriodPokrica").GetByText(PeriodPokrica, new() { Exact = true }).ClickAsync();

        }

        public static async Task IzaberiOpcijuIzListe(IPage page, string selektorListe, string vrednostOpcije, bool koristiSelectOption = true)
        {
            try
            {
                var lista = page.Locator(selektorListe);

                // 1. Proveri da li je opcija već selektovana (korisno ako lista prikazuje selektovani tekst)
                string trenutnoSelektovano = await lista.InnerTextAsync();
                if (trenutnoSelektovano.Trim() == vrednostOpcije.Trim())
                {
                    LogovanjeTesta.LogMessage($"⚠️ Opcija '{vrednostOpcije}' je već selektovana u '{selektorListe}'.", false);
                    return;
                }

                // 2. Ako je HTML <select> element, koristi SelectOptionAsync
                if (koristiSelectOption)
                {
                    var selectElement = page.Locator(selektorListe);
                    var tagName = await selectElement.EvaluateAsync<string>("e => e.tagName");
                    if (tagName.ToLower() == "select")
                    {
                        await selectElement.SelectOptionAsync(new SelectOptionValue { Label = vrednostOpcije });
                        LogovanjeTesta.LogMessage($"✅ Opcija '{vrednostOpcije}' izabrana korišćenjem SelectOptionAsync na '{selektorListe}'.", false);
                        return;
                    }
                }

                // 3. Fallback na klik-based selekciju
                await lista.ClickAsync();
                await page.Locator(selektorListe).GetByText(vrednostOpcije, new() { Exact = true }).First.ClickAsync();
                LogovanjeTesta.LogMessage($"✅ Opcija '{vrednostOpcije}' selektovana klikom u '{selektorListe}'.", false);
            }
            catch (Exception ex)
            {
                LogovanjeTesta.LogException(ex, $"Greška prilikom izbora opcije '{vrednostOpcije}' u listi '{selektorListe}'");
                throw;
            }
        }
        private async Task SvrhaPutovanja(IPage _page, string Svrha)
        {
            //Svrha putovanja
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za privremeni rad u").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za skijanje").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za ekstremne sportove").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za sportski rizik").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za obavljanje građ").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText("Doplatak za profesionalne").ClickAsync();
            //await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            //await _page.GetByText("Turistički").ClickAsync();
            await _page.Locator("#selSvrha > .control-wrapper > .control > .control-main > .multiselect-dropdown").ClickAsync();
            await _page.GetByText(Svrha).ClickAsync();
        }

        private async Task Covid(IPage _page, string Pristanak)
        {
            // Covid 19
            await _page.GetByRole(AriaRole.Button, new() { Name = Pristanak, Exact = true }).ClickAsync();
            //await _page.GetByRole(AriaRole.Button, new() { Name = "Ne", Exact = true }).ClickAsync();
        }

        private async Task TeritorijaSrbije(IPage _page)
        {
            // Teritorija Srbije
            await _page.GetByText("Nalazim se na teritoriji").ClickAsync();
            await _page.Locator("#chkUSrbiji i").ClickAsync();
            await _page.GetByText("Nalazim se na teritoriji").ClickAsync();
        }

        private async Task Dalje(IPage _page, string Kuda)
        {
            // Dalje
            await _page.GetByRole(AriaRole.Button, new() { Name = Kuda }).ClickAsync();
        }

        private async Task ProveraNaStraniciPaketiPokrica(IPage _page)
        {

            await _page.GetByText("Evropa i deo sveta").First.ClickAsync();
            await _page.GetByText("Evropa i deo sveta").Nth(1).ClickAsync();
            await _page.GetByText("Svet", new() { Exact = true }).ClickAsync();
            await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Evropa i deo sveta$") }).Nth(1).ClickAsync();
            await _page.Locator("div").Filter(new() { HasTextRegex = new Regex("^Evropa i deo sveta$") }).First.ClickAsync();
            await _page.GetByText("Suma osiguranja").First.ClickAsync();
            await _page.GetByText("Suma osiguranja").Nth(1).ClickAsync();
            await _page.GetByText("Suma osiguranja").Nth(2).ClickAsync();
            await _page.GetByText("10.000,00").ClickAsync();
            await _page.GetByText("30.000,00").First.ClickAsync();
            await _page.GetByText("30.000,00").Nth(1).ClickAsync();
            await _page.GetByText("Premija osiguranja").First.ClickAsync();
            await _page.GetByText("Premija osiguranja").Nth(1).ClickAsync();
            await _page.GetByText("Premija osiguranja").Nth(2).ClickAsync();
            await _page.GetByText("Evropa i svet osim sledećih").First.ClickAsync();
            await _page.GetByText("Evropa i svet osim sledećih").Nth(1).ClickAsync();
            await _page.GetByText("Ceo svet").ClickAsync();
            await _page.GetByText("Tabela pokrića").First.ClickAsync();
            await _page.Locator("h3 button").ClickAsync();
            await _page.GetByText("Tabela pokrića").Nth(1).ClickAsync();
            await _page.Locator("h3 button").ClickAsync();
            await _page.GetByText("Tabela pokrića").Nth(2).ClickAsync();
            await _page.Locator("h3 button").ClickAsync();
            //await _page.GetByText("Tabela pokrića").First.ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = "" }).ClickAsync();
            //await _page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
            //await _page.GetByText("Tabela pokrića").Nth(1).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = "" }).ClickAsync();
            //await _page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
            //await _page.GetByText("Tabela pokrića").Nth(2).ClickAsync();
            //await _page.GetByRole(AriaRole.Heading, new() { Name = "" }).ClickAsync();
            //await _page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();
        }

        private async Task IzborPaketaPokrica(IPage _page, string PaketPokrica)
        {
            if (PaketPokrica == "Paket1")
                await _page.GetByText("Evropa i deo sveta").First.ClickAsync();
            else if (PaketPokrica == "Paket2")
                await _page.GetByText("Evropa i deo sveta").Nth(1).ClickAsync();
        }

        private async Task UnesiUgovaraca(IPage _page, string UgovaracIme, string UgovaracPrezime, string UgovaracJMBG, string UgovaracUlica, string UgovaracBroj, string UgovaracPretrazi, string UgovaracMesto, string UgovaracPasos, string UgovaracTelefon, string UgovaracMejl)
        {
            await _page.Locator(".input").First.ClickAsync();
            await _page.Locator(".input").First.FillAsync(UgovaracIme);
            await _page.Locator("div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(UgovaracPrezime);
            await _page.Locator("div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(UgovaracJMBG);
            await _page.Locator("div:nth-child(4) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(4) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(UgovaracUlica);
            await _page.Locator("div:nth-child(5) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(UgovaracBroj);
            await _page.Locator("div:nth-child(3) > div > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(3) > div > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(UgovaracPasos);
            await _page.Locator("e-input").Filter(new() { HasText = "Telefon: u formatu +381 XX" }).GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("e-input").Filter(new() { HasText = "Telefon: u formatu +381 XX" }).GetByRole(AriaRole.Textbox).FillAsync(UgovaracTelefon);
            await _page.Locator("e-input").Filter(new() { HasText = "E-mail:" }).GetByRole(AriaRole.Textbox).ClickAsync();
            await _page.Locator("e-input").Filter(new() { HasText = "E-mail:" }).GetByRole(AriaRole.Textbox).FillAsync(UgovaracMejl);

            await _page.Locator(".multiselect-dropdown").First.ClickAsync();
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "pretraži" }).FillAsync(UgovaracPretrazi);
            await _page.GetByText(UgovaracMesto).First.ClickAsync();
        }

        private async Task UnesiDetePrvo(IPage _page, string DeteImePrvo, string DetePrezimePrvo, string DeteJMBGPrvo, string DetePasosPrvo)
        {
            await _page.GetByText("OSIGURANIK 2 - DETE").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(DeteImePrvo);
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(DetePrezimePrvo);
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(DeteJMBGPrvo);
            await _page.Locator("div:nth-child(5) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(DetePasosPrvo);
        }

        private async Task UnesiDeteDrugo(IPage _page, string DeteImeDrugo, string DetePrezimeDrugo, string DeteJMBGDrugo, string DetePasosDrugo)
        {
            await _page.GetByText("OSIGURANIK 3 - DETE").ClickAsync();
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(DeteImeDrugo);
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").PressAsync("Shift+Tab");
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(DetePrezimeDrugo);
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").PressAsync("Tab");
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(DeteJMBGDrugo);
            await _page.Locator("div:nth-child(6) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");
            await _page.Locator("div:nth-child(6) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(6) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(DetePasosDrugo);
        }

        private async Task UnesiSeniora(IPage _page, string SeniorIme, string SeniorPrezime, string SeniorJMBG, string SeniorPasos)
        {
            await _page.GetByText("OSIGURANIK 2 - STARIJA OSOBA").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div > e-input > .control-wrapper > .control > .control-main > .input").First.FillAsync(SeniorIme);

            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(2) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync("SeniorPrezime");
            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();

            await _page.Locator("div:nth-child(5) > div:nth-child(2) > div:nth-child(3) > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(SeniorJMBG);

            await _page.Locator("div:nth-child(5) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").ClickAsync();
            await _page.Locator("div:nth-child(5) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").FillAsync(SeniorPasos);
            await _page.Locator("div:nth-child(5) > div:nth-child(4) > .col-4 > e-input > .control-wrapper > .control > .control-main > .input").PressAsync("Enter");
        }

        private async Task UgovaracJeOsiguranoLice(IPage _page)
        {
            await _page.GetByText("Ugovarač je osigurano lice").ClickAsync();
            await _page.GetByText("Prekopiraj adresne podatke na").ClickAsync();
        }

        private async Task PotvrdaRekapitulacije(IPage _page)
        {
            await _page.Locator("#chkUslovi i").ClickAsync();
            await _page.Locator("#container i").Nth(1).ClickAsync();
            await _page.Locator("#container i").Nth(1).ClickAsync();
        }

        private async Task Placanje(IPage _page, string BrojKartice, string mesecVazenja, string godinaVazenja)
        {
            await _page.GetByRole(AriaRole.Img, new() { Name = "MasterCard" }).ClickAsync();
            await _page.Locator("#cardRadioDiv label").Nth(1).ClickAsync();
            await _page.Locator("#cardRadioDiv label").Nth(2).ClickAsync();

            await _page.GetByPlaceholder("Broj kartice").ClickAsync();
            await _page.GetByPlaceholder("Broj kartice").FillAsync(BrojKartice);

            await _page.GetByLabel("Mesec").ClickAsync();
            await _page.GetByLabel("Mesec").SelectOptionAsync(mesecVazenja);

            await _page.GetByLabel("Godina").ClickAsync();
            await _page.GetByLabel("Godina").SelectOptionAsync(godinaVazenja);

            await _page.Locator("id=input-card-cvv").FillAsync("111");
            await _page.Locator("id=input-card-holder").FillAsync("BM");

            await _page.GetByRole(AriaRole.Button, new() { Name = "POTVRDITI" }).ClickAsync();
        }

    }

}