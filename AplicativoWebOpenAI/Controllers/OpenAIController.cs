using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<JsonResult> GetSentence(string text)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");

                var promptResponse = await OpenAIService.GetSentence(text, key);
                var result = promptResponse.message.content;

                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
