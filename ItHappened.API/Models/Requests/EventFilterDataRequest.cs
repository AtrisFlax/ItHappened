using Microsoft.AspNetCore.Mvc;
using System;

namespace ItHappened.Api.Models.Requests
{
    // @formatter:off
    public class EventFilterDataRequest
    {
        [FromQuery(Name = "FromDateTime")]
        public DateTime? FromDateTime { get; set; }

        [FromQuery(Name = "ToDateTime")]
        public DateTime? ToDateTime { get; set; }

        [FromQuery(Name = "LowerLimitRating")]
        public double? LowerLimitRating { get; set; }

        [FromQuery(Name = "UpperLimitRating")]
        public double? UpperLimitRating { get; set; }

        [FromQuery(Name = "SubstringForMatching")]
        public string SubstringForMatching { get; set; }

        [FromQuery(Name = "ScaleLowerLimit")]
        public double? ScaleLowerLimit { get; set; }

        [FromQuery(Name = "ScaleUpperLimit")]
        public double? ScaleUpperLimit { get; set; }

        [FromQuery(Name = "GpsLatLeftDownCorner")]
        public double? GpsLatLeftDownCorner { get; set; }

        [FromQuery(Name = "GpsLngLeftDownCorner")]
        public double? GpsLngLeftDownCorner { get; set; }

        [FromQuery(Name = "GpsLatRightUpperCorner")]
        public double? GpsLatRightUpperCorner { get; set; }

        [FromQuery(Name = "GpsLngRightUpperCorner")]
        public double? GpsLngRightUpperCorner { get; set; }
    }
    // @formatter:on 
}