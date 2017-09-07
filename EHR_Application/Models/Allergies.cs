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
    class Allergies
    {
        public string Allergy1 { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
    }
}