## AES encryption and decryption an class object in C# ##

This is the best way for AES encryption and decryption an class object in C#. Here i'm explain about AES Key and AES IV usage. And provide an **example to write and read byte[] into filestream using AES encryption and decryption an class object in C#**.

1. Create new class

```
    // Create new class 
    public class Profile
    {
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        internal string Name { get; set; }

        [JsonPropertyName("password")]
        [JsonProperty(PropertyName = "password")]
        internal string Password { get; set; }

        [JsonPropertyName("profileData")]
        [JsonProperty(PropertyName = "profileData")]
        public byte[] ProfileData { get; set; }
    }
```

2. **AES KEY** used the secret key for the symmetric algorithm. This is secret key, is something you keep secret. Anyone who knows your key (or can guess it) can decrypt any data you've encrypted with it (or forge any authentication codes you've calculated with it, etc.).

3. **AES IV** used as initialization vector (IV) for the symmetric algorithm. Initialization vector is, in its broadest sense, just the initial value used to start some iterated process. So you can maintain in your code itself.

```
        // Encryption and decryption key (i.e., secret key for the symmetric algorithm)
        private readonly static byte[] Key = Convert.FromBase64String("AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=");
        // initialization vector
        private readonly static byte[] IV = Convert.FromBase64String("Aq0UThtJhjbuyWXtmZs1rw==");
```
 
4. Example for write and read byte[] into filestream using AES encryption and decryption an class object in C#.

```
class Program
    {
        private readonly static byte[] Key = Convert.FromBase64String("AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=");

        private readonly static byte[] IV = Convert.FromBase64String("Aq0UThtJhjbuyWXtmZs1rw==");

        public static Profile Profile { get; set; }

        static void Main(string[] args)
        {
            Profile = new Profile();
            // Enter file name.
            string fileName = "D:\\Profile.txt";
            Profile.Name = "Ramesh";
            Profile.Password = "Password";
            Console.WriteLine("Enter your option:");
            Console.WriteLine("1. Encryption");
            Console.WriteLine("2. Decryption");
            string option = Console.ReadLine();

            if (option == "1")
            {
                // Create new file using filestream.
                FileStream fsWrite = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                // Convert class object into string.
                string serializeProfile = Newtonsoft.Json.JsonConvert.SerializeObject(Profile);
                // Encrypt profile class object.
                Profile.ProfileData = EncryptStringToBytes(serializeProfile);
                // Write encrypt values into filestream.
                fsWrite.Write(Profile.ProfileData, 0, Profile.ProfileData.Length);
                fsWrite.Close();
            }
            else
            {
                // Read existing encryption file.
                FileStream fsRead = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                // Convert filestream into byte[]
                BinaryReader br = new BinaryReader(fsRead);
                long numBytes = new FileInfo(fileName).Length;
                // Get decrypt string from byte[]. 
                string decryptedText = DecryptStringFromBytes(br.ReadBytes((int)numBytes));
                // Convert string into class oject.
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
```
5. Console output snap

[Console Output snap][1]

  [1]: https://i.stack.imgur.com/cb7fc.png
