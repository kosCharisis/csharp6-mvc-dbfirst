namespace SchoolApp.Security
{
    public class EncryptionUtil
    {
        public static string Encrypt(string plainText)
        {
            var encrypterPassword = BCrypt.Net.BCrypt.HashPassword(plainText);
            return encrypterPassword;
        }

        public static bool IsValidPassword(string plainText, string cipherText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, cipherText);
        }
    }
}
