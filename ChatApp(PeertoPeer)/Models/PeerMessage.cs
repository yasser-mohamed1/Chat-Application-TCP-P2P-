namespace ChatApp_PeertoPeer_.Models
{
    /// <summary>
    /// Represents a message exchanged between peers.
    /// </summary>
    public class PeerMessage
    {
        /// <summary>
        /// Gets or sets the sender of the message.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the message was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

}
