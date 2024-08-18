using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost()]
        [Route("CompleteSentence")]
        public async Task<JsonResult> CompleteSentence(string text)
        {
            try
            {
                string key = Configuration.GetValue<string>("OpenAI:Key");
                var result = await OpenAIService.CompleteSentence(text, key);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
