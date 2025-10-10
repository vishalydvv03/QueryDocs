using RAGChatBot.Domain.Models;
using RAGChatBot.Infrastructure.ResponseHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGChatBot.Services.PineconeServices
{
    public interface IPineconeService
    {
        Task UpsertEmbeddingsAsync(List<EmbeddingChunk> embeddingChunks, string fileName);
        Task<ServiceResult> GenerateAnswer(QueryRequest query);
    }
}
