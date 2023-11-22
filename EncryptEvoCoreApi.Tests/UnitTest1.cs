using System.Text;
using EncryptEvoCoreApi.Controllers;
using EncryptEvoCoreApi.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace EncryptEvoCoreApi.EncryptEvoCoreApi.Tests;

public class UnitTest1
{
    private static byte[] _encryptedkey;
    private static byte[] _encryptediv;

    private static byte[] _encryptedData;

    /*private readonly EncryptLogic _encryptLogic;

    public UnitTest1(EncryptLogic encryptLogic)
    {
        _encryptLogic = encryptLogic ?? throw new ArgumentNullException(nameof(encryptLogic));
    }*/
    [Fact]
    public void EncryptDataSha256_ValidInput_ReturnsEncryptedData()
    {
        var _encryptLogic = new EncryptLogic();
        string data = "Hello, World!";

        string encryptedData = _encryptLogic.EncryptDataSha256(data);

        Assert.NotNull(encryptedData);
    }


    [Fact]
    public void EncryptDataWithCustomKeyAndIV_ValidInput_ReturnsEncryptedData()
    {
        var _encryptLogic = new EncryptLogic();
        string data = "Hello, World!";
        string key = "K70MvBiNRuPf+IzdPb//UoPUp6Wpg96vwi5Lr3Qw82Y=";
        string IV = "foOZt4Dl/SKiDzDPXtHF6Q==";

        var encryptedData = _encryptLogic.EncryptAESWithGeneratedKeyAndIV(data);

        Assert.NotNull(encryptedData.encryptedData);
        _encryptedData = encryptedData.encryptedData; 
        _encryptedkey = encryptedData.generatedKey;
        _encryptediv = encryptedData.generatedIV;
    }


    [Fact]
    public void DecryptDataWithCustomKeyAndIV_ValidInput_ReturnsDecryptedData()
    {
        var _encryptLogic = new EncryptLogic();
        string originalData = "Hello, World!";
        var encryptedData = Convert.ToBase64String(_encryptedData);
        var encryptedKey = Convert.ToBase64String(_encryptedkey);
        var encryptediv = Convert.ToBase64String(_encryptediv);

        string decryptedData = _encryptLogic.DecryptAESWithKeyAndIV(encryptedData, encryptedKey, encryptediv);

        Assert.NotNull(decryptedData);
        Assert.Equal(originalData, decryptedData);
    }
}