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
    [Activity(Label = "   Add New History "  /*, MainLauncher = true*/ )]
    public class NewVisitActivity : Activity
    {
        EditText Description;
        EditText ICDcode;
        EditText ATCcode;
        EditText Dosage;
        int myID,receiverID, VISITID;
        VisitID newMessages;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.newVisit);


            //myID = Intent.GetIntExtra("myID", -1);
            //receiverID = Intent.GetIntExtra("receiverID", -1);

            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += Button1_Click;

           // Button button2 = FindViewById<Button>(Resource.Id.button2);
           // button1.Click += Button1_Click1;

            Button button3 = FindViewById<Button>(Resource.Id.button3);
            button3.Click += Button3_Click;

            Description = FindViewById<EditText>(Resource.Id.editText1);
            ICDcode = FindViewById<EditText>(Resource.Id.editText2);
            ATCcode = FindViewById<EditText>(Resource.Id.editText3);
            Dosage = FindViewById<EditText>(Resource.Id.editText4);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            AddNewVisit();
        }

        private async void AddNewVisit()
        {
            myID = 10000;
            receiverID = 1000;

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
            }
            catch
            {

            }
        }

        private async void Button1_Click1(object sender, EventArgs e)
        {
            if (VISITID != 0 && Description.Text!= null && ICDcode.Text!= null ) {
                string endpoint1;
                PostRest p = new PostRest();
                Address address = new Address();
                endpoint1 = address.Endpoint + "DIagnosis";
                var uri = new Uri(endpoint1);

                NewDiagnosis newdiagnosis = new NewDiagnosis();
                newdiagnosis.VisitID = VISITID;
                newdiagnosis.Description = Description.Text;
                newdiagnosis.ICD_CODE = ICDcode.Text;

                string output = JsonConvert.SerializeObject(newdiagnosis);
                string StrRespPost = await PostRest.Post(output, uri, true);
                if (StrRespPost == "Created")
                {
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
            }
        }

        private async  void Button1_Click(object sender, EventArgs e)
        {
            if ( VISITID!=0 && ATCcode!=null && Dosage!= null) {
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
            }
        }

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