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
using Newtonsoft.Json;

namespace EHR_Application.Activities
{
    [Activity(Label = "Allergies/Reactions" )]
    public class AllergiesActivity : Activity
    {
        List<Allergies> deserializedAllergies;
        private List<Allergies> mItems;
        private ListView mListView;
        int myID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            myID = Intent.GetIntExtra("myID", -1);

            SetContentView(Resource.Layout.Listallergies);
            mListView = FindViewById<ListView>(Resource.Id.AllergiesListView);

            actions();

            //mItems = new List<Allergies>();
            //mItems.Add(new Allergies() { Allergy = "zampon", Reaction = "Redness, swelling, itching, allergic contact dermatitis reactions, stomatitis (inflammation and soreness of the mouth or tongue), cheilitis (inflammation, rash, or painful erosion of the lips, oropharyngeal mucosa, or angles of their mouth), pruritus, hand eczema, generalized or resistant plantar dermatitis, rhinitis, conjunctivitis, and blisters.", Severity = "mild" });
            //mItems.Add(new Allergies() { Allergy = "turi", Reaction = "eksanthima", Severity = "mild" });
            //mItems.Add(new Allergies() { Allergy = "psomi", Reaction = "eksanthima", Severity = "mild" });

            //CustomAdapter3 adapter = new CustomAdapter3(this, mItems);

            //mListView.Adapter = adapter;
        }

        private void actions()
        {
            bool IsValidJson;
            ConsumeRest cRest = new ConsumeRest();
            object strResponse;
            Address address = new Address();
            string endpoint;
            ValidateJson validateJson = new ValidateJson();

            try
            {
                endpoint = address.Endpoint + "AllergiesList/" + myID;
                strResponse = cRest.makeRequest(endpoint);
                IsValidJson = validateJson.IsValidJson(strResponse);
                deserializedAllergies = JsonConvert.DeserializeObject<List<Allergies>>(strResponse.ToString());
                mItems = new List<Allergies>();
                setData();
                CustomAdapter3 adapter = new CustomAdapter3(this, mItems);

                mListView.Adapter = adapter;
            }
            catch
            {
              new AlertDialog.Builder(this)
              .SetTitle("An error has occured")
              .SetMessage("No data found due to unexpected problem")
              .SetIcon(Resource.Drawable.error)
              .Show();
            }
        }

        private void setData()
        {
            for (int i=0; i<deserializedAllergies.Count; i++)
            {
                mItems.Add(new Allergies() { Allergy1 = deserializedAllergies[i].Allergy1 , Reaction =deserializedAllergies[i].Reaction , Severity =deserializedAllergies[i].Severity });
            }
        }
    }
}