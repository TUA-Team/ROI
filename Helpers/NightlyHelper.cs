using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace ROI.Helpers
{
    public static class NightlyHelper
    {
        public static string CheckForNightly(DateTime currentVersion)
        {
            if (!NetworkAvailable()) return null;

            var json = IssueCommentsJson();

            var comments = JArray.Parse(json);
            foreach (JObject comm in comments)
            {
                if (Convert.ToUInt64(comm["user"]["id"]) != 42445050) continue;
                var availableVersion = DateTime.Parse(comm["created_at"].ToString());
                if (availableVersion <= currentVersion) continue;
                Process.Start(comm["body"].ToString());
                return comm["created_at"].ToString();
            }
            return null;
        }

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
                if ((net.OperationalStatus != OperationalStatus.Up) ||
                    (net.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (net.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allows to filter modems, serial, etc.
                if (net.Speed < 10000000)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((net.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (net.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (net.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }
            return false;
        }

        private static string IssueCommentsJson()
        {
            var oldProtocol = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += SCVC;

            string json = string.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                client.DefaultRequestHeaders.Add("User-Agent", "RealmsOfInfinityNightly");
                var response = client.GetAsync("https://api.github.com/repos/TUA-Team/ROI/issues/3/comments").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }

            ServicePointManager.SecurityProtocol = oldProtocol;
            ServicePointManager.ServerCertificateValidationCallback -= SCVC;
            return json;
        }

        private static bool SCVC(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate c,
            System.Security.Cryptography.X509Certificates.X509Chain ch,
            System.Net.Security.SslPolicyErrors ss) => true;
    }
}