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

        [HttpGet]
        [Route("GetAISentence")]
        public async Task<JsonResult> GetAISentence(string dados)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                FileModel file = JsonSerializer.Deserialize<FileModel>(dados);

                if(String.IsNullOrEmpty(file.fileText)) 
                    file.fileText = FileReaderService.ReadFile(file);

                var result = await OpenAIService.GetAISentence(file.question, key, file);

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
                var result = await FileReaderService.UploadFile(file[0]);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
