using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using App.Models;
using Newtonsoft.Json;

namespace App.Helpers
{
    public class DogApiRequestHelper
    {
        HttpClient httpClient = new();

        public DogApiRequestHelper()
        {
            httpClient.BaseAddress = new Uri("https://api.thedogapi.com");

            string key = "694b49e3-4905-4c08-bab9-26ea3e62ec56";
            httpClient.DefaultRequestHeaders.Add("x-api-key", key);
        }

        public async Task<List<DogInfo>> GetDogsAsync()
        {
            string endPoint = "v1/breeds?limit=5";
            var response = await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<DogInfo>>(content);
            }

            return new List<DogInfo>();
        }

        public async Task<List<DogInfo>> GetDogsByNaneAsync(string raca)
        {
            string endPoint = $"v1/breeds/search?q={raca}";
            var response = await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DogInfo>>(content);
            }

            return new List<DogInfo>();
        }

        public async Task<List<DogImages>> GetImagesAsync()
        {
            string endPoint = $"v1/images/search?limit=5";
            var response = await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DogImages>>(content);
            }

            return new List<DogImages>();
        }
    }
}

