
namespace QueryDocs.Domain.Models
{
    public class EmbeddingChunk
    {
        public float[] Vector { get; set; }
        public string ChunkText { get; set; }

        public EmbeddingChunk(float[] vector, string chunkText)
        {
            Vector = vector;
            ChunkText = chunkText;
        }
    }
}
