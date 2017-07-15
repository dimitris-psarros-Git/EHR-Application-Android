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
    class Treat_Medicines
    {

        public int TreatmentID { get; set; }
        public Nullable<int> VisitID { get; set; }
        public string ATC_CODES { get; set; }
        public string Dosage { get; set; }

       // public virtual visit Visit { get; set; }
    }
}