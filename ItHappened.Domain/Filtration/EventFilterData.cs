using System;

namespace ItHappened.Domain
{
    
    public class EventFilterData
    {
        public DateTime? FromDateTime { get; private set; }

        public DateTime? ToDateTime { get; private set; }

        public double? LowerLimitRating { get; private set; }

        public double? UpperLimitRating { get; private set; }

        public string SubstringForMatching { get; private set; }

        public double? ScaleLowerLimit { get; private set; }

        public double? ScaleUpperLimit { get; private set; }

        public double? GpsLatLeftDownCorner { get; private set; }

        public double? GpsLngLeftDownCorner { get; private set; }

        public double? GpsLatRightUpperCorner { get; private set; }

        public double? GpsLngRightUpperCorner { get; private set; }
    }
}