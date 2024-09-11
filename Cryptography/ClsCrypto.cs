using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    /// <summary>
    /// This class is used to implement cryptography.
    /// </summary>

    public class ClsCrypto
    {
       
            static string encryptionKey = "1@3$5&6%";
            private static byte[] key = { };
            private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
            /// <summary>
            /// Used to decrypt string.
            /// </summary>
            /// <param name="stringToDecrypt"></param>
            /// <returns></returns>
            public static string Decrypt(string stringToDecrypt)
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];

                try
                {
                    key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    stringToDecrypt = stringToDecrypt.Replace(" ", "+");

                    int mod4 = stringToDecrypt.Length % 4;
                    if (mod4 > 0)
                    {
                        stringToDecrypt += new string('=', 4 - mod4);
                    }

                    inputByteArray = Convert.FromBase64String(stringToDecrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms,
                      des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            /// <summary>
            /// Used to encrypt base 64 decrypted string
            /// </summary>
            /// <param name="stringToEncrypt"></param>
            /// <returns></returns>
            public static string Encrypt(string stringToEncrypt)
            {
                try
                {
                    key = System.Text.Encoding.UTF8.GetBytes(encryptionKey);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms,
                      des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
}
