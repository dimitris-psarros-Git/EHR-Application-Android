using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Util;
using Android.Content;
using EHR_Application.Activities;
using EHR_Application.Models;
using Android.Views;
using Android.Support.V7.App;
using Android.Preferences;
using EHR_Application.Post_Get;

namespace EHR_Application
{
    [Activity(Label = "    Login   ", Icon = "@drawable/icon" ,Theme = "@style/MyTheme" /*Theme = "@style/MyTheme1"*/ /*"@style/Theme.AppCompat.Light"*/ )]
    public class LoginActivity : AppCompatActivity
    {
        List<NewMessages2> newMessages;
        TextView txt1;
        TextView txt2;
        EditText Username;
        EditText Password;
        int myId = 0;
        bool IsDoctor;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainLayout);
            
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);
            
            txt1 = FindViewById<TextView>(Resource.Id.txt1);    
            txt2 = FindViewById<TextView>(Resource.Id.txt2);

            Username = FindViewById<EditText>(Resource.Id.Username);
            Password = FindViewById<EditText>(Resource.Id.Password);

            Button button1 = FindViewById<Button>(Resource.Id.Login);
            button1.Click += Button1_ClickAsync;

            PeriodicCheckNewMessages();
        }

        private void PeriodicCheckNewMessages()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(2);
            var timer = new System.Threading.Timer((e) =>
            {
                if (myId != 0)
                {
                    MessagesPopUp();
                }
            }, null, startTimeSpan, periodTimeSpan);
        }
        
        private void MessagesPopUp() 
        {
            object strResponse,strResponse2;
            bool IsValidJson,IsValidJson2,newImages;
            string endpoint,endpoint2;
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();

            if (IsDoctor == false) { endpoint  = address.Endpoint + "PatientNewMessages1/" + myId;
                                     endpoint2 = address.Endpoint + "PatientNewImages1/"   + myId; }
            else                   { endpoint  = address.Endpoint + "DoctorNewMessages1/"  + myId;
                                     endpoint2 = address.Endpoint + "DoctorNewImages1/"    + myId; }

            strResponse = cRest.makeRequest(endpoint);   // elegxos gia nea mhnumata keimenou
            strResponse2 = cRest.makeRequest(endpoint2); // elegxos gia nea mhnumata eikonas
            ValidateJson validateJson = new ValidateJson();
            IsValidJson  = validateJson.IsValidJson(strResponse);
            IsValidJson2 = validateJson.IsValidJson(strResponse2);

            if (IsValidJson && IsValidJson2)
            {
                // check for new images
                newMessages = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                if (newMessages.Count != 0) { newImages = true;  }
                else                        { newImages = false; }


                newMessages = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                
                if (newMessages.Count != 0)
                {
                    for(int i=0; i < newMessages.Count; i++)
                    {
                        MessageDelivered(i);                    
                    }
                    
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "New message Arrived", ToastLength.Short).Show();
                    });
                   
                    
                    // Set up an intent so that tapping the notifications returns to this app:
                    Intent intent = new Intent(this, typeof(NewMessagesListActivity));

                    // Pass some information to SecondActivity:
                    Intent.PutExtra("myID", myId);
                    Intent.PutExtra("newImage", newImages);

                    // Create a task stack builder to manage the back stack:
                    TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);

                    // Add all parents of SecondActivity to the stack: 
                    stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(ChatBubbleActivity)));

                    // Push the intent that starts SecondActivity onto the stack:
                    stackBuilder.AddNextIntent(intent);
                    
                    //for (int i = 0; i < newMessages.Count; i++)
                    //{
                        //int NotificName = newMessages[i].PersonID;
                        ///string NotificDate = newMessages[i].Date;
                        //string NotificText = newMessages[i].Text;

                        // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
                        const int pendingIntentId = 0;
                        PendingIntent pendingIntent =
                            PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);
                        

                        //Instantiate the builder and set notification elements:
                        Notification.Builder builder = new Notification.Builder(this)
                            .SetAutoCancel(true)
                            .SetOnlyAlertOnce(true)
                            .SetContentIntent(pendingIntent)
                            //.SetContentTitle("New Message from " + newMessages[i].FirstName + " " + newMessages[i].LastName)
                            //.SetContentText("Text : " + NotificText)
                            .SetContentTitle("New messages arrived")
                            .SetProgress(4, 1, true)
                            
                            .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                            .SetSmallIcon(Resource.Drawable.Icon);
                        
                        // Build the notification:
                        Notification notification = builder.Build();

                        // Get the notification manager:
                        NotificationManager notificationManager =
                            GetSystemService(Context.NotificationService) as NotificationManager;

                        // Publish the notification:
                        const int notificationId = 0;
                        notificationManager.Notify(notificationId, notification);
                    //}
                }
            }
        }

        private async void MessageDelivered(int inumber)
        {
            Address address = new Address();
            string endpoint3;

            PutRest putrest = new PutRest();
            endpoint3 = address.Endpoint + "DataSenders/" + newMessages[inumber].DataSenderID;
            var uri = new Uri(endpoint3);

            NewMessages newmessages = new NewMessages();
            newmessages.DataSenderID = newMessages[inumber].DataSenderID;
            newmessages.Seen = false;
            newmessages.Send = true;

            string output = JsonConvert.SerializeObject(newmessages);
            var StrRespPost = await PutRest.Put(output, uri);
        }
        
        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            String InsertUser = Username.Text;
            String InsertPasw = Password.Text;
            
            string endpoint1;
            PostRest p = new PostRest();
            Address address = new Address();
            endpoint1 = address.Endpoint + "Users";
            var uri = new Uri(endpoint1);
            
            Usercheck usercheck = new  Usercheck();
            usercheck.UserName = InsertUser;
            usercheck.Password = InsertPasw;
            
            string output = JsonConvert.SerializeObject(usercheck);
            var StrRespPost = await PostRest.Post(output, uri, false);
            ValidateJson validatejson = new ValidateJson();
            bool valid = validatejson.IsValidJson(StrRespPost);

            if (valid)   
            {
                try
                {
                    UserIdentity userident = JsonConvert.DeserializeObject<UserIdentity>(StrRespPost.ToString());

                    if (userident != null)
                    {
                        IsDoctor = userident.IsDoctor;
                        myId = userident.PersonID;

                        SaveBool(IsDoctor);  //save in Shared Preferences if it is Doctor

                        if (IsDoctor)
                        {
                            var intent = new Intent(this, typeof(DemographicsDoctorActivity));
                            intent.PutExtra("myID", myId);
                            StartActivity(intent);
                        }
                        else
                        {
                            var intent = new Intent(this, typeof(DemographicActivity));
                            intent.PutExtra("myID", myId);
                            StartActivity(intent);
                        }
                    }
                    else
                    {
                        txt2.Text = "Unsuccessfull Login due to an error";
                    }
                }
                catch
                {
                    txt2.Text = "Unsuccessfull Login due to an error";
                }
            }
            else
            {
                txt2.Text = "Unsuccessfull Login UserName or Password isn't valid" + "\n" + "Or check internet connection" +"\n" + StrRespPost;
            }

        }

        public void AlertBox()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
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

        private void SaveBool(bool IsDoctor)    //save if it is Doctor or Patient
        {
            Context mContext = Android.App.Application.Context;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("Is_Doctor", IsDoctor);
            //editor.Commit();    // applies changes synchronously on older APIs
            editor.Apply();
        }

        public override void OnBackPressed()
        {
            if (this.FragmentManager.BackStackEntryCount > 0)
                base.OnBackPressed();
        }

        public void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}




