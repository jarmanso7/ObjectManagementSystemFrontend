using ObjectManagementSystemFrontend.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Persists data to an external storage system, in this case through a web API
    /// </summary>
    public class DataPersistenceService
    {
        public void Create<T>(T item)
        {
            Console.WriteLine($"Create: {JsonSerializer.Serialize(item)}");
        }
        public void Delete<T>(T item)
        {
            Console.WriteLine($"Delete: {JsonSerializer.Serialize(item)}");
        }

        public void Update<T>(T item)
        {
            Console.WriteLine($"Update: {JsonSerializer.Serialize(item)}");
        }
    }
}
