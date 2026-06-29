using Android.App;
using Android.Content.PM;
using Android.OS;

namespace QrCommerce.Android.Platforms.AndroidPlatform;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
public class MainActivity : MauiAppCompatActivity
{
}
