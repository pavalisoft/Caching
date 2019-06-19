using System.Threading.Tasks;

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents interface for Cache Item Serializer
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified Cached Object.
        /// </summary>
        /// <typeparam name="T">Cached Object Type</typeparam>
        /// <param name="item">Cached Object</param>
        /// <returns>Returns byte array of <paramref name="item"/></returns>
        byte[] Serialize<T>(T item);

        /// <summary>
        /// Serializes the specified Cached Object asynchronous.
        /// </summary>
        /// <typeparam name="T">Cached Object Type</typeparam>
        /// <param name="item">Cached Object</param>
        /// <returns>Retruns byte array of <paramref name="item"/></returns>
        Task<byte[]> SerializeAsync<T>(T item);

        /// <summary>
        /// Deserializes the specified bytes to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Cached Object Type</typeparam>
        /// <param name="bytes">The serialized object.</param>
        /// <returns> The instance of the specified <typeparamref name="T"/></returns>
        T Deserialize<T>(byte[] bytes);

        /// <summary>
        /// Deserializes the specified bytes to <typeparamref name="T"/> asynchronously.
        /// </summary>
        /// <typeparam name="T">Cached Object Type</typeparam>
        /// <param name="bytes">The serialized object.</param>
        /// <returns>The instance of the specified <typeparamref name="T"/></returns>
        Task<T> DeserializeAsync<T>(byte[] bytes);
    }
}