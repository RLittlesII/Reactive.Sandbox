namespace Dialog
{
    public class ConfirmationDetail
    {
        public ConfirmationDetail(string message, string title, string confirmMessage = "Ok", string declineMessage = "Cancel")
        {
            Message = message;
            Title = title;
            ConfirmMessage = confirmMessage;
            DeclineMessage = declineMessage;
        }

        public string Message { get; }

        public string Title { get; }
        
        public string ConfirmMessage { get; }
        
        public string DeclineMessage { get; }
    }
}