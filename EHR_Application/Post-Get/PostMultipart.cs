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
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace EHR_Application.Post_Get
{
    class PostMultipart
    {
        public static async Task<string> PostMultiPart(string json, Uri uri)
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

                    }
                    return e.Status.ToString();
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }

            }
            return null;
        }
    }
}