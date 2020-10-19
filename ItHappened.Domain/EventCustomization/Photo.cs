namespace ItHappened.Domain
{
    public class Photo
    {
        public Photo(byte[] photoBytes)
        {
            PhotoBytes = photoBytes;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public byte[] PhotoBytes { get; }
    }
}