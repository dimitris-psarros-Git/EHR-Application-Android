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
    class NewMessages
    {
        public int DataSenderID { get; set; }
        public Nullable<int> PatientID { get; set; }
        public Nullable<int> DoctorID { get; set; }
        public Nullable<int> Sender { get; set; }
        public string Text { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Seen { get; set; }
        public Nullable<bool> Send { get; set; }
    }
}