using AplicativoWebOpenAI.Models;
using AplicativoWebOpenAI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AplicativoWebOpenAI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("PostUserFile")]
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

                return RedirectToAction("Index", "DocumentChat", fileModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
