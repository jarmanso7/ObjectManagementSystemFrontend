using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Feeds the application with collections of <see cref="GeneralObject"/> and <see cref="Relationship"/> from an external source.
    /// </summary>
    public class DataProvisionService
    {
        private readonly HttpService httpService;
        private readonly MapperService mapperService;

        public DataProvisionService(
            HttpService httpService,
            MapperService mapperService)
        {
            this.httpService = httpService;
            this.mapperService = mapperService;
        }

        /// <summary>
        /// Reads the data from the source.
        /// </summary>
        /// <returns></returns>
        public async Task<(ReadOnlyCollection<GeneralObject>, ReadOnlyCollection<Relationship>)> Read()
        {
            var objectsResponse = await httpService.GeneralObjectsHttpRequest();
            var relationshipsResponse = await httpService.RelationshipsHttpRequest();

            var objects = mapperService.Map(objectsResponse.AsEnumerable());

            var relationships = mapperService.Map(
                                        relationshipsResponse.AsEnumerable(),
                                        objectsResponse.AsEnumerable());

            return (objects, relationships);
        }
    }
}
