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
    public  class ContactsPerson
    {
        public int ContactsID { get; set; }
        public int PersonId { get; set; }
        public int Contactid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}