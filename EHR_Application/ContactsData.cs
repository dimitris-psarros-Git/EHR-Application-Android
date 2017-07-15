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
using EHR_Application.Models;

namespace EHR_Application
{
    public static class ContactsData
    {
        public static List<ContactsPerson2> contactsPerson2 { get; private set; }

        static ContactsData()
        {
            var temp = new List<ContactsPerson2>();

            AddUser(temp);
            AddUser(temp);
            AddUser(temp);

            contactsPerson2 = temp.OrderBy(i => i.FirstName).ToList();
        }

        static void AddUser(List<ContactsPerson2> contactsPerson2)
        {
            contactsPerson2.Add(new ContactsPerson2()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                ImageUrl = "1f60f.png"

            });
            
        }
    }
}