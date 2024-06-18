namespace AplicacaoWeb.Domain
{
    public class Key
    {
        private readonly string _Secret;

        public Key()
        {
            _Secret = "secret_de_codigo:1m1JlBXSIwhRwu4ys7wOkAFD1xYxesWYzhXgCsvPY5gQDDfiKZ";
        }

        public string GetSecret()
        {
            string envSecret = Environment.GetEnvironmentVariable("GSENVSECRET");
            return envSecret + this._Secret;
        }
    }
}
