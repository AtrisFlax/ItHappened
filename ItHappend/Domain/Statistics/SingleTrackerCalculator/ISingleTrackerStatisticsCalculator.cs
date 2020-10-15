﻿using System.Collections.Generic;

namespace ItHappend.Domain.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator<TFact>
    {
        TFact Calculate(EventTracker eventTracker);
    }
}