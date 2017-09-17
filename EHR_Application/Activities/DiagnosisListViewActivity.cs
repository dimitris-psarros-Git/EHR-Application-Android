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
using Android.Content.Res;
using System.IO;
using Android.Support.V7.App;
using Newtonsoft.Json;
using EHR_Application.Models;

namespace EHR_Application.Activities
{
    [Activity(Label = "      Diagnosis   ", Theme = "@style/MyTheme")]
    public class DiagnosisListViewActivity : AppCompatActivity
    {
        Dictionary<string, List<string>> dicMyMap = new Dictionary<string, List<string>>();
        int VisitID;
        string Father;
        object strResponse;
        ExpandableListViewAdapter mAdapter;
        ExpandableListView expandableListView;
        List<string> group = new List<string>();
        List<DIagnosis> Diagnosis;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            bool IsValid;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main2);

            // Data from previous activity
            Father = Intent.GetStringExtra("MyData");
            VisitID = Intent.GetIntExtra("VisitID",-1);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            // SupportActionBar.Title = "Expandable ListView";
            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView);

            
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();
            string endpoint = address.Endpoint + "DIagnosis/"/* + "Diagnosi/"*/ + VisitID ;
            strResponse = cRest.makeRequest(endpoint);
           
            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);
            
            if (IsValid)
            {
                Diagnosis = JsonConvert.DeserializeObject<List<DIagnosis>>(strResponse.ToString());
                SetData(out mAdapter);
                expandableListView.SetAdapter(mAdapter);
            }
            else
            {
                Diagnosis = JsonConvert.DeserializeObject<List<DIagnosis>>("[]".ToString());

                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "\n" + strResponse)
                .SetIcon(Resource.Drawable.error)
                .Show();
            }
        }

        #region  MenuInflater
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
            int Visit_Count = Diagnosis.Count;
            for (int i = 0; i < Diagnosis.Count; i++)
            {
                string Description = "Doctor's Notes : " + Diagnosis[i].Description;
                string ICDchapter  = "Disease category : " + Diagnosis[i].ICD_Chapter;
                string ICDcode     = "Disease : "    + Diagnosis[i].ICD_Code;
                
                List<string> a = new List<string>();
                a.Add(ICDchapter);
                a.Add(ICDcode);
                a.Add(Description);

                group.Add("Diagnosis: " + i.ToString());
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
                Finish1();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}