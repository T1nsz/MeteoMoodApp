namespace MeteoMoodApp.Interfaces
{
    public interface IWallpaperService
    {
        Task SetWallpaperFromBase64(string base64Image);
    }
}