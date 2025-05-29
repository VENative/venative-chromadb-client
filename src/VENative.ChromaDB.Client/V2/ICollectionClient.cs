using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client.V2;

public interface ICollectionClient
{
    Task<Collection> GetDetailsAsync(CancellationToken cancellationToken = default);
    Task<UpdateCollectionResponse> UpdateAsync(UpdateCollectionPayload payload, CancellationToken cancellationToken = default);
    Task<UpdateCollectionResponse> DeleteAsync(CancellationToken cancellationToken = default);
    Task<Collection> ForkAsync(ForkCollectionPayload payload, CancellationToken cancellationToken = default);
    Task<AddCollectionRecordsResponse> AddAsync(AddCollectionRecordsPayload payload, CancellationToken cancellationToken = default);
    Task<AddCollectionRecordsResponse> AddAsync(IEnumerable<string> ids, IEnumerable<IEnumerable<float>>? embeddings = null, IEnumerable<IDictionary<string, object>?>? metadatas = null, IEnumerable<string?>? documents = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(DeleteCollectionRecordsPayload payload, CancellationToken cancellationToken = default);
    Task<GetResponse> GetAsync(IEnumerable<string>? ids = null, IEnumerable<string>? include = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default);
    Task<GetResponse> GetAsync(GetRequestPayload payload, CancellationToken cancellationToken = default);
    Task<QueryResponse> QueryAsync(QueryRequestPayload payload, int? limit = null, int? offset = null, CancellationToken cancellationToken = default);
    Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(UpdateCollectionRecordsPayload payload, CancellationToken cancellationToken = default);
    Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(IEnumerable<string> ids, IEnumerable<IEnumerable<float>>? embeddings = null, IEnumerable<IDictionary<string, object>?>? metadatas = null, IEnumerable<string?>? documents = null, CancellationToken cancellationToken = default);
    Task<UpsertCollectionRecordsResponse> UpsertAsync(UpsertCollectionRecordsPayload payload, CancellationToken cancellationToken = default);
}