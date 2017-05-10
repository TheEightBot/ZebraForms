using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;

namespace ZebraForms.Droid
{
    [Activity(Theme = "@style/MyTheme", MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            try {
                var emdk = Java.Lang.Class.ForName("com.symbol.emdk.EMDKManager");
                Xamarin.Forms.DependencyService.Register<IScanning, ZebraScanning>();
            } catch (ClassNotFoundException) {
                ZXing.Mobile.MobileBarcodeScanner.Initialize(this.Application);
                Xamarin.Forms.DependencyService.Register<IScanning, ZxingScanning>();
            }

            LoadApplication(new App());
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult (requestCode, permissions, grantResults);           
        }
    }
}
