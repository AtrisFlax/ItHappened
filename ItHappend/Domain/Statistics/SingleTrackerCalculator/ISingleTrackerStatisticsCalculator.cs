﻿using System.Collections.Generic;
 using ItHappend.Domain.StatisticsFacts;
 using LanguageExt;

 namespace ItHappend.Domain.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator<TFact>
    {
        Option<LongestBreak> Calculate(EventTracker eventTracker);
    }
}