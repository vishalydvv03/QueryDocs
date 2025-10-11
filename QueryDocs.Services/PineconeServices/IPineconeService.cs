using QueryDocs.Domain.Models;
using QueryDocs.Infrastructure.ResponseHelpers;

namespace QueryDocs.Services.PineconeServices
{
    public interface IPineconeService
    {
        Task UpsertEmbeddingsAsync(List<EmbeddingChunk> embeddingChunks, string fileName);
        Task<ServiceResult> GenerateAnswer(QueryRequest query);
    }
}
