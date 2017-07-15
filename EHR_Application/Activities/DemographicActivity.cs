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
using Newtonsoft.Json;
using Android.Content.Res;
using System.IO;

using EHR_Application.Models;
using Newtonsoft.Json.Linq;
using EHR_Application.Activities;

namespace EHR_Application
{
    [Activity(Label = "Activity2",Theme="@style/Theme.AppCompat.Light.NoActionBar")]
    public class DemographicActivity : Activity
    {
        TextView txt1;
        TextView txt13;
        TextView txt14;
        TextView txt15;
        TextView txt16;
        TextView txt17;
        TextView txt18;
        TextView txt19;
        TextView txt20;
        TextView txt21;
        TextView txt22;
        TextView txt23;
        TextView txt24;
        int PerID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DemogrLayout);

            PerID = Intent.GetIntExtra("PerID", -1);
            //string PerID = Intent.GetStringExtra("PerID") ?? "Data not available";     // prosoxh thelei diorthwsh . metatroph se integer
           
            txt1  = FindViewById<TextView>(Resource.Id.txt1);
            txt13 = FindViewById<TextView>(Resource.Id.txt13);    
            txt14 = FindViewById<TextView>(Resource.Id.txt14);
            txt15 = FindViewById<TextView>(Resource.Id.txt15);
            txt16 = FindViewById<TextView>(Resource.Id.txt16);
            txt17 = FindViewById<TextView>(Resource.Id.txt17);
            txt18 = FindViewById<TextView>(Resource.Id.txt18);
            txt19 = FindViewById<TextView>(Resource.Id.txt19);
            txt20 = FindViewById<TextView>(Resource.Id.txt20);
            txt21 = FindViewById<TextView>(Resource.Id.txt21);
            txt22 = FindViewById<TextView>(Resource.Id.txt22);
            txt23 = FindViewById<TextView>(Resource.Id.txt23);
            txt24 = FindViewById<TextView>(Resource.Id.txt24);

            actions();

            Button button1 = FindViewById<Button>(Resource.Id.health_button);
            button1.Click += Button1_Click;     

            Button button2 = FindViewById<Button>(Resource.Id.sendData);
            button2.Click += Button2_Click;

            Button button3 = FindViewById<Button>(Resource.Id.SelectPerson);
            button3.Click += methodInvokeAlertDialogWithListView;

            Button button4 = FindViewById<Button>(Resource.Id.messages);
            button4.Click += Button4_Click;

            Button button5 = FindViewById<Button>(Resource.Id.photos);
            button5.Click += Button5_Click;

        }

        public void actions()
        {
            txt1.Text = "Patient";
            demographics demogr;
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            string endpoint = "http://192.168.2.3:54240/api/Demographics/" + PerID; 
            strResponse = cRest.makeRequest(endpoint);
            
            ValidateJson validateJson = new ValidateJson();
            IsValidJson = validateJson.IsValidJson(strResponse);

            if (IsValidJson)
            {
                demogr = JsonConvert.DeserializeObject<demographics>(strResponse.ToString());
                txt13.Text = demogr.FirstName;
                txt14.Text = demogr.LastName;
                txt16.Text = demogr.City;
                txt24.Text = demogr.StreetName;
                txt17.Text = demogr.StreetNumber.ToString();
                txt18.Text = demogr.Sex;
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
               .Show();
            }

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ListviewContactsActivity));
            intent.PutExtra("PerID", PerID);
            intent.PutExtra("photos", true);
            StartActivity(intent);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ListviewContactsActivity));
            intent.PutExtra("PerID",PerID);
            intent.PutExtra("messages", true);
            StartActivity(intent);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Receiver == 0)
            {
                new AlertDialog.Builder(this)
                .SetMessage("Select a Person")
                .SetTitle("No-one Selected")
                .Show();
            }
            else
            {
                var intent = new Intent(this, typeof(SendDataActivity));
                intent.PutExtra("PErsonId", Receiver);
                intent.PutExtra("MyString", "This is a string");
                StartActivity(intent);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(History_ListviewActivity));
            StartActivity(intent);
        }
        
        #region ListView AlertDialog
        List<SelectChoice> selectChoice = new List<SelectChoice>();      
        List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
        List<string> _lstDataItem;    //  = new List<string>();
        string Name;
        int Number;
        int Receiver = 0;
        
        void methodInvokeAlertDialogWithListView(object sender, EventArgs e)
        {
            bool IsValidJson1;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();                                            // check it  ( it was changed )           
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            string endpoint = "http://192.168.2.3:54240/api/YOURCONTROLLER/" + PerID;    //Contacts";

            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {

                List<ContactsPerson> deserializedContacts = JsonConvert.DeserializeObject<List<ContactsPerson>>(strResponse.ToString());

                Number = deserializedContacts.Count;

                for (int i = 0; i < deserializedContacts.Count; i++)
                {
                    string NAME = deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName;

                    _lstDataItem.Add(NAME);

                    selectChoice.Add(new SelectChoice
                    {
                        PersonId = deserializedContacts[i].PersonId,
                        Contactid = deserializedContacts[i].Contactid,
                        FullName = deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName
                    });

                }


                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetTitle("Select Doctor");
                var listView = new ListView(this);
                listView.Adapter = new EHR_Application.AlertListViewAdapter(this, _lstDataItem);         // changed
                listView.ItemClick += listViewItemClick;
                dlgAlert.SetView(listView);
                dlgAlert.SetButton("OK", handllerNotingButton);
                dlgAlert.Show();
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetMessage("No data found due to unexpected problem" + "\n" + strResponse)
               .Show();
            }
        }

        void listViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "you clicked on " + _lstDataItem[e.Position], ToastLength.Short).Show();
            Name = _lstDataItem[e.Position];
            for (int i = 0; i < selectChoice.Count; i++)
            {
                if (Name == selectChoice[i].FullName)
                {
                    Receiver = selectChoice[i].Contactid;
                }      
            }
        }

        void handllerNotingButton(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        #endregion
    }
}