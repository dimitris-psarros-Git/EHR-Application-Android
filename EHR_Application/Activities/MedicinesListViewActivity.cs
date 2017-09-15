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
    [Activity(Label = "      Medicines   ", Theme = "@style/MyTheme")]
    public class MedicinesListViewActivity : AppCompatActivity
    {
        int FatherID,VisitID;
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

            //Data from previous activity
            FatherID = Intent.GetIntExtra("MyData", -1);
            VisitID =  Intent.GetIntExtra("VisitID", -1);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //SupportActionBar.Title = "Expandable ListView";
            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView);
            
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            string endpoint = address.Endpoint + "Treat_Medicine/" + VisitID;
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
                .SetIcon(Resource.Drawable.error)
                .Show();
            }
        }

        #region MenuInflater
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.option_menuGener, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings2)
            {
                Toast.MakeText(this, "Exit", ToastLength.Short).Show();
                AlertBox();                        
                return true;
            }
            else if (id == Resource.Id.action_settings3)
            {
                Toast.MakeText(this, "Reload", ToastLength.Short).Show();
                this.Recreate();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion
        
        private void SetData(out ExpandableListViewAdapter mAdapter)
        {
            int Visit_Count = Medicines.Count;
            for (int i = 0; i < Medicines.Count; i++)
            {
                string ATCcode = "ATC-CODE :" + Medicines[i].ATC_CODES;
                string Dosage = " Dosage :" + Medicines[i].Dosage;

                List<string> a = new List<string>();
                a.Add(ATCcode);
                a.Add(Dosage);

                group.Add("Medicine: " + i.ToString());
                dicMyMap.Add(group[i], a);
            }
            mAdapter = new ExpandableListViewAdapter(this, group, dicMyMap);
        }

        private void ExpandableListView_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + mAdapter.GetChild(e.GroupPosition, e.ChildPosition), ToastLength.Short).Show();
            Toast.MakeText(this, "Clicked :" + mAdapter.GetGroup(e.GroupPosition), ToastLength.Short).Show();
        }

        protected void AlertBox()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Confirm Exit");
            alert.SetMessage("Do you really want to exit? ");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
                //Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
                Finish1();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}