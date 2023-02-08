namespace dbCompanyTest.Environment
{
    public class Environment
    {
        private bool changeEnvironment = false;
        internal string useEnvironment
        {
            get
            {
                if (changeEnvironment)
                    return "https://fd9d-1-162-236-203.jp.ngrok.io";
                else
                    return "https://localhost:7100";
            }
        }
    }
}
