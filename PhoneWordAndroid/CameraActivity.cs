
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(_file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            int height = _imageView.Height;
            int width = Resources.DisplayMetrics.WidthPixels;
            Bitmap bitmap = BitmapFactory.DecodeFile(_file.Path);
            var bitmapScalled = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            bitmap.Recycle();
            _imageView.SetImageBitmap(bitmap);

            // display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.
            /*int height = _imageView.Height;
            int width = Resources.DisplayMetrics.WidthPixels;
            using (Bitmap bitmap = _file.Path.LoadAndResizeBitmap(width, height))
            {
                _imageView.RecycleBitmap();
                _imageView.SetImageBitmap(bitmap);

                string filePath = _file.Path;
            }*/
        }

        protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            SetContentView(Resource.Layout.activity_camera);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button cameraButton = FindViewById<Button>(Resource.Id.BtnOpenCameraIntent);
                _imageView = FindViewById<ImageView>(Resource.Id.IvCameraPhoto);

                cameraButton.Click += TakeAPicture;
            }
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            _file = new Java.IO.File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));

            StartActivityForResult(intent, 0);
        }
    }
}

