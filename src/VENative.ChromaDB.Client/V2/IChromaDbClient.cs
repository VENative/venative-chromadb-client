using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VENative.ChromaDB.Client.V2;

public interface IChromaDbClient
{
    Task<string> HealthCheckAsync(CancellationToken cancellationToken = default);
    Task<HeartbeatResponse> HeartbeatAsync(CancellationToken cancellationToken = default);
    Task<ChecklistResponse> PreFlightChecksAsync(CancellationToken cancellationToken = default);
    Task<bool> ResetAsync(CancellationToken cancellationToken = default);
    Task<string> VersionAsync(CancellationToken cancellationToken = default);
    Task<GetUserIdentityResponse> GetUserIdentityAsync(CancellationToken cancellationToken = default);

    Task<CreateTenantResponse> CreateTenantAsync(CreateTenantPayload payload, CancellationToken cancellationToken = default);
    Task<GetTenantResponse> GetTenantAsync(string tenantName, CancellationToken cancellationToken = default);

    Task<IEnumerable<Database>> ListDatabasesAsync(string tenant, int? limit = null, int? offset = null, CancellationToken cancellationToken = default);
    Task<CreateDatabaseResponse> CreateDatabaseAsync(string tenant, CreateDatabasePayload payload, CancellationToken cancellationToken = default);
    Task<Database> GetDatabaseAsync(string tenant, string database, CancellationToken cancellationToken = default);
    Task<DeleteDatabaseResponse> DeleteDatabaseAsync(string tenant, string database, CancellationToken cancellationToken = default);
    Task<UpdateCollectionResponse> DeleteCollectionAsync(string collectionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Collection>> ListCollectionsAsync(string tenant, string database, int? limit = null, int? offset = null, CancellationToken cancellationToken = default);
    Task<int> CountCollectionsAsync(string tenant, string database, CancellationToken cancellationToken = default);
    Task<ICollectionClient> CreateCollectionAsync(string tenant, string database, CreateCollectionPayload payload, CancellationToken cancellationToken = default);
    Task<ICollectionClient> GetCollectionAsync(string tenant, string database, string collectionId, CancellationToken cancellationToken = default);
    Task<ICollectionClient> GetCollectionByIdAsync(string collectionId, CancellationToken cancellationToken = default);
    Task<ICollectionClient> GetCollectionByNameAsync(string name, CancellationToken cancellationToken = default);
}