using System;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace PhoneWordAndroid
{
	public class TranslationHistory
	{
        public string history { get; set; }
        public string historyDate { get; set; }

        public class HistoryViewHolder : RecyclerView.ViewHolder
        {
            public TextView historyText { get; set; }
            public TextView dateText { get; set; }
            public HistoryViewHolder(View itemView) : base(itemView)
            {
                historyText = itemView.FindViewById<TextView>(Resource.Id.tv_historyName);
                dateText = itemView.FindViewById<TextView>(Resource.Id.tv_historyDate);
            }
        }
    }
}

