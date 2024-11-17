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
    public class OpenAIController : Controller
    {
        public OpenAIController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IActionResult Index()
        {
            return View();
        }

        private static string documentAsString;

        [HttpGet]
        [Route("GetAISentence")]
        public async Task<JsonResult> GetAISentence(string question)
        {
            if (String.IsNullOrEmpty(question))
                throw new ArgumentNullException("Question is null.");

            if (String.IsNullOrEmpty(documentAsString))
                throw new ArgumentNullException("Was not possible to read the document.");

            try
            {             
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var result = await OpenAIService.GetAISentence(question, key, documentAsString);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("PostUserFile")]
        public async Task<JsonResult> PostUserFile(List<IFormFile> file)
        {
            try
            {
                var fileModel = await FileReaderService.UploadFile(file[0]);
                documentAsString = FileReaderService.ReadFile(fileModel);

                return Json(fileModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
