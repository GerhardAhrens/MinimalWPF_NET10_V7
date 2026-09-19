namespace System.Windows
{
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptoHelper
    {
        private const int SALTSIZE = 16;
        private const int NONCESIZE = 12;
        private const int TAGSIZE = 16;
        private const int KEYSIZE = 32;
        private static readonly string TOKEN = "EE8041F5-3A80-400F-BFD8-F8C9593509A9";

        public static byte[] Encrypt(string plainText, string password = null)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = TOKEN;
            }

            byte[] salt = RandomNumberGenerator.GetBytes(SALTSIZE);
            byte[] nonce = RandomNumberGenerator.GetBytes(NONCESIZE);

            byte[] key = DeriveKey(password, salt);

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] ciphertext = new byte[plaintextBytes.Length];
            byte[] tag = new byte[TAGSIZE];

            using var aes = new AesGcm(key, TAGSIZE);
            aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

            var result = new byte[
                salt.Length + nonce.Length + ciphertext.Length + tag.Length];

            int offset = 0;
            Buffer.BlockCopy(salt, 0, result, offset, salt.Length);
            offset += salt.Length;

            Buffer.BlockCopy(nonce, 0, result, offset, nonce.Length);
            offset += nonce.Length;

            Buffer.BlockCopy(ciphertext, 0, result, offset, ciphertext.Length);
            offset += ciphertext.Length;

            Buffer.BlockCopy(tag, 0, result, offset, tag.Length);

            return result;
        }

        public static string Decrypt(byte[] encryptedData, string password = null)
        {
            if (string.IsNullOrEmpty(password))
            {
                password = TOKEN;
            }

            byte[] salt = encryptedData[..SALTSIZE];
            byte[] nonce = encryptedData[SALTSIZE..(SALTSIZE + NONCESIZE)];
            byte[] tag = encryptedData[^TAGSIZE..];
            byte[] ciphertext = encryptedData[(SALTSIZE + NONCESIZE)..^TAGSIZE];

            byte[] key = DeriveKey(password, salt);
            byte[] plaintext = new byte[ciphertext.Length];

            using var aes = new AesGcm(key, TAGSIZE);
            aes.Decrypt(nonce, ciphertext, tag, plaintext);

            return Encoding.UTF8.GetString(plaintext);
        }

        // 🔑 Moderne Key-Derivation ohne PBKDF2
        private static byte[] DeriveKey(string password, byte[] salt)
        {
            using var hmac = new HMACSHA256(salt);
            byte[] hash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(password));

            return hash[..KEYSIZE];
        }
    }
}
