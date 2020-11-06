namespace ItHappened.Api.Mapping
{
    public interface IPhotoCoder
    {
        byte[] Encode(string requestPhoto);

        string Decode(byte[] photoBytes);
    }
}