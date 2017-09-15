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

namespace EHR_Application.Models
{
    class imageIDs
    {
        public int DataSenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstName1 { get; set; }
        public string LastName1 { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public int Sender { get; set; }
    }
}