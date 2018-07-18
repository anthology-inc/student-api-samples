using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace DotNetWebApiClient
{
    public class CommandModelClient
    {
        private readonly HttpClient _client;
        private readonly string _entityUrl;

        public CommandModelClient(HttpClient client, string entityUrl)
        {
            _client = client;
            _entityUrl = entityUrl.TrimEnd('/');
        }

        public JObject ExecuteCommand(string command, JObject value)
        {
            var response = _client.PostAsync($"{_entityUrl}/{command}", new StringContent(value.ToString(), Encoding.UTF8, "application/json")).Result;
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
        }

        public JObject Create()
        {
            return ExecuteCommand("create", new JObject());
        }


        public JObject Delete(JObject entity)
        {
            return ExecuteCommand("delete", new JObject { ["payload"] = entity });
        }

        public JObject SaveNew(JObject entity)
        {
            return ExecuteCommand("saveNew", new JObject { ["payload"] = entity });
        }

        public JObject Save(JObject entity)
        {
            return ExecuteCommand("save", new JObject { ["payload"] = entity });
        }

        public JObject Retrieve(int idValue)
        {
            var body = JObject.FromObject(new
            {
                payload = new
                {
                    id = idValue
                }
            });
            return ExecuteCommand("get", body);
        }
    }

}
