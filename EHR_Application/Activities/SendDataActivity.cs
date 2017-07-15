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
using Newtonsoft.Json;
using EHR_Application.Activities;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EHR_Application
{
    [Activity(Label = "SendData")]
    public class SendDataActivity : Activity
    {
        TextView txt1;
        TextView txt8;
        EditText txt7;
        DateTime now;
        
        byte[] image;
        int PerID;
        int ConID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SdataLayout);
            
             PerID = Intent.GetIntExtra("PerID", -1);
             ConID = Intent.GetIntExtra("ConnectID", -1);
            int PersID = Intent.GetIntExtra("PErsonId", -1);
            byte[] image1 = Intent.GetByteArrayExtra("image");
            /*var*/image = Intent.GetByteArrayExtra("picture");   // allaksame to image apo var se byte[]

            txt1 = FindViewById<TextView>(Resource.Id.txt1);
            txt8 = FindViewById<TextView>(Resource.Id.txt8);
            txt7 = FindViewById<EditText>(Resource.Id.editxt);

            Button Sendtext = FindViewById<Button>(Resource.Id.SendText);
            Sendtext.Click += Sendtext_ClickAsync;

            Button SendPhoto = FindViewById<Button>(Resource.Id.SendPhoto);
            SendPhoto.Click += SendPhoto_ClickAsync;

            Button button1 = FindViewById<Button>(Resource.Id.TakePhoto);
            button1.Click += Button_Click;

            Button button2 = FindViewById<Button>(Resource.Id.Gallery);
            button2.Click += Button2_Click;

            Button button3 = FindViewById<Button>(Resource.Id.Reselect);
            button3.Click += methodInvokeAlertDialogWithListView;

            Button button4 = FindViewById<Button>(Resource.Id.button6);
            button4.Click += Button4_Click;
            
            now = DateTime.Now.ToLocalTime();   
            string currentTime = (string.Format("Current Time: {0}", now)); 
        }

        private void SendPhoto_ClickAsync(object sender, EventArgs e)
        {
           if(image == null)
            {
                new AlertDialog.Builder(this)
                .SetMessage("No Image Selected")
                .SetTitle("Message")
                .Show();
            }
            else
            {
                /*
                var requestContent = new MultipartFormDataContent();

                var imageContent = new ByteArrayContent(File.ReadAllBytes(@"C:\Users\User\Desktop\w3.png"));

                image.Headers.ContentType = MediaTypeHeaderValue.Parse("image/txt");

                requestContent.Add(image, "image", "image.png");
                requestContent.Add(new StringContent("en-US"), "language");
                requestContent.Add(new StringContent("blue"), "color");
                requestContent.Add(new StringContent("12/11/15"), "date");
                */
            }
        }

        private async void Sendtext_ClickAsync(object sender, EventArgs e)
        {
            ReceivedMessages sendMessageJson = new ReceivedMessages();
            sendMessageJson.PersonID = PerID;
            sendMessageJson.ReseiverID = ConID;
            sendMessageJson.Text = txt7.Text;
            
            string output = JsonConvert.SerializeObject(sendMessageJson);

            var uri = new Uri("http://192.168.2.3:54240/api/DataSenders");
            var StrRespPost = await PostRest.Post(output, uri);

            if (StrRespPost == "Ok")                                              //     prosoxh den exei nohma thelei diorthwsh
            {
                new AlertDialog.Builder(this)
                .SetMessage("Successfully Send")
                .SetTitle("Message")
                .Show();
            }
            else
            {
                new AlertDialog.Builder(this)
                .SetMessage("Unsuccessfull Send")
                .SetTitle("Message")
                .Show();
            }
        }


        private void Button4_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ChatBubbleActivity));
            //intent.PutExtra("MyString", "This is a string");
            StartActivity(intent);
        }


        #region ListView AlertDialog

        List<SelectChoice> selectChoice = new List<SelectChoice>();
        List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
        List<string> _lstDataItem = new List<string>();
        string Name;
        int Number;

        void methodInvokeAlertDialogWithListView(object sender, EventArgs e)
        {
            bool IsValidJson1;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();                       // check it  ( it was changed )

            ConsumeRest cRest = new ConsumeRest();

            object strResponse;

            string endpoint = "http://192.168.2.3:54240/api/YOURCONTROLLER/" + PerID;     //Contacts"
            strResponse = cRest.makeRequest(endpoint);
            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                List<ContactsPerson> deserializedContacts = JsonConvert.DeserializeObject<List<ContactsPerson>>(strResponse.ToString());
                //Number = deserializedContacts.Count;
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
               .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
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
                    ConID = selectChoice[i].Contactid;
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

        private void Button2_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(FindPictureGalleryActivity));
            StartActivity(intent);  
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Photo2Activity));
            StartActivity(intent);
        }
    }
}