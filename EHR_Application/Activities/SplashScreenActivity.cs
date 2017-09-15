using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace EHR_Application.Activities
{
    [Activity(Label = "  Wellcome " /*, MainLauncher=true*/ , Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar" )]
    public class SplashScreenActivity : AppCompatActivity
    {
        private ProgressBar progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splashsreenlayout);
            //progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
         
            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StartActivity(new Intent(this, typeof(LoginActivity)));
        }
    }
}
