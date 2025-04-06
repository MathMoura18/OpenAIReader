namespace AplicativoWebOpenAI.Models
{
    public class GeminiInputModel
    {
        public GeminiInputModel(List<Content> contents)
        {
            this.contents = contents;
        }

        public List<Content> contents { get; set; }
    }

    public class Content
    {
        public Content(string role, List<Part> parts)
        {
            this.role = role;
            this.parts = parts;
        }

        public string role { get; set; }
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        public Part(string text)
        {
            this.text = text;
        }

        public string text { get; set; }
    }
}
