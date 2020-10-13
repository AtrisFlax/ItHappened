namespace ItHappend.Domain
{
    public class GeoTag
    {
        public double GpsLat { get; }
        public double GpsLng { get; }

        public GeoTag(double gpsLat, double gpsLng)
        {
            GpsLat = gpsLat;
            GpsLng = gpsLng;
        }
    }
}