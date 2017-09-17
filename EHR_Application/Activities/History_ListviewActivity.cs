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
using Android.Content.Res;
using System.IO;
using Newtonsoft.Json;
using EHR_Application.Activities;
using EHR_Application.Models;

namespace EHR_Application
{
    [Activity(Label = "    Medical History  ", /*, Theme = "@style/MyTheme1"*/ Theme = "@style/MyTheme")]

    public class History_ListviewActivity :  AppCompatActivity         // prososxh edw 
    {
        int myID,receiverID;
        int SpecVisitId;
        bool IsValid,IsDoctor;
        object strResponse;
        ExpandableListViewAdapter mAdapter;
        ExpandableListView expandableListView;
        List<string> group = new List<string>();
        List<visit> VISIT;
        List<Visit2> VISIT2;

        //List<visit_full_version> Visit_full;
        Dictionary<string, List<string>> dicMyMap = new Dictionary<string, List<string>>();
        //public object SupportActionBar { get; private set; }
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);      
            SetContentView(Resource.Layout.Main2);

            // Data from previous activity
            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);
            
            
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            // SupportActionBar.Title = "Expandable ListView";
            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView);

            Actions();
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
            //return super.onCreateView(inflater, container, savedInstanceState);
        }
        #endregion

        private void Actions()
        {
            string endpoint;
            ConsumeRest cRest = new ConsumeRest();
            Address address = new Address();

            RetrieveData retrieve = new RetrieveData();  // retrieve "IsDoctor"
            IsDoctor = retrieve.RetreiveBool();
            
            if (IsDoctor == false) { endpoint = address.Endpoint2 + "visits//?PersonId=" + myID; }       
            else                   { endpoint = address.Endpoint2 + "visits//?PersonId=" + receiverID; }

            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);

            if (IsValid)
            {
                VISIT2 = JsonConvert.DeserializeObject<List<Visit2>>(strResponse.ToString());
                VISIT2 = VISIT2.OrderBy(i => i.Date).ToList();

                //convertClasses();
                SetData(out mAdapter);
                expandableListView.SetAdapter(mAdapter);
                expandableListView.ChildClick += ExpandableListView_ChildClick1;
            }
            else
            {
                VISIT2 = JsonConvert.DeserializeObject<List<Visit2>>("[]".ToString());

                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "\n" + strResponse)
                .SetIcon(Resource.Drawable.error)
                .Show();
            }
        }
        
        private void ExpandableListView_ChildClick1(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + mAdapter.GetChild(e.GroupPosition, e.ChildPosition), ToastLength.Short).Show();
            Toast.MakeText(this, "Clicked :" + mAdapter.GetGroup(e.GroupPosition), ToastLength.Short).Show();

            //onoma tou paidiou
            Object NameOfChild = mAdapter.GetChild(e.GroupPosition, e.ChildPosition);

            //onoma tou patera
            Object NameOfFather = mAdapter.GetGroup(e.GroupPosition);
            
            for (int i = 0; i < VISIT2.Count; i++)
            {
                string CreatedName = "Visit " + i.ToString() + "   " + "Date :" + VISIT2[i].Date.ToString();
                if (CreatedName == NameOfFather.ToString())
                {
                    SpecVisitId = VISIT2[i].VisitID;
                }
                string details_visitID1 = "@ Diagnosis VisitID :" + VISIT2[i].VisitID;
                
                int PositionOfFather    = e.GroupPosition;
                int PositionOfSon       = e.ChildPosition;
            }

            if (NameOfChild.ToString() == " Diagnosis ")
            {
                var activity1 = new Intent(this, typeof(DiagnosisListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                activity1.PutExtra("VisitID", SpecVisitId);
                StartActivity(activity1);
            }
            if (NameOfChild.ToString() == " Medicines ")
            {
                var activity1 = new Intent(this, typeof(MedicinesListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                activity1.PutExtra("VisitID", SpecVisitId);
                StartActivity(activity1);
            }
        }
        
        private void SetData(out ExpandableListViewAdapter mAdapter)
        {
            int Visit_Count = VISIT2.Count;
            for (int i = 0; i < VISIT2.Count; i++)
            {
                    string details_date = "Date :" + VISIT2[i].Date.ToString(); 
                    string details_doctor_FullName   = "doctor_FullName :" + VISIT2[i].Doctor.FirstName + " " + VISIT2[i].Doctor.LastName ;
                    string details_doctor_speciality = "doctor_Speciality :" + VISIT2[i].Doctor.Speciality; 
                    
                    List<string> a = new List<string>();
                    a.Add(details_date);
                    a.Add(details_doctor_FullName);
                    a.Add(details_doctor_speciality);
                    a.Add(" Diagnosis ");
                    a.Add(" Medicines ");
                    group.Add("Visit " + i.ToString() + "   " + details_date);
                    dicMyMap.Add(group[i], a);
            }
            mAdapter = new ExpandableListViewAdapter(this, group, dicMyMap);
            
        }
        
        private  void ExpandableListView_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + mAdapter.GetChild(e.GroupPosition, e.ChildPosition), ToastLength.Short).Show();
            Toast.MakeText(this, "Clicked :" + mAdapter.GetGroup(e.GroupPosition), ToastLength.Short).Show();

            //onoma tou paidiou
            Object NameOfChild = mAdapter.GetChild(e.GroupPosition, e.ChildPosition);

            //onoma tou patera
            Object NameOfFather = mAdapter.GetGroup(e.GroupPosition);
            
            int SpecVisitId;

            for (int i = 0; i < VISIT.Count; i++)    // diorthwma
            {
                string CreatedName = "Group-" + i.ToString();
                
                if ( CreatedName == NameOfFather.ToString() )
                {
                  SpecVisitId = VISIT[i].VisitID;
                }
                string details_visitID1 = "@ Diagnosis VisitID :" + VISIT[i].VisitID;
            }
           
                if (NameOfChild.ToString() == "Click to see diagnosis")
                {
                var activity1 = new Intent(this, typeof(DiagnosisListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                StartActivity(activity1);
                }
                if (NameOfChild.ToString() == "Click to see Medicines")
                {
                var activity1 = new Intent(this, typeof(MedicinesListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                StartActivity(activity1);
                }
        }
        
        
        public void AlertBox()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Confirm Exit");
            alert.SetMessage("Do you really want to exit? ");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
                // Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
                Finish1();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void Finish1()
        {
            Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}