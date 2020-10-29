using System;

namespace ItHappened.Domain
{
    public class Photo
    {
        public Guid Id { get; private set; }    
        public Photo(byte[] photoBytes)
        {
            PhotoBytes = photoBytes;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public byte[] PhotoBytes { get; private set; }
    }
}