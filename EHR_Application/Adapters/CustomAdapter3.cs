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
using EHR_Application.Models;

namespace EHR_Application.Adapters
{
    class CustomAdapter3 : BaseAdapter<Allergies>
    {
        public List<Allergies> mItems;
        private Context mContext;

        public CustomAdapter3(Context context, List<Allergies> items)
        {
            mItems = items;
            mContext = context;
        }

        public override Allergies this[int position]
        {
            get { return mItems[position]; }
        }

        public override int Count
        {
            get { return mItems.Count;  }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.rowAllergieslayout,null,false);
            }
            TextView txtAllergy = row.FindViewById<TextView>(Resource.Id.txtAllergy);
            txtAllergy.Text = mItems[position].Allergy1;
            TextView txtReaction = row.FindViewById<TextView>(Resource.Id.txtReaction);
            txtReaction.Text = mItems[position].Reaction;
            TextView txtSeverity = row.FindViewById<TextView>(Resource.Id.txtseverity);
            txtSeverity.Text = mItems[position].Severity;

            return row;
        }
    }
}