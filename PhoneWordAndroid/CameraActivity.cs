
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net;
using Java.IO;
using System.IO;
using Android.Hardware.Lights;
using static Android.Icu.Text.ListFormatter;

namespace PhoneWordAndroid
{
	[Activity (Label = "CameraActivity")]			
	public class CameraActivity : Activity
	{
        private Java.IO.File _dir;
        private Java.IO.File _file;
        private ImageView _imageView;
        private Bitmap bitmapImage;

        [Obsolete]
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bitmap = (Bitmap)data.Extras.Get("data");
            _imageView.SetImageBitmap(bitmap);
        }

        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            SetContentView(Resource.Layout.activity_camera);

            Button cameraButton = FindViewById<Button>(Resource.Id.BtnOpenCameraIntent);
            _imageView = FindViewById<ImageView>(Resource.Id.IvCameraPhoto);

            cameraButton.Click += TakeAPicture;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            StartActivityForResult(intent, 0);
        }
    }
}

