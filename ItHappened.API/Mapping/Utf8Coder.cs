using System.Text;

namespace ItHappened.Api.Mapping
{
    public class Utf8Coder : IPhotoCoder
    {
        public byte[] Encode(string photoString)
        {
            return Encoding.UTF8.GetBytes(photoString);
        }

        public string Decode(byte[] photoBytes)
        {
            return Encoding.UTF8.GetString(photoBytes, 0, photoBytes.Length);
        }
    }
}