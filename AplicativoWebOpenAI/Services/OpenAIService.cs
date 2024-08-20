using AplicativoWebOpenAI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI_API.Models;
using System.Text;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService
    {
        public async static Task<Choice> GetSentence(string text, string key)
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

            return result.choices.First();
        }
    }
}
