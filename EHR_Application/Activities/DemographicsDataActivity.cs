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
using EHR_Application.Models;

namespace EHR_Application.Activities
{
    [Activity(Label = "   PatientData  ")]
    public class DemographicsDataActivity : Activity
    {
        int myID, receiverID;
        Spinner spin;
        Spinner spin1;
        TextView txt1;
        TextView txt13;
        TextView txt14;
        TextView txt15;
        TextView txt16;
        TextView txt17;
        TextView txt18;
        TextView txt19;
        TextView txt21;
        TextView txt24;
        ArrayAdapter adapt;
        ArrayAdapter adapt1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DemogrLayout);

            // Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);
            
            spin1 = FindViewById<Spinner>(Resource.Id.spinner2);
            spin = FindViewById<Spinner>(Resource.Id.spinner1);
            txt1 = FindViewById<TextView>(Resource.Id.txt1);
            txt13 = FindViewById<TextView>(Resource.Id.txt13);
            txt14 = FindViewById<TextView>(Resource.Id.txt14);
            txt15 = FindViewById<TextView>(Resource.Id.txt15);
            txt16 = FindViewById<TextView>(Resource.Id.txt16);
            txt17 = FindViewById<TextView>(Resource.Id.txt17);
            txt18 = FindViewById<TextView>(Resource.Id.txt18);
            txt19 = FindViewById<TextView>(Resource.Id.txt19);
            txt24 = FindViewById<TextView>(Resource.Id.txt24);

            actions();
        }

        public void actions()
        {
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            Address address = new Address();
            string endpoint;
            ValidateJson validateJson = new ValidateJson();
            ///////////// telephones
            try
            {
                List<string> Teleph = new List<string>();
                List<string> emails = new List<string>();
                endpoint = address.Endpoint + "CommunicationList/" + myID;  //1001; 
                strResponse = cRest.makeRequest(endpoint);
                IsValidJson = validateJson.IsValidJson(strResponse);
                List<Communication> communication = JsonConvert.DeserializeObject<List<Communication>>(strResponse.ToString());
                if (communication.Any())
                {
                    for (int i = 0; i < communication.Count; i++)
                    {
                        Teleph.Add(communication[i].Telephone.ToString());
                        emails.Add(communication[i].email);
                    }
                }
                adapt = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, Teleph);
                adapt1 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, emails);
                spin.Adapter = adapt;
                spin1.Adapter = adapt1;
            }
            catch (Exception e)
            {
            }

            txt1.Text = "Patient";
            demographics demogr;
            endpoint = address.Endpoint + "Demographics/" + receiverID;
            strResponse = cRest.makeRequest(endpoint);

            IsValidJson = validateJson.IsValidJson(strResponse);

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
                txt19.Text = demogr.Birthday;
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
        List<SelectChoice> selectChoice = new List<SelectChoice>();
        List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
        List<string> _lstDataItem;    //  = new List<string>();
        string Name;
        int Number;
        int Receiver = 0;

        void methodInvokeAlertDialogWithListView(/*object sender, EventArgs e*/)
        {
            bool IsValidJson1;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();                          // check it  ( it was changed )           
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            Address address = new Address();
            string endpoint = address.Endpoint + "ContactsList/" + receiverID;

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
                dlgAlert.SetTitle(" Contacts ");
                var listView = new ListView(this);
                listView.Adapter = new EHR_Application.AlertListViewAdapter(this, _lstDataItem);         // changed
                //listView.ItemClick += listViewItemClick;
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
        
        void handllerNotingButton(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        #endregion
        
    }
}