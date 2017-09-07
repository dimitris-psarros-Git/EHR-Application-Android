using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Provider;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;

namespace EHR_Application.Activities
{
    [Activity(Label = "    Photo  ")]
    public class Photo2Activity : Activity
    {
        ImageView imageView;
        Byte[] Photopicture;
        Bitmap bitmap;
        int myID,receiverID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PictLayout);

            // Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);

            Button button = FindViewById<Button>(Resource.Id.btnCamera);
            button.Click += Button_Click;
            Button button1 = FindViewById<Button>(Resource.Id.Return);
            button1.Click += Button1_Click;
            Button button2 = FindViewById<Button>(Resource.Id.Cancel);
            button2.Click += Button2_Click;

            imageView = FindViewById<ImageView>(Resource.Id.imageView);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SendDataActivity));
            StartActivity(intent);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SendDataActivity));
            intent.PutExtra("image", Photopicture);
            intent.PutExtra("myID", myID);
            intent.PutExtra("receiverID", receiverID);
            StartActivity(intent);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }
        
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            bitmap = (Bitmap)data.Extras.Get("data");
            Photopicture = BitmapToByte();
            imageView.SetImageBitmap(bitmap);            
        }

        protected byte[] BitmapToByte()
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            byte[] bitmapData = stream.ToArray();
            return bitmapData;
        }
    }
}