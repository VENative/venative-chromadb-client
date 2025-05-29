using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client.V2;

public class CollectionClient : ICollectionClient
{
    private readonly ChromaDbClient _chromaClient;
    private readonly string _tenant;
    private readonly string _database;
    private readonly string _collectionId;

    public CollectionClient(ChromaDbClient chromaClient, string tenant, string database, string collectionId)
    {
        _chromaClient = chromaClient;
        _tenant = tenant;
        _database = database;
        _collectionId = collectionId;
    }

    public Task<AddCollectionRecordsResponse> AddAsync(AddCollectionRecordsPayload payload,
                                                       CancellationToken cancellationToken = default)
        => _chromaClient.AddRecordsInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => _chromaClient.CountRecordsInternalAsync(_tenant, _database, _collectionId, cancellationToken);

    public Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(DeleteCollectionRecordsPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.DeleteRecordsInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<UpdateCollectionResponse> DeleteAsync(CancellationToken cancellationToken = default)
        => _chromaClient.DeleteCollectionInternalAsync(_tenant, _database, _collectionId, cancellationToken);

    public Task<Collection> ForkAsync(ForkCollectionPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.ForkCollectionInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<GetResponse> GetAsync(GetRequestPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.GetRecordsInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<Collection> GetDetailsAsync(CancellationToken cancellationToken = default)
        => _chromaClient.GetCollectionInternalAsync(_tenant, _database, _collectionId, cancellationToken);

    public Task<QueryResponse> QueryAsync(QueryRequestPayload payload, int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
        => _chromaClient.QueryRecordsInternalAsync(_tenant, _database, _collectionId, payload, limit, offset, cancellationToken);

    public Task<UpdateCollectionResponse> UpdateAsync(UpdateCollectionPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.UpdateCollectionInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(UpdateCollectionRecordsPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.UpdateRecordsInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<UpsertCollectionRecordsResponse> UpsertAsync(UpsertCollectionRecordsPayload payload, CancellationToken cancellationToken = default)
        => _chromaClient.UpsertRecordsInternalAsync(_tenant, _database, _collectionId, payload, cancellationToken);

    public Task<AddCollectionRecordsResponse> AddAsync(IEnumerable<string> ids, IEnumerable<IEnumerable<float>>? embeddings = null, IEnumerable<IDictionary<string, object>?>? metadatas = null, IEnumerable<string?>? documents = null, CancellationToken cancellationToken = default)
    {
        return AddAsync(new AddCollectionRecordsPayload
        {
            Ids = ids,
            Documents = documents,
            Embeddings = embeddings,
            Metadatas = metadatas,
        }, cancellationToken);
    }

    public Task<DeleteCollectionRecordsResponse> DeleteRecordsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        return DeleteRecordsAsync(new DeleteCollectionRecordsPayload
        {
            Ids = ids,
        }, cancellationToken);
    }

    public Task<GetResponse> GetAsync(IEnumerable<string>? ids = null, IEnumerable<string>? include = null, int? limit = null, int? offset = null, CancellationToken cancellationToken = default)
    {
        return GetAsync(new GetRequestPayload
        {
            Ids = ids,
            Include = include,
            Limit = limit,
            Offset = offset,
        }, cancellationToken);
    }

    public Task<UpdateCollectionRecordsResponse> UpdateRecordsAsync(IEnumerable<string> ids, IEnumerable<IEnumerable<float>>? embeddings = null, IEnumerable<IDictionary<string, object>?>? metadatas = null, IEnumerable<string?>? documents = null, CancellationToken cancellationToken = default)
    {
        return UpdateRecordsAsync(new UpdateCollectionRecordsPayload
        {
            Ids = ids,
            Embeddings = embeddings,
            Metadatas = metadatas,
            Documents = documents,
        }, cancellationToken);
    }
}