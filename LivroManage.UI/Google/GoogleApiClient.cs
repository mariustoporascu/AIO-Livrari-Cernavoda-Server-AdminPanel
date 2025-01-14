﻿using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace LivroManage.UI.Google
{
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
}
