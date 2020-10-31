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
            var photoBytes = Encoding.UTF8.GetBytes(photoString);
            return new Photo(photoBytes);
        }
    }
}