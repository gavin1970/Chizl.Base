namespace Chizl.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    
    public static class AesEncryptor
    {
        private static readonly int SaltSize = 16;          // 16=128 bit, 32=256 bit
        private static readonly int Iterations = 10000;

        /// <summary>
        /// Encrypts the given plaintext using AES encryption with a password-derived key.
        /// <code>
        /// var plainText = "String text goes here.";
        /// var pass = "MyStrongPass$23!";
        /// var encrypted = AesEncryptor.Encrypt(secret, pass);
        /// var decrypted = AesEncryptor.Decrypt(encrypted, pass);
        /// Console.WriteLine($"Encrypted: {encrypted}");
        /// Console.WriteLine($"Decrypted: {decrypted}");
        /// </code>
        /// </summary>
        /// <param name="plainText">the string to encrypt</param>
        /// <param name="password">encryption password w/ PBKDF2</param>
        /// <returns>Base64-encoded string containing the salt and encrypted data</returns>        
        public static string Encrypt(string plainText, string password)
        {
            /// <param name="plainText">  </param>
            /// <param name="password"></param>
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(salt); }

            // Implementing the PBKDF2 (Password-Based Key Derivation Function 2) algorithm to
            // derive a cryptographic key and initialization vector (IV) from the provided password and salt.
#if NETSTANDARD2_0
            using (var keyDerivation = new Rfc2898DeriveBytes(password, salt, Iterations))
#else
            using (var keyDerivation = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
#endif
            {
                // Derive a 256-bit key and a 128-bit IV from the password and salt
                var key = keyDerivation.GetBytes(32); // 256 bits
                // The IV is derived from the same key derivation function, ensuring
                // that it is unique for each encryption operation due to the random salt.
                var iv = keyDerivation.GetBytes(16);  // 128 bits

                // Create an AES encryption object and set its key and IV
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    // Create an encryptor from the AES object
                    // and a memory stream to hold the encrypted data
                    using (var encryptor = aes.CreateEncryptor())
                    {
                        // The salt is prepended to the encrypted data so that it can be used during decryption
                        using (var ms = new MemoryStream())
                        {
                            // Write the salt to the beginning of the memory stream,
                            // ensuring that it is available for the decryption process.
                            ms.Write(salt, 0, salt.Length);
                            // Create a CryptoStream that encrypts data written to it
                            // and a StreamWriter to write the plaintext
                            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                // Write the plaintext to the CryptoStream, which will encrypt
                                // it and write the encrypted data to the memory stream.
                                using (var sw = new StreamWriter(cs))
                                    sw.Write(plainText);    // The StreamWriter is disposed here,
                                                            // which will flush the final block
                                                            // of encrypted data to the memory stream.
                            }

                            // Convert the encrypted data (including the prepended salt)
                            // to a Base64 string for easy storage and transmission.
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a Base64-encoded, AES-encrypted string using the specified password.
        /// <code>
        /// var plainText = "String text goes here.";
        /// var pass = "MyStrongPass$23!";
        /// var encrypted = AesEncryptor.Encrypt(secret, pass);
        /// var decrypted = AesEncryptor.Decrypt(encrypted, pass);
        /// Console.WriteLine($"Encrypted: {encrypted}");
        /// Console.WriteLine($"Decrypted: {decrypted}");
        /// </code>
        /// </summary>
        /// <remarks>The input must be encrypted using the corresponding encryption method that embeds the
        /// salt at the beginning of the cipher text. Uses PBKDF2 for key derivation and AES for decryption.</remarks>
        /// <param name="cipherTextBase64">The Base64-encoded string containing the salt and encrypted data.</param>
        /// <param name="password">The password used to derive the cryptographic key and IV.</param>
        /// <returns>The decrypted plaintext string.</returns>
        public static string Decrypt(string cipherTextBase64, string password)
        {
            // Convert the Base64-encoded string back to a byte
            // array, which contains the salt and the encrypted data.
            byte[] cipherBytes = Convert.FromBase64String(cipherTextBase64);

            // Extract the salt from the beginning of the byte array
            byte[] salt = new byte[SaltSize];
            // The salt is extracted from the beginning of the byte
            // array, allowing the decryption process to
            Array.Copy(cipherBytes, 0, salt, 0, SaltSize);

            // Implementing the PBKDF2 (Password-Based Key Derivation Function 2) algorithm to derive the
            // same cryptographic key and initialization vector (IV) from the provided password and extracted salt.
#if NETSTANDARD2_0
            using (var keyDerivation = new Rfc2898DeriveBytes(password, salt, Iterations))
#else
            using (var keyDerivation = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
#endif
            {
                // Derive the same 256-bit key and 128-bit IV from the password and salt
                var key = keyDerivation.GetBytes(32);
                // The IV is derived from the same key derivation function, ensuring that it matches
                var iv = keyDerivation.GetBytes(16);

                // Create an AES decryption object and set its key and IV
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    // Create a decryptor from the AES object and a memory stream to read the encrypted data
                    using (var decryptor = aes.CreateDecryptor())
                    // The memory stream is initialized to read from the byte array starting after the salt,
                    using (var ms = new MemoryStream(cipherBytes, SaltSize, cipherBytes.Length - SaltSize))
                    // Create a CryptoStream that decrypts data read from it and a StreamReader to read the decrypted plaintext
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    // Read the decrypted plaintext from the CryptoStream using a StreamReader and return it as a string.
                    using (var sr = new StreamReader(cs))
                        return sr.ReadToEnd(); // The StreamReader is disposed here, which
                                               // will close the CryptoStream and the memory stream.
                }
            }
        }
    }
}
