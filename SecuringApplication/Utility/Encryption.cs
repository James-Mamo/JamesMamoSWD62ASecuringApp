using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace SecuringApplication.Utility
{
    public class Encryption
    {
        public string Hash(string clearText)
        {
            //convert the clearText into an array of bytes
            byte[] clearTextAsBytes = Encoding.UTF32.GetBytes(clearText);
            byte[] digest = Hash(clearTextAsBytes);

            string digestAsString = Convert.ToBase64String(digest);

            return digestAsString;
        }

        public static byte[] Hash(byte [] clearTextBytes)
        {
            SHA512 myAlg = SHA512.Create();

            byte[] digest = myAlg.ComputeHash(clearTextBytes);
            return digest;
        }

        static string password = "Pa$$w0rd";
        static byte[] salt = new byte[]
        {
            20,1,34,56,78,34,11,111,234,43,180,139,127,34,52,45,255,253,1
        };

        public static string SymmetricEncrypt(string clearData)
        {
            byte[] clearDataAsBytes = Encoding.UTF32.GetBytes(clearData);

            Encryption enc = new Encryption();
            byte[] cipherAsBytes = enc.SymmetricEncrypt(clearDataAsBytes);

            string cipher = Convert.ToBase64String(cipherAsBytes);
            cipher = cipher.Replace("+", "_");
            cipher = cipher.Replace("/", "|");
            cipher = cipher.Replace("=", "?");

            return cipher;
        }

        public static string SymmetricDecrypt(string cipher)
        {
            cipher = cipher.Replace("_", "+");
            cipher = cipher.Replace("|", "/");
            cipher = cipher.Replace("?", "=");

            byte[] cipherDataAsBytes = Convert.FromBase64String(cipher);
            Encryption enc = new Encryption();
            byte[] clearDataAsBytes = enc.SymmetricDecrypt(cipherDataAsBytes);

            string originalText = Encoding.UTF32.GetString(clearDataAsBytes);

            return originalText;
        }

        public byte[] SymmetricEncryptWithKeys(byte[] clearData,byte[] SecretKey,byte[] Iv )
        {
            //0 declare algorithm
            Rijndael myAlg = Rijndael.Create();

            //1 Generate Keys
            var keys = GenerateKeys();


            //2 load the data into a MemoryStream
            MemoryStream msIn = new MemoryStream(clearData);
            msIn.Position = 0;

            //3 declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();

            //4 declaring a Stream which handles data encryption
            CryptoStream cs = new CryptoStream(msOut,
                myAlg.CreateEncryptor(SecretKey, Iv),
                CryptoStreamMode.Write
                );

            //5 we start the encrypting
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut

            cs.FlushFinalBlock();

            cs.Close();

            return msOut.ToArray();
        }
        public byte[] SymmetricEncrypt(byte[] clearData)
        {
            //0 declare algorithm
            Rijndael myAlg = Rijndael.Create();

            //1 Generate Keys
            var keys = GenerateKeys();

            //2 load the data into a MemoryStream
            MemoryStream msIn = new MemoryStream(clearData);
            msIn.Position = 0;

            //3 declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();
            
            //4 declaring a Stream which handles data encryption
            CryptoStream cs = new CryptoStream(msOut,
                myAlg.CreateEncryptor(keys.SecretKey,keys.Iv),
                CryptoStreamMode.Write
                );

            //5 we start the encrypting
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut

            cs.FlushFinalBlock();

            cs.Close();

            return msOut.ToArray();
        }

        public MemoryStream HybridEncrypt(MemoryStream clearFile, string publicKey)
        {
            Rijndael myAlg = Rijndael.Create();
            myAlg.GenerateKey(); myAlg.GenerateIV();
            var key = myAlg.Key; var iv = myAlg.IV;

            
            var encryptedBytes = SymmetricEncryptWithKeys(clearFile.ToArray(), key, iv);




            byte[] encryptedKey = AsymmetricEncrypt(key, publicKey);
            byte[] encryptedIv = AsymmetricEncrypt(iv, publicKey);
            MemoryStream msOut = new MemoryStream();
            msOut.Write(encryptedKey, 0, encryptedKey.Length);
            msOut.Write(encryptedIv, 0, encryptedIv.Length);

            MemoryStream encryptedFileContent = new MemoryStream(encryptedBytes);
            encryptedFileContent.Position = 0;
            encryptedFileContent.CopyTo(msOut);

            return msOut;
        }

        public static MemoryStream HybridDecrypt(MemoryStream encFile,string privateKey)
        {
            encFile.Position = 0;
            byte[] retrievedEncKey = new byte[128];
            encFile.Read(retrievedEncKey, 0, 128);

            byte[] retrievedIvKey = new byte[128];
            encFile.Read(retrievedIvKey, 0, 128);

            byte[] key = AsymmetricDecrypt(retrievedEncKey, privateKey);
            byte[] iv = AsymmetricDecrypt(retrievedIvKey, privateKey);



            MemoryStream actualEncryptedFileContent = new MemoryStream();
            encFile.CopyTo(actualEncryptedFileContent);

            byte[] fileDecrypted = SymmetricDecrypt(actualEncryptedFileContent.ToArray(), key, iv);

            MemoryStream actualDecryptedFile = new MemoryStream();
            actualDecryptedFile.Write(fileDecrypted);

            return actualDecryptedFile;

        }

        public static string SignData(MemoryStream data, string privateKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);

            byte[] dataAsBytes = data.ToArray();

            byte[] digest = Hash(dataAsBytes);

            byte[] signatureAsBytes = myAlg.SignHash(digest, "SHA512");

            return Convert.ToBase64String(signatureAsBytes);
        }

        public static bool VerifyData(MemoryStream data, string publicKey,string signature)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);

            byte[] dataAsBytes = data.ToArray();

            byte[] digest = Hash(dataAsBytes);

            byte[] signatureAsBytes = Convert.FromBase64String(signature);

            bool valid = myAlg.VerifyHash(digest,"SHA512",signatureAsBytes);

            return valid;
        }

        public  byte[] SymmetricDecrypt(byte[] cipherAsBytes)
        {
            Rijndael myAlg = Rijndael.Create();

            //1 Generate Keys
            var keys = GenerateKeys();

            //2 load the data into a MemoryStream
            MemoryStream msIn = new MemoryStream(cipherAsBytes);
            msIn.Position = 0;

            //3 declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();

            //4 declaring a Stream which handles data decryption
            CryptoStream cs = new CryptoStream(msOut,
                myAlg.CreateDecryptor(keys.SecretKey, keys.Iv),
                CryptoStreamMode.Write
                );

            //5 we start the encrypting
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut

            cs.FlushFinalBlock();

            cs.Close();

            return msOut.ToArray();
        }

        public static byte[] SymmetricDecrypt(byte[] cipherAsBytes, byte[] SecretKey, byte[] Iv)
        {
            Rijndael myAlg = Rijndael.Create();

            //1 Generate Keys
            var keys = GenerateKeys();

            //2 load the data into a MemoryStream
            MemoryStream msIn = new MemoryStream(cipherAsBytes);
            msIn.Position = 0;

            //3 declare where to store the encrypted data
            MemoryStream msOut = new MemoryStream();

            //4 declaring a Stream which handles data decryption
            CryptoStream cs = new CryptoStream(msOut,
                myAlg.CreateDecryptor(SecretKey, Iv),
                CryptoStreamMode.Write
                );

            //5 we start the encrypting
            msIn.CopyTo(cs);

            //6. make sure that the data is all written (flushed) into msOut

            cs.FlushFinalBlock();

            cs.Close();

            return msOut.ToArray();
        }


        public static SymmetricKeys GenerateKeys()
        {
            Rijndael myAlg = Rijndael.Create();

            Rfc2898DeriveBytes myGenerator = new Rfc2898DeriveBytes(password, salt);

            SymmetricKeys keys = new SymmetricKeys()
            {
                SecretKey = myGenerator.GetBytes(myAlg.KeySize / 8),
                Iv = myGenerator.GetBytes(myAlg.BlockSize / 8)
            };

            return keys;
        }

        public static AsymmetricKeys GenerateAsymmetricKeys()
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();

            AsymmetricKeys myKeys = new AsymmetricKeys()
            {
                PublicKey = myAlg.ToXmlString(false),
                PrivateKey = myAlg.ToXmlString(true)
            };


            return myKeys;
        }

        public string AsymmetricEncrypt(string data, string publicKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);

            byte[] dataAsBytes = Encoding.UTF32.GetBytes(data);
            byte[] cipher = myAlg.Encrypt(dataAsBytes, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(cipher);
        }

        public static byte [] AsymmetricEncrypt(byte[] data, string publicKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(publicKey);
            byte[] cipher = myAlg.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            return cipher;
        }


        public string AsymmetricDecrypt(string cipher, string privateKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);

            byte[] cipherAsBytes = Encoding.UTF32.GetBytes(cipher);
            byte[] originalTextAsBytes = myAlg.Decrypt(cipherAsBytes, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF32.GetString(originalTextAsBytes);
        }

        public static byte[] AsymmetricDecrypt(byte[] cipher, string privateKey)
        {
            RSACryptoServiceProvider myAlg = new RSACryptoServiceProvider();
            myAlg.FromXmlString(privateKey);

            byte[] originalTextAsBytes = myAlg.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);

            return originalTextAsBytes;
        }
        public class AsymmetricKeys
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }
        public class SymmetricKeys
        {
            public byte[] SecretKey { get; set; }
            public byte[] Iv { get; set; }
        }
    }
}
