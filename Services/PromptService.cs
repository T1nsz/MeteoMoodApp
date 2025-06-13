using GeoTimeZone;
using TimeZoneConverter;
using MeteoMoodApp.Models;

namespace MeteoMoodApp.Services
{
    class PromptService
    {

        public static class PromptGenerator
        {
            public static string GeneratePromptFromWeather(WeatherData data)
            {
                var weather = data.WeatherConditions?.FirstOrDefault();
                var main = data.MainInfo;
                var wind = data.WindInfo;
                var clouds = data.CloudInfo;
                var sys = data.SystemInfo;

                string weatherDescription = weather?.Description ?? "clear sky";
                string temperature = $"Temperature around {Math.Round(main.Temperature)}°C, feels like {Math.Round(main.FeelsLike)}°C";
                string windDescription = wind != null ? $"light wind at {wind.Speed} m/s" : "";
                string cloudDescription = clouds != null && clouds.All > 50 ? "many clouds in the sky" :
                                           clouds != null && clouds.All > 10 ? "a few scattered clouds" :
                                           "a clear sky";

                // Правильное локальное время
                var currentTime = ConvertToLocalTime(data.Timestamp, data.Coordinates.Latitude, data.Coordinates.Longitude);
                var sunriseTime = ConvertToLocalTime(sys.Sunrise, data.Coordinates.Latitude, data.Coordinates.Longitude).TimeOfDay;
                var sunsetTime = ConvertToLocalTime(sys.Sunset, data.Coordinates.Latitude, data.Coordinates.Longitude).TimeOfDay;

                string timeOfDay = GetTimeOfDay(currentTime.TimeOfDay, sunriseTime, sunsetTime);
                string season = GetSeason(currentTime, data.Coordinates.Latitude);

                string baseScene = season switch
                {
                    "winter" => "Snowy nature scenery with white fields at the bottom, snow-covered forests and small houses in the distance",
                    "spring" => "Fresh nature scenery with blooming fields, forests starting to grow leaves, and cozy houses in the distance",
                    "summer" => "Green nature scenery with fields full of grass, dense forests, and small rural houses in the distance",
                    "autumn" => "Autumn nature scenery with orange and yellow trees, harvested fields, and distant houses under cloudy skies",
                    _ => "Nature scenery with open fields, forests and small houses in the distance"
                };

                return $"{timeOfDay} {weatherDescription}, {temperature}, {cloudDescription}, {windDescription}. {baseScene}.";
            }

            private static DateTimeOffset ConvertToLocalTime(long timestamp, double latitude, double longitude)
            {
                var timeZoneId = TimeZoneLookup.GetTimeZone(latitude, longitude).Result; // e.g., "Asia/Yekaterinburg"
                var tzInfo = TZConvert.GetTimeZoneInfo(timeZoneId);
                var utcTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzInfo);
                return new DateTimeOffset(localTime);
            }

            private static string GetTimeOfDay(TimeSpan currentTime, TimeSpan sunrise, TimeSpan sunset)
            {
                if (currentTime < sunrise)
                    return "Before sunrise:";
                if (currentTime >= sunrise && currentTime < TimeSpan.FromHours(10))
                    return "Early morning:";
                if (currentTime >= TimeSpan.FromHours(10) && currentTime < TimeSpan.FromHours(17))
                    return "Afternoon:";
                if (currentTime >= TimeSpan.FromHours(17) && currentTime < sunset)
                    return "Evening:";
                return "Night time:";
            }

            private static string GetSeason(DateTimeOffset dateTime, double latitude)
            {
                int month = dateTime.Month;
                bool isNorthernHemisphere = latitude >= 0;

                return month switch
                {
                    12 or 1 or 2 => isNorthernHemisphere ? "winter" : "summer",
                    3 or 4 or 5 => isNorthernHemisphere ? "spring" : "autumn",
                    6 or 7 or 8 => isNorthernHemisphere ? "summer" : "winter",
                    9 or 10 or 11 => isNorthernHemisphere ? "autumn" : "spring",
                    _ => "unknown"
                };
            }
        }
    }
}
