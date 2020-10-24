using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ItHappened.Api.Models.Requests
{
    public class EventFilterRequest
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
        public string CommentRegexPattern { get; set; }

        [FromQuery(Name = "ScaleLowerLimit")]
        public double? ScaleLowerLimit { get; set; }

        [FromQuery(Name = "ScaleUpperLimit")]
        public double? ScaleUpperLimit { get; set; }

        [FromQuery(Name = "GpsLatLeftCorn")]
        public double? GpsLatLeftCorn { get; set; }

        [FromQuery(Name = "GpsLngLeftCorn")]
        public double? GpsLngLeftCorn { get; set; }

        [FromQuery(Name = "GpsLatRigthCorn")]
        public double? GpsLatRigthCorn { get; set; }

        [FromQuery(Name = "GpsLngRightCorn")]
        public double? GpsLngRightCorn { get; set; }

    }
}

