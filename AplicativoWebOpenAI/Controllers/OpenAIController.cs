using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OpenAI_API.Moderation;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AplicativoWebOpenAI.Controllers
{
    public class OpenAIController : Controller
    {
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
        [Route("GetSentence")]
        public async Task<JsonResult> GetAISentence(string text)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var result = await OpenAIService.GetAISentence(text, key);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Contact AI with a question
        /// </summary>
        /// <param name="text">Your question to AI</param>
        /// <returns>
        /// </returns>
        /// <response code="200">Sucess</response>
        [HttpPost]
        [Route("PostUserFile")]
        public async Task<JsonResult> PostUserFile(IFormFile file, string question)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var result = await OpenAIService.GetSentenceFromUserFile(key, file, question);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
