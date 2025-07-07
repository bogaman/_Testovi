namespace UAT
{

    public class Alati : Osiguranje
    {
        private const string OVPN_GUI_PATH = @"C:\Program Files\OpenVPN\bin\openvpn-gui.exe";
        private const string VPN_CONFIG_NAME = "mojvpn.ovpn"; // samo ime fajla, bez putanje
        private const string VPN_IP_PREFIX = "10.41.";
        private const int VPN_TIMEOUT_SEK = 60;

        /// <summary>
        /// Proverava da li je OpenVPN GUI pokrenut, ako nije, pokreće ga i uspostavlja VPN konekciju.
        /// </summary>
        public static void PokreniVpnAkoTreba()
        {
            // 1. Proveri da li GUI postoji
            if (!DaLiJeOpenVpnGuiPokrenut())
            {
                Console.WriteLine("🔄 OpenVPN GUI nije pokrenut. Pokrećem...");
                //System.Windows.MessageBox.Show("🔄 OpenVPN GUI nije pokrenut. Pokrećem...");
                Process.Start(OVPN_GUI_PATH);
                Thread.Sleep(3000); // Sačekaj da GUI startuje
            }

            // 2. Proveri da li već postoji VPN IP
            string? vpnIp = VratiVpnLokalnuAdresu();
            if (vpnIp != null)
            {
                Console.WriteLine($"✅ VPN konekcija već aktivna. IP: {vpnIp}");
                //System.Windows.MessageBox.Show($"✅ VPN konekcija već aktivna. IP: {vpnIp}");
                return;
            }

            // 3. Pokreni konekciju
            Console.WriteLine("🌐 Nema VPN IP. Pokrećem konekciju...");
            //System.Windows.MessageBox.Show("🌐 Nema VPN IP. Pokrećem konekciju...");
            Process.Start(OVPN_GUI_PATH, $"--connect {VPN_CONFIG_NAME}");

            // 4. Čekaj na VPN IP (do 60 sekundi)
            int cekano = 0;
            while (cekano < VPN_TIMEOUT_SEK)
            {
                vpnIp = VratiVpnLokalnuAdresu();
                if (vpnIp != null)
                {
                    Console.WriteLine($"✅ VPN konekcija uspostavljena. IP: {vpnIp}");
                    //System.Windows.MessageBox.Show($"✅ VPN konekcija uspostavljena. IP: {vpnIp}");
                    return;
                }

                Console.WriteLine("⏳ Čekam VPN IP...");
                Thread.Sleep(5000);
                cekano += 5;
            }

            throw new Exception("❌ VPN IP nije dodeljena u roku od 60 sekundi.");
        }

        /// <summary>
        /// Proverava da li je OpenVPN GUI pokrenut.
        /// <para>Prebrojava koliko ima "openvpn-gui" procesa.</para>
        /// </summary>
        /// <returns>Vraća True ako je broj procesa > 0, tj. ako je OpenVPN GUI pokrenut, inače False.</returns>
        public static bool DaLiJeOpenVpnGuiPokrenut()
        {
            /******************
            System.Windows.MessageBox.Show($"Proces VPN je {Process.GetProcessesByName("openvpn-gui").Length}",
                                           "VPN Status",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Information);
            ********************/
            return Process.GetProcessesByName("openvpn-gui").Length > 0;
        }

        /// <summary>
        /// Vraća lokalnu IP adresu VPN adaptera ako je aktivan.    
        /// Pretražuje sve mrežne interfejse i traži one koji su u statusu "Up" i imaju tip "Ethernet" ili "Wireless".
        /// Proverava da li ime ili opis interfejsa sadrži ključne reči koje se obično koriste za VPN adaptere.
        /// </summary>
        public static string? VratiVpnLokalnuAdresu()
        {
            // Nazivi koji najčešće označavaju VPN adapter
            string[] vpnKljucevi = ["tap", "tun", "openvpn", "wintun"];

            var interfejsi = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni =>
                    ni.OperationalStatus == OperationalStatus.Up &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    !ni.Description.Contains("Intel") &&
                    !ni.Description.Contains("Realtek") &&
                    vpnKljucevi.Any(k => ni.Name.ToLower().Contains(k) || ni.Description.ToLower().Contains(k)));

            foreach (var ni in interfejsi)
            {
                var ipProps = ni.GetIPProperties();
                foreach (var ip in ipProps.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
            return null;
        }


        public static bool DaLiPostojiVpnLokalnaAdresa()
        {
            return VratiVpnLokalnuAdresu() != null;
        }

        public static void IskljuciVpn()
        {
            Console.WriteLine("⛔ Isključujem VPN konekciju...");
            //System.Windows.MessageBox.Show("⛔ Isključujem VPN konekciju...");
            try
            {
                Process.Start(OVPN_GUI_PATH, "--command disconnect_all");
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Greška pri isključivanju VPN-a: " + ex.Message);
            }
        }







        private static void CekajNaVpnIp(int timeoutSekundi = 60)
        {
            int cekano = 0;
            while (cekano < timeoutSekundi)
            {
                var ip = VratiVpnLokalnuAdresu();
                if (ip != null)
                {
                    Console.WriteLine("✅ VPN IP dodeljena: " + ip);
                    return;
                }

                Console.WriteLine("⏳ Čekam na VPN IP...");
                Thread.Sleep(5000);
                cekano += 5;
            }

            throw new Exception("❌ VPN IP nije dodeljena u roku od " + timeoutSekundi + " sekundi.");
        }

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


        public static void PokreniVpnKonekciju()
        {
            Process.Start(OVPN_GUI_PATH, $"--connect {VPN_CONFIG_NAME}");
            Console.WriteLine($"Pokrenuta konekcija: {VPN_CONFIG_NAME}");
        }

        public static void CekajNaVpnIp()
        {
            int maxPokusaja = 12; // ukupno 60 sekundi
            int cekanje = 5000;

            for (int i = 0; i < maxPokusaja; i++)
            {
                if (DaLiJeVpnIpUOpsegu())
                {
                    Console.WriteLine("VPN IP potvrđena.");
                    return;
                }

                Console.WriteLine("Čekam da VPN IP bude u opsegu 10.41...");
                Task.Delay(cekanje).Wait();
            }

            throw new Exception("VPN IP adresa nije u opsegu 10.41. nakon čekanja.");
        }

        public static bool DaLiJeVpnIpUOpsegu()
        {
            try
            {
                using var client = new HttpClient();
                var ip = client.GetStringAsync("https://api.ipify.org").Result;
                Console.WriteLine("Trenutna IP: " + ip);
                return ip.StartsWith("10.41.");
            }
            catch
            {
                return false;
            }
        }

    }


}