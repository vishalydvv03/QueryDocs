using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using RAGChatBot.Domain.Models;
using RAGChatBot.Infrastructure.ResponseHelpers;
using RAGChatBot.Services.OpenAIServices;
using RAGChatBot.Services.PineconeServices;
using System.Text;
using UglyToad.PdfPig;

namespace RAGChatBot.Services.DocumentServices
{
    public class DocumentService : IDocumentService
    {
      
        private readonly IPineconeService pineconeService;
        private readonly IOpenAIService openAiService;
        public DocumentService(IPineconeService pineconeService, IOpenAIService openAiService)
        { 
            this.pineconeService = pineconeService;
            this.openAiService = openAiService; 
        }

        public async Task<ServiceResult> ProcessDocument(IFormFile file)
        {
            var result = new ServiceResult();
            var text = await ExtractText(file);
            if (string.IsNullOrWhiteSpace(text))
            {
                result.SetBadRequest("File content is empty or file not supported");
            }
            else
            {
                var chunks = ChunkText(text);
                if (chunks.Count == 0)
                {
                    result.SetFailure("Could not split into chunks");
                }
                else
                {
                    var embeddingChunks = new List<EmbeddingChunk>();

                    foreach (var chunk in chunks)
                    {
                        var vector = await openAiService.CreateEmbeddings(chunk);
                        embeddingChunks.Add(new EmbeddingChunk(vector, chunk));
                    }

                    if (embeddingChunks == null || embeddingChunks.Count == 0)
                    {
                        result.SetFailure("OpenAI API returned no embeddings.");
                    }
                    else
                    {
                        await pineconeService.UpsertEmbeddingsAsync(embeddingChunks, file.FileName);

                        result.SetSuccess($"Stored {embeddingChunks.Count} embeddings in Pinecone for {file.FileName}");
                    }
                }
            }
            return result;
        }

        private async Task<string?> ExtractText(IFormFile file)
        {
            string? text = null;
            string extension = Path.GetExtension(file.FileName).ToLower();

            switch (extension)
            {
                case ".txt":
                    {
                        using var reader = new StreamReader(file.OpenReadStream());
                        text = (await reader.ReadToEndAsync()).Trim();
                        break;
                    }

                case ".pdf":
                    {
                        var sb = new StringBuilder();
                        using (var pdf = PdfDocument.Open(file.OpenReadStream()))
                        {
                            foreach (var page in pdf.GetPages())
                                sb.AppendLine(page.Text);
                        }

                        text = sb.ToString().Trim();
                        break;
                    }

                case ".docx":
                    {
                        using var doc = WordprocessingDocument.Open(file.OpenReadStream(), false);
                        text = doc.MainDocumentPart?.Document?.Body?.InnerText.Trim();
                        break;
                    }

                default:
                    text = null;
                    break;
            }

            return text;
        }

        private List<string> ChunkText(string text, int chunkSize = 1000, int overlap = 200)
        {
            var chunks = new List<string>();

            int start = 0;
            while (start < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - start);
                string chunk = text.Substring(start, length);
                chunks.Add(chunk);
                start += chunkSize - overlap;
            }

            return chunks;
        }
        
    }

}
