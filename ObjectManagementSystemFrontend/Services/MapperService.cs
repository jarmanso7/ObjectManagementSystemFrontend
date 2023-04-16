using ObjectManagementSystemFrontend.DTOs;
using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    ///     Provides conversion methods between DTOs and Model classes.
    ///     The conversion from JSON to DTO is handled in <see cref="HttpService"/>
    /// </summary>
    public class MapperService
    {

        /// <summary>
        ///     Maps a collection of RelationshipDTO to Relationship. The model <see cref="Relationship" /> contains some properties of the other relevant type <see cref="GeneralObject" />.
        /// </summary>
        /// <param name="relationshipsDTO">The relationships dto.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>A collection of Relationships</returns>
        public ReadOnlyCollection<GeneralObject>Map( IEnumerable<GeneralObjectDTO> generalObjectsDTO)
        {
            List<GeneralObject> generalObjectCollection = new();

            if (generalObjectsDTO != null)
            {
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
            }

            return new ReadOnlyCollection<GeneralObject>(generalObjectCollection);
        }

        /// <summary>
        ///     Maps a collection of RelationshipDTO to Relationship. The model <see cref="Relationship" /> contains some properties of the other relevant type <see cref="GeneralObject" />.
        /// </summary>
        /// <param name="relationshipsDTO">The relationships dto.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>A collection of Relationships</returns>
        public ReadOnlyCollection<Relationship>Map(IEnumerable<RelationshipDTO> relationshipsDTO, IEnumerable<GeneralObjectDTO> objectsDTO)
        {
            List<Relationship> relationshipCollection = new();

            if (relationshipsDTO != null && objectsDTO != null)
            {
                foreach (var relationshipDTO in relationshipsDTO)
                {
                    var fromObject = objectsDTO.FirstOrDefault(o => o.Id == relationshipDTO.FromId);
                    var toObject = objectsDTO.FirstOrDefault(o => o.Id == relationshipDTO.ToId);

                    var relationship = Map(relationshipDTO, fromObject, toObject);

                    relationshipCollection.Add(relationship);
                }
            }

            return new ReadOnlyCollection<Relationship>(relationshipCollection);
        }

        /// <summary>
        /// Maps <see cref="Relationship" />, which contains some properties of the other relevant type <see cref="GeneralObject" />, to <see cref="RelationshipDTO" />.
        /// </summary>
        /// <param name="relationship">The relationship.</param>
        /// <param name="generalObjectDTO">The general object dto.</param>
        /// <returns></returns>
        private Relationship Map(RelationshipDTO relationshipDTO, GeneralObjectDTO from, GeneralObjectDTO to)
        {
            return new Relationship
            {
                Id = relationshipDTO.Id,
                Type = relationshipDTO.Type,
                From = Map(from),
                To = Map(to)
            };
        }

        private GeneralObject Map(GeneralObjectDTO generalObjectDTO)
        {
            return new GeneralObject
            {
                Id = generalObjectDTO.Id,
                Type = generalObjectDTO.Type,
                Name = generalObjectDTO.Name,
                Description = generalObjectDTO.Description
            };
        }
    }
}