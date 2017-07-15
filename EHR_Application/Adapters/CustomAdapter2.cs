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
    public class CustomAdapter2 : BaseAdapter<ContactsPerson2>
    {
        List<ContactsPerson2> contactsPersons2;

        public CustomAdapter2(List<ContactsPerson2> contactsPerson2)
        {
            this.contactsPersons2 = contactsPerson2;
        }

        public override ContactsPerson2 this[int position]
        {
            get
            {
                return contactsPersons2[position];
            }
        }

        public override int Count
        {
            get
            {
                return contactsPersons2.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public string GetItemFirstName(int position)
        {
            return contactsPersons2[position].FirstName;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListViewRowlayout, parent, false);

                var photo = view.FindViewById<ImageView>(Resource.Id.photoImageView);
                var name = view.FindViewById<TextView>(Resource.Id.nameTextView);
                var department = view.FindViewById<TextView>(Resource.Id.departmentTextView);

                view.Tag = new ViewHolder() { Photo = photo, Name = name, Department = department };
            }

            var holder = (ViewHolder)view.Tag;

            holder.Photo.SetImageDrawable(ImageManager.Get(parent.Context, contactsPersons2[position].ImageUrl));
            holder.Name.Text = contactsPersons2[position].FirstName;
            holder.Department.Text = contactsPersons2[position].LastName;
            
            return view;
        }
    }
}