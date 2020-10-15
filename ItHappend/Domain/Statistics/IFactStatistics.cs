using System;

namespace ItHappend.Domain
{
    public interface IFactStatistics 
    {
        string Description { get; }
        double Priority { get; }
        public IFactStatistics CreateFactStatistics(EventTracker[] eventTrackers);
    }
}