using System;
using Foundation;
using UIKit;
using GlobalToast;
using Core.Models;

namespace PhoneWordiOS
{
	public partial class CameraViewController : UIViewController
	{
		static UIImagePickerController picker;
		static UIImageView statickImageView;

		public CameraViewController(IntPtr handle) : base(handle)
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
            statickImageView = this.cameraPhotoImageview;
			openCamaraButton.TouchUpInside += (object sender, EventArgs e) =>
			{
                if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
                    picker = new UIImagePickerController();
                    picker.Delegate = new CameraDelegate();
                    picker.SourceType = UIImagePickerControllerSourceType.Camera;
                    NavigationController.PresentViewController(picker, true, null);
                }
                else
                {
                    Toast.MakeToast("Camera is not available").Show();
                }
            };
        }

        class CameraDelegate : UIImagePickerControllerDelegate
        {
            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
            {
                picker.DismissModalViewController(true);
                var image = info.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
                CameraViewController.statickImageView.Image = image;
            }
        }
    }
}


