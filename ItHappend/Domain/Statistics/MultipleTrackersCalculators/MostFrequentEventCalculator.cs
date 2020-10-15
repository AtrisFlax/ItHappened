﻿using System;
using System.Collections.Generic;

namespace ItHappend.Domain.Calculators
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEvent>
    {
        public MostFrequentEvent Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            throw new NotImplementedException();
        }
    }
}