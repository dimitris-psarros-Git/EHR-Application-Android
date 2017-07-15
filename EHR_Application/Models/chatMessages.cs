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

namespace EHR_Application
{
    public  class chatMessages
    {

        public string ChatMessage { get; set; }
        public DateTime Date { get; set; }
        public bool IsSend { get; set; }


    }
}