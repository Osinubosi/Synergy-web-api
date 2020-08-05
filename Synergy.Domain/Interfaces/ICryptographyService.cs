using Synergy.Domain.ServiceModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Domain.Interfaces
{
    public interface ICryptographyService
    {
        HashDetail GenerateHash(string input);
        bool ValidateHash(string input, string salt, string hashedValue);
        string Base64Encode(string plainText);
        string Base64Decode(string base64EncodedData);
        #region FlutterWave encryption
        string GetFlutterwaveEcryptionKey(string secretKey);
        string FlutterWaveEncryptData(string encryptionKey, string model);
        string FlutterWaveDecryptData(string encryptedData, string encryptionKey);
        #endregion

        #region Generate Client Key
        ClientApiKey EncryptApiKey(string clientId, string keyPrefix);
        string EncryptBvn(string bvn);
        string DecryptBvn(string encryptBvn);
        string DecryptApiKey(string clientId, string encryptKey);
        #endregion
    }
}
