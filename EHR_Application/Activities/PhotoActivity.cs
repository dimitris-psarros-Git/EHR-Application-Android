using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Net;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Android.Media;
using System.Collections;
using System.Drawing;
using System.Drawing;


namespace EHR_Application
{
    [Activity(Label = "Photo")]
    public class PhotoActivity : Activity
    {

        //////////////////////////////////////////////      start new code


        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);
        //    //_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
        //    SetContentView(Resource.Layout.PictureLayout);
        //    Button button = FindViewById<Button>(Resource.Id.myButton);
        //    //ImageButton button = FindViewById<ImageButton>(Resource.Id.myButton);
        //    button.Click += BtnCameraClick;
        //}

        //private string _imageUri;
        //private Intent intent;

        //private Boolean isMounted
        //{
        //    get
        //    {
        //        return Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted);
        //    }
        //}

        //public void BtnCameraClick(object sender, EventArgs eventArgs)
        //{
        //    var uri = ContentResolver.Insert(isMounted
        //                                         ? MediaStore.Images.Media.ExternalContentUri
        //                                         : MediaStore.Images.Media.InternalContentUri, new ContentValues());
        //    _imageUri = uri.ToString();
        //    intent = new Intent(MediaStore.ActionImageCapture);
        //    // bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
        //    intent.PutExtra(MediaStore.ExtraOutput, uri);
        //    StartActivityForResult(intent, 1001);
        //}

        //byte[] Picture_array;

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{

        //    if (resultCode == Result.Ok && requestCode == 1001)
        //    {
        //        Android.Graphics.Bitmap bitmap;
        //        byte[] Picture_array;
        //        try  //  get the bytes from the image
        //        {
        //            bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
        //                Picture_array = stream.ToArray();
        //            }

        //        }
        //        catch (Java.IO.IOException e)
        //        {
        //            //Exception Handling
        //        }

        //    }
        //}

        /*
        Android.Net.Uri _currentImageUri = Android.Net.Uri.Parse(_imageUri);
        Android.Graphics.Bitmap bitmap = BitmapFactory.DecodeStream(ContentResolver.OpenInputStream(_currentImageUri));    //changes

        byte[] bitmapData = null;

        using (MemoryStream stream = new MemoryStream())
        {
            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);    // changed
            bitmapData = stream.ToArray();
        }

        bitmap.Dispose();
        }
        */




        //public Android.Media.Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    using (MemoryStream mStream = new MemoryStream(byteArrayIn))
        //    {

        //        MemoryStream ms = new MemoryStream(byteArrayIn);
        //        Android.Media.Image returnImage = Android.Media.Image.FromStream(ms);
        //        return returnImage;
        //        using (var image = System.Drawing.Image.FromStream(sourcePath))
        //            return Android.Media.Image.FromStream(mStream);
        //    }
        //}

        // }
        //}

        /////////////////////////////////////////////////////////////      end new code



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
            App._dir = new Java.IO.File(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "Κάμερα");
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
            App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);

            //take image bytes
            //It's a good idea that you check this before accessing the data
            if (requestCode == 0 && resultCode == Result.Ok)
            {
                //get the image bitmap from the intent extras
               // var image = (Android.Graphics.Bitmap)data.Extras.Get("data");
                Android.Graphics.Bitmap photo = (Android.Graphics.Bitmap)data.Extras.Get("data");
            }

                //    ////////////   start new code
                //    //Android.Graphics.Bitmap bitmap;
                //    //byte[] Picture_array;
                //    //try  //  get the bytes from the image
                //    //{
                //    //    bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                //    //    using (MemoryStream stream = new MemoryStream())
                //    //    {
                //    //        bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
                //    //        Picture_array = stream.ToArray();
                //    //    }

                //    //}
                //    //catch (Java.IO.IOException e)
                //    //{
                //    //    //Exception Handling
                //    //}
                //    ////////////  end new code

                //    // you might also like to check whether image is null or not
                //    // if (image == null) do something


                //    var image = (Android.Graphics.Bitmap)data.Extras.Get("data");

                //    //convert bitmap into byte array
                //    byte[] bitmapData;
                //    using (var stream = new MemoryStream())
                //    {
                //        image.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                //        bitmapData = stream.ToArray();
                //    }

                //    Intent intent = new Intent(this, typeof(SendDataActivity));

                //    intent.PutExtra("picture", bitmapData);

                //    StartActivity(intent);
                //}


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

    }

    public static class BitmapHelpers
    {
        public static Android.Graphics.Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Android.Graphics.Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }


    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Android.Graphics.Bitmap bitmap;
    }


}