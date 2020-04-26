using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;

namespace AESEncryptionDecrpytionForClassObject
{
    class Program
    {
        private readonly static byte[] Key = Convert.FromBase64String("AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=");

        private readonly static byte[] IV = Convert.FromBase64String("Aq0UThtJhjbuyWXtmZs1rw==");

        public static Profile Profile { get; set; }

        static void Main(string[] args)
        {
            Profile = new Profile();
            string fileName = "D:\\Profile.txt";
            Profile.Name = "Ramesh";
            Profile.Password = "Password";
            Console.WriteLine("Enter your option:");
            Console.WriteLine("1. Encryption");
            Console.WriteLine("2. Decryption");
            string option = Console.ReadLine();

            if (option == "1")
            {
                FileStream fsWrite = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                string serializeProfile = Newtonsoft.Json.JsonConvert.SerializeObject(Profile);
                Profile.ProfileData = EncryptStringToBytes(serializeProfile);
                fsWrite.Write(Profile.ProfileData, 0, Profile.ProfileData.Length);
                fsWrite.Close();
            }
            else
            {
                FileStream fsRead = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fsRead);
                long numBytes = new FileInfo(fileName).Length;
                string decryptedText = DecryptStringFromBytes(br.ReadBytes((int)numBytes));
                Profile DeserializeProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<Profile>(decryptedText);
                Console.WriteLine("Name :" + DeserializeProfile.Name);
                Console.WriteLine("Password :" + DeserializeProfile.Password);
                Console.ReadKey();
                fsRead.Close();
            }
        }

        private static byte[] EncryptStringToBytes(string profileText)
        {
            byte[] encryptedAuditTrail;

            using (Aes newAes = Aes.Create())
            {
                newAes.Key = Key;
                newAes.IV = IV;

                ICryptoTransform encryptor = newAes.CreateEncryptor(Key, IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(profileText);
                        }
                        encryptedAuditTrail = msEncrypt.ToArray();
                    }
                }
            }

            return encryptedAuditTrail;
        }

        private static string DecryptStringFromBytes(byte[] profileText)
        {
            string decryptText;

            using (Aes newAes = Aes.Create())
            {
                newAes.Key = Key;
                newAes.IV = IV;

                ICryptoTransform decryptor = newAes.CreateDecryptor(Key, IV);

                using (MemoryStream msDecrypt = new MemoryStream(profileText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }


            return decryptText;
        }
    }
}
