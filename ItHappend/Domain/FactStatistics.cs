using System;

namespace ItHappend.Domain
{
    public abstract class FactStatistics //: IFactStatistics<FactStatistics>
    {
        protected FactStatistics(string description, double priority)
        {
            Description = description;
            Priority = priority;
        }

        public string Description { get; }
        public double Priority { get; }

        public abstract FactStatistics ApplicabilityFunction(EventTracker[] events);
        
    }
}