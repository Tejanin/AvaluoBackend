namespace AvaluoAPI.Utilities
{
    public class Hasher
    {
        public static string Hash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password + salt, 13);
        }
        public static bool Verify(string password,string salt, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password + salt, hashedPassword);
        }

    }
}
