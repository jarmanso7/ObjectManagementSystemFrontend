using ObjectManagementSystemFrontend.DTOs;
using System.Net.Http.Json;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Provides interaction with external agents via Http
    /// </summary>
    public class HttpService
    {
        private const string ApiRootUriConfigurationKey = "ApiRootUri";

        private const string objectsEndpoint = "objects";
        private const string relationshipsEndpoint = "relationships";

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public HttpService( HttpClient httpClient,
                            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the Objects via HTTP request.
        /// </summary>
        /// <returns></returns>
        public async Task<GeneralObjectDTO[]> GeneralObjectsHttpRequest()
        {
            return await httpClient.GetFromJsonAsync<GeneralObjectDTO[]>(GetApiEndpoint(objectsEndpoint));
        }

        /// <summary>
        /// Gets the Relationships via HTTP request.
        /// </summary>
        /// <returns></returns>
        public async Task<RelationshipDTO[]> RelationshipsHttpRequest()
        {
            return await httpClient.GetFromJsonAsync<RelationshipDTO[]>(GetApiEndpoint(relationshipsEndpoint));
        }

        /// <summary>
        /// Sends out a post request to create a new general object.
        /// </summary>
        /// <param name="generalObjectDTO">The general object dto.</param>
        public async Task CreateGeneralObjectRequest(GeneralObjectDTO generalObjectDTO)
        {
            await httpClient.PostAsJsonAsync(GetApiEndpoint(objectsEndpoint), generalObjectDTO);
        }

        /// <summary>
        /// Sends out a post request to create a new relationship.
        /// </summary>
        /// <param name="RelationshipDTO">The relationship dto.</param>
        public async Task CreateRelationshiptRequest(RelationshipDTO relationshipDTO)
        {
            await httpClient.PostAsJsonAsync(GetApiEndpoint(relationshipsEndpoint), relationshipDTO);
        }

        /// <summary>
        /// Sends out a delete request to delete a general object.
        /// </summary>
        /// <param name="generalObjectDTO">The general object dto.</param>
        public async Task DeleteGeneralObjectRequest(GeneralObjectDTO generalObjectDTO)
        {
            var deleteUri = GetApiEndpoint(objectsEndpoint) + "/" + generalObjectDTO.Id;

            await httpClient.DeleteAsync(deleteUri);
        }

        /// <summary>
        /// Sends out a delete request to delete a new relationship.
        /// </summary>
        /// <param name="RelationshipDTO">The relationship dto.</param>
        public async Task DeleteRelationshiptRequest(RelationshipDTO relationshipDTO)
        {
            var deleteUri = GetApiEndpoint(relationshipsEndpoint) + "/" + relationshipDTO.Id;

            await httpClient.DeleteAsync(deleteUri);
        }

        /// <summary>
        /// Sends out a put request to update a general object.
        /// </summary>
        /// <param name="generalObjectDTO">The general object dto.</param>
        public async Task UpdateGeneralObjectRequest(GeneralObjectDTO generalObjectDTO)
        {
            await httpClient.PutAsJsonAsync(GetApiEndpoint(objectsEndpoint), generalObjectDTO);
        }

        /// <summary>
        /// Sends out a put request to update a new relationship.
        /// </summary>
        /// <param name="RelationshipDTO">The relationship dto.</param>
        public async Task UpdateRelationshiptRequest(RelationshipDTO relationshipDTO)
        {
            await httpClient.PutAsJsonAsync(GetApiEndpoint(relationshipsEndpoint), relationshipDTO);
        }

        private string GetApiEndpoint(string relativePath)
        {
            return configuration[ApiRootUriConfigurationKey] + relativePath;
        }
    }
}
