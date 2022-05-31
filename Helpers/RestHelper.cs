using System;
using System.Linq;
using System.Net;
using System.Threading;
using Laybuy.Helpers;
using Newtonsoft.Json;
using RestSharp;

namespace Laybuy.Helper
{
    public static class RestHelper
{
        public static IRestResponse GetRequest(string endpoint)
        {
            var client = new RestClient(GlobalVariables.APIUrl);
            var request = new RestRequest(endpoint, Method.GET);            
            var response = client.Execute(request);
            return response;
        }
    }
}
