
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Core.DB;
using Core.Models;
using Android.Locations;
using Android.Nfc;
using static Android.Media.MicrophoneInfo;
using AndroidX.Core.Content;
using Android;
using Android.Content.PM;
using AndroidX.Core.App;

namespace PhoneWordAndroid
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, ILocationListener
    {
        private CancellationTokenSource cts;
        private double currentLat;
        private double currentLng;
        private TextView txtLatNat;
        private TextView txtLngNat;
        private Android.Locations.Location currentLocation;
        private LocationManager locationManager;
        string locationProvider;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_map);
            SetupUI();
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetupUI()
        {
            Button getCoordinatesButton = FindViewById<Button>(Resource.Id.BtnGetCoordinates);
            TextView lat = FindViewById<TextView>(Resource.Id.latitude);
            TextView lon = FindViewById<TextView>(Resource.Id.longitude);
            Button openMaps = FindViewById<Button>(Resource.Id.BtnOpenMaps);

            Button getCoordinatesNativeButton = FindViewById<Button>(Resource.Id.btnGetCoordinatesNative);
            txtLatNat = FindViewById<TextView>(Resource.Id.latitudeNative);
            txtLngNat = FindViewById<TextView>(Resource.Id.longitudeNative);
            InitializeLocationManager();

            if (!(ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted))
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, 1);
            }

            getCoordinatesButton.Click += async (sender, e) =>
            {
                var coordinates = await GetCurrentLocation();
                if (coordinates.Success)
                {
                    lat.Text = $"Latitude: {coordinates.Value.Latitude.ToString()}";
                    lon.Text = $"Longitude: {coordinates.Value.Longitude.ToString()}";
                }
                else
                {
                    Toast.MakeText(this, coordinates.Error, ToastLength.Short).Show();
                }
            };

            openMaps.Click += async (sender, e) =>
            {
                if (currentLat == 0 && currentLng == 0)
                {
                    var coordinates = await GetCurrentLocation();
                    if (coordinates.Success)
                    {
                        await Map.OpenAsync(coordinates.Value.Latitude, coordinates.Value.Longitude, new MapLaunchOptions
                        {
                            Name = "Current Location",
                            NavigationMode = NavigationMode.None
                        });
                    }
                }
                else
                {
                    await Map.OpenAsync(currentLat, currentLng, new MapLaunchOptions
                    {
                        Name = "Current Location",
                        NavigationMode = NavigationMode.None
                    });
                }
            };

            getCoordinatesNativeButton.Click += (sender, e) =>
            {

            };
        }

        private async Task<Result<Coordinates>> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    var coordinates = new Coordinates() { Id = 1, Latitude = location.Latitude, Longitude = location.Longitude, Altitude = location.Longitude };
                    return Core.DB.Result.Ok<Coordinates>(coordinates);
                }
                else
                {
                    return Core.DB.Result.Fail<Coordinates>("could not get location");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                return Core.DB.Result.Fail<Coordinates>(fnsEx.ToString());
            }
            catch (FeatureNotEnabledException fneEx)
            {
                return Core.DB.Result.Fail<Coordinates>(fneEx.ToString());
            }
            catch (PermissionException pEx)
            {
                return Core.DB.Result.Fail<Coordinates>(pEx.ToString());
            }
            catch (Exception ex)
            {
                return Core.DB.Result.Fail<Coordinates>(ex.ToString());
            }
        }

        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                Toast.MakeText(this, "Error trying to get the current location", ToastLength.Short).Show();
            }
            else
            {
                txtLatNat.Text = currentLocation.Latitude.ToString();
                txtLngNat.Text = currentLocation.Longitude.ToString();
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}

