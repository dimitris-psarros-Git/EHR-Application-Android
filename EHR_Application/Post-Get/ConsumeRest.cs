using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Drawing;

namespace EHR_Application
{

    public enum httpVerb1
    {
        GET,
        POST,
        PUT,
        DELETE
    }


    class ConsumeRest
    {
        public string endPoint { get; set; }
        public httpVerb1 httpMethod { get; set; }
        public object JObject { get; private set; }
        public object JsonConvert { get; private set; }

        public ConsumeRest()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb1.GET;
        }

        
        public object makeRequest(string endpoint)   
        {
            object strResponceValue = null; 
            try
            {
                WebRequest webRequest1 = WebRequest.Create(endpoint);
                WebResponse webResponse;
                
                webResponse = webRequest1.GetResponse();

                ////////////////////   check this new code  
                //Image webImage = Image.FromStream(webResponse.GetResponseStream());
                ////////////////////    end of new code

                using (Stream responseStream = webResponse.GetResponseStream())
                {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponceValue = reader.ReadToEnd();
                            }
                }
              
               
                     //Otherwise with HttpWebRequest
               // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
               // request.Method = httpMethod.ToString();
               ////  request.Accept = "application/json";
               // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
               // if (response.StatusCode == HttpStatusCode.OK)
               // {
               //     //  throw new ApplicationException("error code:" + response.StatusCode.ToString());

               //     using (Stream responseStream = response.GetResponseStream())
               //     {
               //         using (StreamReader reader = new StreamReader(responseStream))
               //         {
               //             strResponceValue = reader.ReadToEnd();
               //         }
               //     }
               //     // response.Close();
               // }
               
                return strResponceValue;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                   //MessageBox.Show("Status Code : ", ((HttpWebResponse)e.Response).StatusCode);
                   //main.Print(/*"Status Description : ",*/ ((HttpWebResponse)e.Response).StatusDescription);
                }
                // Mporeis na guriseis e.Status , e.Response.StatusDescription , e.Responce.StatusCode 
                return e.Message.ToString();
            }
            catch (Exception e)
            {
              
       //         main.Print(/*"\nThe following Exception was raised : "+ */ e.Message);
                return e.Message.ToString() ;
            }
        }

    }


}