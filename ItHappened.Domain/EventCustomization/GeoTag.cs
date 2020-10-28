using System;

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