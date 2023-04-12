using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Core;
using Xamarin.Essentials;
using Core.DB;
using Core.Models;
using static Android.Media.MicrophoneInfo;

namespace PhoneWordAndroid
{
    [Activity(Label = "Phone Word", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private CancellationTokenSource cts;
        private double currentLat;
        private double currentLng;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            SetupUI();

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetupUI()
        {
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            TextView translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneWord);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button translationHistoryButton = FindViewById<Button>(Resource.Id.TranslationHistoryButton);
            Button getCoordinatesButton = FindViewById<Button>(Resource.Id.BtnGetCoordinates);
            TextView lat = FindViewById<TextView>(Resource.Id.latitude);
            TextView lon = FindViewById<TextView>(Resource.Id.longitude);
            Button openMaps = FindViewById<Button>(Resource.Id.BtnOpenMaps);

            string translatedNumber = string.Empty;

            translateButton.Click += (sender, e) =>
            {
                translatedNumber = Core.PhoneTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(translatedNumber))
                {
                    translatedPhoneWord.Text = string.Empty;
                }
                else
                {
                    translatedPhoneWord.Text = translatedNumber;                 
                    var db = new DatabaseManager(new DB.PathManager());
                    db.StoreData(new PhoneNumber() { phoneNumber = translatedNumber }, Core.Util.Enums.DBModels.User.ToString());
                    translationHistoryButton.Enabled = true;
                }
            };

            translationHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(TranslateHistoryActivity));
                StartActivity(intent);
            };

            getCoordinatesButton.Click += async (sender, e) =>
            {
                var coordinates = await GetCurrentLocation();
                if (coordinates.Success)
                {
                    lat.Text = $"Latitude: {coordinates.Value.Latitude.ToString()}";
                    lon.Text = $"Longitude: {coordinates.Value.Longitude.ToString()}";
                } else
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }
    }
}
