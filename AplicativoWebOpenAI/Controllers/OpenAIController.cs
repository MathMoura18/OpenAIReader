using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OpenAI_API.Moderation;
using System.Text;
using System.Configuration;
using OpenAI.Threads;
using AplicativoWebOpenAI.Models;

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

        /// <summary>
        /// Contact AI with a question
        /// </summary>
        /// <param name="question">Your question to AI</param>
        /// <param name="pdfText">File Text</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Sucess</response>
        [HttpGet]
        [Route("GetAISentence")]
        public async Task<JsonResult> GetAISentence(string question, FileModel file)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var result = await OpenAIService.GetAISentence(question, key, file);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Download the user file
        /// </summary>
        /// <param name="file">User file</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Sucess</response>
        [HttpPost]
        [Route("PostUserFile")]
        public async Task<FileModel> PostUserFile(List<IFormFile> file)
        {
            try
            {
                var result = await FileReaderService.UploadFile(file[0]);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Contact AI with a question
        ///// </summary>
        ///// <param name="text">Your question to AI</param>
        ///// <returns>
        ///// </returns>
        ///// <response code="200">Sucess</response>
        //[HttpGet]
        //[Route("GetSentence")]
        //public async Task<JsonResult> GetAISentence(string text)
        //{
        //    try
        //    {
        //        string key = Configuration.GetValue<string>("OpenAI:Key");

        //        var result = await OpenAIService.GetAISentence(text, key);

        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
