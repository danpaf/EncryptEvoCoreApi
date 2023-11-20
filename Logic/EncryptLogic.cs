using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptEvoCoreApi.Logic
{
    public class EncryptLogic
    {
        public string EncryptDataSha256(string data)
        {
            byte[] encryptedBytes = EncryptAES(data);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptData(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            return DecryptAES(encryptedBytes);
        }

        public string EncryptDataWithCustomKeyAndIV(string data, string k, string IV)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(k);
            byte[] IVBytes = Encoding.UTF8.GetBytes(IV);
            byte[] encryptedBytes = EncryptAESWithCustomKeyAndIV(data, keyBytes, IVBytes);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptDataWithCustomKeyAndIV(string encryptedData, string k, string IV)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(k);
            byte[] IVBytes = Encoding.UTF8.GetBytes(IV);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            return DecryptAESWithCustomKeyAndIV(encryptedBytes, keyBytes, IVBytes);
        }

        private byte[] EncryptAES(string data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] encryptedBytes;

                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    encryptedBytes = ms.ToArray();
                }

                return encryptedBytes;
            }
        }

        private string DecryptAES(byte[] encryptedBytes)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                byte[] IV = new byte[16];
                Array.Copy(encryptedBytes, 0, IV, 0, IV.Length);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, IV);
                string decryptedData;

                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedBytes, IV.Length, encryptedBytes.Length - IV.Length);
                        cryptoStream.FlushFinalBlock();
                        decryptedData = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }

                return decryptedData;
            }
        }

        public (byte[] encryptedData, byte[] generatedKey, byte[] generatedIV) EncryptAESWithGeneratedKeyAndIV(
            string data)
        {
            byte[] generatedKey;
            byte[] generatedIV;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();
                    generatedKey = aes.Key;
                    generatedIV = aes.IV;

                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        byte[] encryptedData = ms.ToArray();
                        return (encryptedData, generatedKey, generatedIV);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                return (Array.Empty<byte>(), Array.Empty<byte>(), Array.Empty<byte>());
            }
        }

        private byte[] EncryptAESWithCustomKeyAndIV(string data, byte[] key, byte[] IV)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = IV;
                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        private string DecryptAESWithCustomKeyAndIV(byte[] encryptedBytes, byte[] key, byte[] IV)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = IV;
                    aes.Mode = CipherMode.CBC;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Возможно, что нужно будет обработать исключение более тщательно или записать дополнительную информацию об ошибке
            }
        }
        public string DecryptAESWithKeyAndIV(string encryptedText, string key, string iv)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = Convert.FromBase64String(key);
                byte[] ivBytes = Convert.FromBase64String(iv);

                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }


        public string DecryptAESWithRandomKeyAndIV(string encryptedText)
        {
            try
            {
                byte[] encryptedBytes =
                    Convert.FromBase64String(encryptedText); // Преобразование строки в массив байтов

                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.GenerateKey(); // Генерация случайного ключа
                    aes.GenerateIV(); // Генерация случайного IV

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Возможно, что нужно будет обработать исключение более тщательно или записать дополнительную информацию
            }
        }
    }
}