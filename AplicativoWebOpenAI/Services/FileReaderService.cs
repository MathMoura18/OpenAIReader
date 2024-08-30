using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Text;
using Path = System.IO.Path;
using Aspose.Pdf.Operators;
using OpenAI.Threads;
using AplicativoWebOpenAI.Models;

namespace AplicativoWebOpenAI.Services
{
    public class FileReaderService
    {
        public static string ReadFile(FileModel model)
        {
            try
            {                
                string pdfText = "";

                using (PdfReader reader = new PdfReader(model.filePath))
                {
                    pdfText += "Page 1: \n";
                    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        if(pagenumber > 1)
                            pdfText += $".\n Page {pagenumber}: \n";
                        
                        pdfText += PdfTextExtractor.GetTextFromPage(reader, pagenumber, strategy);
                        pdfText += Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(pdfText)));
                    }
                }
                model.fileText = pdfText;

                return model.fileText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in reading the file: {ex}");
            }
        }

        public async static Task<FileModel> UploadFile(IFormFile postedFile)
        {
            if (postedFile == null)
                throw new Exception("Error in reading the file: File is null");

            try
            {
                FileModel model = new FileModel();

                model.filePath = Path.Combine("Files/");

                string fullPath = Path.GetFullPath(model.filePath);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(model.filePath);

                model.fileName = postedFile.FileName;
                model.filePath += Path.GetFileName(postedFile.FileName);

                using (Stream fileStream = new FileStream(model.filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }

                fullPath += Path.GetFileName(postedFile.FileName);
                model.filePath = fullPath;

                return model;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error uploading the file: {ex}");
            }
        }
    }
}
