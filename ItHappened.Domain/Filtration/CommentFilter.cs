using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain
{
    public class CommentFilter : IEventsFilter
    {
        public string Name { get; }
        public string SearchSubstring { get; }

        public CommentFilter(string name, string searchSubstring)
        {
            Name = name;
            SearchSubstring = searchSubstring;
        }
        
        public IEnumerable<Event> Filter(IEnumerable<Event> events)
        {
            return events
                .Where(@event => @event.CustomizationsParameters.Comment.IsSome)
                .Where(@event => @event.CustomizationsParameters.Comment.ValueUnsafe().Text
                    .Contains(SearchSubstring, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }
}