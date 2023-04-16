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

        private const string requestObjectsEndpoint = "objects";
        private const string requestRelationshipsEndpoint = "relationships";

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public HttpService( HttpClient httpClient,
                            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the objects via HTTP request.
        /// </summary>
        /// <returns></returns>
        public async Task<GeneralObjectDTO[]> GeneralObjectsHttpRequest()
        {
            return await httpClient.GetFromJsonAsync<GeneralObjectDTO[]>(GetApiEndpoint(requestObjectsEndpoint));
        }

        /// <summary>
        /// Gets the Relationships via HTTP request.
        /// </summary>
        /// <returns></returns>
        public async Task<RelationshipDTO[]> RelationshipsHttpRequest()
        {
            return await httpClient.GetFromJsonAsync<RelationshipDTO[]>(GetApiEndpoint(requestRelationshipsEndpoint));
        }

        private string GetApiEndpoint(string relativePath)
        {
            return configuration[ApiRootUriConfigurationKey] + relativePath;
        }
    }
}
