using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetODataClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //set the folliwng variables to match your environment.  It is recommended you use KeyStore to store credentials.
            string rootUri = "http://university-a.campusnexus.cloud/";
            string userName = "user@university-a.campusnexus.cloud";
            string password = "password";

            var httpClient = new HttpClient();

            //set authentication header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}")));

            //invoke HTTP Get
            var response = httpClient.GetAsync($"{rootUri}ds/odata/Courses").Result;
            response.EnsureSuccessStatusCode();

            //deserialize response into a JObject
            var result = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

            //write values to console
            foreach (var child in result["value"])
            {
                Console.WriteLine(child["Code"]);
                Console.WriteLine(child["Name"]);
            }

            Console.ReadLine();
        }
    }
}