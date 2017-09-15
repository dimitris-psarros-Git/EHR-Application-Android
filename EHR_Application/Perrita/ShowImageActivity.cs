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
using Newtonsoft.Json;
using EHR_Application.Models;
using Android.Graphics;

namespace EHR_Application.Activities
{
    [Activity(Label = "ShowImageActivity")]
    public class ShowImageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ShowIm);

            MainCode();
        }

        public void MainCode()
        {
            ConsumeRest cRest = new ConsumeRest();

            object strResponse;
            Address address = new Address();
            string endpoint = address.Endpoint + "DataSenders/2052";
        
            strResponse = cRest.makeRequest(endpoint);

            /*List<RecievedImage> */
            if (strResponse != null)
            {
                var deserializedContacts = JsonConvert.DeserializeObject<RecievedImage>(strResponse.ToString());
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found due to unexpected problem" + "n/" + strResponse)
                .Show();
            }

        }

        public Bitmap ByteArrayToImage(byte[] imageData)
        {
            var bmpOutput = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            return bmpOutput;
        }

    }
}