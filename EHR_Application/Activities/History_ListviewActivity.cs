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

namespace EHR_Application
{
    [Activity(Label = "History_Listview", Theme = "@style/MyTheme")]

    public class History_ListviewActivity :  AppCompatActivity                   // prososxh edw 
    {
        bool IsValid;
        object strResponse;
        ExpandableListViewAdapter mAdapter;
        ExpandableListView expandableListView;
        List<string> group = new List<string>();
        List<visit> VISIT;
        
       // List<visit_full_version> Visit_full;
        Dictionary<string, List<string>> dicMyMap = new Dictionary<string, List<string>>();

      //  public object SupportActionBar { get; private set; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);      
            SetContentView(Resource.Layout.Main2);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // SupportActionBar.Title = "Expandable ListView";

            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView);

            Actions();
          
        }

        private void Actions()
        {
            ConsumeRest cRest = new ConsumeRest();
            string endpoint = "http://192.168.2.13:54240//api/visits//?PersonId=1002";
            strResponse = cRest.makeRequest(endpoint);

            ValidateJson validateJson = new ValidateJson();
            IsValid = validateJson.IsValidJson(strResponse);

            if (IsValid)
            {
                //object JsonText = LoadJson1();
                VISIT = JsonConvert.DeserializeObject<List<visit>>(strResponse.ToString());

                // convertClasses();

                SetData(out mAdapter);
                expandableListView.SetAdapter(mAdapter);
                expandableListView.ChildClick += ExpandableListView_ChildClick1;
            }
            else
            {
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("An error has occured")
                .SetMessage("No data found do to unexpected problem" + "n/" + strResponse)
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


            ////////////////////////////// add new code'

            int SpecVisitId;

            for (int i = 0; i < VISIT.Count; i++)
            {
                string CreatedName = "Group-" + i.ToString();

                if (CreatedName == NameOfFather.ToString())
                {
                    SpecVisitId = VISIT[i].VisitID;
                }

                string details_visitID1 = "@ Diagnosis VisitID :" + VISIT[i].VisitID;

            }

            //////////////////////////////   end of new code

            if (NameOfChild.ToString() == " Diagnosis ")
            {
                var activity1 = new Intent(this, typeof(DiagnosisListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                StartActivity(activity1);
            }
            if (NameOfChild.ToString() == " Medicines ")
            {
                var activity1 = new Intent(this, typeof(MedicinesListViewActivity));
                activity1.PutExtra("MyData", NameOfFather.ToString());
                StartActivity(activity1);
            }
            if (NameOfChild.ToString() == " Doctor details ")
            {
                // prepei na oloklhrwthei
            }
        }

        private void SetData(out ExpandableListViewAdapter mAdapter)
        {

            // mas deinetai apo prwigoumnh selida to PersonID opote 
            // tha kanoyme siriakh anazhthsh gia na doume ta <visits> 
            // tou sugkekrimenou asthenh                                   //kalese to   < /?...... >

            int[] max = new int[100];
            
            int Visit_Count = VISIT.Count;

            for (int i = 0; i < VISIT.Count; i++)
            {
                     
                    string details_date = "Date :" + VISIT[i].Date;
                    string details_visitID1 = "@ Diagnosis VisitID :" + VISIT[i].VisitID;
                    string details_doctor_personID = "details_doctor_personID :" + VISIT[i].DoctorPersonID.ToString();
                    string details_PersonID = "PersonID :" + VISIT[i].PersonID.ToString();
                    string details_NumberO_visits = "Number of visits :" + VISIT[i].NumberOfVisit.ToString();
                    

                    List<string> a = new List<string>();
                    a.Add(details_date);
                    a.Add(details_visitID1);
                    a.Add(details_doctor_personID);
                    a.Add(details_PersonID);
                    a.Add(details_NumberO_visits);
                    a.Add(" Diagnosis ");
                    a.Add(" Medicines ");
                    a.Add(" Doctor details ");
                  
                    group.Add("Group-" + i.ToString());
                    dicMyMap.Add(group[i], a);
                  
            }
            mAdapter = new ExpandableListViewAdapter(this, group, dicMyMap);
            
        }

        /*
        public  void ExpandableListView_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Toast.MakeText(this, "Clicked :" + mAdapter.GetChild(e.GroupPosition, e.ChildPosition), ToastLength.Short).Show();
            Toast.MakeText(this, "Clicked :" + mAdapter.GetGroup(e.GroupPosition), ToastLength.Short).Show();

            //onoma tou paidiou
            Object NameOfChild = mAdapter.GetChild(e.GroupPosition, e.ChildPosition);

            //onoma tou patera
            Object NameOfFather = mAdapter.GetGroup(e.GroupPosition);


            ////////////////////////////// add new code'

            int SpecVisitId;

            for (int i = 0; i < VISIT.Count; i++)
            {
                string CreatedName = "Group-" + i.ToString();
                
                if ( CreatedName == NameOfFather.ToString() )
                {
                  SpecVisitId = VISIT[i].VisitID;
                }

                string details_visitID1 = "@ Diagnosis VisitID :" + VISIT[i].VisitID;

            }

                //////////////////////////////   end of new code

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
        */


        private string Generate_Names()
        {
            int i = 0;
            i = i + 1;
            string Object = "group" + i.ToString();
            return Object;
        }

        public object LoadJson1()          // load json from asset
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