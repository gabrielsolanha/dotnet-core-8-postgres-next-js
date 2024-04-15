using System.Collections.Generic;

namespace AplicacaoWeb.Responses
{
    public class ErrorMessages
    {
        public ErrorMessages()
        {
            Messages = new List<string>();
        }
        public ErrorMessages(string message)
        {
            Messages = new List<string>();
            Messages.Add(message);
        }
        public List<string> Messages { get; set; }
        public List<string> Add(string message)
        {
            Messages.Add(message);
            return Messages;
        }
    }
}
