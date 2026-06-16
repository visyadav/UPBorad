using System.Security.Cryptography;
using System.Text;

namespace API.Services;

public interface IEncryptionService
{
    string EncryptResponse(string plainText);
}

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionService(IConfiguration configuration)
    {
        var responseKeyStr = configuration["EncryptionSettings:ResponseKey"] ?? "my32lengthsupersecretnooneknows1";
        var responseIvStr = configuration["EncryptionSettings:ResponseIV"] ?? "my16lengthsecret";
        
        _key = Encoding.UTF8.GetBytes(responseKeyStr);
        _iv = Encoding.UTF8.GetBytes(responseIvStr);
    }

    public string EncryptResponse(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }
}
