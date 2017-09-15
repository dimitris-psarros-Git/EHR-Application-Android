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
using EHR_Application.Models;
using Newtonsoft.Json;
using Android.Graphics;
using EHR_Application.Adapters;

namespace EHR_Application.Activities
{
    [Activity(Label = "    Images   "/*, Theme = "@style/MyTheme", MainLauncher = true*/)]
    public class ImagesActivity : Activity
    {
        ImageView imageView;
        List<ContactsPerson2> contactsPerson2;
        List<imagesChat> imageChats;
        List<imageIDs> imageIds;
        CustomAdapter2 Adapt;
        Bitmap bitmap;
        ListView  myList;
        byte[] Image;
        int PictureID;
        int myID, connectID;
        TextView txtImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesList);

            // Data from previous Activity
            myID = Intent.GetIntExtra("myID", -1);
            connectID = Intent.GetIntExtra("receiverID", -1);

            myList = FindViewById<ListView>(Resource.Id.listViewImages);
            imageView = FindViewById<ImageView>(Resource.Id.imageViewV);
            txtImage = FindViewById<TextView>(Resource.Id.textImage);

            Adapt = new CustomAdapter2(contactsPerson2);

            Actions();

            myList.Adapter = new CustomAdapter2(contactsPerson2);
            myList.ItemClick += MyList_ItemClick;
        }
        
        private void Actions()
        {
            bool IsValidJson;
            string endpoint;
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();

            object strResponse;
            Address address = new Address();

            RetrieveData retrieve = new RetrieveData();  // retrieve "IsDoctor"
            bool IsDoctor = retrieve.RetreiveBool();

            if (IsDoctor == false) { endpoint = address.Endpoint + "Messages3/" + myID + "/" + connectID ;}
            else                   { endpoint = address.Endpoint + "Messages3/" + connectID + "/" + myID ;}

            strResponse = cRest.makeRequest(endpoint);
            IsValidJson = validateJson.IsValidJson(strResponse);
            if (IsValidJson == true)
            {
                imageIds = JsonConvert.DeserializeObject<List<imageIDs>>(strResponse.ToString());
                imageIds = imageIds.OrderBy(i => i.FirstName).ToList();
            }
            else
            {
                new AlertDialog.Builder(this)
              .SetTitle("An error has occured")
              .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
              .Show();
            }
            SetData();
        }
        
        public void SetData()
        {
            var temp = new List<ContactsPerson2>();
            for (int i = 0; i < imageIds.Count; i++)
            {
                Adduser(temp, i);
            }
            contactsPerson2 = temp.OrderBy(i => i.FirstName).ToList();   // xwris auth thn entolh uparxei sfalma !!
        }

        public void Adduser(List<ContactsPerson2> contactsPerson2, int k)
        {
            string Message;
            
            RetrieveData retrieve = new RetrieveData();
            bool IsDoctor = retrieve.RetreiveBool();
           
            if      (IsDoctor == false && imageIds[k].Sender == 1) { Message = imageIds[k].FirstName + " " + imageIds[k].LastName; }
            else if (IsDoctor == true  && imageIds[k].Sender == 0) { Message = imageIds[k].FirstName1 + " " + imageIds[k].LastName1; }
            else { Message = "My Message "; }

            contactsPerson2.Add(new ContactsPerson2()
            {
                FirstName = Message,                         //imageIds[k].DataSenderId.ToString(),
                LastName = imageIds[k].Date.ToString(),
                ImageUrl = "contact.png"
            });
        }

        private void MyList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + Adapt.GetItemId(e.Position), ToastLength.Short).Show();
            int NumbPressed = (int)Adapt.GetItemId(e.Position);
            Toast.MakeText(this, "You Pressed : " + contactsPerson2[NumbPressed].FirstName, ToastLength.Short).Show();
            PictureID = imageIds[e.Position].DataSenderId;   
            Action2();
        }
        
        private void Action2()
        {
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();
            Address address = new Address();
           
            object strResponse2;
            string endpoint2 = address.Endpoint + "DataSenders/" + PictureID;
            strResponse2 = cRest.makeRequest(endpoint2);
            IsValidJson = validateJson.IsValidJson(strResponse2);
            if (IsValidJson == true)
            {
                imageChats = JsonConvert.DeserializeObject<List<imagesChat>>(strResponse2.ToString());
                Image = imageChats[0].Picture;
                Bitmap bitmap = BitmapFactory.DecodeByteArray(Image, 0,
                Image.Length);
                imageView.SetImageBitmap(bitmap);

                txtImage.Text = imageChats[0].Text;
            }
            else
            {
                new AlertDialog.Builder(this)
              .SetTitle("An error has occured")
              .SetMessage("No data found due to unexpected problem")
              .Show();
            }

        }
    }
}