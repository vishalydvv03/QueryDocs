using QueryDocs.Domain.Models;
using QueryDocs.Infrastructure.ResponseHelpers;

namespace QueryDocs.Services.PineconeServices
{
    public interface IPineconeService
    {
        Task UpsertEmbeddingsAsync(List<EmbeddingChunk> embeddingChunks, string fileName, int userId);
        Task<ServiceResult> GenerateAnswer(QueryRequest query, int userId);
    }
}
