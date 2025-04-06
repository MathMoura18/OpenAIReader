using System.Text;
using Path = System.IO.Path;
using OpenAI.Threads;
using AplicativoWebOpenAI.Models;
using OpenAI.Files;
using UglyToad.PdfPig;

namespace AplicativoWebOpenAI.Services
{
    public class FileReaderService
    {
        public static string ReadFile(FileModel model)
        {
            try
            {                
				//using (PdfReader reader = new PdfReader(model.filePath))
				//{
				//    pdfText += "Page 1: \n";
				//    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
				//    {
				//        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

				//        if (pagenumber > 1)
				//            pdfText += $".\n Page {pagenumber}: \n";

				//        pdfText += PdfTextExtractor.GetTextFromPage(reader, pagenumber, strategy);
				//        pdfText += Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(pdfText)));
				//    }
				//}

				var textoCompleto = new StringBuilder();

				using (var pdf = PdfDocument.Open(model.filePath))
				{
					textoCompleto.AppendLine($"Page 1: \n");

					int countPage = 2;

					foreach (var pagina in pdf.GetPages())
					{
						textoCompleto.AppendLine($".\n Page {countPage}: \n");
						textoCompleto.AppendLine(pagina.Text); // Processa uma página por vez.
						countPage++;
					}
				}

                DeleteFile(model.filePath);

                return textoCompleto.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in reading the file: {ex}");
            }
        }        
		
		public static string FileToBase64(IFormFile file)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                // Converte os bytes para Base64
                string base64String = Convert.ToBase64String(fileBytes);

				return base64String;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in FileToBase64: {ex}");
            }
        }

        public async static Task<FileModel> UploadFile(IFormFile postedFile)
        {
            if (postedFile == null && postedFile.Length > 0)
                throw new Exception("Error in UploadFile: File is null");

            try
            {
				FileModel fileModel = new FileModel();

				// Caminho onde o arquivo será salvo no servidor
				fileModel.filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", postedFile.FileName);

				// Certifique-se de que o diretório "uploads" existe
				Directory.CreateDirectory(Path.GetDirectoryName(fileModel.filePath));

				// Salvar o arquivo no servidor
				using (var stream = new FileStream(fileModel.filePath, FileMode.Create))
				{
					await postedFile.CopyToAsync(stream);
				}

				return fileModel;
			}
            catch (Exception ex) 
            {
                throw new Exception($"Error in UploadFile: {ex}");
            }
        }

		private static Boolean DeleteFile(string filePath)
		{
            try
            {
                if (filePath == null && filePath.Length > 0)
                    throw new Exception("Error in UploadFile: File path is null");

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception ex)  
            {
                throw ex;
            }
        }
    }
}
