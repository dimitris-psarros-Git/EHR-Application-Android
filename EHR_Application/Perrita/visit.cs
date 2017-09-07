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
    class visit
    {
        public int VisitID { get; set; }
        public string Date { get; set; }
        public int DoctorPersonID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public int NumberOfVisit { get; set; }

        public List<DIagnosis> DIagnosis { get; set; }       // <object>
        public List<Treat_Medicines> Treat_Medicines { get; set; }

    }
}