using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AESEncryptionDecrpytionForClassObject
{
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
}
