using System;

namespace ItHappened.Api.Contracts.Responses.Trackers
{
    public class GetTrackerResponse
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool HasPhoto { get; }
        public bool HasScale { get; }
        public bool HasRating { get; }
        public bool HasGeoTag { get; }
        public bool HasComment { get; }
    }
}