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
using EHR_Application.Post_Get;

namespace EHR_Application
{
    class PostRest         
    {
        public static async Task<string> Post(string json, Uri uri, bool Status)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    var response2 = await client.PostAsync(uri, content);     
                    var stringID = response2.Content.ReadAsStringAsync().Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        //  var responseContent = await response2.Content.ReadAsStringAsync();
                        //  var authData = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

                        if (Status) { return response2.StatusCode.ToString(); }
                        else { return response2.Content.ReadAsStringAsync().Result.ToString(); }
                    }
                    else
                    {
                        return response2.StatusCode.ToString();
                    }
                }
                catch (WebException e)
                {
                      return e.Status.ToString();
                }
                catch (Exception e)
                {
                      return e.Message.ToString();
                }
            }
        }
    }

    

}