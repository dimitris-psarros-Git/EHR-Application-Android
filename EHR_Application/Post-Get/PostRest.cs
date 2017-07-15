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
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace EHR_Application
{
    class PostRest         
    {
        public static async Task<string> Post(string json,Uri uri)
        {
            using (var client = new HttpClient())
            {
               
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response2 = await client.PostAsync(uri, content).ConfigureAwait(false);

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