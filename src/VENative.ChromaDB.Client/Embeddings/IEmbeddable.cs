using System.Collections.Generic;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client.Embeddings;

public interface IEmbeddable
{
    Task<IEnumerable<IEnumerable<float>>> Generate(IEnumerable<string> texts);
}
