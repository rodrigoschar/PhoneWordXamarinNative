using Core;
using Core.DB;
using Core.Models;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace PhoneWordiOS
{
    public partial class ViewController : UIViewController
    {
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

            goToLocationButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                MapViewController mapView = this.Storyboard.InstantiateViewController("MapViewController") as MapViewController;
                if (mapView != null)
                {
                    this.NavigationController.PushViewController(mapView, true);
                }
            };
        }
    }
}
