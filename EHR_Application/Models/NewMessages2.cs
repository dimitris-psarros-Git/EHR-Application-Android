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
    class NewMessages2
    {
        public int DataSenderID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public Nullable<bool> Seen { get; set; }
    }
}