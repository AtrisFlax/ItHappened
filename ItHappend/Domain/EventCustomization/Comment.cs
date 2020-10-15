namespace ItHappend.Domain.EventCustomization
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