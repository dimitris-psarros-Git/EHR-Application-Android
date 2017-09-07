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
    class SendMessages
    {
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int Sender { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public bool Seen { get; set; }
        public bool Send { get; set; }

    }
}