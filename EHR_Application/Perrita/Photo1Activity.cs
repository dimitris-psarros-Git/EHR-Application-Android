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
using Java.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;

namespace EHR_Application.Activities
{
    [Activity(Label = "Photo1Activity")]
    public class Photo1Activity : Activity
    {
        ImageView _imageView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PictureLayout);
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.myButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                button.Click += TakeAPicture;
            }
        }
        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo32");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 0 && resultCode == Result.Ok)
            {
                //get the image bitmap from the intent extras
                // var image = (Android.Graphics.Bitmap)data.Extras.Get("data");
                Android.Graphics.Bitmap photo = (Android.Graphics.Bitmap)data.Extras.Get("data");
            }

            // Make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        //public static class BitmapHelpers
        //{
        //    public static Android.Graphics.Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        //    {
        //        // First we get the the dimensions of the file on disk
        //        BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
        //        BitmapFactory.DecodeFile(fileName, options);

        //        // Next we calculate the ratio that we need to resize the image by
        //        // in order to fit the requested dimensions.
        //        int outHeight = options.OutHeight;
        //        int outWidth = options.OutWidth;
        //        int inSampleSize = 1;

        //        if (outHeight > height || outWidth > width)
        //        {
        //            inSampleSize = outWidth > outHeight
        //                               ? outHeight / height
        //                               : outWidth / width;
        //        }

        //        // Now we will load the image and have BitmapFactory resize it for us.
        //        options.InSampleSize = inSampleSize;
        //        options.InJustDecodeBounds = false;
        //        Android.Graphics.Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

        //        return resizedBitmap;
        //    }
        //}
    }

    public  class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
}