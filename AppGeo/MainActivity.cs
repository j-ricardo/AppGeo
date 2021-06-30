using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace AppGeo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView tvDados;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            tvDados = (TextView)FindViewById(Resource.Id.tvDados);
            tvDados.Click += TvDados_Click;
            tvDados.Text = "Clique aqui para buscar localização";
        }

        private async void TvDados_Click(object sender, EventArgs e)
        {
            await RequestLocationAsync();
        }

        private async Task RequestLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();//.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Position position = new Position(location.Latitude, location.Longitude);
                    var possivelEnderecos = await new Geocoder().GetAddressesForPositionAsync(position);
                    var endereco = possivelEnderecos.FirstOrDefault(); //.ToString();
                    tvDados.Text = $"Latitude: {location.Latitude}\nLongitude: {location.Longitude}\nAltitude: {location.Altitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}