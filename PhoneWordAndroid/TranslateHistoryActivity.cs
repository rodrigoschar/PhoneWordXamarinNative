
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace PhoneWordAndroid
{
    [Activity(Label = "@string/translationHistory")]
    public class TranslateHistoryActivity : Activity
    {
        RecyclerView recyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        List<TranslationHistory> historyList;
        TranslationHistoryAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_translation_history);

            var db = new Core.DB.DatabaseManager(new DB.PathManager());
            var phoneNumbers = db.GetAllData<Core.Models.PhoneNumber>();

            historyList = new List<TranslationHistory>();
            if (phoneNumbers.Success)
            {
                foreach (Core.Models.PhoneNumber item in phoneNumbers.Value)
                {
                    historyList.Add(new TranslationHistory() { history = item.phoneNumber });
                }
            }

            recyclerView = FindViewById<RecyclerView>(Resource.Id.rv_translation_history);
            mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);
            adapter = new TranslationHistoryAdapter(historyList);
            recyclerView.SetAdapter(adapter);
        }
    }
}

