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
    class NewDiagnosis
    {
        public int VisitID { get; set; }
        public string Description { get; set; }
        public int ICD_Code_Id { get; set; }
        public string ICD_Chapter { get; set; }

    }
}