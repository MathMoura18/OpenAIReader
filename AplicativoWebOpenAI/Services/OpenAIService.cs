using AplicativoWebOpenAI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI_API.Models;
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using LangChain;
using LangChain.DocumentLoaders;
using LangChain.Providers.OpenAI.Predefined;
using LangChain.Providers.OpenAI;
using LangChain.Databases.Sqlite;
using LangChain.Extensions;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService
    {
        public async static Task<string> GetAISentence(string text, string key)
        {
            try
            {
                var result = new OpenAIViewModel();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

                    List<Message> listMessageModel = new List<Message>();

                    Message messageSystem = new Message();
                    messageSystem.role = "system";
                    messageSystem.content = "You are a helpful assistant.";
                    listMessageModel.Add(messageSystem);

                    Message messageUser = new Message();
                    messageUser.role = "user";
                    messageUser.content = $"{text}";
                    listMessageModel.Add(messageUser);

                    var model = new OpenAIInputModel(listMessageModel);

                    var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    result = await response.Content.ReadFromJsonAsync<OpenAIViewModel>();
                }

                var promptResponse = result.choices.First();
                return promptResponse.message.content;
            }
            catch (Exception ex)
            {
                return $"Error in calling AI API: {ex}";
            }
        }

        public async static Task<string> GetSentenceFromUserFile(string AIKey, IFormFile userFile, string question)
        {
            var provider = new OpenAiProvider(AIKey);

            var llm = new OpenAiChatModel(provider, "gpt-4");
            var embeddingModel = new TextEmbeddingV3SmallModel(provider);

            using var vectorDatabase = new SqLiteVectorDatabase(dataSource: "vectors.db");
            var vectorCollection = await vectorDatabase.AddDocumentsFromAsync<PdfPigPdfLoader>(
                embeddingModel, // Used to convert text to embeddings
                dimensions: 1536, // Should be 1536 for TextEmbeddingV3SmallModel
                dataSource: DataSource.FromUrl($"{userFile}"),
                textSplitter: null); // Default is CharacterTextSplitter(ChunkSize = 4000, ChunkOverlap = 200)

            var answer = await llm.GenerateAsync(
            $"""
             Use the following pieces of context to answer the question at the end.
             If the answer is not in context then just say that you don't know, don't try to make up an answer.
             Question: {question}
             Helpful Answer:
             """, cancellationToken: CancellationToken.None).ConfigureAwait(false);

            return answer;
        }
    }
}
