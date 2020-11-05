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

        [FromQuery(Name = "CommentRegexPattern")]
        public string SubstringForMatching { get; set; }

        [FromQuery(Name = "ScaleLowerLimit")]
        public double? ScaleLowerLimit { get; set; }

        [FromQuery(Name = "ScaleUpperLimit")]
        public double? ScaleUpperLimit { get; set; }

        [FromQuery(Name = "GpsLatLeftCorner")]
        public double? GpsLatLeftCorner { get; set; }

        [FromQuery(Name = "GpsLngLeftCorner")]
        public double? GpsLngLeftCorner { get; set; }

        [FromQuery(Name = "GpsLatRightCorner")]
        public double? GpsLatRightCorner { get; set; }

        [FromQuery(Name = "GpsLngRightCorner")]
        public double? GpsLngRightCorner { get; set; }
    }
    // @formatter:on 
}