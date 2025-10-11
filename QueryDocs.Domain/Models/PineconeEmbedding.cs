
namespace QueryDocs.Domain.Models
{
    using Pinecone;

    public class PineconeEmbedding
    {
        public string VectorId { get; set; } = string.Empty;
        public float[] Vectors { get; set; } = Array.Empty<float>();
        public MetadataMap Metadata { get; set; } = new MetadataMap();

        public PineconeEmbedding(string vectorId, float[] vectors, string userId, string chunkText, string fileName)
        {
            VectorId = vectorId;
            Vectors = vectors;
            Metadata = new MetadataMap
            {
                { "UserId", userId },
                { "OriginalText", chunkText },
                { "fileName", fileName },
                { "UploadedAt", DateTime.UtcNow.ToString("o") }
            };
        }
    }
}
