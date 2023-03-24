using Core;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace PhoneWordiOS
{
    public partial class ViewController : UIViewController
    {
        //static readonly List<string> phoneNumbers = new List<string>();
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
                    //phoneNumbers.Add(translatedNumber);
                    var db = new Core.DB.DatabaseManager(new DB.PathManager());
                    db.CreatePhoneNumber(new Core.Models.PhoneNumber() { phoneNumber = translatedNumber });
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
                //var callHistory =
                  //    (CallHistoryController)this.Storyboard.InstantiateViewController("mainNavController");
                if (callHistory != null)
                {
                    //callHistory.phoneNumbers = phoneNumbers;
                    this.NavigationController.PushViewController(callHistory, true);
                }
            };
        }
    }
}
