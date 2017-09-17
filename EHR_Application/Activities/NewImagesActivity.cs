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
using EHR_Application.Adapters;
using Android.Graphics;
using Newtonsoft.Json;
using EHR_Application.Post_Get;

namespace EHR_Application.Activities
{
    [Activity(Label = "    New Images  ", Theme = "@style/MyTheme" /*, MainLauncher = true*/)]
    public class NewImagesActivity : Activity
    {
        ImageView imageView;
        List<ContactsPerson2> contactsPerson2;
        List<imagesChat> imageChats;
        List<imageIDs> imageIds;
        CustomAdapter2 Adapt;
        //Bitmap bitmap;
        ListView myList;
        byte[] Image;
        int PictureID;
        int myID;                         

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesList);

            //Data from Previous Activity
            myID = Intent.GetIntExtra("myID", -1);


            myList = FindViewById<ListView>(Resource.Id.listViewImages);
            imageView = FindViewById<ImageView>(Resource.Id.imageViewV);

            Adapt = new CustomAdapter2(contactsPerson2);

            Actions();

            myList.Adapter = new CustomAdapter2(contactsPerson2);
            myList.ItemClick += MyList_ItemClick;
        }
        
        private void Actions()
        {
            string endpoint;
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();

            object strResponse;
            Address address = new Address();

            RetrieveData retrieve = new RetrieveData();  // retrieve "IsDoctor"
            bool IsDoctor = retrieve.RetreiveBool();
            
            if (IsDoctor == false) { endpoint = address.Endpoint + "PatientNewPhotos/" + myID; }
            else                   { endpoint = address.Endpoint + "DoctorNewPhotos/" + myID;  }

            strResponse = cRest.makeRequest(endpoint);
            IsValidJson = validateJson.IsValidJson(strResponse);
            if (IsValidJson == true)
            {
                imageIds = JsonConvert.DeserializeObject<List<imageIDs>>(strResponse.ToString());
                imageIds = imageIds.OrderBy(i => i.FirstName).ToList();
                SetData();
            }
            else
            {
                imageIds = JsonConvert.DeserializeObject<List<imageIDs>>("[]".ToString());

                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetIcon(Resource.Drawable.error)
               .SetMessage("No data found due to unexpected problem" + "\n" + strResponse)
               .Show();
            }
        }

        public void SetData()
        {
            var temp = new List<ContactsPerson2>();
            for (int i = 0; i < imageIds.Count; i++)
            {
                Adduser(temp, i);
            }
            contactsPerson2 = temp.OrderBy(i => i.FirstName).ToList();   
        }

        public void Adduser(List<ContactsPerson2> contactsPerson2, int k)
        {
           
            contactsPerson2.Add(new ContactsPerson2()
            {
                FirstName = imageIds[k].FirstName + "  " + imageIds[k].LastName ,                     
                LastName =  imageIds[k].Date.ToString(),
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
            DeleteFromNew(PictureID);
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
                Bitmap bitmap = BitmapFactory.DecodeByteArray(Image,0,Image.Length);
                imageView.SetImageBitmap(bitmap);
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetIcon(Resource.Drawable.error)
               .SetMessage("No data found due to unexpected problem")
               .Show();
            }
        }

        private async void DeleteFromNew(int datasendID)
        {
            Address address = new Address();
            PutRest putrest = new PutRest();
            string endpoint3;

            endpoint3 = address.Endpoint + "DataSenders/" + datasendID;
            var uri = new Uri(endpoint3);

            NewMessages newmessages = new NewMessages();
            newmessages.DataSenderID = datasendID;
            newmessages.Send = true;
            newmessages.Seen = true;
            string output = JsonConvert.SerializeObject(newmessages);
            var StrRespPost = await PutRest.Put(output, uri);
        }
    }
}