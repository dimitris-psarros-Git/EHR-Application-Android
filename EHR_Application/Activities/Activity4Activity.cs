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
using Android.Graphics;
using System.Drawing;
using System.IO;
using Android.Media;

namespace EHR_Application
{
    [Activity(Label = "Activity4")]
    public class Activity4Activity : Activity
    {
        ImageView image2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout4);
            byte[] image = Intent.GetByteArrayExtra("image");    // tha mporouse na einai kai <var>
            image2 = FindViewById<ImageView>(Resource.Id.imageView1);

        }



        public void PrintImage(byte[] byteArray)
        {
            Android.Graphics.Bitmap bmp = BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length);
            //ImageView image = (ImageView)findViewById(R.id.imageView1);
            
        }


        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

                return returnImage;
            }
            catch
            {
                return null ;
            }
        }
    }
}