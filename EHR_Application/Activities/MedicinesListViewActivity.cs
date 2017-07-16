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
using Android.Support.V7.App;
using Newtonsoft.Json;
using Android.Content.Res;
using System.IO;
using EHR_Application.Models;

namespace EHR_Application.Activities
{
    [Activity(Label = "MedicinesListViewActivity", Theme = "@style/MyTheme")]
    public class MedicinesListViewActivity : AppCompatActivity
    {
        object strResponse;
        ExpandableListViewAdapter mAdapter;
        ExpandableListView expandableListView;
        List<string> group = new List<string>();
        List<Treat_Medicines> Medicines;
        // List<visit_full_version> Visit_full;
        Dictionary<string, List<string>> dicMyMap = new Dictionary<string, List<string>>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            bool IsValid;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main2);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // SupportActionBar.Title = "Expandable ListView";

            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView);

            
            ConsumeRest cRest = new ConsumeRest();
            string endpoint = "http://192.168.1.70:54240/api/Treat_Medicines";
            strResponse = cRest.makeRequest(endpoint);
            

            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);

           

            if (IsValid)
            {
                Medicines = JsonConvert.DeserializeObject<List<Treat_Medicines>>(strResponse.ToString());
                SetData(out mAdapter);
                expandableListView.SetAdapter(mAdapter);
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "n/" + strResponse)
                .Show();
            }


        }

        /*
        private void SetData(out ExpandableListViewAdapter mAdapter)
        {
            List<string> groupA = new List<string>();
            groupA.Add("A-1");
            groupA.Add("A-2");
            groupA.Add("A-3");
            
            List<string> groupB = new List<string>();
            groupB.Add("B-1");
            groupB.Add("B-2");
            groupB.Add("B-3");

            group.Add("Group A");
            group.Add("Group B");
            dicMyMap.Add(group[0], groupA);
            dicMyMap.Add(group[1], groupB);

            mAdapter = new ExpandableListViewAdapter(this, group, dicMyMap);

        }
        */

        private void SetData(out ExpandableListViewAdapter mAdapter)
        {
            // mas deinetai apo prwigoumnh selida to PersonID opote 
            // tha kanoyme siriakh anazhthsh gia na doume ta <visits> 
            // tou sugkekrimenou asthenh                                   //kalese to   < /?...... >

            int[] max = new int[100];

            int Visit_Count = Medicines.Count;

            for (int i = 0; i < Medicines.Count; i++)
            {

                string ATCcode = "ATC-CODE :" + Medicines[i].ATC_CODES;
                string Dosage = " Dosage :" + Medicines[i].Dosage;
                

                List<string> a = new List<string>();
                a.Add(ATCcode);
                a.Add(Dosage);
               
                group.Add("Group-" + i.ToString());
                dicMyMap.Add(group[i], a);

            }
            mAdapter = new ExpandableListViewAdapter(this, group, dicMyMap);

        }




        private void ExpandableListView_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + mAdapter.GetChild(e.GroupPosition, e.ChildPosition), ToastLength.Short).Show();
            Toast.MakeText(this, "Clicked :" + mAdapter.GetGroup(e.GroupPosition), ToastLength.Short).Show();
        }



            public object LoadJson1()         
            {
            object jsontext;
            AssetManager assets = this.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("visitsDETAILS.txt")))
            {
                jsontext = sr.ReadToEnd();
            }

            return jsontext;
             }


    }
}