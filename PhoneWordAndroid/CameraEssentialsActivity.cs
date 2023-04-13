
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;

namespace PhoneWordAndroid
{
    [Activity(Label = "CameraEssentialsActivity")]
    public class CameraEssentialsActivity : Activity
    {
        private ImageView _imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_camera_essentials);

            _imageView = FindViewById<ImageView>(Resource.Id.essentialsImageView);
            Button cameraEssentialsButton = FindViewById<Button>(Resource.Id.btnOpenCameraEssentials);
            cameraEssentialsButton.Click += TakeAPictureWithEssentials;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void TakeAPictureWithEssentials(object sender, EventArgs eventArgs)
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                var bytes = ms.ToArray();

                var bitmap = bytesToUIImage(bytes);
                if (bitmap != null)
                {
                    _imageView.SetImageBitmap(bitmap);
                }
            }
        }

        public static Bitmap bytesToUIImage(byte[] bytes)
        {
            if (bytes == null)
                return null;

            Bitmap bitmap;


            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            //Create a folder for the images if not exists
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(documentsFolder, "images"));

            string imatge = System.IO.Path.Combine(documentsFolder, "images", "image.jpg");


            System.IO.File.WriteAllBytes(imatge, bytes.Concat(new Byte[] { (byte)0xD9 }).ToArray());

            bitmap = BitmapFactory.DecodeFile(imatge);

            return bitmap;
        }
    }
}

