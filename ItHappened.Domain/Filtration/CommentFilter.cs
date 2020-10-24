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

        public IEnumerable<Event> Filter(IEnumerable<Event> events)
        {
            return events.Where(@event => @event.CustomizationsParameters.Comment.IsSome)
                .Where(@event =>
                    Regex.IsMatch(@event.CustomizationsParameters.Comment.ValueUnsafe().Text, RegexPattern,
                        RegexOptions.IgnoreCase)).ToList();
        }
    }
}