using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AplicativoWebOpenAI.Controllers
{
    public class OpenAIController : Controller
    {
        private readonly ILogger<OpenAIController> _Logger;
        private readonly IOpenAIService _openAIService;

        public OpenAIController(ILogger<OpenAIController> logger, IOpenAIService openAIService)
        {
            _Logger = logger;
            _openAIService = openAIService;
        }

        [HttpPost()]
        [Route("CompleteSentence")]
        public async Task<IActionResult> CompleteSentence(string text)
        {
            var result = await _openAIService.CompleteSentence(text);
            return Ok(result);
        }
    }
}
