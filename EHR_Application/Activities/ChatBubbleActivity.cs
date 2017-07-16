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

namespace EHR_Application
{
    [Activity(Label = "ChatBubble",Icon ="@drawable/icon",Theme ="@style/Theme.AppCompat.Light.NoActionBar")]
    public class ChatBubbleActivity : AppCompatActivity
    {
        List<chatMessages> lstChat = new List<chatMessages>();
        List<ReceivedMessages> receivedMessag;
        object strResponse;
        int PerID;
        int PerCon;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BubbleListView);

            //info from previous activity
            PerID = Intent.GetIntExtra("PerID", -1);
            PerCon = Intent.GetIntExtra("ConnectID", -1);

            setupMessages1();
            CustomAdapter customAdapter = new CustomAdapter(lstChat, this);
            ListView lstView = FindViewById<ListView>(Resource.Id.myListView);
            lstView.Adapter = customAdapter ;
        }
        
        //private void setupMessages()
        //{
        //    lstChat.Add(new chatMessages() { ChatMessage = "Hello !", IsSend = true });
        //    lstChat.Add(new chatMessages() { ChatMessage = "Hello !", IsSend = false });
        //    lstChat.Add(new chatMessages() { ChatMessage = "Are you fine !", IsSend = true });
        //    lstChat.Add(new chatMessages() { ChatMessage = "Yes !", IsSend = false });
        //} 
        
        private void setupMessages1()
        {
            bool IsValid;
            
            List<ReceivedMessages> chMessages;
            string endpoint = "http://192.168.1.70:54240/api/Messages1/"+PerID+"/"+PerCon;

            ConsumeRest cRest = new ConsumeRest();
            strResponse = cRest.makeRequest(endpoint);
            
            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);
            
            if (IsValid)
            {
                chMessages = JsonConvert.DeserializeObject<List<ReceivedMessages>>(strResponse.ToString());
                receivedMessag = chMessages.OrderBy(i => i.Date).ToList();

                for (int i = 0; i < chMessages.Count; i++)
                {
                    lstChat.Add(new chatMessages() { ChatMessage = receivedMessag[i].Text + "\n" + receivedMessag[i].Date , IsSend = receivedMessag[i].IsMe });
                }
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "\n" + strResponse)
                .Show();
            }
        }
    }
}