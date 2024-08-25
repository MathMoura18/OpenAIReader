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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using UglyToad.PdfPig.Graphics;
using static System.Net.Mime.MediaTypeNames;
using LangChain.Splitters.Text;

namespace AplicativoWebOpenAI.Services
{
    public class OpenAIService
    {
        public async static Task<string> GetSentenceFromUserFile(string AIKey, string question, string pdfText, string filePath)
        {
            try
            {
                var provider = new OpenAiProvider(AIKey);

                var llm = new OpenAiChatModel(provider, "gpt-4");
                var embeddingModel = new TextEmbeddingV3LargeModel(provider);

                var answer = await llm.GenerateAsync(
                $"""
                 Use the following pieces of context to answer the question at the end.
                 If the answer is not in context then just say that you don't know, don't try to make up an answer.

                 This is a file converted to String: {pdfText}

                 Question: {question}
                 Helpful Answer: I did not understand your question, please write it again.
                 """, cancellationToken: CancellationToken.None).ConfigureAwait(false);

                return answer;
            }
            catch (Exception ex)
            {
                return $"Error in calling AI API: {ex}";
            }
        }

        public async static Task<string> GetAISentence(string question, string key, string pdfText)
        {
            try
            {
                if (String.IsNullOrEmpty(pdfText))
                    return "Olá! Por favor, forneça o documento que você gostaria que eu lesse e sobre o qual você gostaria de fazer perguntas.";
                
                var result = new OpenAIViewModel();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

                    List<Message> listMessageModel = new List<Message>();

                    Message messageSystem = new Message();
                    messageSystem.role = "system";
                    messageSystem.content = $"You are a PDF Reader and answer questions about documents. This is a documents converted to String: {pdfText}";
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
                return promptResponse.message.content;
            }
            catch (Exception ex)
            {
                return $"Error in calling AI API: {ex}";
            }
        }
    }
}
