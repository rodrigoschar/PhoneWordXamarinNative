// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PhoneWordiOS
{
	[Register ("CameraEssentialsViewController")]
	partial class CameraEssentialsViewController
	{
		[Outlet]
		UIKit.UIButton openCameraButton { get; set; }

		[Outlet]
		UIKit.UIImageView photoImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (photoImageView != null) {
				photoImageView.Dispose ();
				photoImageView = null;
			}

			if (openCameraButton != null) {
				openCameraButton.Dispose ();
				openCameraButton = null;
			}
		}
	}
}
