using ObjectManagementSystemFrontend.Models;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Persists data to an external storage system.
    /// </summary>
    public class DataPersistenceService
    {
        private readonly HttpService httpService;
        private readonly MapperService mapperService;

        public DataPersistenceService(
            HttpService httpService,
            MapperService mapperService)
        {
            this.httpService = httpService;
            this.mapperService = mapperService;
        }

        /// <summary>
        /// Creates the general object.
        /// </summary>
        /// <param name="generalObject">The general object.</param>
        public async Task CreateGeneralObject(GeneralObject generalObject)
        {
            var generalObjectDTO = mapperService.Map(generalObject);

            await this.httpService.CreateGeneralObjectRequest(generalObjectDTO);
        }

        /// <summary>
        /// Creates the relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public async Task CreateRelationship(Relationship relationship)
        {
            var relationshipDTO = mapperService.Map(relationship);

            await this.httpService.CreateRelationshiptRequest(relationshipDTO);
        }

        /// <summary>
        /// Deletes the general object.
        /// </summary>
        /// <param name="generalObject">The general object.</param>
        /// <returns></returns>
        public async Task DeleteGeneralObject(GeneralObject generalObject)
        {
            var generalObjectDTO = mapperService.Map(generalObject);

            await this.httpService.DeleteGeneralObjectRequest(generalObjectDTO);
        }

        /// <summary>
        /// Deletes the relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public async Task DeleteRelationship(Relationship relationship)
        {
            var relationshipDTO = mapperService.Map(relationship);

            await this.httpService.DeleteRelationshiptRequest(relationshipDTO);
        }

        /// <summary>
        /// Updates the general object.
        /// </summary>
        /// <param name="generalObject">The general object.</param>
        /// <returns></returns>
        public async Task UpdateGeneralObject(GeneralObject generalObject)
        {
            var generalObjectDTO = mapperService.Map(generalObject);

            await this.httpService.UpdateGeneralObjectRequest(generalObjectDTO);
        }

        /// <summary>
        /// Updates the relationship.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        public async Task UpdateRelationship(Relationship relationship)
        {
            var relationshipDTO = mapperService.Map(relationship);

            await this.httpService.UpdateRelationshiptRequest(relationshipDTO);
        }
    }
}
