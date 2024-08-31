using ChatApp_PeertoPeer_.Models;

namespace ChatApp_PeertoPeer_.Utilities
{

    /// <summary>
    /// Provides methods for serializing and deserializing <see cref="PeerMessage"/> objects to and from JSON format.
    /// </summary>
    public static class JsonMessageHandler
    {
        /// <summary>
        /// Serializes a <see cref="PeerMessage"/> object to a JSON string.
        /// </summary>
        /// <param name="message">The <see cref="PeerMessage"/> to serialize.</param>
        /// <returns>A JSON string representation of the message.</returns>
        public static string SerializeMessage(PeerMessage message)
        {
            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            return System.Text.Json.JsonSerializer.Serialize(message, options);
        }

        /// <summary>
        /// Deserializes a JSON string to a <see cref="PeerMessage"/> object.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized <see cref="PeerMessage"/> object.</returns>
        public static PeerMessage DeserializeMessage(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<PeerMessage>(json)!;
        }
    }
}
