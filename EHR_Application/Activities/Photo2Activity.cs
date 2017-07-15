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
    [Activity(Label = "Photo2Activity")]
    public class Photo2Activity : Activity
    {
        ImageView imageView;
        Byte[] Photopicture;
        Bitmap bitmap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PictLayout);

            Button button = FindViewById<Button>(Resource.Id.btnCamera);

            Button button1 = FindViewById<Button>(Resource.Id.Return);
            button1.Click += Button1_Click;
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            button.Click += Button_Click;

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SendDataActivity));
            intent.PutExtra("image", Photopicture);
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

        public byte[] BitmapToByte()
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            byte[] bitmapData = stream.ToArray();
            return bitmapData;
        }
    }
}