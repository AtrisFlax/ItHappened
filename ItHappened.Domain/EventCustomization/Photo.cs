using System.Text;

namespace ItHappened.Domain
{
    public class Photo
    {
        public byte[] PhotoBytes { get; }
        
        public Photo(byte[] photoBytes)
        {
            PhotoBytes = photoBytes;
        }

        public static Photo Create(string photoString)
        {
            //TODO в классе Base64Converter есть encoder
            //TODO вероятно не придется кодировать\декодировать фото. Хранить фото просто в base64 кодировке в byte[]
            var photoBytes = Encoding.UTF8.GetBytes(photoString);
            return new Photo(photoBytes);
        }
    }
}