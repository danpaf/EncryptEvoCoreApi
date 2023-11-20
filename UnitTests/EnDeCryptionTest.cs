using EncryptEvoCoreApi.Logic;
using Xunit;

namespace EncryptEvoCoreApi.UnitTests
{
    [Collection("EncryptLogicTests")]
    public class EnDeCryptionTest
    {
        private readonly EncryptLogic _encryptLogic;

        public EnDeCryptionTest(EncryptLogicFixture fixture)
        {
            _encryptLogic = fixture.EncryptLogicInstance;
        }

        [Fact]
        public void EncryptDataSha256_ValidInput_ReturnsEncryptedData()
        {
            // Arrange
            string data = "Hello, World!";

            // Act
            string encryptedData = _encryptLogic.EncryptDataSha256(data);

            // Assert
            Assert.NotNull(encryptedData);
            Assert.NotEmpty(encryptedData);
        }

        [Fact]
        public void DecryptData_ValidInput_ReturnsDecryptedData()
        {
            // Arrange
            string originalData = "Hello, World!";
            string encryptedData = _encryptLogic.EncryptDataSha256(originalData);

            // Act
            string decryptedData = _encryptLogic.DecryptData(encryptedData);

            // Assert
            Assert.NotNull(decryptedData);
            Assert.NotEmpty(decryptedData);
            Assert.Equal(originalData, decryptedData);
        }

        [Fact]
        public void EncryptDataWithCustomKeyAndIV_ValidInput_ReturnsEncryptedData()
        {
            // Arrange
            string data = "Hello, World!";
            string key = "MySecretKey";
            string IV = "MyInitializationVector";

            // Act
            string encryptedData = _encryptLogic.EncryptDataWithCustomKeyAndIV(data, key, IV);

            // Assert
            Assert.NotNull(encryptedData);
            Assert.NotEmpty(encryptedData);
        }

        [Fact]
        public void DecryptDataWithCustomKeyAndIV_ValidInput_ReturnsDecryptedData()
        {
            // Arrange
            string originalData = "Hello, World!";
            string key = "MySecretKey";
            string IV = "MyInitializationVector";
            string encryptedData = _encryptLogic.EncryptDataWithCustomKeyAndIV(originalData, key, IV);

            // Act
            string decryptedData = _encryptLogic.DecryptDataWithCustomKeyAndIV(encryptedData, key, IV);

            // Assert
            Assert.NotNull(decryptedData);
            Assert.NotEmpty(decryptedData);
            Assert.Equal(originalData, decryptedData);
        }
    }
}
