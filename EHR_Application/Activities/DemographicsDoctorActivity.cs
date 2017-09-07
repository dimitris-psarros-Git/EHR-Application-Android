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
using Android.Preferences;
using EHR_Application.Post_Get;

namespace EHR_Application.Activities
{
    [Activity(Label = "    Doctor Info " /*, MainLauncher = true*/ , Theme = "@style/MyTheme1")]
    public class DemographicsDoctorActivity : Activity
    {
        int myID;
        //ArrayAdapter adapt;
        //ArrayAdapter adapt1;

        TextView txtFirstName;
        TextView txtLastName;
        TextView txtSpeciality;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DemographicDoctor);
            myID = Intent.GetIntExtra("myID", -1);
            
            /////////////////////     auta prepei na svistoun
            //myID = 10000;
            //SaveBool(true);
            ////////////////////     telos auta pou prepei na svistoun
            
            txtFirstName  = FindViewById<TextView>(Resource.Id.txtFirstName);
            txtLastName   = FindViewById<TextView>(Resource.Id.txtLastName);
            txtSpeciality = FindViewById<TextView>(Resource.Id.txtSpeciality);

            Actions();
        }


        /////////////////////////////////////////// ayto prepei na svistei
        //protected void SaveBool(bool IsDoctor)         // save if it is Doctor or Patient
        //{
        //    Context mContext = Android.App.Application.Context;
        //    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
        //    ISharedPreferencesEditor editor = prefs.Edit();
        //    editor.PutBoolean("Is_Doctor", IsDoctor);
        //    // editor.Commit();    // applies changes synchronously on older APIs
        //    editor.Apply();
        //}
        //////////////////////////////////////////// telos auto pou prepei na svistei


        #region MenuInflater
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.option_menuDoctorDemogr, menu);
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
            else if (id == Resource.Id.action_settings11)
            {
                methodInvokeAlertDialogWithListView2();
                return true;
            }
            else if (id == Resource.Id.action_settings12)
            {
                methodInvokeAlertDialogWithListView3();
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
            else if (id == Resource.Id.action_settings)
            {
                Toast.MakeText(this, "Send Data", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("Allergy", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings1)
            {
                Toast.MakeText(this, "Messages", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("messages", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings1)
            {
                Toast.MakeText(this, "PatientsData", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("patientData", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings5)
            {
                Toast.MakeText(this, "Health History of Patient", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("HealthHistory", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings6)
            {
                Toast.MakeText(this, "Photos", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("photos", true);
                StartActivity(intent);
                return true;
            }
            else if (id == Resource.Id.action_settings9)
            {
                Toast.MakeText(this, "Add Health History", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(ListviewContactsActivity));
                intent.PutExtra("myID", myID);
                intent.PutExtra("newvisit", true);
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

        protected void Actions()
        {
            ValidateJson validateJson = new ValidateJson();
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            Doctor2 demogr;
            bool IsValidJson;
            object strResponse;
            string endpoint;


            endpoint = address.Endpoint + "Doctors/" + 10000;//myID;
                strResponse = cRest.makeRequest(endpoint);
                IsValidJson = validateJson.IsValidJson(strResponse);

                if (IsValidJson)
                {
                    demogr = JsonConvert.DeserializeObject<Doctor2>(strResponse.ToString());
                    txtFirstName.Text = demogr.FirstName;
                    txtLastName.Text = demogr.LastName;
                    txtSpeciality.Text = demogr.Speciality;
                }
                else{ 
                       new AlertDialog.Builder(this)
                      .SetTitle("An error has occured")
                      .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
                      .Show();
                }
        }


        #region ListView AlertDialog
        //List<SelectChoice> selectChoice = new List<SelectChoice>();
        //List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
        List<string> _lstDataItem;   
        //int Number;
        
        void methodInvokeAlertDialogWithListView()
        {
            string endpoint;
            bool IsValidJson1;
            object strResponse;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();

            endpoint = address.Endpoint + "DoctorFriends/" + myID;

            strResponse = cRest.makeRequest(endpoint);
            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                List<friends> deserializedContacts = JsonConvert.DeserializeObject<List<friends>>(strResponse.ToString());

                //Number = deserializedContacts.Count;
                for (int i = 0; i < deserializedContacts.Count; i++)
                {
                    string NAME = deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName;

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


        /// /////////////////////////////////////////////////////////////    add new code 
        #region
        List<friends> deserializedContacts3;
        void methodInvokeAlertDialogWithListView3()
        {
            string endpoint;
            bool IsValidJson1;
            object strResponse;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            myID = 10000;
            endpoint = address.Endpoint + "DoctorFriends/" + myID;

            strResponse = cRest.makeRequest(endpoint);
            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                deserializedContacts3 = JsonConvert.DeserializeObject<List<friends>>(strResponse.ToString());

                //Number = deserializedContacts.Count;
                for (int i = 0; i < deserializedContacts3.Count; i++)
                {
                    string NAME = deserializedContacts3[i].FirstName + " " + deserializedContacts3[i].LastName;
                    _lstDataItem.Add(NAME);
                }

                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetTitle(" Contacts ");
                var listView = new ListView(this);
                listView.Adapter = new EHR_Application.AlertListViewAdapter(this, _lstDataItem);
                listView.ItemClick += ListView_ItemClick1;
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

        private void ListView_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            int sl = deserializedContacts3[e.Position].PersonID;
            String Personaldata = PersonalData(sl);

            Toast.MakeText(this, "you clicked on " + _lstDataItem[e.Position], ToastLength.Short).Show();
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle( "Patient : " + deserializedContacts3[e.Position].FirstName + deserializedContacts3[e.Position].LastName );
            alert.SetMessage(Personaldata);
            alert.SetCancelable(true);
            Dialog dialog = alert.Create();
            dialog.Show();
            
        }
        #endregion
        ///////////////////////////////////////////////////////////////////   end of new code

        ///////////////////////////////////////////////////////////////////    add new code
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

            string endpoint = address.Endpoint + "DoctorNewMessages/" + myID;
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                 deserializedContacts = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                 receivedMes = deserializedContacts.OrderBy(c => c.LastName).ToList();

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
            alert.SetIcon(Resource.Drawable.message);
            alert.SetCancelable(true);
            Dialog dialog = alert.Create();
            dialog.Show();
            DeleteFromNew(receivedMes[e.Position].DataSenderID);
        }

        void handllerNotingButton1(object sender, DialogClickEventArgs e)
        {
            AlertDialog objAlertDialog = sender as AlertDialog;
            Button btnClicked = objAlertDialog.GetButton(e.Which);
            Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        }
        #endregion
        ////////////////////////////////////////////////////////////////    end of new code

        private async void DeleteFromNew(int datasendID)
        {
            Address address = new Address();
            PutRest putrest = new PutRest();
            string endpoint3;

            endpoint3 = address.Endpoint + "DataSenders/" + datasendID;
            var uri = new Uri(endpoint3);

            NewMessages newmessages = new NewMessages();
            newmessages.DataSenderID = datasendID;
            newmessages.Seen = true;

            string output = JsonConvert.SerializeObject(newmessages);
            var StrRespPost = await PutRest.Put(output, uri);
        }


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

        public string PersonalData(int? PersonID)
        {
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            demographics demogr;
            string endpoint = address.Endpoint + "Demographics/" + PersonID;//myID;
            object strResponse = cRest.makeRequest(endpoint);
            demographics demogra = JsonConvert.DeserializeObject<demographics>(strResponse.ToString());
            string info ="FullName: "+ demogra.FirstName + " " + demogra.LastName +"\n" + "Dieuthinsh :"+demogra.City+ " " + demogra.StreetName+ " "+ demogra.StreetNumber;
            return info;
        }

    }
}