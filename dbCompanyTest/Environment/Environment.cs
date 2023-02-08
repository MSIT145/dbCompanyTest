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
                    return "";
                else
                    return "https://localhost:7100";
            }
        }
    }
}
