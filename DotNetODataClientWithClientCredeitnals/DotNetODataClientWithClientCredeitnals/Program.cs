using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DotNetODataClientWithClientCredeitnals
{
    /// <summary>
    /// This sample shows how to query the CampusNexus API from a console application
    /// which uses application permissions.
    /// </summary>
    class Program
    {
        //set the following variables to match your environment.  It is recommended you use KeyStore to store credentials.
        const string rootUri = "[Update with base url of CampusNexus Student], e.g. https://sisclientweb-12345.campusnexus.cloud/";
        const string tenantId = "[Enter here the tenantID or domain name for your Azure AD tenant]";
        const string clientId = "[Enter here the ClientId for your application]";
        const string clientSecret = "[Enter here a client secret for your application]";
        const string resource = "api://[Enter_client_ID_Of_CampusNexusStudent-v2_from_Azure_Portal,_e.g._2ec40e65-ba09-4853-bcde-bcb60029e596]";

        private static void Main(string[] args)
        {
            //retrieve access token
            string accessToken = GetAccessToken();
            using (var httpClient = new HttpClient())
            {
                //set authentication header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //invoke HTTP Get
                string odataquery = $"{rootUri}ds/odata/Courses?$select=Code,Name&$filter=IsActive eq true&$top=20";
                var response = httpClient.GetAsync(odataquery).Result;
                response.EnsureSuccessStatusCode();

                //deserialize response into a JObject
                var result = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

                //write values to console
                Console.WriteLine("CampusNexus Api result for query {0}: \n", odataquery);
                foreach (var child in result["value"])
                {
                    Console.WriteLine($" {child["Code"]}: {child["Name"]}");
                }
            }
            Console.ReadLine();
        }

        /// <summary>
        /// A simplified way of retrieving token as application
        /// Ideal approach is to go after language specific libaries/feature: https://aka.ms/msal-net-client-credentials
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            string tokenEndpointUri = string.Format("https://login.microsoftonline.com/{0}/oauth2/token", tenantId);

            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_Id", clientId),
                new KeyValuePair<string, string>("client_Secret", clientSecret),
                new KeyValuePair<string, string>("resource", resource)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpointUri)
            {
                Content = new FormUrlEncodedContent(keyValues)
            };

            string accessToken = null;
            using (HttpClient httpClient = new HttpClient())
            {
                var response = httpClient.SendAsync(request).Result;
                var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(response.Content.ReadAsStringAsync().Result);

                if (dict.ContainsKey("access_token"))
                    accessToken = dict["access_token"].ToString();
                else if (dict.ContainsKey("error"))
                {
                    string errorMessage = dict.ContainsKey("error_description") ? string.Format("{0}: {1}", dict["error"], dict["error_description"]) : dict["error"].ToString();
                    Console.WriteLine(errorMessage);
                }
            }
            return accessToken;
        }
    }
}
