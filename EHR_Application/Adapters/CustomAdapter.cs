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
using Java.Lang;
using Com.Github.Library.Bubbleview;

namespace EHR_Application
{
    public class CustomAdapter : Android.Widget.BaseAdapter
    {
        public List<chatMessages> lstChat;
        public Context context;
        public LayoutInflater inflater;
        

        public CustomAdapter(List<chatMessages> lstChat, Context context)        
        {
            this.lstChat = lstChat;
            this.context = context;
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);    // tsekare to ksana
        }
        
        public override int Count {
            get
            {
                return lstChat.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (convertView == null)
            {
                if (lstChat[position].IsSend == true)
                    view = inflater.Inflate(Resource.Layout.list_send, null);
                else
                    view = inflater.Inflate(Resource.Layout.list_recv, null);
            }
            BubbleTextView bubbleText = view.FindViewById<BubbleTextView>(Resource.Id.bubbleChat);
            bubbleText.Text = lstChat[position].ChatMessage;
            return view;
            
        }
    }
}

