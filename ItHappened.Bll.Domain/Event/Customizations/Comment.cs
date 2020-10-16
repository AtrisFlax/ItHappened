namespace ItHappened.Bll.Domain.Customizations
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