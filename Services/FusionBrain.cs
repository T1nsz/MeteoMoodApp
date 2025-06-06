using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace MeteoMoodApp.Services
{
    public class FusionBrainAPI
    {
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public FusionBrainAPI(string url, string apiKey, string secretKey)
        {
            _url = url;
            _apiKey = apiKey;
            _secretKey = secretKey;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Key", $"Key {apiKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Secret", $"Secret {secretKey}");
        }

        public async Task<string> GetPipelineAsync()
        {
            var response = await _httpClient.GetStringAsync(_url + "key/api/v1/pipelines");
            var data = JsonConvert.DeserializeObject<List<Pipeline>>(response);
            return data[0].Id;
        }

        public async Task<string> GenerateAsync(string prompt, string pipeline, int images = 1, int width = 1024, int height = 1024)
        {
            var generateParams = new
            {
                type = "GENERATE",
                numImages = images,
                width = width,
                height = height,
                generateParams = new
                {
                    query = prompt
                }
            };

            var formData = new MultipartFormDataContent
                {
                    { new StringContent(pipeline), "pipeline_id" },
                    { new StringContent(JsonConvert.SerializeObject(generateParams), Encoding.UTF8, "application/json"), "params" }
                };

            var response = await _httpClient.PostAsync(_url + "key/api/v1/pipeline/run", formData);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenerationResult>(responseData);
            return result.Uuid;
        }

        public async Task<List<string>> CheckGenerationAsync(string requestId, int attempts = 10, int delay = 10000)
        {
            List<string> files = null;
            for (int i = 0; i < attempts; i++)
            {
                var response = await _httpClient.GetStringAsync(_url + "key/api/v1/pipeline/status/" + requestId);
                var data = JsonConvert.DeserializeObject<GenerationStatus>(response);

                if (data.Status == "DONE")
                {
                    files = data.Result.Files;
                    break;
                }

                await Task.Delay(delay);
            }
            return files;
        }

        private class Pipeline
        {
            public string Id { get; set; }
        }

        private class GenerationResult
        {
            public string Uuid { get; set; }
        }

        private class GenerationStatus
        {
            public string Status { get; set; }
            public Result Result { get; set; }
        }

        private class Result
        {
            public List<string> Files { get; set; }
        }
    }
}
