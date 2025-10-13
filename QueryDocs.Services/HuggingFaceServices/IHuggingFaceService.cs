using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDocs.Services.HuggingFaceServices
{
    public interface IHuggingFaceService
    {
        Task<float[]> CreateEmbeddingsFromHuggingFace(string text);
    }
}
