using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;
using ToDo.Core;
using ToDo.UI;

namespace ToDo.Droid
{
    [Activity(
        Label = "ToDo.Droid",
        Theme = "@style/MyTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, App, FormsApp>
    {
        protected override void OnCreate(Bundle bundle)
        {
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, bundle);
            base.OnCreate(bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}