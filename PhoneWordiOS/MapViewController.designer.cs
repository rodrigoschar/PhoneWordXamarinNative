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
	[Register ("MapViewController")]
	partial class MapViewController
	{
		[Outlet]
		UIKit.UIButton getCoordinatesButton { get; set; }

		[Outlet]
		UIKit.UILabel latitudeLabel { get; set; }

		[Outlet]
		UIKit.UILabel latitudeNativeLabel { get; set; }

		[Outlet]
		UIKit.UILabel LongitudeLabel { get; set; }

		[Outlet]
		UIKit.UILabel longitudNativeLabel { get; set; }

		[Outlet]
		UIKit.UIButton openMapButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (getCoordinatesButton != null) {
				getCoordinatesButton.Dispose ();
				getCoordinatesButton = null;
			}

			if (latitudeLabel != null) {
				latitudeLabel.Dispose ();
				latitudeLabel = null;
			}

			if (LongitudeLabel != null) {
				LongitudeLabel.Dispose ();
				LongitudeLabel = null;
			}

			if (openMapButton != null) {
				openMapButton.Dispose ();
				openMapButton = null;
			}

			if (latitudeNativeLabel != null) {
				latitudeNativeLabel.Dispose ();
				latitudeNativeLabel = null;
			}

			if (longitudNativeLabel != null) {
				longitudNativeLabel.Dispose ();
				longitudNativeLabel = null;
			}
		}
	}
}
