using System;
using Android.Views;
using System.Collections.Generic;
using AndroidX.RecyclerView.Widget;
using static PhoneWordAndroid.TranslationHistory;

namespace PhoneWordAndroid
{
	public class TranslationHistoryAdapter : RecyclerView.Adapter
    {
        public List<TranslationHistory> translationHistory;
        public TranslationHistoryAdapter(List<TranslationHistory> history)
        {
            translationHistory = history;
        }

        public override int ItemCount
        {
            get { return translationHistory.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            HistoryViewHolder vh = holder as HistoryViewHolder;
            vh.historyText.Text = translationHistory[position].history;
            vh.dateText.Text = translationHistory[position].historyDate;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.HistoryCard, parent, false);
            HistoryViewHolder vh = new HistoryViewHolder(itemView);
            return vh;
        }
    }
}

