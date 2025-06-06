using Android.App;
using Android.Content;
using Android.Graphics;
using MeteoMoodApp.Platforms.Android;
using MeteoMoodApp.Interfaces;
using Application = Android.App.Application;
using Android.OS;

namespace MeteoMoodApp.Platforms.Android
{
    public class WallpaperService : IWallpaperService
    {
        public async Task SetWallpaperFromBase64(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            string fileName = System.IO.Path.Combine(FileSystem.CacheDirectory, "wallpaper.png");

            await File.WriteAllBytesAsync(fileName, imageBytes);

            var wallpaperManager = (WallpaperManager)Application.Context.GetSystemService(Context.WallpaperService);

            using (var inputStream = new FileStream(fileName, FileMode.Open))
            using (var bitmap = BitmapFactory.DecodeStream(inputStream))
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    wallpaperManager.SetBitmap(bitmap, null, true, WallpaperManagerFlags.System);
                }
                else
                {
                    wallpaperManager.SetBitmap(bitmap);
                }
            }
        }
    }
}
