using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OShop.UI.Extras
{
    public static class OrderStatusEnum
    {
        public const string Plasata = "Plasata";
        public const string Preluata = "Preluata";
        public const string Modificata = "Modificata";
        public const string Pregatita = "Pregatita pentru livrare";
        public const string SpreLivrare = "In curs de livrare";
        public const string Livrata = "Livrata";
        public const string Anulata = "Anulata";
    }
    public interface IGoogleApiDirections
    {
        Task<string> GetDirections(double latCourier, double longCourier, double latHome, double longHome);
    }
    public class GoogleApiClient : IGoogleApiDirections
    {
        private HttpClient _client;
        private string apiKey;
        public GoogleApiClient(IConfiguration config)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://maps.googleapis.com/maps/");
            apiKey = config["ConnectionStrings:GoogleApiKey"];
        }
        public async Task<string> GetDirections(double latCourier, double longCourier, double latHome, double longHome)
        {
            var response = await _client.GetAsync("api/directions/json?mode=driving&transit_routing_preference=less_driving&origin=" +
                 latCourier.ToString("N7", CultureInfo.InvariantCulture) + "," +
                 longCourier.ToString("N7", CultureInfo.InvariantCulture) + "&destination=" +
                 latHome.ToString("N7", CultureInfo.InvariantCulture) + "," +
                 longHome.ToString("N7", CultureInfo.InvariantCulture) +
                "&language=ro&region=RO&key=" + apiKey);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return json;
                }
            }
            return string.Empty;

        }
    }
    public class EmailSender
    {
        private string emailUser;
        private string emailPass;

        public EmailSender(IConfiguration config)
        {
            emailUser = config["ConnectionStrings:EmailUser"];
            emailPass = config["ConnectionStrings:EmailPass"];
        }
        public bool SendEmail(string userEmail, string subject, string bodyText)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailUser);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = bodyText;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(emailUser, emailPass);
            client.Host = "mail5015.site4now.net";
            client.Port = 587;
            client.EnableSsl = true;
            client.Timeout = 30000;
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
