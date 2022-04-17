using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Functions.Util.Cryptography
{
    public static class Cryptography
    {
        //private static byte[] BIV = { 0x50, 0x80, 0xF1 ,0xDD, 0xDE,0x3C, 0xF2};
        private static byte[] BIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2,0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49,0xAB, 0xBC };
        private const string Key = "LiB3rtyY0uR4plic4ti0nF0r0Rd3r50k";
        public static string Encrypts(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            byte[] key = Convert.FromBase64String(Key);
            byte[] text = new UTF8Encoding().GetBytes(value);

            Rijndael rijndael = new RijndaelManaged();
            rijndael.KeySize = 256;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cript = new CryptoStream(mStream, rijndael.CreateEncryptor(key, BIV), CryptoStreamMode.Write);
            cript.Write(text, 0, text.Length);
            cript.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        public static string Decrypts(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
            try
            {
                // Cria instancias de vetores de bytes com as chaves
                byte[] key = Convert.FromBase64String(Key);
                byte[] text = Convert.FromBase64String(value);


                Rijndael rijndael = new RijndaelManaged();
                rijndael.KeySize = 256;
                MemoryStream mStream = new MemoryStream();
                CryptoStream decript = new CryptoStream(mStream, rijndael.CreateDecryptor(key, BIV), CryptoStreamMode.Write);
                decript.Write(text, 0, text.Length);
                decript.FlushFinalBlock();
                UTF8Encoding utf8 = new UTF8Encoding();

                return utf8.GetString(mStream.ToArray());
            }

            
            catch
            {
                return value;
            }
        }

    }
}
