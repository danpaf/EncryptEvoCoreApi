using System;
using EncryptEvoCoreApi.Logic;
using Microsoft.AspNetCore.Mvc;

namespace EncryptEvoCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        private readonly EncryptLogic _encryptLogic;

        public EncryptionController(EncryptLogic encryptLogic)
        {
            _encryptLogic = new EncryptLogic();
        }

        [HttpPost("EncryptSha256")]
        public IActionResult EncryptSha256([FromBody] string data)
        {
            try
            {
                string encryptedData = _encryptLogic.EncryptDataSha256(data);
                return Ok(new
                {
                    Method = "SHA256 Encryption",
                    EncryptedData = encryptedData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error encrypting data: " + ex.Message);
            }
        }
        [HttpPost("EncryptAes")]
        public IActionResult EncryptData([FromForm] string data)
        {
            try
            {
                    var encryptedData = _encryptLogic.EncryptAESWithGeneratedKeyAndIV(data);
                    return Ok(new 
                    {
                        Method = "AES Encryption",
                        EncryptedData = encryptedData.encryptedData,
                        GeneratedKey = "Auto Generated KEY: " + Convert.ToBase64String(encryptedData.generatedKey),
                        GeneratedIV = "Auto Generated IV: " + Convert.ToBase64String(encryptedData.generatedIV)
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error encrypting data: " + ex.Message);
            }
        }


        [HttpPost("DecryptAes")]
        public IActionResult DecryptData([FromForm] string encryptedData, [FromForm] string key, [FromForm] string iv)
        {
            try
            {
                var decryptedData = _encryptLogic.DecryptAESWithKeyAndIV(encryptedData,key,iv);
                return Ok(new { DecryptedData = decryptedData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error decrypting data", Message = ex.Message });
            }
        }

    }
}