namespace ItHappened.Domain
{
    public class GeoTag
    {
        public GeoTag(double gpsLat, double gpsLng)
        {
            GpsLat = gpsLat;
            GpsLng = gpsLng;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public double GpsLat { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public double GpsLng { get; }
    }
}