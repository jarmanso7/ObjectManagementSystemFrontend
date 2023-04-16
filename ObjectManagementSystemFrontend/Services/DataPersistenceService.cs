using System.Text.Json;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Persists data to an external storage system, in this case through a web API
    /// </summary>
    public class DataPersistenceService
    {
        /// <summary>
        /// Creates the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Create<T>(T item)
        {
            Console.WriteLine($"Create: {JsonSerializer.Serialize(item)}");
        }
        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Delete<T>(T item)
        {
            Console.WriteLine($"Delete: {JsonSerializer.Serialize(item)}");
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Update<T>(T item)
        {
            Console.WriteLine($"Update: {JsonSerializer.Serialize(item)}");
        }
    }
}
