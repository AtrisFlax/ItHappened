namespace ItHappened.Domain.Customizations
{
    public class Photo
    {
        public byte[] PhotoBytes { get; }

        public Photo(byte[] photoBytes)
        {
            PhotoBytes = photoBytes;
        }
    }
}