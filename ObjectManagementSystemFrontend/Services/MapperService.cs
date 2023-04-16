using ObjectManagementSystemFrontend.DTOs;
using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    ///     Provides conversion methods between DTOs and Model classes.
    ///     The conversion from JSON to DTO is handled via <see cref="System.Net.Http.Json.HttpClientJsonExtensions"/>
    /// </summary>
    public class MapperService
    {

        /// <summary>
        ///     Maps a collection of RelationshipDTO to Relationship. The model <see cref="Relationship" /> contains some properties of the other relevant type <see cref="GeneralObject" />.
        /// </summary>
        /// <param name="relationshipsDTO">The relationships dto.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>A collection of Relationships</returns>
        public ReadOnlyCollection<GeneralObject> Map( IEnumerable<GeneralObjectDTO> generalObjectsDTO)
        {
            List<GeneralObject> generalObjectCollection = new();

            foreach (var generalObjectDTO in generalObjectsDTO)
            {
                var generalObject = new GeneralObject
                {
                    Id = generalObjectDTO.Id,
                    Type = generalObjectDTO.Type,
                    Name = generalObjectDTO.Name,
                    Description = generalObjectDTO.Description
                };

                generalObjectCollection.Add(generalObject);
            }
            
            return new ReadOnlyCollection<GeneralObject>(generalObjectCollection);
        }

        /// <summary>
        ///     Maps a collection of RelationshipDTO to Relationship. The model <see cref="Relationship" /> contains some properties of the other relevant type <see cref="GeneralObject" />.
        /// </summary>
        /// <param name="relationshipsDTO">The relationships dto.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>A collection of Relationships</returns>
        public ReadOnlyCollection<Relationship> Map(IEnumerable<RelationshipDTO> relationshipsDTO, IEnumerable<GeneralObject> objects)
        {
            List<Relationship> relationshipCollection = new();

            foreach (var relationshipDTO in relationshipsDTO)
            {
                var relationship = new Relationship
                {
                    Id = relationshipDTO.Id,
                    Type = relationshipDTO.Type,
                    From = objects.FirstOrDefault(o => o.Id == relationshipDTO.FromId),
                    To = objects.FirstOrDefault(o => o.Id == relationshipDTO.ToId)
                };

                relationshipCollection.Add(relationship);
            }
            
            return new ReadOnlyCollection<Relationship>(relationshipCollection);
        }
    }
}