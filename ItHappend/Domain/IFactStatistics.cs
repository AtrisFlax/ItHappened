using System;

namespace ItHappend.Domain
{
    public interface IFactStatistics
    {
        string Description { get; }
        double Priority { get; }
        
        IFactStatistics ApplicabilityFunction(EventTracker[] events);
    }
}