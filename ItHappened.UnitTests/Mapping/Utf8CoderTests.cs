using ItHappened.Api.Mapping;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class Utf8CoderTests
    {
        private Utf8Coder _utf8Coder = new Utf8Coder();

        [SetUp]
        public void Setup()
        {
            _utf8Coder = new Utf8Coder();
        }

        [Test]
        public void EncodeDecode()
        {
            //arrange
            var text = "Mindbox";

            //act
            var bytes = _utf8Coder.Encode(text);
            var textResult = _utf8Coder.Decode(bytes);

            //assert
            Assert.AreEqual(text, textResult);
        }

        [Test]
        public void DecodeEncode()
        {
            //arrange
            var bytes = new byte[] {0x1, 0x4, 0x5, 0x1, 0x9};

            //act
            var text = _utf8Coder.Decode(bytes);
            var bytesResult = _utf8Coder.Encode(text);

            //assert
            Assert.AreEqual(bytes, bytesResult);
        }
    }
}