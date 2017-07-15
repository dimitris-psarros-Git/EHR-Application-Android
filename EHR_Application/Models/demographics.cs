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
    class demographics
    {
        public int PERSONID { get; set; }
        
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
     
        public string Sex { get; set; }
     
        public string Country { get; set; }
      
        public string City { get; set; }
      
        public string StreetName { get; set; }
      
        public int StreetNumber { get; set; }
      
        public string Birthday { get; set; }
    }
}