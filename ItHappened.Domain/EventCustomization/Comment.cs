using System;

namespace ItHappened.Domain
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }

        public Comment(string text)
        {
            Text = text;
        }
    }
}