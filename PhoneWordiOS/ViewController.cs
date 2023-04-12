using Core;
using Core.DB;
using Core.Models;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;
using GlobalToast;

namespace PhoneWordiOS
{
    public partial class ViewController : UIViewController
    {
        private CancellationTokenSource cts;
        private double currentLat;
        private double currentLng;

        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            SetupUI();
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
        }

        private void SetupUI()
        {
            string translatedNumber = "";

            translateButton.TouchUpInside += (object sender, EventArgs e) => {
                translatedNumber = PhoneTranslator.ToNumber(
                    phoneNumberTextField.Text);

                phoneNumberTextField.ResignFirstResponder();

                if (translatedNumber == "")
                {
                    callButton.SetTitle("Call ", UIControlState.Normal);
                    callButton.Enabled = false;
                }
                else
                {
                    var db = new Core.DB.DatabaseManager(new DB.PathManager());
                    db.StoreData(new Core.Models.PhoneNumber() { phoneNumber = translatedNumber }, Core.Util.Enums.DBModels.User.ToString());
                    callButton.SetTitle("Call " + translatedNumber,
                        UIControlState.Normal);
                    callButton.Enabled = true;
                }
            };

            callButton.TouchUpInside += (object sender, EventArgs e) => {
                var url = new NSUrl("tel:" + translatedNumber);

                if (!UIApplication.SharedApplication.OpenUrl(url))
                {
                    var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
            };

            callHistoryButton.TouchUpInside += (object sender, EventArgs e) => {
                CallHistoryController callHistory = this.Storyboard.InstantiateViewController("CallHistoryController") as CallHistoryController;
                if (callHistory != null)
                {
                    this.NavigationController.PushViewController(callHistory, true);
                }
            };

            getCoordinatesButton.TouchUpInside += async (object sender, EventArgs e) =>
            {
                var coordinates = await GetCurrentLocation();
                if (coordinates.Success)
                {
                    currentLat = coordinates.Value.Latitude;
                    currentLng = coordinates.Value.Longitude;
                    latitudeLabel.Text = $"Latitude: {coordinates.Value.Latitude.ToString()}";
                    LongitudeLabel.Text = $"Longitude: {coordinates.Value.Longitude.ToString()}";
                }
                else
                {
                    Toast.MakeToast(coordinates.Error).Show();
                }
            };

            openMapsButton.TouchUpInside += async (object sender, EventArgs e) =>
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
                } else
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
    }
}
