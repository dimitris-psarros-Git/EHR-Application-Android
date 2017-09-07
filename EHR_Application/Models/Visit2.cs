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
    class Visit2
    {
        public Doctor2 Doctor { get; set; }
        public int VisitID { get; set; }
        public DateTime Date { get; set; }
    }
}