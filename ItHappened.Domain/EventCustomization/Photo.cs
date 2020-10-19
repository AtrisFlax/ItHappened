namespace ItHappened.Domain
{
    public class Photo
    {
        public Photo(byte[] photoBytes)
        {
            PhotoBytes = photoBytes;
        }

        public byte[] PhotoBytes { get; }
    }
}