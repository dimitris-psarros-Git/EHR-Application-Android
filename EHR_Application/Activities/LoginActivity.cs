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
using Android.Graphics;

namespace EHR_Application
{
    [Activity(Label = "    Login   ", Icon = "@drawable/icon" ,Theme = "@style/MyTheme" /*Theme = "@style/MyTheme1"*/ /*"@style/Theme.AppCompat.Light"*/ )]
    public class LoginActivity : AppCompatActivity
    {
        List<NewMessages2> newMessages, newMessages2;
        TextView txt1;
        TextView txt2;
        EditText Username;
        EditText Password;
        int myId;//= 0;
        bool IsDoctor;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainLayout);

            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            txt2 = null;
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
                newMessages2 = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse2.ToString());
                if (newMessages2.Count != 0) { newImages = true;  }
                else                        { newImages = false; }


                newMessages = JsonConvert.DeserializeObject<List<NewMessages2>>(strResponse.ToString());
                
                if (newMessages.Count != 0 || newImages==true )
                {
                    if (newMessages.Count != 0)
                    {
                        for (int i = 0; i < newMessages.Count; i++)
                        {
                            MessageDelivered(i,false);
                        }
                    }
                    if(newImages == true)
                    {
                        for (int i = 0; i < newMessages2.Count; i++)
                        {
                            MessageDelivered(i,true);
                        }
                    }
                    
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "New message Arrived", ToastLength.Short).Show();
                    });
                   
                    // Set up an intent so that tapping the notifications returns to this app:
                    Intent intent = new Intent(this, typeof(NewMessagesListActivity));

                    // Pass some information to SecondActivity:
                    intent.PutExtra("myID", myId);
                    intent.PutExtra("newImage", newImages);
                    Intent.PutExtra("message", "Greetings from MainActivity!");

                    // Create a task stack builder to manage the back stack:
                    TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);

                    // Add all parents of SecondActivity to the stack: 
                    stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(NewMessagesListActivity)));

                    // Push the intent that starts SecondActivity onto the stack:
                    stackBuilder.AddNextIntent(intent);
                    
                   
                        // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
                        const int pendingIntentId = 0;
                        PendingIntent pendingIntent =
                            PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);
                        

                        //Instantiate the builder and set notification elements:
                        Notification.Builder builder = new Notification.Builder(this)
                            .SetAutoCancel(true)
                            .SetOnlyAlertOnce(true)
                            .SetContentIntent(pendingIntent)
                            .SetContentTitle("New messages arrived")
                            .SetContentText("There are "+newMessages.Count.ToString()+" new text messages , and "+newMessages2.Count.ToString()+" new image messages")
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
                }
            }
        }

        private  async  void MessageDelivered(int inumber,bool image)
        {
            Address address = new Address();
            string endpoint3;
            string output;
            PutRest putrest = new PutRest();
            NewMessages newmessages = new NewMessages();
            if (image == true) {
                endpoint3 = address.Endpoint + "DataSenders/" + newMessages2[inumber].DataSenderID;
                
                newmessages.DataSenderID = newMessages2[inumber].DataSenderID;
                newmessages.Seen = false;
                newmessages.Send = true;
                output = JsonConvert.SerializeObject(newmessages);
            }
            else
            {
                endpoint3 = address.Endpoint + "DataSenders/" + newMessages[inumber].DataSenderID;
                newmessages.DataSenderID = newMessages[inumber].DataSenderID;
                newmessages.Seen = false;
                newmessages.Send = true;
                output = JsonConvert.SerializeObject(newmessages);
            }
            var uri = new Uri(endpoint3);
          
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
                catch
                {
                    txt2.Text = "Unsuccessfull Login due to an error";
                }
            }
            else if(StrRespPost=="NotFound")
            {
                txt2.Text = "Error"+ "\n" + "Unsuccessfull Login UserName or Password isn't valid ," + StrRespPost;
                txt2.SetTextColor(Color.Red);
                Password.Text = "";
            }
            else
            {
                txt2.Text = "An error occured :" + "\n" + StrRespPost;
                txt2.SetTextColor(Color.Red);
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




