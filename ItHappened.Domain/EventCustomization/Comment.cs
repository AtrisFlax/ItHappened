namespace ItHappened.Domain
{
    public class Comment
    {
        public string Text { get; }

        public Comment(string text)
        {
            Text = text;
        }
    }
}