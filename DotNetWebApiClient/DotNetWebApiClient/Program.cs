using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DotNetWebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //set the folliwng variables to match your environment.  It is recommended you use KeyStore to store credentials.
            string rootUri = "http://university-a.campusnexus.cloud/";
            string userName = "user@university-a.campusnexus.cloud";
            string password = "password";

            var httpClient = new HttpClient();
            rootUri = $"{rootUri}/api/commands/Academics/CourseType";

            //set authentication header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}")));
            var commandModelClient = new CommandModelClient(httpClient, rootUri);

            //create
            var result = commandModelClient.Create();

            //save new
            var data = (JObject)result.SelectToken("payload.data");
            data["code"] = "Test";
            data["name"] = "Test";
            result = commandModelClient.SaveNew(data);

            //update
            data = (JObject)result.SelectToken("payload.data");
            data["name"] = "Test2";
            result = commandModelClient.Save(data);

            //retrieve
            var idToken = result.SelectToken("payload.data.id");
            result = commandModelClient.Retrieve(idToken.Value<int>());

            //delete
            data = (JObject)result.SelectToken("payload.data");
            result = commandModelClient.Delete(data);

            Console.ReadKey();
        }
    }
}
