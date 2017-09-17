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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EHR_Application
{
    public class ValidateJson
    {
        public  bool IsValidJson(object stringValue)            
        {
            if (string.IsNullOrWhiteSpace(stringValue.ToString()) )
            {
                return false;
            }
            else
            {
                string StringValue = stringValue.ToString();            
                var value = StringValue.Trim();

                if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                    (value.StartsWith("[") && value.EndsWith("]"))) //For array
                {
                    try
                    {
                        var obj = JToken.Parse(value);
                        return true;
                    }
                    catch (JsonReaderException)
                    {
                        return false;
                    }
                }

                return false;
            }
        }
    }

}