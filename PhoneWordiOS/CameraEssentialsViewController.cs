using System;
using UIKit;
using Xamarin.Essentials;
using GlobalToast;
using System.IO;
using System.Drawing;
using System.Linq;

namespace PhoneWordiOS
{
	public partial class CameraEssentialsViewController : UIViewController
	{
		public CameraEssentialsViewController(IntPtr handle) : base(handle)
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
			// Release any cached data, images, etc that aren't in use.
		}

        private void SetupUI()
        {
            
            openCameraButton.TouchUpInside += async (object sender, EventArgs e) =>
            {
                var result = await MediaPicker.CapturePhotoAsync();

                if (result != null)
                {
                    UIImage myImg = UIImage.FromFile(result.FullPath);
                    photoImageView.Image = myImg;
                } else
                {
                    Toast.MakeToast("Could not get data").Show();
                }
                
            };
        }
    }
}


