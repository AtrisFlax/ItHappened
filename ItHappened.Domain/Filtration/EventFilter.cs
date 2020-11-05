using System;

namespace ItHappened.Domain
{
    public class EventFilter
    {
        public DateTime? FromDateTime { get; private set; }

        public DateTime? ToDateTime { get; private set; }

        public double? LowerLimitRating { get; private set; }

        public double? UpperLimitRating { get; private set; }

        public string SubstringForMatching { get; private set; }

        public double? ScaleLowerLimit { get; private set; }

        public double? ScaleUpperLimit { get; private set; }

        public double? GpsLatLeftCorner { get; private set; }

        public double? GpsLngLeftCorner { get; private set; }

        public double? GpsLatRightCorner { get; private set; }

        public double? GpsLngRightCorner { get; private set; }
    }
}