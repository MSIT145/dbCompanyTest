using System.Security.Policy;
using System.Text.Json;

namespace dbCompanyTest.Environment
{
    public class Environment
    {
        string apiKey = "2LU0i8A48bax1cQNKoRH5OFwfOG_43RaqcDa3yYViTPCgHtG7";
        bool open = false;
        internal string useEnvironment
        {
            get
            {
                if (open)
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), "https://api.ngrok.com/endpoints"))
                        {
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                            client.DefaultRequestHeaders.Add("Ngrok-Version", "2");
                            var response = client.SendAsync(request).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var x = response.Content.ReadAsStringAsync().Result;
                                Root root = JsonSerializer.Deserialize<Root>(x);
                                string myurl = root.endpoints[0].public_url;
                                return myurl;
                            }
                            else
                                return "";
                        }
                    }
                else
                    return "https://localhost:7100";
            }
        }
    }
}
