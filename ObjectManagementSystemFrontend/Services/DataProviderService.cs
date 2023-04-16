using ObjectManagementSystemFrontend.Models;
using System.Net.Http.Json;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Provides the application data from an external source, in this case a web API.
    /// </summary>
    public class DataProviderService
    {
        private const string requestObjectsEndpoint = "objects";
        private const string requestRelationshipsEndpoint = "relationships";

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public DataProviderService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// Reads the data from the source.
        /// </summary>
        /// <returns></returns>
        public async Task<(List<GeneralObject>,List<Relationship>)> Read()
        {
            var fetchedObjects = await httpClient.GetFromJsonAsync<GeneralObject[]>(getApiEndpoint(requestObjectsEndpoint));
            var fetchedRelationships = await httpClient.GetFromJsonAsync<RelationshipDTO[]>(getApiEndpoint(requestRelationshipsEndpoint));

            List<GeneralObject> objects = new();
            List<Relationship> relationships = new();

            if (fetchedObjects != null)
            {
                objects.AddRange(fetchedObjects);
            }

            if (fetchedRelationships != null)
            {
                foreach (var relationshipDTO in fetchedRelationships)
                {
                    var relationship = new Relationship
                    {
                        Id = relationshipDTO.Id,
                        Type = relationshipDTO.Type,
                        From = objects.FirstOrDefault(o => o.Id == relationshipDTO.FromId),
                        To = objects.FirstOrDefault(o => o.Id == relationshipDTO.ToId)
                    };

                    relationships.Add(relationship);
                }
            }

            return (objects, relationships);
        }

        private string getApiEndpoint(string relativePath)
        {
            return configuration["ApiRootUri"] + relativePath;
        }
    }
}
