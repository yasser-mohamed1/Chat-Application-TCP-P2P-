using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;

namespace ChatApp_PeertoPeer_.Networking
{
    /// <summary>
    /// Handles peer discovery over UDP by broadcasting and listening for discovery requests.
    /// </summary>
    public class PeerDiscovery
    {
        private UdpClient udpClientBroadCast;
        private UdpClient udpClientListen;
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
            udpClientBroadCast = new UdpClient { EnableBroadcast = true };
            IPAddress broadcastIP = IPAddress.Parse("192.168.1.255");
            IPEndPoint broadcastEndpoint = new IPEndPoint(broadcastIP, 8000);
            int counter = 5;

            while (counter-- > 0)
            {
                string discoveryMessage = "DISCOVERY_REQUEST";
                byte[] data = Encoding.UTF8.GetBytes(discoveryMessage);

                await udpClientBroadCast.SendAsync(data, data.Length, broadcastEndpoint);
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
            udpClientListen = new UdpClient(8000);

            while (true)
            {
                try
                {
                    UdpReceiveResult result = await udpClientListen.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                    IPEndPoint senderEndpoint = result.RemoteEndPoint;
                    string localEndpoint = GetLocalIPAddress();
                    string remoteAddress = senderEndpoint.Address.ToString();
                    if (receivedMessage == "DISCOVERY_REQUEST" && remoteAddress != localEndpoint)
                    {
                        Log($"Discovery request received from {senderEndpoint.Address}:{senderEndpoint.Port}");
                        Log($"You can connect to it through Address: {senderEndpoint.Address} and Port: 5000");

                        string responseMessage = $"DISCOVERY_RESPONSE from {localEndpoint}";
                        byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);

                        IPEndPoint senderEndpointResponse = new IPEndPoint(senderEndpoint.Address, 8000);
                        await udpClientListen.SendAsync(responseData, responseData.Length, senderEndpointResponse);
                    }
                    else if (receivedMessage.StartsWith("DISCOVERY_RESPONSE"))
                    {
                        Log($"Received response from peer: {senderEndpoint.Address}:{senderEndpoint.Port}");
                        Log($"You can connect to it through Address: {senderEndpoint.Address} and Port: 5000");
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
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.OperationalStatus == OperationalStatus.Up &&
                    (netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                     netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    var gatewayAddress = netInterface.GetIPProperties().GatewayAddresses.FirstOrDefault();
                    if (gatewayAddress != null)
                    {
                        foreach (UnicastIPAddressInformation ip in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                                !IPAddress.IsLoopback(ip.Address))
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return "No valid IPv4 address found.";
        }

        /// <summary>
        /// Logs a message to the specified <see cref="RichTextBox"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void Log(string message)
        {
            if (logBox.InvokeRequired)
            {
                logBox.Invoke((MethodInvoker)delegate
                {
                    logBox.AppendText(message + Environment.NewLine);
                });
            }
            else
            {
                logBox.AppendText(message + Environment.NewLine);
            }
        }
    }
}