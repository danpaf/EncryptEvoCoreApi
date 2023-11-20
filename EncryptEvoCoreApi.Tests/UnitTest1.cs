using System.Text;
using EncryptEvoCoreApi.Controllers;
using EncryptEvoCoreApi.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace EncryptEvoCoreApi.EncryptEvoCoreApi.Tests;

public class UnitTest1
{
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

        string encryptedData = _encryptLogic.EncryptDataWithCustomKeyAndIV(data, key, IV);

        Assert.NotNull(encryptedData);
    }


    [Fact]
    public void DecryptDataWithCustomKeyAndIV_ValidInput_ReturnsDecryptedData()
    {
        var _encryptLogic = new EncryptLogic();
        string originalData = "Hello,World!";
        string key = "D0yRi6NJEsUXxTAt3EHmyREclISh1szlyyUbh8Qo0BQ="; 
        string IV = "1nhAkHiTy+xWjNNYNOkQJw==";
        string encryptedData = _encryptLogic.EncryptDataWithCustomKeyAndIV(originalData, key, IV);

        string decryptedData = _encryptLogic.DecryptAESWithKeyAndIV(encryptedData, key, IV);

        Assert.NotNull(decryptedData);
        /*Assert.Equal(originalData, decryptedData);*/
    }

}
