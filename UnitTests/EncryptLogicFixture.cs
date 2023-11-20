using EncryptEvoCoreApi.Logic;

namespace EncryptEvoCoreApi.UnitTests;

public class EncryptLogicFixture
{
    public EncryptLogic EncryptLogicInstance { get; }

    public EncryptLogicFixture()
    {
        EncryptLogicInstance = new EncryptLogic();
    }
}