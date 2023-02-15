using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;

namespace dbCompanyTest.Environment
{
    public class Environment
    {
        string apiKey = "2LU0i8A48bax1cQNKoRH5OFwfOG_43RaqcDa3yYViTPCgHtG7";
        //string apiKey = "2Lfh6JnTJCDuc3ES58TS4RXDREl_4GWqEQHoZZwEoKEFDTmg5";//LU
        public static bool open = true;
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
                            var x = response.Content.ReadAsStringAsync().Result;
                            Root root = JsonSerializer.Deserialize<Root>(x);
                            if (root.endpoints.Count == 0)
                                return "https://localhost:7100";
                            else
                            {
                                string LineBotKey = "CiB9XbeXDnIXgfN8u7zbtIFGkaxP+VXghErm0tE/bntZJ6M9VZrIKvxUoLT2/38sLsDIXthopd+NwlcX/DT+LJKuOMUp9TJ/VlqVlcrsWjp1cjwFDzaL/2KcN3b+vNRgnP83LrM+iA6QYkFt/VqKiAdB04t89/1O/w1cDnyilFU=";
                                var data = new Dictionary<string, string>()
                                {
                                    { "endpoint", root.endpoints[0].public_url+"/api/LineBot/Webhook" }
                                };
                                HttpClient Line = new HttpClient();
                                HttpRequestMessage lineRequest = new HttpRequestMessage(new HttpMethod("Put"), "https://api.line.me/v2/bot/channel/webhook/endpoint")
                                {
                                    Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
                                };
                                Line.DefaultRequestHeaders.Add("Authorization", "Bearer " + LineBotKey);
                                var lineResponse = Line.SendAsync(lineRequest).Result;
                                var a = lineResponse.Content.ReadAsStringAsync().Result;
                                return root.endpoints[0].public_url;
                            }
                        }
                    }
                else
                    return "https://localhost:7100";
            }
        }
    }
}
