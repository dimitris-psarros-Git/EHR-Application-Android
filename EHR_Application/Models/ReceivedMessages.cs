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
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public int? IsMe { get; set; }
    }
}