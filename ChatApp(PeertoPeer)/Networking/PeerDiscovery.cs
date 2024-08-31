using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatApp_PeertoPeer_.Networking
{
    /// <summary>
    /// Handles peer discovery over UDP by broadcasting and listening for discovery requests.
    /// </summary>
    public class PeerDiscovery
    {
        private UdpClient udpClient;
        private RichTextBox logBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerDiscovery"/> class with the specified log output control.
        /// </summary>
        /// <param name="logBox">The <see cref="RichTextBox"/> control for logging events.</param>
        public PeerDiscovery(RichTextBox logBox)
        {
            this.logBox = logBox;
        }

        /// <summary>
        /// Broadcasts a discovery request asynchronously to find other peers.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task BroadcastDiscoveryAsync()
        {
            udpClient = new UdpClient { EnableBroadcast = true };
            IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, 8000);

            int counter = 5;

            while (counter-- > 0)
            {
                string discoveryMessage = "DISCOVERY_REQUEST";
                byte[] data = Encoding.UTF8.GetBytes(discoveryMessage);

                await udpClient.SendAsync(data, data.Length, broadcastEndpoint);
                Log("Discovery request broadcasted.");

                await Task.Delay(5000);
            }
        }

        /// <summary>
        /// Listens for incoming discovery requests asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ListenForDiscoveryAsync()
        {
            udpClient = new UdpClient(8000);

            while (true)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                    IPEndPoint senderEndpoint = result.RemoteEndPoint;
                    string localEndpoint = GetLocalIPAddress();
                    string remoteAddress = senderEndpoint.Address.ToString();
                    if (receivedMessage == "DISCOVERY_REQUEST" && remoteAddress != localEndpoint)
                    {
                        Log($"Discovery request received from {senderEndpoint.Address}:{senderEndpoint.Port}");

                        string responseMessage = $"DISCOVERY_RESPONSE from {localEndpoint}";
                        byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);

                        await udpClient.SendAsync(responseData, responseData.Length, senderEndpoint);
                        Log("Discovery response sent.");
                    }
                    else if (receivedMessage.StartsWith("DISCOVERY_RESPONSE"))
                    {
                        Log($"Received response from peer: {senderEndpoint.Address}:{senderEndpoint.Port}");
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error in discovery listener: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Gets the local IP address of the peer.
        /// </summary>
        /// <returns>The local IP address as a string.</returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// Logs a message to the specified <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void Log(string message)
        {
            logBox.Invoke(() =>
            {
                logBox.AppendText(message + Environment.NewLine);
            });
        }
    }
}
