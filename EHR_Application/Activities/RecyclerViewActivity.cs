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

namespace EHR_Application.Activities
{
    [Activity(Label = "RecyclerViewActivity")]
    public class RecyclerViewActivity : Activity
    {
        System.Drawing.Image Image2;
        System.Drawing.Image Image4;
        private RecyclerView recycler;
        private RecyclerViewAdapter adapter;
        private RecyclerView.LayoutManager layoutManager;
        List<DataRecyclerView> lstData = new List<DataRecyclerView>();
        imaGG imaGg;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecyclerviewLayout);

            ////////////////////////// start new code 
            object JsonText = LoadJson();

            imaGg = JsonConvert.DeserializeObject<imaGG>(JsonText.ToString());

            //Image4 = byteArrayToImage(imaGg.Picture);
            Bitmap bitmap22 = bytesToBitmap(imaGg.Picture);
            Image i = (Image)bitmap22;
            //Image4 = Base64ToImage(imageBytes);
            //////////////////////////  end new code

            InputData();
            recycler = FindViewById<RecyclerView>(Resource.Id.recycler);
            recycler.HasFixedSize = true;
            layoutManager = new LinearLayoutManager(this);
            recycler.SetLayoutManager(layoutManager);
            adapter = new RecyclerViewAdapter(lstData);
            recycler.SetAdapter(adapter);
            recycler.Click += Recycler_Click;     //check this line 
        }

        private void Recycler_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InputData()
        {
            //lstData.Add(new DataRecyclerView() { imageId = Resource.Drawable.Icon, description = "Icon" });
              lstData.Add(new DataRecyclerView() { /*imageId = Image4,*/ description = "Keys" });
            //lstData.Add(new DataRecyclerView() { imageId = Resource.Drawable.Icon, description = "Icon" });
            //lstData.Add(new DataRecyclerView() { imageId = Resource.Drawable.keys, description = "Keys" });
            //lstData.Add(new DataRecyclerView() { imageId = Resource.Drawable.EHRimage, description = "EHRimage" });
        }

       
        public static Android.Graphics.Bitmap bytesToBitmap(byte[] imageBytes)
        {

            Android.Graphics.Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

            return bitmap;
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {

            //Image2  = (Bitmap)((new ImageConverter()).ConvertFrom(byteArrayIn));
            //imageBytes = Convert.FromBase64String(hfValue);
            MemoryStream img = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(img);
            return returnImage;
            //return Image2;
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