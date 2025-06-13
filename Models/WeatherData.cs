using Newtonsoft.Json;

namespace MeteoMoodApp.Models
{
    /// <summary>
    /// Представляет данные о погодных условиях для конкретного города, полученные из API.
    /// <para>
    /// Эти данные обычно приходят в формате JSON и содержат информацию о названии города, его координатах, погодных условиях, температуре и других параметрах.
    /// После получения JSON их десериализуют (парсят) в этот класс с помощью, например, библиотеки Newtonsoft.Json или System.Text.Json.
    /// </para>
    /// <para>
    /// Благодаря этим моделям в приложении можно легко получить доступ к любым необходимым параметрам погоды для отображения, анализа или дальнейшей обработки.
    /// </para>
    /// </summary>
    /// 
        public class WeatherData
        {
            [JsonProperty("name")]
            public string City { get; set; }

            [JsonProperty("coord")]
            public Coord Coordinates { get; set; }

            [JsonProperty("weather")]
            public List<Weather> WeatherConditions { get; set; }

            [JsonProperty("base")]
            public string Base { get; set; }

            [JsonProperty("main")]
            public Main MainInfo { get; set; }

            [JsonProperty("visibility")]
            public long Visibility { get; set; }

            [JsonProperty("wind")]
            public Wind WindInfo { get; set; }

            [JsonProperty("clouds")]
            public Clouds CloudInfo { get; set; }

            [JsonProperty("dt")]
            public long Timestamp { get; set; }

            [JsonProperty("sys")]
            public Sys SystemInfo { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("cod")]
            public long StatusCode { get; set; }
        }

        public class Clouds
        {
            [JsonProperty("all")]
            public int All { get; set; }
        }

        public class Coord
        {
            [JsonProperty("lon")]
            public double Longitude { get; set; }

            [JsonProperty("lat")]
            public double Latitude { get; set; }
        }

        public class Main
        {
            [JsonProperty("temp")]
            public double Temperature { get; set; }

            [JsonProperty("feels_like")]
            public double FeelsLike { get; set; }

            [JsonProperty("temp_min")]
            public double MinimumTemperature { get; set; }

            [JsonProperty("temp_max")]
            public double MaximumTemperature { get; set; }

            [JsonProperty("pressure")]
            public int Pressure { get; set; }

            [JsonProperty("humidity")]
            public int Humidity { get; set; }
        }

        public class Sys
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("sunrise")]
            public int Sunrise { get; set; }

            [JsonProperty("sunset")]
            public int Sunset { get; set; }
        }

        public class Weather
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("main")]
            public string Main { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }
        }

        public class Wind
        {
            [JsonProperty("speed")]
            public double Speed { get; set; }

            [JsonProperty("deg")]
            public int Direction { get; set; }

            [JsonProperty("gust")]
            public double Gust { get; set; }
        }



    public static class WeatherIconHelper
    {
        private static readonly Dictionary<int, string> WeatherIcons = new Dictionary<int, string>
    {
        

        { 200, "11d" }, // Thunderstorm
        { 201, "11d" },
        { 202, "11d" },
        { 210, "11d" },
        { 211, "11d" },
        { 212, "11d" },
        { 221, "11d" },
        { 230, "11d" },
        { 231, "11d" },
        { 232, "11d" },

        { 300, "09d" }, // Drizzle
        { 301, "09d" },
        { 302, "09d" },
        { 310, "09d" },
        { 311, "09d" },
        { 312, "09d" },
        { 313, "09d" },
        { 314, "09d" },
        { 321, "09d" },

        { 500, "10d" }, // Rain
        { 501, "10d" },
        { 502, "10d" },
        { 503, "10d" },
        { 504, "10d" },
        { 511, "13d" },
        { 520, "09d" },
        { 521, "09d" },
        { 522, "09d" },
        { 531, "09d" },

        { 600, "13d" }, // Snow
        { 601, "13d" },
        { 602, "13d" },
        { 611, "13d" },
        { 612, "13d" },
        { 613, "13d" },
        { 615, "13d" },
        { 616, "13d" },
        { 620, "13d" },
        { 621, "13d" },
        { 622, "13d" },

        { 701, "50d" }, // Mist
        { 711, "50d" },
        { 721, "50d" },
        { 731, "50d" },
        { 741, "50d" },
        { 751, "50d" },
        { 761, "50d" },
        { 771, "50d" },
        { 781, "50d" },

        { 800, "01" }, // Clear
        { 801, "02" }, // Few clouds
        { 802, "03" }, // Scattered clouds
        { 803, "04" }, // Broken clouds
        { 804, "04" } // Overcast clouds
    };

        public static string GetIconName(int weatherId, bool isNight)
        {
            // Для иконок с 800 по 804 добавляем 'd' или 'n' в зависимости от времени суток
            if (weatherId >= 800 && weatherId <= 804)
            {
                return WeatherIcons.TryGetValue(weatherId, out var iconName) ? iconName + (isNight ? "n" : "d") : "default.png";
            }

            // Для всех остальных значений просто возвращаем дневные иконки
            return WeatherIcons.TryGetValue(weatherId, out var dayIcon) ? dayIcon : "default.png";
        }
    }
}
