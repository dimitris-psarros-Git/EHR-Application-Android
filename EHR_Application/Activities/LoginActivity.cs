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

namespace EHR_Application
{
    [Activity(Label = "EHR_Application", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class LoginActivity : Activity
    {
        TextView txt1;
        TextView txt2;
        EditText Username;
        EditText Password;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainLayout);

            

            Button button1 = FindViewById<Button>(Resource.Id.Login);
            button1.Click += Button1_ClickAsync;     // on the click button pressed

            txt1 = FindViewById<TextView>(Resource.Id.txt1);    // for 2 different data
            txt2 = FindViewById<TextView>(Resource.Id.txt2);

            Username = FindViewById<EditText>(Resource.Id.Username);
            Password = FindViewById<EditText>(Resource.Id.Password);
        }
        
        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            String InsertUser = Username.Text;
            String InsertPasw = Password.Text;

            /////////////////////////////////////////////////////////////////////////////   start new code

            /*
            PostRest p = new PostRest();
            var uri = new Uri("http://192.168.2.3:54240/api/Users");
            
            Usercheck usercheck = new  Usercheck();
            usercheck.UserName = InsertUser;
            usercheck.Password = InsertPasw;
            
            string output = JsonConvert.SerializeObject(usercheck);
            
            var StrRespPost = await PostRest.Post(output, uri);
            */

            var StrRespPost="{\"PersonID\":\"1000\",\"IsDoctor\":\"false\"}";  // edw to evala authereta giati eixa thema me thn epistrofh tou post apo to web api

            ValidateJson validatejson = new ValidateJson();            
            bool valid  = validatejson.IsValidJson(StrRespPost);

            if (true)   // prosoxh den exei nohma thelei diorthwsh
            {
                Userch userch = JsonConvert.DeserializeObject<Userch>(StrRespPost.ToString());
                if (userch!=null)
                {
                    bool IsDoctor = userch.IsDoctor;
                    int PersonId = userch.PersonID;
                    if (IsDoctor)
                    {
                        var intent = new Intent(this, typeof(RecyclerViewActivity));
                        intent.PutExtra("PerID", PersonId);
                        StartActivity(intent);
                    }
                    else
                    {
                        var intent = new Intent(this, typeof(RecyclerViewActivity));
                        intent.PutExtra("PerID", PersonId);
                        StartActivity(intent);
                    }
                }
                else
                {
                    new AlertDialog.Builder(this)
                                   .SetMessage("Unsuccessfull Login.\n" + "True Again")
                                   .SetTitle("Message")
                                   .Show();
                }
            }
            else
            {
                new AlertDialog.Builder(this)
                .SetMessage("Unsuccessfull Login.\n" + "True Again")
                .SetTitle("Message")
                .Show();
            }

            /////////////////////////////////////////////////////////////////////////////   end new code
  
            //List<User> user;
            //if (false)
            //{
            //    txt2.Text = "No data on this address !!";     // lathos elegxos. panta tha exei kati giati gurizwkai sfalamata
            //}         
            //else if ( true/*strResponse is string  && !string.IsNullOrWhiteSpace(strResponse.ToString()) */)
            //{
            //     //Print(" Rest client created ");
            //     //object JsonText = strResponse;
            //     object JsonText = LoadJson(); // upoad from file
            //     //Print(JsonText.ToString());
            //    try
            //    {
            //        bool IsDoctor;
            //        bool Found = false;
            //        int Number = 0;
            //        user = JsonConvert.DeserializeObject<List<User>>(JsonText.ToString());            
            //        if (user.Any())
            //        {
            //            int CountUsers = user.Count;
            //            //bool IsDoctor;
            //            for (int i = 0; i < CountUsers; i++)
            //            {
            //                string USERNAME = user[i].UserName;
            //                string PASSWORD = user[i].Password;
            //                if (USERNAME == InsertUser && PASSWORD == InsertPasw)
            //                {
            //                    IsDoctor = user[i].IsDoctor;
            //                    Found = true;
            //                    Number = i;
            //                }
            //            }
            //            if (Found == true)  // there is an assignement with this username and password
            //            {
            //                IsDoctor = user[Number].IsDoctor;
            //                int PerID = user[Number].PersonID;
            //                if (IsDoctor)
            //                {
            //                    var intent = new Intent(this, typeof(ShowImageActivity));
            //                    intent.PutExtra("PerID", "PerIDppppppp");
            //                    StartActivity(intent);
            //                }
            //                else
            //                {
            //                    var intent = new Intent(this, typeof(ShowImageActivity));
            //                    intent.PutExtra("PerID", PerID);
            //                    StartActivity(intent);
            //                }
            //            }
            //            else
            //            {
            //                txt2.Text = " Your UserName or Password is not correct. Try again ";
            //            }                 
            //        }
            //    }
            //    catch
            //    {
            //        txt2.Text = " No data for Users at database";
            //    }
            //}
            
        }
        
        //public void Print(string strDebugTest)
        //{
        //    try
        //    {
        //        System.Diagnostics.Debug.Write(strDebugTest + System.Environment.NewLine);
        //        txt1.Text = txt1.Text + strDebugTest + System.Environment.NewLine;
        //        //txtview1.SelectionStart = txtview1.TextLength;
        //        //txtview1.ScrollToCaret();
        //    }
        //    catch (Java.Lang.Exception ex)
        //    {
        //        System.Diagnostics.Debug.Write(ex.Message, ToString() + System.Environment.NewLine);
        //    }
        //}
        
        //public object LoadJson()          // load json from asset
        //{
        //    object jsontext;
        //    AssetManager assets = this.Assets;
        //    using (StreamReader sr = new StreamReader(assets.Open("userDETAILS.txt")))
        //    {
        //        jsontext = sr.ReadToEnd();
        //    }
        //    return jsontext;
        //}

    }


}

