namespace System.Windows
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Bietet sichere Ver- und Entschlüsselung von Strings mittels AES-GCM.
    /// </summary>
    /// <example>
    /// 1. Schlüssel generieren (Sollte sicher in Key-Vaults oder Umgebungsvariablen verwahrt werden)
    /// byte[] secretKey = CryptoHelper.ReadKeyFromFile();
    /// var protector = new CryptoHelper(secretKey);
    /// 
    /// 2. Verschlüsseln
    /// string encrypted = protector.Encrypt(originalText);
    /// 
    /// 3. Entschlüsseln
    /// string decrypted = protector.Decrypt(encrypted);
    /// </example>
    public sealed class CryptoHelper
    {
        // C# 14 / .NET 10 Best Practice: Festlegung der kryptografischen Konstanten via modernem Static Readonly
        private static readonly int NonceSize = AesGcm.NonceByteSizes.MaxSize; // 12 Bytes
        private static readonly int TagSize = AesGcm.TagByteSizes.MaxSize;     // 16 Bytes
        private static readonly string fileKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeneralKey.bin");

        private readonly byte[] _key;

        /// <summary>
        /// Initialisiert eine neue Instanz mit einem 256-Bit Schlüssel.
        /// </summary>
        /// <param name="key">Der 32-Byte (256-Bit) geheime Schlüssel.</param>
        public CryptoHelper(byte[] key)
        {
            ArgumentNullException.ThrowIfNull(key);
            if (key.Length != 32)
            {
                throw new ArgumentException("Der Schlüssel muss exakt 32 Bytes (256-Bit) lang sein.", nameof(key));
            }
            _key = (byte[])key.Clone(); // Kopie erstellen, um externe Manipulation zu verhindern
        }

        /// <summary>
        /// Verschlüsselt einen String und gibt das Ergebnis als Base64-String zurück.
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            // .NET 10 optimierte Speicherallokation mittels Spans
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce); // Sichere Zufallszahlen für die Nonce

            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[TagSize];

            // AES-GCM Verschlüsselung ausführen
            using (var aesGcm = new AesGcm(_key, TagSize))
            {
                aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
            }

            // Kombiniertes Array erstellen: [Nonce (12B)] + [Tag (16B)] + [Ciphertext]
            byte[] result = new byte[NonceSize + TagSize + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
            Buffer.BlockCopy(cipherBytes, 0, result, NonceSize + TagSize, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Entschlüsselt einen Base64-verschlüsselten String.
        /// </summary>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            byte[] encryptedData = Convert.FromBase64String(cipherText);

            if (encryptedData.Length < NonceSize + TagSize)
            {
                throw new CryptographicException("Die verschlüsselten Daten sind korrupt oder zu kurz.");
            }

            // Segmente mittels Spans (modernes .NET) effizient und ohne Kopieraufwand auslesen
            ReadOnlySpan<byte> encryptedSpan = encryptedData;
            ReadOnlySpan<byte> nonce = encryptedSpan[..NonceSize];
            ReadOnlySpan<byte> tag = encryptedSpan.Slice(NonceSize, TagSize);
            ReadOnlySpan<byte> cipherBytes = encryptedSpan[(NonceSize + TagSize)..];

            byte[] decryptedBytes = new byte[cipherBytes.Length];

            // AES-GCM Entschlüsselung ausführen
            using (var aesGcm = new AesGcm(_key, TagSize))
            {
                aesGcm.Decrypt(nonce, cipherBytes, tag, decryptedBytes);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        /// <summary>
        /// Hilfsmethode zur sicheren Generierung eines neuen 256-Bit Schlüssels.
        /// </summary>
        public static byte[] GenerateRandomKey()
        {
            byte[] key = new byte[32];
            RandomNumberGenerator.Fill(key);
            return key;
        }

        public static byte[] SaveGenerateRandomKey()
        {
            byte[] key = new byte[32];
            RandomNumberGenerator.Fill(key);
            File.WriteAllBytes(fileKeyPath, key);
            return key;
        }

        public static byte[] ReadKeyFromFile()
        {
            if (File.Exists(fileKeyPath) == false)
            {
                byte[] key = new byte[32];
                RandomNumberGenerator.Fill(key);
                File.WriteAllBytes(fileKeyPath, key);
                return key;
            }

            return File.ReadAllBytes(fileKeyPath);
        }
    }
}
