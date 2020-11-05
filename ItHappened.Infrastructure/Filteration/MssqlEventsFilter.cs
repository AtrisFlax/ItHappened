using System;
using System.Collections.Generic;
using ItHappened.Domain;
using Microsoft.EntityFrameworkCore.Internal;

namespace ItHappened.Infrastructure
{
    public class MssqlEventsFilter : IMssqlFilter
    {
        private const string MssqlTimeFormat = "yyyyMMdd HH:mm:ss.fff";
        public string CreateFilterMsSqlPredicates(EventFilterData filterDataData, string tableName)
        {
            var stringPredicates = CreateStringPredicates(filterDataData, tableName);
            var generalPredicate = !stringPredicates.Any() ? string.Empty : string.Join(" and ", stringPredicates);
            return generalPredicate;
        }

        private static List<string> CreateStringPredicates(EventFilterData filterDataData, string tableName)
        {
            var stringFilterPredicates = new List<string>();
             
             //ToString("yyyy-MM-dd HH:mm:ss.fff")
            //date
            if (filterDataData.FromDateTime.HasValue)
            {
                stringFilterPredicates.Add($"CAST({tableName}.HappensDate AS DATE) >= CAST('{GetMsSqlTimeString((DateTime)filterDataData.FromDateTime)}' AS DATETIME)");
            }

            if (filterDataData.ToDateTime.HasValue)
            {
                stringFilterPredicates.Add($"CAST({tableName}.HappensDate AS DATE) <= CAST('{GetMsSqlTimeString((DateTime)filterDataData.ToDateTime)}' AS DATETIME)");
            }

            //scale
            if (filterDataData.ScaleLowerLimit.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.scale >= {filterDataData.ScaleLowerLimit}");
            }

            if (filterDataData.ScaleUpperLimit.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.scale <= {filterDataData.ScaleUpperLimit}");
            }

            //rating
            if (filterDataData.LowerLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.rating >= {filterDataData.LowerLimitRating}");
            }

            if (filterDataData.UpperLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.rating <= {filterDataData.UpperLimitRating}");
            }

            //comment (by substring) 
            if (!string.IsNullOrEmpty(filterDataData.SubstringForMatching))
            {
                stringFilterPredicates.Add($"{tableName}.comment LIKE '%{filterDataData.SubstringForMatching}%'"); //possible sql injection? direct string import 
            }

            //geotag
            if (filterDataData.GpsLatLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.latitudeGeo >= {filterDataData.GpsLatLeftCorner}");
            }

            if (filterDataData.GpsLatRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.latitudeGeo <= {filterDataData.GpsLatRightCorner}");
            }

            if (filterDataData.GpsLngLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.longitudeGeo >= {filterDataData.GpsLngLeftCorner}");
            }

            if (filterDataData.GpsLngRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.longitudeGeo <= {filterDataData.GpsLngRightCorner}");
            }

            return stringFilterPredicates;
        }

        private static string GetMsSqlTimeString(DateTime time)
        {
            return time.ToString(MssqlTimeFormat);
        }
    }
}