using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Persists data to an external storage system, in this case through a web API
    /// </summary>
    public class DataPersistenceService
    {
        public void OnGeneralObjectsChanged()
        {
            Console.WriteLine("Write collection of objects to the backend");
        }

        public void OnRelationshipsChanged()
        {
            Console.WriteLine("Write collection of relationships to the backend");
        }

        public void OnObjectItemPropertyChanged()
        {
            Console.WriteLine("Update object to the backend");
        }

        public void OnRelationshipItemPropertyChanged()
        {
            Console.WriteLine("Update relationship to the backend");
        }
    }
}
