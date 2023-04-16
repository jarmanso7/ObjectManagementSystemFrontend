using ObjectManagementSystemFrontend.Models;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Feeds the application with collections of <see cref="GeneralObject"/> and <see cref="Relationship"/> from an external source.
    /// </summary>
    public class DataProvisionService
    {
        private readonly HttpService httpService;
        private readonly MapperService relationshipMapperService;

        public DataProvisionService(
            HttpService httpService,
            MapperService relationshipMapperService)
        {
            this.httpService = httpService;
            this.relationshipMapperService = relationshipMapperService;
        }

        /// <summary>
        /// Reads the data from the source.
        /// </summary>
        /// <returns></returns>
        public async Task<(List<GeneralObject>,List<Relationship>)> Read()
        {
            var objectsResponse = await httpService.GeneralObjectsHttpRequest();
            var relationshipsResponse = await httpService.RelationshipsHttpRequest();

            List<GeneralObject> objects = new();
            List<Relationship> relationships = new();

            if (objectsResponse != null)
            {
                objects.AddRange(objectsResponse);
            }

            if (relationshipsResponse != null)
            {
                relationshipMapperService.Map(relationshipsResponse, objects);
            }

            return (objects, relationships);
        }
    }
}
