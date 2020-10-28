using System;
using LanguageExt;

namespace ItHappened.Api.Models.Responses
{
    public class TrackerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CustomizationSettingsResponse CustomizationSettings { get; set; }
    }
}