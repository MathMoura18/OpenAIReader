namespace AplicativoWebOpenAI.Models
{
    public class OpenAIInputModel
    {
        public OpenAIInputModel(List<Message> messages)
        {
            model = "gpt-4o";
            this.messages = messages;
            temperature = 0.2m;
            max_tokens = 500;
        }

        public string model { get; set; }
        public List<Message> messages { get; set; }
        public int max_tokens { get; set; }
        public decimal temperature { get; set; }
    }
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }

        public Message(string role, string content) 
        { 
            this.role = role;
            this.content = content;
        }
    }
}
