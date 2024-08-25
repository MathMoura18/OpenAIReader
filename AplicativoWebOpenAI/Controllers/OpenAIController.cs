using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OpenAI_API.Moderation;
using System.Text;
using System.Configuration;
using OpenAI.Threads;

namespace AplicativoWebOpenAI.Controllers
{
    public class OpenAIController : Controller
    {
        private static string pdfText;

        public OpenAIController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Contact AI with a question
        /// </summary>
        /// <param name="text">Your question to AI</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Sucess</response>
        [HttpGet]
        [Route("GetAISentenceByFile")]
        public async Task<JsonResult> GetAISentenceByFile(string question)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                string filePath = FileReaderService.GetFullFilePath();

                var result = await OpenAIService.GetAISentence(question, key, pdfText);

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
        public async Task<IActionResult> PostUserFile(List<IFormFile> file)
        {
            try
            {
                FileReaderService.UploadFile(file[0]);

                pdfText = FileReaderService.ReadFile();

                return Ok();
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
