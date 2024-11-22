using AplicativoWebOpenAI.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService
    {
        static List<Message> chatHistory = new List<Message>();
        public async static Task<string> GetAISentence(string question, string key, string documentAsString)
        {   
            if (String.IsNullOrEmpty(question))
                throw new ArgumentNullException("The user input is null");            
            
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("OpenAI API key is null");            
            
            if (String.IsNullOrEmpty(documentAsString))
                throw new ArgumentNullException("Document as String is null");
        
            try
            {                
                var result = new OpenAIViewModel();

                using (var httpClient = new HttpClient())
                {
                    // httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "sk-proj-bev-vMCDTANhCpFt1NUH9Y2ruktIfHzfiw79xq08-N2hiGWyWg9GWr8cuNWs"+"6Msq376J8AV742T3BlbkFJ6qW_kZkj_7cvWSMuqg94oyekPFCkHagGKxla9941xGOzjElmRgf1KiFI3k-PokOd4bbco9xyMA");
    
                    if (result.choices == null)
                        chatHistory.Add(new Message("system", $"You are a PDF Reader and answer questions about documents. After give your final answer, also write exactly from which part you took it to answer the question, write like this 'Source: (part from document)'. This is the user document converted to String: {documentAsString}"));

                    chatHistory.Add(new Message("user", question));

                    var model = new OpenAIInputModel(chatHistory);

                    var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                
                    result = await response.Content.ReadFromJsonAsync<OpenAIViewModel>();
    
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        using var jsonDoc = JsonDocument.Parse(responseString);
                        string assistantReply = jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                        // Adiciona a resposta ao histórico
                        chatHistory.Add(new Message("assistant", assistantReply));
                    }
                    else
                        throw new Exception($"Erro ao acessar a API: {response.StatusCode} - {response.ReasonPhrase}");
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
    }
}
