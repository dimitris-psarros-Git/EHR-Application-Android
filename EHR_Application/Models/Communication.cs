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
    class Communication
    {
        public int CommunicationID { get; set; }
        public Nullable<int> PersonID { get; set; }
        public int Telephone { get; set; }
        public string email { get; set; }

    }
}