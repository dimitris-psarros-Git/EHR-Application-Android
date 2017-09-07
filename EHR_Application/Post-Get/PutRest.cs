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
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace EHR_Application.Post_Get
{
    class PutRest
    {
        public static async Task<string> Put(string json, Uri uri)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    var response2 = await client.PutAsync(uri, content);
                    var stringID = response2.Content.ReadAsStringAsync().Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        var responseContent = await response2.Content.ReadAsStringAsync();
                        var authData = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
                    }
                    return response2.StatusCode.ToString();
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        //Toast.Show("Status Code : ", ((HttpWebResponse)e.Response).StatusCode);
                        //main.Print(/*"Status Description : ",*/ ((HttpWebResponse)e.Response).StatusDescription);
                    }
                    //main.Print(/*"\r\nWebException Raised. The following error occured :",*/ e.Message);
                    return e.Status.ToString();
                }
                catch (Exception e)
                {
                    //main.Print(/*"\nThe following Exception was raised : "+ */ e.Message);
                    return e.Message.ToString();
                }

            }
        }
    }
}