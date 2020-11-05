using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public class MssqlFilter : IMssqlFilter
    {
        public IEnumerable<string> CreateFilterMsSqlPredicates(EventFilter filterData)
        {
            var stringFilterPredicates = new List<string>();

            //date
            if (filterData.FromDateTime.HasValue)
            {
                var a = DateTime.Now;
                var c = a.ToString("s");
                stringFilterPredicates.Add($"HappensDate >= '{(DateTime)filterData.FromDateTime:s}'");
            }

            if (filterData.ToDateTime.HasValue)
            {
                stringFilterPredicates.Add($"HappensDate <= '{(DateTime)filterData.ToDateTime:s}'");
            }

            //scale
            if (filterData.ScaleLowerLimit.HasValue)
            {
                stringFilterPredicates.Add($"scale >= {filterData.ScaleLowerLimit}");
            }

            if (filterData.ScaleUpperLimit.HasValue)
            {
                stringFilterPredicates.Add($"scale <= {filterData.ScaleUpperLimit}");
            }

            //rating
            if (filterData.LowerLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"rating >= {filterData.LowerLimitRating}");
            }

            if (filterData.UpperLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"rating <= {filterData.UpperLimitRating}");
            }

            //comment (by substring)
            if (!string.IsNullOrEmpty(filterData.SubstringForMatching))
            {
                stringFilterPredicates.Add($"comment LIKE '{filterData.SubstringForMatching}'");
            }

            //geotag
            if (filterData.GpsLatLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"latitudeGeo >= {filterData.GpsLatLeftCorner}");
            }

            if (filterData.GpsLatRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"latitudeGeo <= {filterData.GpsLatRightCorner}");
            }

            if (filterData.GpsLngLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"longitudeGeo >= {filterData.GpsLngLeftCorner}");
            }

            if (filterData.GpsLngRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"longitudeGeo <= {filterData.GpsLngRightCorner}");
            }

            return stringFilterPredicates;
        }
    }
}