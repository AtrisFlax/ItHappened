namespace ItHappened.Domain
{
    public class GeoTag
    {
        public GeoTag(double gpsLat, double gpsLng)
        {
            GpsLat = gpsLat;
            GpsLng = gpsLng;
        }

        public double GpsLat { get; }
        public double GpsLng { get; }
    }
}