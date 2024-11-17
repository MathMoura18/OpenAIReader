using AplicativoWebOpenAI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService
    {
        static List<Message> listMessageModel = new List<Message>();
        
        public async static Task<string> GetAISentence(string question, string key, string documentAsString)
        {           
            try
            {                
                var result = new OpenAIViewModel();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

                    Message messageSystem = new Message();
                    messageSystem.role = "system";
                    messageSystem.content = $"You are a PDF Reader and answer questions about documents. After give your final answer, also write exactly from which part you took it to answer the question, write like this 'Source: (part from document)'. This is the user document converted to String: {documentAsString}";
                    listMessageModel.Add(messageSystem);

                    Message messageUser = new Message();
                    messageUser.role = "user";
                    messageUser.content = question;

                    listMessageModel.Add(messageUser);

                    var model = new OpenAIInputModel(listMessageModel);

                    var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                
                    result = await response.Content.ReadFromJsonAsync<OpenAIViewModel>();
                }

                var promptResponse = result.choices.First();

                string finalresponse = promptResponse.message.content;

                if (finalresponse.Contains("Source:")){
                    int index = finalresponse.IndexOf("Source:");
                    string answerSource = finalresponse.Substring(index);
                    finalresponse = finalresponse.Remove(index).Trim();
                }                

                return finalresponse;
            }
            catch (Exception ex)
            {
                return $"Error calling OpenAI API: {ex}";
            }
        }

        //public async static Task<string> GetSentenceFromUserFile(string AIKey, string question, string pdfText, string filePath)
        //{
        //    try
        //    {
        //        var provider = new OpenAiProvider(AIKey);

        //        var llm = new OpenAiChatModel(provider, "gpt-4");
        //        var embeddingModel = new TextEmbeddingV3LargeModel(provider);

        //        var answer = await llm.GenerateAsync(
        //        $"""
        //         Use the following pieces of context to answer the question at the end.
        //         If the answer is not in context then just say that you don't know, don't try to make up an answer.

        //         This is a file converted to String: {pdfText}

        //         Question: {question}
        //         Helpful Answer: I did not understand your question, please write it again.
        //         """, cancellationToken: CancellationToken.None).ConfigureAwait(false);

        //        return answer;
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Error in calling AI API: {ex}";
        //    }
        //}
    }
}
