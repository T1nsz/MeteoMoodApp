using System.Globalization;

namespace MeteoMoodApp.Converters
{
    public class UnixTimestampToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long unixTime)
            {
                var dt = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                return "Updated: " + dt.ToString("HH:mm", culture);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
