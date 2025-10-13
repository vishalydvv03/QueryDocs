using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QueryDocs.Domain.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace QueryDocs.Services.OpenRouterServices
{
    public class OpenRouterService : IOpenRouterService
    {
        private readonly IHttpClientFactory httpClient;
        private readonly OpenRouterSettings openRouterSettings;

        public OpenRouterService(IHttpClientFactory httpClient, IOptions<OpenRouterSettings> options)
        {
            this.httpClient = httpClient;
            this.openRouterSettings = options.Value;
        }

        public async Task<string?> CompleteChat(string message)
        {
            string? resultText = string.Empty;

            var client = httpClient.CreateClient("OpenRouterClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openRouterSettings.ApiKey);

            var payload = new
            {
                model = openRouterSettings.Model,
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    dynamic? jsonResult = JsonConvert.DeserializeObject(json);
                    resultText = jsonResult?.choices[0].message.content;
                }
            }
            return resultText;
        }

    }
}

