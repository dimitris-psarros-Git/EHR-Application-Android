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
using Java.Util;
using Android.Preferences;
using EHR_Application.Post_Get;
using System.Globalization;

namespace EHR_Application
{
    [Activity(Label = "    Patient Info  ",Theme = "@style/MyTheme1"  /*Theme="@style/Theme.AppCompat.Light.NoActionBar"*/)]
    public class DemographicActivity : Activity
    {
        Spinner spin;
        Spinner spin1;
        TextView txt13;
        TextView txt14;
        TextView txt15;
        TextView txt16;
        TextView txt17;
        TextView txt18;
        TextView txt19;
        TextView txt24;
        int myID;
        ArrayAdapter adapt;
        ArrayAdapter adapt1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DemogrLayout);

            //Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            
            spin1 = FindViewById<Spinner>(Resource.Id.spinner2);
            spin  = FindViewById<Spinner>(Resource.Id.spinner1);
            txt13 = FindViewById<TextView>(Resource.Id.txt13);    
            txt14 = FindViewById<TextView>(Resource.Id.txt14);
            txt15 = FindViewById<TextView>(Resource.Id.txt15);
            txt16 = FindViewById<TextView>(Resource.Id.txt16);
            txt17 = FindViewById<TextView>(Resource.Id.txt17);
            txt18 = FindViewById<TextView>(Resource.Id.txt18);
            txt19 = FindViewById<TextView>(Resource.Id.txt19);
            txt24 = FindViewById<TextView>(Resource.Id.txt24);
            
            Actions();

            //bool IsDoctor = RetrieveBool();
        }
        
        #region MenuInflater
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.option_menuDemogr, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings3)
            {
                this.Recreate();
                return true;
            }
            else if (id == Resource.Id.action_settings7)
            {
                methodInvokeAlertDialogWithListView();
                return true;
            }
            else if (id == Resource.Id.action_settings4)
            {
                Toast.MakeText(this, "Allergies/Reactions", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(AllergiesActivity));
                intent.PutExtra("myID", myID);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings8)
            {
                Toast.MakeText(this, " Notes ", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ToDoActivity));
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings12)
            {
                Toast.MakeText(this, " New Images ", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(NewImagesActivity));
                intent.PutExtra("myID", myID);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings)
            {
                Toast.MakeText(this, "Send Data", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("sendData", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings1)
            {
                Toast.MakeText(this, "Messages", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID",myID);
                intent.PutExtra("messages", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings9)
            {
                Toast.MakeText(this, "Check new Messages", ToastLength.Short).Show();
                methodInvokeAlertDialogWithListView2();
                return true;
            }
            else if (id == Resource.Id.action_settings5)
            {
                Toast.MakeText(this, "Health History", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(History_ListviewActivity));
                intent.PutExtra("myID", myID);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings6)
            {
                int PersonalID = myID;
                Toast.MakeText(this, "Photos", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("photos", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings2)
            {
                Toast.MakeText(this, "Exit", ToastLength.Short).Show();
                AlertBox();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion
        
        public void Actions()
        {
            List<string> Teleph = new List<string>();
            List<string> emails = new List<string>();
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();
            Address address = new Address();
            object strResponse;
            bool IsValidJson;
            string endpoint;
            demographics demogr;

            endpoint = address.Endpoint + "Demographics/" + myID;

            strResponse = cRest.makeRequest(endpoint);
            IsValidJson = validateJson.IsValidJson(strResponse);

            DateTime d;

            if (IsValidJson)
            {
                demogr = JsonConvert.DeserializeObject<demographics>(strResponse.ToString());
                txt13.Text = demogr.FirstName;
                txt14.Text = demogr.LastName;
                txt15.Text = demogr.Country;
                txt16.Text = demogr.City;
                txt24.Text = demogr.StreetName;
                txt17.Text = demogr.StreetNumber.ToString();
                txt18.Text = demogr.Sex;
                txt19.Text = demogr.Birthday.ToString();

                if (demogr.Communications.Count !=0) {
                        for (int i = 0; i < demogr.Communications.Count; i++)
                        {
                            Teleph.Add(demogr.Communications[i].Telephone.ToString());
                            emails.Add(demogr.Communications[i].email);
                        }

                    adapt =  new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Teleph);
                    adapt1 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, emails);
                    spin.Adapter = adapt;
                    spin1.Adapter = adapt1;
                }
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
               .Show();
            }
        }
        
        #region ListView AlertDialog
      
        List<string> _lstDataItem;    
        int Number;
        void methodInvokeAlertDialogWithListView()
        {
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            bool IsValidJson1;
            object strResponse;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();                              
            
            string endpoint = address.Endpoint + "PatientFriends/" + myID;
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                List<friends> deserializedContacts = JsonConvert.DeserializeObject<List<friends>>(strResponse.ToString());

                Number = deserializedContacts.Count;
                for (int i = 0; i < deserializedContacts.Count; i++)
                {
                    string NAME = deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName+ "  " + deserializedContacts[i].speciality;

                    _lstDataItem.Add(NAME);
                }

                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetTitle(" Contacts ");
                var listView = new ListView(this);
                listView.Adapter = new EHR_Application.AlertListViewAdapter(this, _lstDataItem);        
                dlgAlert.SetView(listView);
                dlgAlert.SetButton("OK", handllerNotingButton);
                dlgAlert.Show();
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetIcon(Resource.Drawable.error)
               .SetMessage("No data found due to unexpected problem" + "\n" + strResponse)
               .Show();
            }
        }
        void handllerNotingButton(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        #endregion
        

        #region ListView AlertDialog
        List<string> _lstDataItem2;
        List<NewMessages2> deserializedContacts, receivedMes;
        void methodInvokeAlertDialogWithListView2()
        {
            object strResponse;
            bool IsValidJson1;
            _lstDataItem2 = new List<string>();
            _lstDataItem2.Clear();
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();

            string endpoint = address.Endpoint + "PatientNewMessages/" + myID;
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                deserializedContacts = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                receivedMes = deserializedContacts.OrderBy(c => c.FirstName).ToList();
                for (int i = 0; i < receivedMes.Count; i++)
                {
                    string NAME = "New Message from: " + receivedMes[i].FirstName + " " + receivedMes[i].LastName;
                    _lstDataItem2.Add(NAME);
                }

                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetTitle(" New Messages ");
                var listView = new ListView(this);
                listView.Adapter = new EHR_Application.AlertListViewAdapter(this, _lstDataItem2);
                listView.ItemClick += ListView_ItemClick;
                dlgAlert.SetView(listView);
                dlgAlert.SetButton("OK", handllerNotingButton1);
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

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "you clicked on " + _lstDataItem2[e.Position], ToastLength.Short).Show();
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(receivedMes[e.Position].FirstName + "  " + receivedMes[e.Position].LastName);
            alert.SetMessage(receivedMes[e.Position].Text);
            alert.SetCancelable(true);
            alert.SetIcon(Resource.Drawable.messageImage);
            Dialog dialog = alert.Create();
            dialog.Show();

            RetrieveData RD = new RetrieveData();
            RD.DeleteFromNew(receivedMes[e.Position].DataSenderID);        
        }
        void handllerNotingButton1(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        #endregion
        
        public void AlertBox()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm Exit");
            alert.SetMessage("Do you really want to exit? ");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
            Finish1();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}