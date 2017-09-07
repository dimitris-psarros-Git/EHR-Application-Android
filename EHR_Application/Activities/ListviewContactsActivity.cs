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
using Android.Preferences;

namespace EHR_Application.Activities
{
    [Activity(Label = "     Contacts  ")]
    public class ListviewContactsActivity : Activity
    {
        public static List<ContactsPerson2> ContactsPer { get; private set; }
        ListView myList;
        CustomAdapter2 Cadapter;
        List<ContactsPerson2> contactsPerson2;
        List<friends> deserializedFriends;
      
        int myID;
        int receiverID;
        bool message;
        bool photo;
        bool sendData,Allergy;
        bool patientData;
        bool HealthHistory;
        bool IsValidJson1;
        bool IsDoctor;
        bool newvisit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListViewMainLayout);

            // Data from previous activity
            myID           = Intent.GetIntExtra("myID", -1);
            message        = Intent.GetBooleanExtra("messages", false);
            photo          = Intent.GetBooleanExtra("photos", false);
            sendData       = Intent.GetBooleanExtra("sendData", false);
            HealthHistory  = Intent.GetBooleanExtra("HealthHistory", false);
            patientData    = Intent.GetBooleanExtra("patientData", false);
            Allergy        = Intent.GetBooleanExtra("Allergy", false);
            newvisit       = Intent.GetBooleanExtra("newvisit", false);

            myList = FindViewById<ListView>(Resource.Id.listView);
            Cadapter = new CustomAdapter2(contactsPerson2);

            IsDoctor = RetrieveBool();
            Actions();

            myList.Adapter = new CustomAdapter2(contactsPerson2);  
            myList.ItemClick += MyList_ItemClick;
        }
       
        private bool RetrieveBool()
        {
            Context mContext = Android.App.Application.Context;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            bool mBool = prefs.GetBoolean("Is_Doctor", false);
            return mBool;
        }

        List<FullNames> fullnameList1 = new List<FullNames>();
        private void Actions()
        {
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            string endpoint;
            Address address = new Address();
           
            if (IsDoctor == false)
            {
                endpoint = address.Endpoint + "PatientFriends/" + myID;
            }
            else
            {
                endpoint = address.Endpoint + "DoctorFriends/" + myID;
            }
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                deserializedFriends = JsonConvert.DeserializeObject<List<friends>>(strResponse.ToString());
            }
            SetData();
        }
            
        public void SetData()
        {
            var temp = new List<ContactsPerson2>();
            for (int i = 0; i < deserializedFriends.Count; i++)
            {
                Adduser(temp, i);
            }
            contactsPerson2 = temp.OrderBy(i => i.FirstName).ToList();   // xwris auth thn entolh uparxei sfalma !!
        }

        public void Adduser(List<ContactsPerson2> contactsPerson2, int k)
        {
            contactsPerson2.Add(new ContactsPerson2()
            {
                FirstName = deserializedFriends[k].FirstName,
                LastName = deserializedFriends[k].LastName,
                ImageUrl = "contact.png"
            });
        }
        
        private void MyList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            receiverID = 0;
            Toast.MakeText(this, "Clicked :" + Cadapter.GetItemId(e.Position), ToastLength.Short).Show();
            int NumbPressed = (int)Cadapter.GetItemId(e.Position);
            Toast.MakeText(this, "You Pressed : " + contactsPerson2[NumbPressed].FirstName + contactsPerson2[NumbPressed].LastName, ToastLength.Short).Show();

            string firstNameContact = contactsPerson2[NumbPressed].FirstName;
            string lastNameContact = contactsPerson2[NumbPressed].LastName;
            
            ////////////////////////////////////////////////     third approach
            for (int i = 0; i < deserializedFriends.Count; i++)
            {
                if ((firstNameContact.ToString() + lastNameContact.ToString()) == (deserializedFriends[i].FirstName + deserializedFriends[i].LastName))
                {
                    receiverID = deserializedFriends[i].PersonID;
                }
            }
            ////////////////////////////////////////////////     end of third approach

            /////////////////////////////////   new approach
            receiverID = deserializedFriends[e.Position].PersonID;
            ////////////////////////////////   end new approach

            if (receiverID != 0 )
            {
                if (message)
                {
                    var intent = new Intent(this, typeof(ChatBubbleActivity));
                    intent.PutExtra("myID", myID);
                    intent.PutExtra("receiverID", receiverID);
                    StartActivity(intent);
                }
                if (photo)
                {
                    var intent = new Intent(this, typeof(ListviewContactsActivity));
                    intent.PutExtra("myID", myID);
                    intent.PutExtra("ConnectID", receiverID);
                    StartActivity(intent);
                }
                if (sendData)
                {
                    var intent = new Intent(this, typeof(SendDataActivity));
                    intent.PutExtra("myID", myID);
                    intent.PutExtra("receiverID", receiverID);
                    StartActivity(intent);
                }
                if (HealthHistory)   // mono otan syndethei giatros
                {
                    var intent = new Intent(this, typeof(History_ListviewActivity));
                    intent.PutExtra("myID", receiverID);
                    //intent.PutExtra("receiverID", receiverID);
                    StartActivity(intent);
                }
                if (patientData)   // mono otan syndethei giatros
                {
                    var intent = new Intent(this, typeof(DemographicsDataActivity));
                    intent.PutExtra("myID", myID);
                    intent.PutExtra("receiverID", receiverID);
                    StartActivity(intent);
                }
                if (Allergy)   // mono otan syndethei giatros
                {
                    var intent = new Intent(this, typeof(DemographicsDataActivity));
                    intent.PutExtra("myID", receiverID);
                    StartActivity(intent);
                }
                if (newvisit)
                {
                    var intent = new Intent(this, typeof(NewVisitActivity));
                    intent.PutExtra("myID", myID);
                    intent.PutExtra("receiverID", receiverID);
                    StartActivity(intent);
                }
            }
        }
    }
}