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

namespace EHR_Application.Adapters
{
    public class CustomAdapter4 : BaseAdapter<ContactsPerson5>
    {
        List<ContactsPerson5> contactsPersons5;
        

        public  CustomAdapter4(List<ContactsPerson5> contactsPerson5)
        {
            this.contactsPersons5 = contactsPerson5;
        }
        
        public override int Count
        {
            get
            {
                return contactsPersons5.Count;
            }
        }

        public override ContactsPerson5 this[int position]
        {
            get
            {
                return contactsPersons5[position];
            }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public string GetItemFirstName(int position)
        {
            return contactsPersons5[position].FirstName;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.rowperonsitems, null, false);
            }
            TextView txtFirstName = row.FindViewById<TextView>(Resource.Id.txtFirstName);
            txtFirstName.Text = contactsPersons5[position].FirstName;
            TextView txtLastName = row.FindViewById<TextView>(Resource.Id.txtLastName);
            txtLastName.Text = contactsPersons5[position].LastName;

            return row;
        }
    }
}