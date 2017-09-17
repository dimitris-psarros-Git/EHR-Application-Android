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

namespace EHR_Application.Activities
{
    [Activity(Label = "   Add New History " , Theme = "@style/MyTheme" )]
    public class NewVisitActivity : Activity
    {
        List<string> ICD_Codes,ICD_Codes1;
        Spinner spin;
        Spinner spin1;
        TextView txt18;
        EditText Description;
        EditText ICDcode;
        EditText ATCcode;
        EditText Dosage;
        int myID,receiverID, VISITID;
        int icdID;
        string icdName ;
        string Chapter;
        string icdChapter;
        VisitID newMessages;
        ArrayAdapter adapt;
        ArrayAdapter adapt1;
        List<icdChapters> icdchapters;
        List<icd_codes> icdcodes,icdcodesspecial;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.newVisit);

            myID = Intent.GetIntExtra("myID", -1);
            receiverID = Intent.GetIntExtra("receiverID", -1);

            spin1 = FindViewById<Spinner>(Resource.Id.spinner2);
            spin  = FindViewById<Spinner>(Resource.Id.spinner1);

            txt18 = FindViewById<TextView>(Resource.Id.textView18);

            Button button1 = FindViewById<Button>(Resource.Id.button1);   // new diagnosi
            button1.Click += Button1_Click;

            Button button4 = FindViewById<Button>(Resource.Id.button4);   // new treatment
            button4.Click += Button4_Click;

            Button button3 = FindViewById<Button>(Resource.Id.button3);   // new visit
            button3.Click += Button3_Click;

            Description = FindViewById<EditText>(Resource.Id.editText5);
           
            ATCcode = FindViewById<EditText>(Resource.Id.editText3);
            Dosage = FindViewById<EditText>(Resource.Id.editText4);

            Actions();
            
