namespace AplicacaoWeb.Models.Dtos.Responses
{
    public class ErrorMessages
    {
        public ErrorMessages(string message)
        {
            Messages = new List<string>
            {
                message
            };
        }
        public List<string> Messages { get; set; }
        public List<string> Add(string message)
        {
            Messages.Add(message);
            return Messages;
        }
    }
}
