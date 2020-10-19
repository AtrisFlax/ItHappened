namespace ItHappened.Domain
{
    public class Comment
    {
        public Comment(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}