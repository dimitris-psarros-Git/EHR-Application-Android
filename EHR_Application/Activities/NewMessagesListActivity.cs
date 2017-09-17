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
using EHR_Application.Post_Get;
using Android.Support.V7.App;

namespace EHR_Application.Activities
{
    [Activity(Label = "    New Messages  ", Theme = "@style/MyTheme")]
    public class NewMessagesListActivity : AppCompatActivity
    {
        List<ContactsPerson5> contactsPerson5;
        List<NewMessages2> newMessages;
        ListView lstNames;
        TextView txtImage;
        //Object messages;
        CustomAdapter4 adapter;
        int myID;                      
        bool IsDoctor,newImages;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListNewMessages);

            string message = Intent.Extras.GetString("message", "");
            myID = Intent.GetIntExtra("myID", -1);
            newImages = Intent.GetBooleanExtra("newImage",false);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            lstNames = FindViewById<ListView>(Resource.Id.listViewMessages);
            txtImage = FindViewById<TextView>(Resource.Id.textView3);

            RetrieveData retrieve = new RetrieveData();  // retrieve "IsDoctor"
            IsDoctor = retrieve.RetreiveBool();

            adapter = new CustomAdapter4(contactsPerson5);
            Actions();
            
            //adapter = new CustomAdapter4(lstSource);

            lstNames.Adapter = new CustomAdapter4(contactsPerson5);
            lstNames.ItemClick += LstNames_ItemClick;
        }

        #region MenuInflater
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.option_menuGener, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings2)
            {
                Toast.MakeText(this, "Exit", ToastLength.Short).Show();
                AlertBox();
                return true;
            }
            else if (id == Resource.Id.action_settings3)
            {
                Toast.MakeText(this, "Reload", ToastLength.Short).Show();
                this.Recreate();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion


        private void LstNames_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + adapter.GetItemId(e.Position), ToastLength.Short).Show();
            int NumbPressed = (int)adapter.GetItemId(e.Position);
            Toast.MakeText(this, "You Pressed : " + newMessages[NumbPressed].FirstName + newMessages[NumbPressed].LastName, ToastLength.Short).Show();

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(newMessages[e.Position].FirstName + "  " + newMessages[e.Position].LastName);
            alert.SetMessage(newMessages[e.Position].Text);
            alert.SetCancelable(true);
            alert.SetIcon(Resource.Drawable.message);
            Dialog dialog = alert.Create();
            dialog.Show();
            
            DeleteFromNew(newMessages[e.Position].DataSenderID);
        }

        private async void DeleteFromNew(int datasendID)
        {
            Address address = new Address();
            PutRest putrest = new PutRest();
            string endpoint3;

            endpoint3 = address.Endpoint + "DataSenders/" + datasendID;
            var uri = new Uri(endpoint3);

            NewMessages newmessages = new NewMessages();
            newmessages.DataSenderID = datasendID;
            newmessages.Send = true;

            string output = JsonConvert.SerializeObject(newmessages);
            var StrRespPost = await PutRest.Put(output, uri);
        }

        protected void Actions()
        {
            object strResponse;
            bool IsValidJson;
            string endpoint;
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();

            if (newImages == false) { txtImage.Text = "No new images"; }
            else { txtImage.Text = " New images found!! " + "\n" + " Check for them at <New Images>"; }

            if (IsDoctor == false) { endpoint = address.Endpoint + "PatientNewMessages/" + myID; }
            else { endpoint = address.Endpoint + "DoctorNewMessages/" + myID; }

            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValidJson = validateJson.IsValidJson(strResponse);

            if (IsValidJson)
            {
                newMessages = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                newMessages = newMessages.OrderBy(i => i.FirstName).ToList();
            
            SetData();
            }
            else
            {
                newMessages = JsonConvert.DeserializeObject<List<NewMessages2>>("[]".ToString());

               new Android.App.AlertDialog.Builder(this)
              .SetTitle("An error has occured")
              .SetMessage("No data found due to unexpected problem "+ "\n" + strResponse)
              .SetIcon(Resource.Drawable.error)
              .Show();
            }
        }

        public void SetData()
        {
            var temp = new List<ContactsPerson5>();
            for (int i = 0; i < newMessages.Count; i++)
            {
                Adduser(temp, i);
            }
            contactsPerson5 = temp.OrderBy(i => i.FirstName).ToList();   // xwris auth thn entolh uparxei sfalma !!
        }

        public void Adduser(List<ContactsPerson5> contactsPerson5, int k)
        {
            contactsPerson5.Add(new ContactsPerson5()
            {
                FirstName = newMessages[k].FirstName,
                LastName =  newMessages[k].LastName
            });
        }

        protected void AlertBox()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Confirm Exit");
            alert.SetMessage("Do you really want to exit? ");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
                //Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
                Finish1();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}