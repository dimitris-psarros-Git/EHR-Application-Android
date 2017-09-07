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
using Android.Support.V7.Widget;
using EHR_Application.Adapters;
using Android.Content.Res;
using System.IO;
using Android.Media;
using EHR_Application.Models;
using Newtonsoft.Json;
using System.Drawing;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Java.Util;

namespace EHR_Application.Activities
{
    [Activity(Label = "RecyclerViewActivity")]
    public class RecyclerViewActivity : Activity
    {

        private RecyclerView recycler;
        private RecyclerViewAdapter adapter;
        private RecyclerView.LayoutManager layoutManager;
        Android.Graphics.Bitmap bitmapList,bitmap4;
        List<lstBitmap> Allbitmap = new List<lstBitmap>();
        List<lstBitmap> Allbitmap2 = new List<lstBitmap>();
        List<imageViews> Allimages = new List<imageViews>();
        List<DataRecyclerView> lstData = new List<DataRecyclerView>();
        List<imageIDs> imageIds;
        List<imagesChat> imageChats;
        imagesChat imageRecycle;
        Drawable icon;
        Drawable image23,imageRecycle1, imageRecycle12;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecyclerviewLayout);

            bitmap4 = Start();

            //Android.Graphics.Bitmap bitmap12 = ResizeBitmap(bitmap4, 600, 400);
            Android.Graphics.Bitmap bitmap12 = scaleDown(bitmap4, 300, true);
            imageRecycle12 = new BitmapDrawable(bitmap12);

            //int count = Allbitmap.Count;
            //for (int i=0; i<count; i++)
            //{
            //    Android.Graphics.Bitmap bitmapitem = scaleDown(Allbitmap[i].BitMap, 300, true);
            //    //Allbitmap2.Add(new lstBitmap() { BitMap = bitmapitem });
            //    Drawable imageRecycle23 = new BitmapDrawable(bitmapitem);
            //    Allimages.Add(new imageViews() {Image = imageRecycle23 });
            //}
            

            ///////////////////// new code see what isreturned
            

            /////////////////////   end of new code

            //LoadFromResources();

            object JsonText = LoadJson();

            imageRecycle = JsonConvert.DeserializeObject<imagesChat>(JsonText.ToString());

            Android.Graphics.Bitmap bitmap = BitmapFactory.DecodeByteArray(imageRecycle.Picture, 0, imageRecycle.Picture.Length);
            //Android.Graphics.Bitmap bitmap1 = ResizeBitmap(bitmap, 600, 400);
            Android.Graphics.Bitmap bitmap1 = scaleDown(bitmap, 3000, true);  // scale of image
            imageRecycle1 = new BitmapDrawable(bitmap1);
            
            //System.Drawing.Image Image42 = byteArrayToImage(imaGg.Picture);
            //Android.Graphics.Bitmap bitmap22 = bytesToBitmap(imaGg.Picture);
            //Android.Media.Image i = (Android.Media.Image)bitmap22;
           
            ///////////////////////////////////////////////////////////////////////////////////   o kwdikas
            InputData();

