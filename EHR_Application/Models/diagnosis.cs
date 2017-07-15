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
    class DIagnosis
    {
        public int DiagnosisID { get; set; }
        public Nullable<int> VisitID { get; set; }
        public string Description { get; set; }
        public string ICD_CODE { get; set; }
        
    }
}