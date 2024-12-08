using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OpenAI_API.Moderation;
using System.Text;
using System.Configuration;
using OpenAI.Threads;
using AplicativoWebOpenAI.Models;
using Newtonsoft;
using System.Text.Json;

namespace AplicativoWebOpenAI.Controllers
{
    public class DocumentChatController : Controller
    {
        public DocumentChatController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IActionResult Index(FileModel fileModel)
        {
            return View(fileModel);
        }

        [HttpGet]
        [Route("GetAISentence")]
        public async Task<JsonResult> GetAISentence(string question, string documentText)
        {
            if (String.IsNullOrEmpty(question))
                throw new ArgumentNullException("Question is null.");

            try
            {             
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var result = await OpenAIService.GetAISentence(question, key, documentText);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
