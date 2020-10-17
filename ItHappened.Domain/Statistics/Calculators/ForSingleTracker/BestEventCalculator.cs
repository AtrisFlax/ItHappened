﻿using System;
using System.Linq;
using ItHappend.Domain.Statistics.StatisticsFacts;
using ItHappened.Domain.EventCustomization;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public class BestEventCalculator
    {
        public Option<BestEventFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<BestEventFact>.None;
            const string factName = "Лучшее событие";
            var bestEvent = eventTracker.Events.OrderBy(eventItem => eventItem.Rating).Last();
            var priority = bestEvent.Rating.Value();
            var bestEventComment = bestEvent.Comment.Match(
                Some: comment => comment,
                None: () => Option<Comment>.None);
            var description = $"Событие {eventTracker.Name} с самым высоким рейтингом " +
                                 $"{bestEvent.Rating} произошло {bestEvent.HappensDate}";
            description += bestEventComment.IsSome ?
                $" с комментарием {bestEventComment.ValueUnsafe().Text}" :
                " (комментарий отсутсвует)";
            
            return Option<BestEventFact>.Some(new BestEventFact(
                factName,
                description,
                priority,
                bestEvent.Rating.Value(),
                bestEvent.HappensDate,
                bestEventComment,
                bestEvent));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = eventTracker.Events
                .Count(eventItem => eventItem.Rating.IsSome) >= 10;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = eventTracker.Events
                                                                  .OrderBy(eventItem => eventItem.HappensDate)
                                                                  .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(90);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = eventTracker.Events
                                                                     .OrderBy(eventItem => eventItem.Rating)
                                                                     .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen && 
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}