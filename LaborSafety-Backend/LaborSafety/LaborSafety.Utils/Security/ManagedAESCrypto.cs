using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaborSafety.Utils.Security
{
    public class ManagedAESCrypto
    {
        private static string PrivateKey = "dalIaSkQF11VZ3SMqgJ3eQ==,YJXoVwQ8P4kdxpIMrcyTDM72ECgeObJ8U8LQYUClzKE=";

        public static string Encrypt(string plainText, byte[] Key = null, byte[] IV = null)
        {
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged()
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {
                ICryptoTransform encryptor = null;
                // Create a decryptor    
                if (Key != null && IV != null)
                    encryptor = aes.CreateEncryptor(Key, IV);
                else
                {
                    aes.IV = Convert.FromBase64String(PrivateKey.Split(',')[0]);
                    aes.Key = Convert.FromBase64String(PrivateKey.Split(',')[1]);

                    encryptor = aes.CreateEncryptor();
                }

                using (encryptor)
                {
                    byte[] plainByte = ASCIIEncoding.UTF8.GetBytes(plainText);

                    byte[] cipherText = encryptor.TransformFinalBlock(plainByte, 0, plainText.Length);
                    return Convert.ToBase64String(cipherText);
                }
            }
        }

        public static string Decrypt(string cipherText, byte[] Key = null, byte[] IV = null)
        {
            // Create AesManaged    
            using (AesManaged aes = new AesManaged()
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {
                ICryptoTransform decryptor = null;
                // Create a decryptor    
                if (Key != null && IV != null)
                    decryptor = aes.CreateDecryptor(Key, IV);
                else
                {
                    aes.IV = Convert.FromBase64String(PrivateKey.Split(',')[0]);
                    aes.Key = Convert.FromBase64String(PrivateKey.Split(',')[1]);

                    decryptor = aes.CreateDecryptor();
                }

                using (decryptor)
                {
                    byte[] encryptedBytes = Convert.FromBase64CharArray(cipherText.ToCharArray(), 0, cipherText.Length);
                    return ASCIIEncoding.UTF8.GetString(decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
                }
            }
        }

        public static string GenerateKey()
        {
            using (var aesEncryption = new AesManaged())
            {
                aesEncryption.KeySize = 256;
                aesEncryption.BlockSize = 128;
                aesEncryption.Mode = CipherMode.CBC;
                aesEncryption.Padding = PaddingMode.PKCS7;
                aesEncryption.GenerateIV();
                string ivStr = Convert.ToBase64String(aesEncryption.IV);
                aesEncryption.GenerateKey();
                string keyStr = Convert.ToBase64String(aesEncryption.Key);

                var privateKey = ivStr + "," + keyStr;

                return privateKey;
            }
        }
    }
}
