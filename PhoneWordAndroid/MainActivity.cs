using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Core;
using Core.DB;
using Core.Models;

namespace PhoneWordAndroid
{
    [Activity(Label = "Phone Word", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            SetupUI();

        }

        private void SetupUI()
        {
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            TextView translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneWord);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button translationHistoryButton = FindViewById<Button>(Resource.Id.TranslationHistoryButton);
            Button goToLocationButton = FindViewById<Button>(Resource.Id.BtnOpenMaps);

            string translatedNumber = string.Empty;

            translateButton.Click += (sender, e) =>
            {
                translatedNumber = Core.PhoneTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(translatedNumber))
                {
                    translatedPhoneWord.Text = string.Empty;
                }
                else
                {
                    translatedPhoneWord.Text = translatedNumber;                 
                    var db = new DatabaseManager(new DB.PathManager());
                    db.StoreData(new PhoneNumber() { phoneNumber = translatedNumber }, Core.Util.Enums.DBModels.User.ToString());
                    translationHistoryButton.Enabled = true;
                }
            };

            translationHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(TranslateHistoryActivity));
                StartActivity(intent);
            };

            goToLocationButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(MapActivity));
                StartActivity(intent);
            };
        }
    }
}
