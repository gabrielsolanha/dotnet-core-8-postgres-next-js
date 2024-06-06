namespace AplicacaoWeb.Domain
{
    public class GenerateHashService
    {
        private readonly string _salt;

        public GenerateHashService()
        {
            _salt = "salt_de_codigo:m0[jpvA2K1=D-HzYDBHLyEewoRoUbQd;Mi)xqw9f),dFefk967";
        }

        public string GenerateHashedPassword(string userHash)
        {
            string envSalt = Environment.GetEnvironmentVariable("GSENVSALT");

            if (string.IsNullOrEmpty(envSalt))
            {
                throw new InvalidOperationException("Environment variable 'GSENVSALT' is not set.");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userHash + envSalt + this._salt, salt);

            return hashedPassword;
        }
        public bool VerifyPassword(string userHash, string storedHashedPassword)
        {
            string envSalt = Environment.GetEnvironmentVariable("GSENVSALT");

            if (string.IsNullOrEmpty(envSalt))
            {
                throw new InvalidOperationException("Environment variable 'GSENVSALT' is not set.");
            }

            string combinedHash = userHash + envSalt + this._salt;
            return BCrypt.Net.BCrypt.Verify(combinedHash, storedHashedPassword);
        }
    }
}
