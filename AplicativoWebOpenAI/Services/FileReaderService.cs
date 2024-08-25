using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Text;
using Path = System.IO.Path;
using Aspose.Pdf.Operators;
using OpenAI.Threads;

namespace AplicativoWebOpenAI.Services
{
    public class FileReaderService
    {
        private static string fullFilePath;

        public static string GetFullFilePath()
        {
            return fullFilePath; 
        }
        
        public static string ReadFile()
        {
            try
            {
                string pdfText = "";

                string path = GetFullFilePath();

                using (PdfReader reader = new PdfReader(path))
                {
                    for (int pagenumber = 1; pagenumber <= reader.NumberOfPages; pagenumber++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        pdfText += $"Page {pagenumber}: ";
                        pdfText += PdfTextExtractor.GetTextFromPage(reader, pagenumber, strategy);
                        pdfText += Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(pdfText)));
                    }
                }
                return pdfText;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in reading the file: {ex}");
            }
        }

        public async static void UploadFile(IFormFile postedFile)
        {
            try
            {
                string filePath = "";
                filePath = Path.Combine("Files/");
                string fullPath = Path.GetFullPath(filePath);

                if (postedFile == null)
                {
                    throw new Exception("Error trying to upload the file: File is null");
                }

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(filePath);

                filePath += Path.GetFileName(postedFile.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }
                fullPath += Path.GetFileName(postedFile.FileName);
                fullFilePath = fullPath;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error uploading the file: {ex}");
            }
        }
    }
}
