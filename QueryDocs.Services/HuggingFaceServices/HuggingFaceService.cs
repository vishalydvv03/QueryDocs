using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QueryDocs.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QueryDocs.Services.HuggingFaceServices
{
    public class HuggingFaceService : IHuggingFaceService
    {
        private readonly IHttpClientFactory httpClient;
        private readonly HuggingFaceSettings hfSettings;

        public HuggingFaceService(IHttpClientFactory httpClient,
                                  IOptions<HuggingFaceSettings> options)
        {
            this.httpClient = httpClient;
            this.hfSettings = options.Value;
        }

        public async Task<float[]> CreateEmbeddingsFromHuggingFace(string text)
        {
            float[] embedding = Array.Empty<float>();
            var client = httpClient.CreateClient("HuggingFaceClient");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", hfSettings.ApiKey);

            var payload = new { inputs = text };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(hfSettings.ModelEndpoint, content);

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                var doubleArray = JsonConvert.DeserializeObject<double[]>(json) ?? Array.Empty<double>();
                embedding = doubleArray.Select(d => (float)d).ToArray();
            }
            return embedding;
        }
    }
}