            spin.ItemSelected += Spin_ItemSelected;
            spin1.ItemSelected += Spin1_ItemSelected;
        }

        private void Spin1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            icdID = icdcodes[e.Position].col0;
            icdName = icdcodes[e.Position].col9;
            PrintSelects();
        }

        private async void Button4_Click(object sender, EventArgs e)   // new treatment
        {
            if (VISITID != 0 && ATCcode != null && Dosage != null)
            {
                string endpoint2;
                PostRest p = new PostRest();
                Address address = new Address();
                endpoint2 = address.Endpoint + "Treat_Medicines";
                var uri = new Uri(endpoint2);

                NewMedicines newmedicines = new NewMedicines();
                newmedicines.VisitID = VISITID;
                newmedicines.ATC_CODES = ATCcode.Text;
                newmedicines.Dosage = Dosage.Text;

                string output = JsonConvert.SerializeObject(newmedicines);
                string StrRespPost = await PostRest.Post(output, uri, true);
                if (StrRespPost == "Created")
                {
                    ATCcode = null;
                    Dosage  = null;

                    new AlertDialog.Builder(this)
                    .SetMessage("Successful")
                    .SetTitle("Message")
                    .Show();
                }
                else
                {
                    new AlertDialog.Builder(this)
                    .SetMessage("Unsuccessfull !!" + "\n" + "Something went wrong" + "\n" + StrRespPost)
                    .SetTitle("Message")
                    .SetIcon(Resource.Drawable.error)
                    .Show();
                }
            }
            else
            {
                new AlertDialog.Builder(this)
                    .SetMessage("You need to choose ATC_code and Dosage")
                    .SetTitle("Message")
                    .SetIcon(Resource.Drawable.error)
                    .Show();
            }
        }

        private void Spin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Chapter = icdchapters[e.Position].col1;
            icdChapter = icdchapters[e.Position].col2;
            //icdcodesspecial = icdcodes.Where(c => (c.col4.ToString() == Chapter)).ToList();
            //SetIcdCodes();
            PrintSelects();
        }

        private void PrintSelects()
        {
            //if (icdChapter != null) { txt18.Text = "Selected : " + icdChapter; }
            //else if(icdName != null){ txt18.Text = "Selected : " + icdName; }
            //else if (icdChapter!=null && icdName != null){ txt18.Text = "Selected : " + icdChapter + "\n" + icdName; }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            AddNewVisit();
        }

        private void Actions()
        {
            List<string> ICD_Chapters = new List<string>();
            ICD_Codes = new List<string>();
            ICD_Codes1 = new List<string>();
            ConsumeRest cRest = new ConsumeRest();
            ValidateJson validateJson = new ValidateJson();
            Address address = new Address();
            object strResponse,strResponse2;
            bool IsValidJson,IsValidJson2;
            string endpoint,endpoint2;
            
            endpoint = address.Endpoint + "icd_chapters";
            endpoint2 = address.Endpoint + "icd_code";

            strResponse = cRest.makeRequest(endpoint);
            IsValidJson = validateJson.IsValidJson(strResponse);
            strResponse2 = cRest.makeRequest(endpoint2);
            IsValidJson2 = validateJson.IsValidJson(strResponse2);

            if (IsValidJson && IsValidJson2)
            {
                icdchapters = JsonConvert.DeserializeObject<List<icdChapters>>(strResponse.ToString());
                icdcodes    = JsonConvert.DeserializeObject<List<icd_codes>>(strResponse2.ToString());

                if (icdchapters.Count != 0)
                {
                    for (int i = 0; i < icdchapters.Count; i++)
                    {
                        ICD_Chapters.Add(icdchapters[i].col2.ToString());

                    }
                    adapt = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ICD_Chapters);
                    spin.Adapter = adapt;
                }

                //icdcodesspecial = icdcodes;

                if (icdcodes.Count != 0)
                {
                    for (int i = 0; i < icdcodes.Count; i++)
                    {
                        ICD_Codes.Add(icdcodes[i].col4.ToString()+". "+icdcodes[i].col9.ToString());
                    }
                    adapt1 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ICD_Codes);
                    spin1.Adapter = adapt1;
                }
            }
            else
            {
                new AlertDialog.Builder(this)
               .SetTitle("An error has occured")
               .SetIcon(Resource.Drawable.error)
               .SetMessage("No data found due to unexpected problem" + "\n" + strResponse)
               .Show();
            }
        }

        //private void SetIcdCodes()
        //{
        //    //spin1.Adapter(null);
            
        //    if (icdcodesspecial.Count != 0)
        //    {
        //        for (int i = 0; i < icdcodesspecial.Count; i++)
        //        {
        //            ICD_Codes1.Add(icdcodesspecial[i].col4.ToString() + " " + icdcodesspecial[i].col9.ToString());
        //        }
        //        adapt1 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, ICD_Codes1);
        //        spin1.Adapter = adapt1;
        //    }
        //}

        private async void AddNewVisit()    // new visit
        {
            
            if (VISITID == 0)
            {
                string endpoint;
                PostRest p = new PostRest();
                Address address = new Address();
                endpoint = address.Endpoint + "Visits";
                var uri = new Uri(endpoint);

                NewVisit newvisit = new NewVisit();
                newvisit.PersonID = receiverID;
                newvisit.DoctorPersonID = myID;

                string output = JsonConvert.SerializeObject(newvisit);
                string StrRespPost = await PostRest.Post(output, uri, false);
                try
                {
                    newMessages = JsonConvert.DeserializeObject<VisitID>(StrRespPost.ToString());
                    VISITID = newMessages.visitId;

                    new AlertDialog.Builder(this)
                   .SetMessage("New visit has been created")
                   .SetTitle("Message")
                   .Show();
                }
                catch
                {
                    new AlertDialog.Builder(this)
                   .SetMessage("An error has occur")
                   .SetTitle("Message")
                   .Show();
                }
            }
            else
            {
                new AlertDialog.Builder(this)
                    .SetMessage("You already have created a new Visit.If you need to create a new one  you need to exit from this Visit ")
                    .SetTitle(" No need for new Visit ")
                    .Show();
            }
        }

        private async void Button1_Click(object sender, EventArgs e)  // new Diagnosis
        {
            if (VISITID != 0 && icdID !=0 && Chapter != null ) {

                string endpoint1;
                PostRest p = new PostRest();
                Address address = new Address();
                endpoint1 = address.Endpoint + "DIagnosis";
                var uri = new Uri(endpoint1);

                NewDiagnosis newdiagnosis = new NewDiagnosis();
                newdiagnosis.VisitID = VISITID;
                newdiagnosis.Description = Description.Text;
                newdiagnosis.ICD_Code_Id = icdID;
                newdiagnosis.ICD_Chapter = Chapter; 

                string output = JsonConvert.SerializeObject(newdiagnosis);
                string StrRespPost = await PostRest.Post(output, uri, true);
                if (StrRespPost == "Created")
                {
                    Chapter = null;
                    icdID = 0;

                    new AlertDialog.Builder(this)
                    .SetMessage("Successful")
                    .SetTitle("Message")
                    .Show();
                }
                else
                {
                    new AlertDialog.Builder(this)
                    .SetMessage("Unsuccessfull!!" + "\n" + "Something went wrong" + "\n" + StrRespPost)
                    .SetTitle("Message")
                    .SetIcon(Resource.Drawable.error)
                    .Show();
                }
            }
            else
            {
                new AlertDialog.Builder(this)
                    .SetMessage("You need to choose ICD_code and ICD_chapter")
                    .SetTitle("Message")
                    .SetIcon(Resource.Drawable.error)
                    .Show();
            }
        }

        //private async  void Button1_Click(object sender, EventArgs e)    // new Treatment
        //{
        //    if ( VISITID!=0 && ATCcode!=null && Dosage!= null) {
        //        string endpoint2;
        //        PostRest p = new PostRest();
        //        Address address = new Address();
        //        endpoint2 = address.Endpoint + "Treat_Medicines";
        //        var uri = new Uri(endpoint2);

        //        NewMedicines newmedicines = new NewMedicines();
        //        newmedicines.VisitID = VISITID;
        //        newmedicines.ATC_CODES = ATCcode.Text;
        //        newmedicines.Dosage = Dosage.Text;

        //        string output = JsonConvert.SerializeObject(newmedicines);
        //        string StrRespPost = await PostRest.Post(output, uri, true);
        //        if (StrRespPost == "Created")
        //        {
        //            new AlertDialog.Builder(this)
        //            .SetMessage("Successful")
        //            .SetTitle("Message")
        //            .Show();
        //        }
        //        else
        //        {
        //            new AlertDialog.Builder(this)
        //            .SetMessage("Unsuccessfull !!" + "\n" + "Something went wrong" + "\n" + StrRespPost)
        //            .SetTitle("Message")
        //            .SetIcon(Resource.Drawable.error)
        //            .Show();
        //        }
        //    }
        //    else
        //    {
        //        new AlertDialog.Builder(this)
        //            .SetMessage("You need to choose ATC_code and Dosage")
        //            .SetTitle("Message")
        //            .SetIcon(Resource.Drawable.error)
        //            .Show();
        //    }
        //}

        public override void OnBackPressed()
        {
                  AlertBox();
        }

        public void AlertBox()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Message");
            alert.SetMessage("Do you want to exit from this visit?");
            alert.SetPositiveButton("Exit", (senderAlert, args) => { base.OnBackPressed(); });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => { });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }

}