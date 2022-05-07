using Newtonsoft.Json.Linq;
using ROI.Content.Configs;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using Terraria;
using Terraria.ModLoader;

namespace ROI
{
    partial class ROIMod
    {
        public DebugConfig DebugConfig;

        private void LoadDebug()
        {
            DebugConfig = ModContent.GetInstance<DebugConfig>();

            if (DebugConfig.Nightly)
                CheckForNightly();

            if (!DebugConfig.DebugMode)
                return;
        }

        // TODO: document how this works
        private void CheckForNightly()
        {
            if (!NetworkAvailable())
                return;

            string path = Path.Combine(Main.SavePath, "ROI-beta-timestamp.txt");
            DateTime data;

            if (File.Exists(path))
            {
                File.Delete(path);

                if (!DateTime.TryParse(File.ReadAllText(path), out data))
                {
                    Logger.Warn("Unable to read nightly-release version timestamp");
                    return;
                }
            }
            else
            {
                data = DateTime.MinValue;
            }

            string json = ReleasesJson();

            JArray releases = JArray.Parse(json);
            foreach (JObject release in releases)
            {
                DateTime availableVersion = DateTime.Parse(release["created_at"].ToString());
                if (availableVersion <= data)
                    continue;

                JToken asset = JArray.Parse(release["assets"].ToString()).First;

                if (asset["name"].ToString().Equals("beta-release.zip"))
                {
                    Process.Start(asset["browser_download_url"].ToString());
                    File.WriteAllText(path, release["created_at"].ToString());
                }
            }
        }

        private string ReleasesJson()
        {
            // some stupid legacy stuff, somehow necessary
            var oldProtocol = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += SCVC;

            string json = string.Empty;
            try
            {
                // TODO: figure out how to do this without HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                    client.DefaultRequestHeaders.Add("User-Agent", "RealmsOfInfinityNightly");
                    var response = client.GetAsync("https://API.github.com/repos/TUA-Team/ROI/releases").GetAwaiter().GetResult();
                    json = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException)
            {
                Logger.Error("Could not download nightly");
            }

            ServicePointManager.SecurityProtocol = oldProtocol;
            ServicePointManager.ServerCertificateValidationCallback -= SCVC;
            return json;
        }

        private static bool SCVC(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate c,
            System.Security.Cryptography.X509Certificates.X509Chain ch,
            System.Net.Security.SslPolicyErrors ss) => true;

        //https://stackoverflow.com/questions/520347/how-do-i-check-for-a-network-connection
        private static bool NetworkAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            NetworkInterface[] array = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < array.Length; i++)
            {
                NetworkInterface net = array[i];
                // discard because of standard reasons
                if (net.OperationalStatus != OperationalStatus.Up ||
                    net.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    net.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                // this allows to filter modems, serial, etc.
                if (net.Speed < 10000000)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if (net.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    net.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (net.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }
            return false;
        }
    }
}
