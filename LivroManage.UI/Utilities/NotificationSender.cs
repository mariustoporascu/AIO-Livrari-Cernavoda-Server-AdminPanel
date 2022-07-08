using FirebaseAdmin.Messaging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LivroManage.UI.Utilities
{
    public class NotificationSender
    {
        public NotificationSender() { }
        public async Task SendNotif(string apiKey, string appId, string userToken, string msg)
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
