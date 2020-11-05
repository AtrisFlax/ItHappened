using System;
using System.Collections.Generic;
using ItHappened.Domain;
using Microsoft.EntityFrameworkCore.Internal;

namespace ItHappened.Infrastructure
{
    public class MssqlEventsFilter : IMssqlFilter
    {
        private const string MssqlTimeFormat = "yyyyMMdd HH:mm:ss.fff";
        public string CreateFilterMsSqlPredicates(EventFilter filterData, string tableName)
        {
            var stringPredicates = CreateStringPredicates(filterData, tableName);
            var generalPredicate = !stringPredicates.Any() ? string.Empty : string.Join(" and ", stringPredicates);
            return generalPredicate;
        }

        private static List<string> CreateStringPredicates(EventFilter filterData, string tableName)
        {
            var stringFilterPredicates = new List<string>();
             
             //ToString("yyyy-MM-dd HH:mm:ss.fff")
            //date
            if (filterData.FromDateTime.HasValue)
            {
                stringFilterPredicates.Add($"CAST({tableName}.HappensDate AS DATE) >= CAST('{GetMsSqlTimeString((DateTime)filterData.FromDateTime)}' AS DATETIME)");
            }

            if (filterData.ToDateTime.HasValue)
            {
                stringFilterPredicates.Add($"CAST({tableName}.HappensDate AS DATE) <= CAST('{GetMsSqlTimeString((DateTime)filterData.ToDateTime)}' AS DATETIME)");
            }

            //scale
            if (filterData.ScaleLowerLimit.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.scale >= {filterData.ScaleLowerLimit}");
            }

            if (filterData.ScaleUpperLimit.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.scale <= {filterData.ScaleUpperLimit}");
            }

            //rating
            if (filterData.LowerLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.rating >= {filterData.LowerLimitRating}");
            }

            if (filterData.UpperLimitRating.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.rating <= {filterData.UpperLimitRating}");
            }

            //comment (by substring) 
            if (!string.IsNullOrEmpty(filterData.SubstringForMatching))
            {
                stringFilterPredicates.Add($"{tableName}.comment LIKE '%{filterData.SubstringForMatching}%'"); //possible sql injection? direct string import 
            }

            //geotag
            if (filterData.GpsLatLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.latitudeGeo >= {filterData.GpsLatLeftCorner}");
            }

            if (filterData.GpsLatRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.latitudeGeo <= {filterData.GpsLatRightCorner}");
            }

            if (filterData.GpsLngLeftCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.longitudeGeo >= {filterData.GpsLngLeftCorner}");
            }

            if (filterData.GpsLngRightCorner.HasValue)
            {
                stringFilterPredicates.Add($"{tableName}.longitudeGeo <= {filterData.GpsLngRightCorner}");
            }

            return stringFilterPredicates;
        }

        private static string GetMsSqlTimeString(DateTime time)
        {
            return time.ToString(MssqlTimeFormat);
        }
    }
}