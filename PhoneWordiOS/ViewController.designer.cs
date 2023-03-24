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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton callButton { get; set; }

		[Outlet]
		UIKit.UIButton callHistoryButton { get; set; }

		[Outlet]
		UIKit.UITextField phoneNumberTextField { get; set; }

		[Outlet]
		UIKit.UIButton translateButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (phoneNumberTextField != null) {
				phoneNumberTextField.Dispose ();
				phoneNumberTextField = null;
			}

			if (translateButton != null) {
				translateButton.Dispose ();
				translateButton = null;
			}

			if (callButton != null) {
				callButton.Dispose ();
				callButton = null;
			}

			if (callHistoryButton != null) {
				callHistoryButton.Dispose ();
				callHistoryButton = null;
			}
		}
	}
}
