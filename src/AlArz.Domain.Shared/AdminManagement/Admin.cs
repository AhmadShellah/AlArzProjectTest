using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlArz.AdminManagement
{
    public class Admin
    {
        public const string admin = "admin";
        public const string Language = "lang";
        public const string English = "en";
        public const string Arabic = "ar";
        public const string GetPublicIpAddressFromUser = "http://checkip.dyndns.org";
        public const string Attachment = "attachment:Default";
        public const string LogPath = "LogPath:Default";
    }

    public class CheckAdmin
    {
        public string GetPublicIpAddress()
        {
            try
            {
                string response = string.Empty;
                using (var webClient = new WebClient())
                {
                    response = webClient.DownloadString(Admin.GetPublicIpAddressFromUser);
                }

                string pattern = @"(?<=Current IP Address: )(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";

                Match match = Regex.Match(response, pattern);
                if (match.Success)
                {
                    response = match.Value;
                }
                else
                {
                    response = "0.0.0.0";
                }

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetPublicIpAddressAsync()
        {
            var httpClient = new HttpClient();

            string response = await httpClient.GetStringAsync(Admin.GetPublicIpAddressFromUser).ConfigureAwait(false);
            string pattern = @"(?<=Current IP Address: )(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";

            Match match = Regex.Match(response, pattern);
            if (match.Success)
            {
                string publicIp = match.Value;
                return publicIp;
            }
            else
            {
                return null;
            }
        }

        public string GetComputerName()
        {
            string computerName = Environment.MachineName;

            return computerName;
        }

        public string GetPhysicalAddress()
        {
            NetworkInterface[] networkCards = NetworkInterface.GetAllNetworkInterfaces();

            var macAddress = networkCards[0].GetPhysicalAddress().ToString();

            return macAddress;
        }

        public CheckAdmin()
        {

        }
        public CheckAdmin(bool? isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public CheckAdmin(Guid? userId)
        {
            UserId = userId;
        }

        public CheckAdmin(bool? isAdmin, Guid? userId)
        {
            IsAdmin = isAdmin;
            UserId = userId;
        }


        public bool? IsAdmin { get; set; }
        public Guid? UserId { get; set; }

    }
}
