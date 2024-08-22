using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Moderation;
using System.Text;

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
    }
}
