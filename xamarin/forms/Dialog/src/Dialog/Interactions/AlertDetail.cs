namespace Dialog
{
    public class AlertDetail
    {

        public AlertDetail(string title, string message, string cancel = "Ok")
        {
            Title = title;
            Message = message;
            Cancel = cancel;
        }

        public string Title { get; }

        public string Message { get; }

        public string Cancel { get; }
    }
}