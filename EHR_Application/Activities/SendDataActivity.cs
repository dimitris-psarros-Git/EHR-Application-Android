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
using System.Threading.Tasks;
using Android.Preferences;

namespace EHR_Application
{
    [Activity(Label = "     SendData  ", Theme = "@style/MyTheme1")]
    public class SendDataActivity : Activity
    {
        TextView txt1;
        EditText message1;
        EditText message2;
        DateTime now;
        //List<ContactsPerson2> contactsPerson2;

        bool IsDoctor;
        byte[] image;
        byte[] image1,image2;
        int myID;
        int receiverID;
        DateTime datetime;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SdataLayout);
            
            // Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);
            int PersID = Intent.GetIntExtra("PErsonId", -1);
            image1 = Intent.GetByteArrayExtra("image");
            image = Intent.GetByteArrayExtra("picture");   // allaksame to image apo var se byte[]

            RetrieveData retrieve = new RetrieveData();  // retrieve "IsDoctor"
            IsDoctor = retrieve.RetreiveBool();

            txt1 = FindViewById<TextView>(Resource.Id.txt1);
            message1 = FindViewById<EditText>(Resource.Id.editTxt1);
            message2 = FindViewById<EditText>(Resource.Id.editxt);

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

        //#region MenuInflater
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.options_menu, menu);
        //    return true;
        //}
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    if (id == Resource.Id.action_settings3)
        //    {
        //        Toast.MakeText(this, "Refresh", ToastLength.Short).Show();
        //        this.Recreate();
        //        return true;
        //    }
        //    else if (id == Resource.Id.action_settings2)
        //    {
        //        Toast.MakeText(this, "Exit", ToastLength.Short).Show();
        //        AlertBox();
        //        return true;
        //    }
        //    return base.OnOptionsItemSelected(item);
        //    //return super.onCreateView(inflater, container, savedInstanceState);
        //}
        //#endregion

        private async void SendPhoto_ClickAsync(object sender, EventArgs e)
        {
            if (image1 == null)
            {
                new AlertDialog.Builder(this)
                .SetMessage("No Image Selected")
                .SetTitle("Message")
                .Show();
            }
            else
            {
                datetime = DateTime.Now.ToLocalTime();
            
                String Message1 = message1.Text;
                using (var client = new HttpClient())
                {
                    Address address = new Address();
                    string endpoint = address.Endpoint + "DataSenderImage";   

                    var requestContent = new MultipartFormDataContent();
                    image2 = image1;
                    var imageContent = new ByteArrayContent(image2);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/txt");
                    
                    if (IsDoctor == false)        //  to ID toy asthenh prepei na stalei prwto
                    {
                        requestContent.Add(new StringContent(myID.ToString()), "myID");
                        requestContent.Add(new StringContent(receiverID.ToString()), "receiverID");
                        requestContent.Add(new StringContent(0.ToString()), "Sender");
                    }
                    else
                    {
                        requestContent.Add(new StringContent(receiverID.ToString()), "receiverID");
                        requestContent.Add(new StringContent(myID.ToString()), "myID");
                        requestContent.Add(new StringContent(1.ToString()), "Sender");
                    }
                    requestContent.Add(imageContent, "image", "image.png");
                    requestContent.Add(new StringContent(Message1.ToString()), "Message");
                    requestContent.Add(new StringContent("false"), "Seen");
                    requestContent.Add(new StringContent(datetime.ToString()), "date");     //check it
                    requestContent.Add(new StringContent("false"), "Send");

                    //var response = 
                    await client.PostAsync(endpoint, requestContent);//.Result;   
                    //var input = await response.Content.ReadAsStringAsync();
                }
            }
        }

        private async void Sendtext_ClickAsync(object sender, EventArgs e)
        {
            datetime = DateTime.Now.ToLocalTime();
            

            SendMessages sendMessageJson = new SendMessages();

            if (IsDoctor == false)        //  to ID tou asthenh prepei na stalei prwto
            {
                sendMessageJson.PatientID = myID;
                sendMessageJson.DoctorID = receiverID;
                sendMessageJson.Sender = 0;        
            }
            else
            {
                sendMessageJson.PatientID = receiverID;
                sendMessageJson.DoctorID = myID;
                sendMessageJson.Sender = 1;
            }
            sendMessageJson.Text = message2.Text;
            sendMessageJson.Seen = false;
            sendMessageJson.Date = datetime.ToString();
            sendMessageJson.Send = false;

            string output = JsonConvert.SerializeObject(sendMessageJson);
            Address address = new Address();
            string endpoint = address.Endpoint + "DataSenders";
            var uri = new Uri(endpoint);
            var StrRespPost = await PostRest.Post(output, uri,true);
            
            if (StrRespPost == "Created")        // tsekare to giati mporei na stalei kai na epistrepsei diaforetiko mhnuma
            {
                new AlertDialog.Builder(this)
                .SetMessage("Successfully Send")
                .SetTitle("Message")
                .Show();
            }
            else
            {
                new AlertDialog.Builder(this)
                .SetMessage("Unsuccessfull Send!!"+ "\n" + "Something went wrong" + "\n" + StrRespPost)
                .SetTitle("Message")
                .SetIcon(Resource.Drawable.error)
                .Show();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ChatBubbleActivity));
            intent.PutExtra("myID", myID);
            intent.PutExtra("receiverID", receiverID);
            StartActivity(intent);
        }
        

        #region ListView AlertDialog
        List<SelectChoice> selectChoice = new List<SelectChoice>();
        List<Tuple<int, string>> mylist = new List<Tuple<int, string>>();
        List<FullNames> fullnameList1 = new List<FullNames>();
        List<string> _lstDataItem = new List<string>();
        List<friends> deserializedContacts;
        string Name;


        void methodInvokeAlertDialogWithListView(object sender, EventArgs e)
        {
            string endpoint2;
            bool IsValidJson1;
            _lstDataItem = new List<string>();
            _lstDataItem.Clear();
            //object strResponse2;
            ConsumeRest cRest = new ConsumeRest();
            //List<FullNames> fullName;

            object strResponse;
            Address address = new Address();

            if (IsDoctor == false)
            {
                endpoint2 = address.Endpoint + "PatientFriends/" + myID;
            }
            else
            {
                endpoint2 = address.Endpoint + "DoctorFriends/" + myID;
            }

            strResponse = cRest.makeRequest(endpoint2);
            ValidateJson validateJson = new ValidateJson();
            IsValidJson1 = validateJson.IsValidJson(strResponse);

            if (IsValidJson1)
            {
                deserializedContacts = JsonConvert.DeserializeObject<List<friends>>(strResponse.ToString());
                for (int i = 0; i < deserializedContacts.Count; i++)
                {
                    string NAME = deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName;

                    _lstDataItem.Add(NAME);
                    
                }
               
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetTitle("Select Person");
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
               .SetIcon(Resource.Drawable.error)
               .Show();
            }
        }
        
        void listViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "you clicked on " + _lstDataItem[e.Position], ToastLength.Short).Show();
            Name = _lstDataItem[e.Position];
            int Location = e.Position;

            //////////////////////////////////////////////   check this
            receiverID = deserializedContacts[e.Position].PersonID;
            //////////////////////////////////////////////    end of check


            for (int i = 0; i < deserializedContacts.Count; i++)
            {
                if (deserializedContacts[i].FirstName + " " + deserializedContacts[i].LastName == Name)
                {
                    receiverID = deserializedContacts[i].PersonID;
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
            intent.PutExtra("myID", myID);
            intent.PutExtra("receiverID", receiverID);
            StartActivity(intent);
        }

        public void AlertBox()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm Exit");
            alert.SetMessage("Do you really want to exit? ");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
                // Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
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