using System.Diagnostics;
using Newtonsoft.Json;
using MeteoMoodApp.Models;
using System.Text;

namespace MeteoMoodApp.Services
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<WeatherData> GetWeatherData(string query)
        {
            WeatherData weatherData = null;

            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return weatherData;
        }
    } 
}
        //public async Task<string> GenerateAsync(string query, string prompt, string pipeline, int images = 1, int width = 1024, int height = 1024)
        //{
        //    var generateParams = new
        //    {
        //        type = "GENERATE",
        //        numImages = images,
        //        width = width,
        //        height = height,
        //        generateParams = new
        //        {
        //            query = prompt
        //        }
        //    };

        //    var formData = new MultipartFormDataContent
        //{
        //    { new StringContent(pipeline), "pipeline_id" },
        //    { new StringContent(JsonConvert.SerializeObject(generateParams), Encoding.UTF8, "application/json"), "params" }
        //};

        //    var response = await _client.PostAsync(_url + "key/api/v1/pipeline/run", formData);
        //    var responseData = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<GenerationResult>(responseData);
        //    return result.Uuid;
        //}

        //public async Task<List<string>> CheckGenerationAsync(string requestId, int attempts = 10, int delay = 10000)
        //{
        //    List<string> files = null;
        //    for (int i = 0; i < attempts; i++)
        //    {
        //        var response = await _client.GetStringAsync(_url + "key/api/v1/pipeline/status/" + requestId);
        //        var data = JsonConvert.DeserializeObject<GenerationStatus>(response);

        //        if (data.Status == "DONE")
        //        {
        //            files = data.Result.Files;
        //            break;
        //        }

        //        await Task.Delay(delay);
        //    }
        //    return files;
        //}

        //public async Task<List<ReverseGeocodingResponse>> GetLocationData(string query)
        //{
        //    List<ReverseGeocodingResponse> locationData = null;

        //    try
        //    {
        //        var response = await _client.GetAsync(query);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            locationData = JsonConvert.DeserializeObject<List<ReverseGeocodingResponse>>(content);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return locationData;
        //}

