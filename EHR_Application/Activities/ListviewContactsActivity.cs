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
using EHR_Application.Adapters;
using EHR_Application.Models;
using Newtonsoft.Json;

namespace EHR_Application.Activities
{
    [Activity(Label = "ListviewContactsActivity")]
    public class ListviewContactsActivity : Activity
    {
        public static List<ContactsPerson2> ContactsPer { get; private set; }
        ListView myList;
        CustomAdapter2 Cadapter;
        List<ContactsPerson2> contactsPerson2;
        //ContactsPerson2 conPer;
        //List<ContactsPerson2> contactsPer;
        bool IsValidJson1;
        List<ContactsPerson> deserializedContacts;
        int PerID;
        bool message;
        bool photo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListViewMainLayout);

            // get data from previous activity
            PerID = Intent.GetIntExtra("PerID", -1);
            message = Intent.GetBooleanExtra("messages", false);
            photo = Intent.GetBooleanExtra("photos", false);

            myList = FindViewById<ListView>(Resource.Id.listView);
            Cadapter = new CustomAdapter2(contactsPerson2);    
            Actions();
           
            myList.Adapter = new CustomAdapter2(contactsPerson2);  // ContactsData.contactsPerson2
            myList.ItemClick += MyList_ItemClick;
        }
       
        public void Actions()
        {
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            string endpoint = "http://192.168.2.3:54240/api/YOURCONTROLLER/" + PerID;
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                deserializedContacts = JsonConvert.DeserializeObject<List<ContactsPerson>>(strResponse.ToString());
                SetDat();
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetMessage("No data found do to unexpected problem" + "n/" + strResponse)
               .Show();
            }  
        }

        public void SetDat()
        {
            var temp = new List<ContactsPerson2>();
            for (int i = 0; i < deserializedContacts.Count; i++)
            {
                Adduser(temp,i);
            }
            contactsPerson2 = temp.OrderBy(i => i.FirstName).ToList();   // xwris auth thn entolh uparxei sfalma !!

           
        }

        public void Adduser(List<ContactsPerson2> contactsPerson2, int k)
        {
            if (k != 1)
            {
                contactsPerson2.Add(new ContactsPerson2()
                {
                    FirstName = deserializedContacts[k].FirstName,
                    LastName = deserializedContacts[k].LastName,
                    ImageUrl = "images.jpg"
                });
            }
            else
            {
                contactsPerson2.Add(new ContactsPerson2()
                {
                    FirstName = deserializedContacts[k].FirstName,
                    LastName = deserializedContacts[k].LastName,
                    ImageUrl = "1f60f.png"
                });
            }
        }

        int connectwith;


        private void MyList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            connectwith = 0;

            Toast.MakeText(this, "Clicked :" + Cadapter.GetItemId(e.Position), ToastLength.Short).Show();
            int NumbPressed = (int)Cadapter.GetItemId(e.Position);
            Toast.MakeText(this, "You Pressed : " + contactsPerson2[NumbPressed].FirstName + contactsPerson2[NumbPressed].LastName, ToastLength.Short).Show();

            string firstNameContact = contactsPerson2[NumbPressed].FirstName;
            string lastNameContact = contactsPerson2[NumbPressed].LastName;

            for (int i=0; i< deserializedContacts.Count; i++)
            {
                if ((firstNameContact.ToString() + lastNameContact.ToString()) == (deserializedContacts[i].FirstName + deserializedContacts[i].LastName))
                {
                    connectwith = deserializedContacts[i].Contactid;
                }
            }
            
            if (connectwith != 0 )
            {
                if (message)
                {
                    var intent = new Intent(this, typeof(ChatBubbleActivity));
                    intent.PutExtra("PerID", PerID);
                    intent.PutExtra("ConnectID", connectwith);
                    StartActivity(intent);
                }
                if (photo)
                {
                    var intent = new Intent(this, typeof(SendDataActivity));
                    intent.PutExtra("PerID", PerID);
                    intent.PutExtra("ConnectID", connectwith);
                    StartActivity(intent);
                }
            }
        }
    }
}