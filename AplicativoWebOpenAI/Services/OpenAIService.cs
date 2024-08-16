using AplicativoWebOpenAI.Models;
using Microsoft.Extensions.Options;
using OpenAI_API.Models;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAIModel _openAiConfig;

        public OpenAIService(IOptionsMonitor<OpenAIModel> optionsMonitor) 
        {
            _openAiConfig = optionsMonitor.CurrentValue;
        }

        public async Task<string> CompleteSentence (string text)
        {
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);
            var result = await api.Completions.GetCompletion(text);
            return result;
        }

        public async Task<string> CompleteSentenceAdvance(string text)
        {
            var api = new OpenAI_API.OpenAIAPI(_openAiConfig.Key);

            var result = await api.Completions.CreateCompletionAsync(new OpenAI_API.Completions.CompletionRequest(text, model: Model.GPT4, temperature: 0.1));

            return result.Completions[0].Text;
        }
    }
}
