using System;
using ItHappend.Domain;
using NUnit.Framework;

namespace ItHappend.UnitTests
{
    public class EventTrackerTests
    {
        [Test]
        public void CreateEmptyTracker()
        {
            //arrange
            
            var eventTracker = new EventTracker;

            //act

            //assert
        }

        private void EventTracker(bool emptyOrNot)
        {
            emptyOrNot ? return EventTracker{
                (Guid.NewGuid(), "Tracker Name", ) :
            }
        }
        
        
    }
}