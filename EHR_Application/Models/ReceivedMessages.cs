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
    class ReceivedMessages
    {
      
        public int id { get; set; }
        public int PersonID { get; set; }
        public int ReseiverID { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public bool IsMe { get; set; }

    }
}