using AplicativoWebOpenAI.Models;
using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Moderation;
using System.Configuration;

namespace AplicativoWebOpenAI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostUserFile(List<IFormFile> file)
        {
            if(file.Count == 0)
            {
                throw new ArgumentNullException("File is null");
            }

            try
            {
                FileModel fileModel = await FileReaderService.UploadFile(file[0]);
                fileModel.fileText = FileReaderService.ReadFile(fileModel);
                fileModel.fileBase64 = FileReaderService.FileToBase64(file[0]);

                return Json(fileModel.fileText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAISentence(string question, string documentText, string chatHistory)
        {
            if (String.IsNullOrEmpty(question))
                throw new ArgumentNullException("Question is null.");

            try
            {
                string key = Configuration.GetValue<string>("Gemini:Key");

                var result = await GeminiService.GetAISentence(question, key, documentText, chatHistory);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
