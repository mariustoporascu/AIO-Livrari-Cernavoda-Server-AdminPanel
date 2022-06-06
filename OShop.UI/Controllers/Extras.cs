using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
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
            client.Host = "mail5002.smarterasp.net";
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
    public static class NotificationSender
    {
        public static async void SendNotif(string apiKey, string appId,string userToken, string msg)
        {
            try
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://onesignal.com/api/v1/notifications"),
                    Headers =
                        {
                            { "Accept", "application/json" },
                            { "Authorization", $"Basic {apiKey}" },
                        },
                    Content = new StringContent($"{{\"app_id\":\"{appId}\"," +
                    $"\"include_player_ids\":[\"{userToken}\"]," +
                    $"\"headings\":{{\"en\":\"Info Comenzi\"}}," +
                    $"\"contents\":{{\"en\":\"{msg}\"}}," +
                    $"\"priority\":10}}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(body);
                }
                var message = new Message()
                {
                    //Data = new Dictionary<string, string>()
                    //    {
                    //        { "myData", "1337" },
                    //    },
                    //Token = registrationToken,
                    Token = userToken,
                    Notification = new Notification()
                    {
                        Title = "Info Comenzi",
                        Body = msg,
                    },
                    Android = new AndroidConfig()
                    {
                        Notification = new AndroidNotification()
                        {
                            Title = "Info Comenzi",
                            Body = msg,
                            Sound = "default"
                        }
                    }

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
