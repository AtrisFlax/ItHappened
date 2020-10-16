using System;

namespace ItHappend.Domain.EventCustomization
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

        protected bool Equals(GeoTag other)
        {
            return GpsLat.Equals(other.GpsLat) && GpsLng.Equals(other.GpsLng);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GeoTag) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GpsLat, GpsLng);
        }
    }
}