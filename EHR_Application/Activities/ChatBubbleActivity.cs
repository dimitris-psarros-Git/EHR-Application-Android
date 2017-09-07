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
using Android.Support.V7.App;
using Newtonsoft.Json;
using Android.Preferences;

namespace EHR_Application
{
    [Activity(Label = "    Chat   ",Icon ="@drawable/icon", Theme = "@style/MyTheme"  /*,Theme ="@style/Theme.AppCompat.Light.NoActionBar"*/)]
    public class ChatBubbleActivity : AppCompatActivity
    {
        List<chatMessages> lstChat = new List<chatMessages>();
        List<ReceivedMessages> receivedMessag;
        object strResponse;
        int myID;
        int receiverID;
        bool IsDoctor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BubbleListView);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);

            IsDoctor = RetrieveBool();
            setupMessages();
            CustomAdapter customAdapter = new CustomAdapter(lstChat, this);
            ListView lstView = FindViewById<ListView>(Resource.Id.myListView);
            lstView.Adapter = customAdapter ;
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
                History_ListviewActivity histListView = new History_ListviewActivity();
                histListView.AlertBox();
                return true;
            }
            else if (id == Resource.Id.action_settings3)
            {
                Toast.MakeText(this, "Reload", ToastLength.Short).Show();
                this.Recreate();
                return true;
            }
            return base.OnOptionsItemSelected(item);
            //return super.onCreateView(inflater, container, savedInstanceState);
        }
        #endregion

        private bool RetrieveBool()
        {
            Context mContext = Android.App.Application.Context;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            bool mBool = prefs.GetBoolean("Is_Doctor", false);
            return mBool;
        }
        
        private void setupMessages()
        {
            bool IsValid;
            string endpoint;
                
            List<ReceivedMessages> chMessages;
            Address address = new Address();

            if (IsDoctor == false) { endpoint = address.Endpoint + "MessagesCommunication/" + myID + "/" + receiverID; }
            else { endpoint = address.Endpoint + "MessagesCommunication/" + receiverID + "/" + myID; }
            
            ConsumeRest cRest = new ConsumeRest();
            strResponse = cRest.makeRequest(endpoint);
            
            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);
            
            if (IsValid)
            {
                chMessages = JsonConvert.DeserializeObject<List<ReceivedMessages>>(strResponse.ToString());
                receivedMessag = chMessages.OrderBy(i => i.Date).ToList();
                bool  isSend;
                for (int i = 0; i < chMessages.Count; i++)
                {
                    if ((IsDoctor == false) && (receivedMessag[i].IsMe == 0)) { isSend = true; }
                    else if ((IsDoctor == true) && (receivedMessag[i].IsMe == 1)) { isSend = true; }
                    else { isSend = false; }
                    lstChat.Add(new chatMessages() { ChatMessage = receivedMessag[i].Text + "\n" + receivedMessag[i].Date , IsSend = isSend });
                }
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "\n" + strResponse)
                .SetIcon(Resource.Drawable.error)
                .Show();
            }
        }
    }
}