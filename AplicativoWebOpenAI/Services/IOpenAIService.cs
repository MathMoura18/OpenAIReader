namespace AplicativoWebOpenAI.Services
{
    public interface IOpenAIService
    {
        Task<string> CompleteSentence(string text);
        Task<string> CompleteSentenceAdvance(string text);
    }
}
