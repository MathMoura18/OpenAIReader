using AplicativoWebOpenAI.Models;
using System.Text.Json;
using System.Text;
using Aspose.Pdf.Operators;

namespace AplicativoWebOpenAI.Services
{
    public class GeminiService
    {
        public async static Task<List<Content>> GetAISentence(string question, string key, string documentAsString, string chatHistory)
        {
            if (String.IsNullOrEmpty(question))
                throw new ArgumentNullException("The user input is null");

            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("Gemini API key is null");

            if (String.IsNullOrEmpty(documentAsString))
                throw new ArgumentNullException("Document as String is null");

            try
            {
                List<Content> contents = new List<Content>();

                if (!String.IsNullOrEmpty(chatHistory))
                {
                    contents = JsonSerializer.Deserialize<List<Content>>(chatHistory);
                }

                List<Part> parts = new List<Part>();
                parts.Add(new Part(question));
                contents.Add(new Content("user", parts));

                var requestBody = new
                {
                    system_instruction = new
                    {
                        parts = new[]
                        {
                            new { text = $"You are a PDF Reader and answer questions about documents. After give your final answer, also write exactly from which part you took it to answer the question, write like this 'Source: (part from document)'. This is the user document converted to String: {documentAsString}"}
                        }
                    },
                    contents
                };

                var json = JsonSerializer.Serialize(requestBody);

                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={key}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        using var jsonDoc = JsonDocument.Parse(responseString);
                        string assistantReply = jsonDoc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

                        if (assistantReply.Contains("Source:"))
                        {
                            int index = assistantReply.IndexOf("Source:");
                            string answerSource = assistantReply.Substring(index);
                            assistantReply = assistantReply.Remove(index).Trim();
                        }

                        // Adiciona a resposta ao histórico
                        List<Part> modelParts = new List<Part>();
                        modelParts.Add(new Part(assistantReply));
                        contents.Add(new Content("model", modelParts));

                        return contents;
                    }
                    else
                        throw new Exception($"Erro ao acessar a API: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calling Gemini API: {ex}");
            }
        }
    }
}