            recycler = FindViewById<RecyclerView>(Resource.Id.recycler);
            recycler.HasFixedSize = true;
            layoutManager = new LinearLayoutManager(this);
            recycler.SetLayoutManager(layoutManager);
            adapter = new RecyclerViewAdapter(lstData);
            recycler.SetAdapter(adapter);
           /////////////////////////////////////////////////////////////////////////////////////  end kwdika
        }

        public Android.Graphics.Bitmap Start()
        {
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();

            object strResponse;
            Address address = new Address();
            string endpoint = address.Endpoint+ "Messages2/" + 1000 + "/" + 1001;
            //string endpoint = "http://192.168.2.6:54240/api/Messages2/" + 1000 + "/" + 1001;
            strResponse = cRest.makeRequest(endpoint);
            IsValidJson = validateJson.IsValidJson(strResponse);
            if (IsValidJson == true)
            {
                imageIds = JsonConvert.DeserializeObject<List<imageIDs>>(strResponse.ToString());
                int DataSenId = imageIds[0].DataSenderId;
            }
            int k = 0;
            if (imageIds.Count != 0) {
                for (int i = 0; i < imageIds.Count; i++) {
                    //bitmapList = null;
                    object strResponse2;
                    //Address address = new Address()
                    string endpoint2 = address.Endpoint + "DataSenders/" + imageIds[i].DataSenderId;
                    strResponse2 = cRest.makeRequest(endpoint2);
                    IsValidJson = validateJson.IsValidJson(strResponse);
                    if (IsValidJson == true)
                    {
                        imageChats = JsonConvert.DeserializeObject<List<imagesChat>>(strResponse2.ToString());

                        ////// new code
                        //bitmapList = BitmapFactory.DecodeByteArray(imageChats[i].Picture, 0, imageChats[i].Picture.Length);
                        //Allbitmap.Add(new lstBitmap() { BitMap = bitmapList});
                        //bitmapList = null;
                        /////   end new code
                    }
                }
                bitmapList = BitmapFactory.DecodeByteArray(imageChats[0].Picture, 0, imageChats[0].Picture.Length);
                
                return bitmapList;
            }
            return null;
        }

        public static Android.Graphics.Bitmap scaleDown(Android.Graphics.Bitmap realImage, float maxImageSize , Boolean filter)
        {
            float ratio = Math.Min(
                    (float)maxImageSize / realImage.Width,
                    (float)maxImageSize / realImage.Height);
            double width = Math.Round((float)ratio * realImage.Width);
            double height = Math.Round((float)ratio * realImage.Height);

            Android.Graphics.Bitmap newBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(realImage, (int)width,
                    (int)height, filter);
            return newBitmap;
        }

        private Android.Graphics.Bitmap ResizeBitmap(Android.Graphics.Bitmap originalImage, int widthToScae, int heightToScale)
        {
            Android.Graphics.Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(widthToScae, heightToScale, Android.Graphics.Bitmap.Config.Argb8888);

            float originalWidth = originalImage.Width;
            float originalHeight = originalImage.Height;

            Canvas canvas = new Canvas(resizedBitmap);

            float scale = /*this.Width*/600 / originalWidth;

            float xTranslation = 0.0f;
            float yTranslation = (/*this.Height*/ 400 - originalHeight * scale) / 2.0f;

            Matrix transformation = new Matrix();
            transformation.PostTranslate(xTranslation, yTranslation);
            transformation.PreScale(scale, scale);

            Paint paint = new Paint();
            paint.FilterBitmap = true;

            canvas.DrawBitmap(originalImage, transformation, paint);

            return resizedBitmap;
        }

        private void InputData()
        {
            //lstData.Add(new DataRecyclerView() { imageId = Resource.Drawable.Icon, description = "Icon" });
            lstData.Add(new DataRecyclerView() { imageId = imageRecycle12, description = "Keys" });
            lstData.Add(new DataRecyclerView() { imageId = imageRecycle12, description = "Keys" });
            lstData.Add(new DataRecyclerView() { imageId = imageRecycle12, description = "Keys" });
            lstData.Add(new DataRecyclerView() { imageId = imageRecycle1, description = "Keys" });

            //int Count = Allimages.Count;
            //for(int i=0; i<Count; i++)
            //{
            //    lstData.Add(new DataRecyclerView() { imageId = Allimages[i].Image, description = "New Objects" });
            //}

        }


        public static Android.Graphics.Bitmap bytesToBitmap(byte[] imageBytes)
        {
            Android.Graphics.Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            return bitmap;
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.Image returnImage = null;
            MemoryStream img = new MemoryStream(byteArrayIn);
            returnImage = System.Drawing.Image.FromStream(img);
            return returnImage;
        }
        
        public void LoadFromResources()   
        {
            icon = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.Icon, null);
            Android.Graphics.Bitmap b = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Icon);
            //string imagefileName = "EHRimage.jpg";
            //int id = (int)typeof(Resource.Drawable).GetField(imagefileName).GetValue(null);
        }

        public object LoadJson()       
        {
            object jsontext;
            AssetManager assets = this.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("us.txt")))
            {
                jsontext = sr.ReadToEnd();
            }
            return jsontext;
        }
    }

}