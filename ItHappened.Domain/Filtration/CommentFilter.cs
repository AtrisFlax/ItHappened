using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain
{
    public class CommentFilter : IEventsFilter
    {
        public string Name { get; }
        public string RegexPattern { get; }
        
        public CommentFilter(string name, string regexPattern)
        {
            Name = name;
            RegexPattern = regexPattern;
        }
        public IReadOnlyCollection<Event> Filter(IReadOnlyCollection<Event> events)
        {
            return events.Where(@event => @event.Comment.IsSome)
                .Where(@event =>
                    Regex.IsMatch(@event.Comment.ValueUnsafe().Text, RegexPattern, RegexOptions.IgnoreCase)).ToList();
        }
    }
}