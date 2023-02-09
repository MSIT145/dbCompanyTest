namespace dbCompanyTest.Environment
{
    public class Environment
    {
        private bool changeEnvironment = true;
        internal string useEnvironment
        {
            get
            {
                if (changeEnvironment)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), "https://ngrok.hmi.tw/2LU0i8A48bax1cQNKoRH5OFwfOG_43RaqcDa3yYViTPCgHtG7"))
                        {
                            //request.Headers.TryAddWithoutValidation("Authorization", "Bearer" + ApiKey);
                            //request.Headers.TryAddWithoutValidation("Ngrok-Version", "2");
                            var response = client.SendAsync(request).Result;
                            var x = response.Content.ReadAsStringAsync().Result;
                            int site = x.IndexOf(":");
                            string myUrl = "https://" + x.Substring(0, site);
                            return myUrl;
                        }
                    }
                }
                else
                    return "https://localhost:7100";
            }
        }


    }
}
