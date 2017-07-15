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
using Android.Support.V7.Widget;

namespace EHR_Application.Adapters
{
    public class RecyclerViewHolder : RecyclerView.ViewHolder //,View.IOnClickListener,View.IOnLongClickListener
    {
        public ImageView imageView;
        public TextView txtDescription;
        
        public RecyclerViewHolder(View itemView):base(itemView)
        {
            imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            txtDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);
            
        }
        
    }


    class RecyclerViewAdapter : RecyclerView.Adapter
    {
        private List<DataRecyclerView> lstData = new List<DataRecyclerView>();

        public RecyclerViewAdapter(List<DataRecyclerView> lstData)
        {
            this.lstData = lstData;
        }

        public override int ItemCount
        {
            get
            {
                return lstData.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
            //viewHolder.imageView.SetImageResource(lstData[position].imageId);
            viewHolder.imageView.SetImageDrawable(null);                           //tsekare thn allagh pou egine
            viewHolder.txtDescription.Text = lstData[position].description;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.RecyclerItemLayout, parent, false);
            return new RecyclerViewHolder(itemView);
        }
    }
}