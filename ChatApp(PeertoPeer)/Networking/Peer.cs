using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatApp_PeertoPeer_.Models;
using ChatApp_PeertoPeer_.Utilities;

namespace ChatApp_PeertoPeer_.Networking
{
    /// <summary>
    /// Represents a peer that can communicate with other peers in a network.
    /// </summary>
    public class Peer
    {
        public string Name { get; set; }
        private TcpListener listener;
        private int port;
        private RichTextBox logBox;
        private Action refreshPeerList;

        /// <summary>
        /// Event triggered when a message is received from a peer.
        /// </summary>
        public event Action<string> OnMessageReceived;

        private ConcurrentDictionary<string, string> peerNames = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Stores the connected peers in a thread-safe dictionary.
        /// </summary
        private ConcurrentDictionary<string, TcpClient> connectedPeers = new ConcurrentDictionary<string, TcpClient>();

        public Peer(int port, RichTextBox logBox, Action refreshPeerList, string Name)
        {
            this.port = port;
            this.logBox = logBox;
            this.refreshPeerList = refreshPeerList;
            this.Name = Name;
        }

        /// <summary>
        /// Starts listening for incoming peer connections asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartListenerAsync()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Log($"Listening on port {port}...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                string clientEndpoint = client.Client.RemoteEndPoint!.ToString()!;
                connectedPeers.TryAdd(clientEndpoint, client);
                Log($"Connected with peer: {clientEndpoint}");
                refreshPeerList?.Invoke();
                _ = HandleClientAsync(client, clientEndpoint);
            }
        }

        /// <summary>
        /// Handles the incoming connection from a peer and continuously listens for messages from the client asynchronously.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> representing the connected peer.</param>
        /// <param name="clientEndpoint">The address and port number of the connected peer as a string.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method reads messages from the connected peer, processes the message by deserializing the <see cref="PeerMessage"/>, 
        /// and then invokes the <see cref="OnMessageReceived"/> event to notify subscribers of the received message. 
        /// It also handles peer disconnection and updates the peer list.
        /// </remarks>
        private async Task HandleClientAsync(TcpClient client, string clientEndpoint)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            string peerName = await ExchangePeerNamesAsync(client, stream);
            AddPeerName(clientEndpoint, peerName);
            refreshPeerList?.Invoke();

            while (client.Connected)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    PeerMessage peerMessage = JsonMessageHandler.DeserializeMessage(message);
                    string formattedMessage = FormatPeerMessage(peerMessage.Sender, peerMessage.Content, peerMessage.Timestamp);

                    OnMessageReceived?.Invoke(formattedMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error receiving data from {clientEndpoint}: {ex.Message}");
                    break;
                }
            }

            connectedPeers.TryRemove(clientEndpoint, out _);
            refreshPeerList?.Invoke();
            client.Close();
        }

        /// <summary>
        /// Connects to another peer asynchronously using the specified address and port.
        /// </summary>
        /// <param name="peerAddress">The IP address of the peer to connect to.</param>
        /// <param name="peerPort">The port number of the peer to connect to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ConnectToPeerAsync(string peerAddress, int peerPort)
        {
            string clientEndpoint = $"{peerAddress}:{peerPort}";
            if (connectedPeers.ContainsKey(clientEndpoint))
            {
                MessageBox.Show($"Already connected to peer {clientEndpoint}");
                return;
            }

            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(peerAddress, peerPort);
                refreshPeerList?.Invoke();
                if (connectedPeers.TryAdd(clientEndpoint, client))
                {
                    MessageBox.Show($"Connected to peer {clientEndpoint}");
                    _ = HandleClientAsync(client, clientEndpoint);
                }
                else
                {
                    MessageBox.Show($"Failed to add connection to peer {clientEndpoint}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to peer {clientEndpoint}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a message to the specified peer asynchronously.
        /// </summary>
        /// <param name="peerAddress">The IP address of the peer to send the message to.</param>
        /// <param name="peerPort">The port number of the peer to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SendMessageToPeerAsync(string peerAddress, int peerPort, string message)
        {
            string clientEndpoint = $"{peerAddress}:{peerPort}";
            if (connectedPeers.TryGetValue(clientEndpoint, out TcpClient client))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Log($"Message sent to {clientEndpoint}");
            }
            else
            {
                Log($"No active connection to peer {clientEndpoint}");
            }
        }


        /// <summary>
        /// Broadcasts a message to all connected peers asynchronously.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task BroadcastMessageAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            foreach (var kvp in connectedPeers)
            {
                TcpClient client = kvp.Value;
                if (client.Connected)
                {
                    NetworkStream stream = client.GetStream();
                    await stream.WriteAsync(data, 0, data.Length);
                    Log($"Message broadcasted to {kvp.Key}");
                }
            }
        }

        /// <summary>
        /// Closes all connections to connected peers.
        /// </summary>
        public void CloseAllConnections()
        {
            foreach (var kvp in connectedPeers)
            {
                kvp.Value.Close();
            }
            connectedPeers.Clear();
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

        /// <summary>
        /// Exchanges peer names with the connected client.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> representing the connected peer.</param>
        /// <param name="stream">The <see cref="NetworkStream"/> used to communicate with the peer.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The result contains the peer's name.</returns>
        /// <remarks>
        /// This method sends the local peer's name to the connected peer and receives the peer's name in return.
        /// </remarks>
        private async Task<string> ExchangePeerNamesAsync(TcpClient client, NetworkStream stream)
        {
            byte[] nameData = Encoding.UTF8.GetBytes(Name);
            await stream.WriteAsync(nameData, 0, nameData.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        /// <summary>
        /// Retrieves the addresses of all connected peers.
        /// </summary>
        /// <returns>An array of strings representing the addresses of connected peers.</returns>
        /// <remarks>
        /// This method returns the endpoints (addresses and ports) of all currently connected peers.
        /// </remarks>
        public string[] GetConnectedPeers()
        {
            return connectedPeers.Keys.ToArray();
        }

        /// <summary>
        /// Formats a message received from a peer into a readable string with timestamp.
        /// </summary>
        /// <param name="sender">The name of the peer that sent the message.</param>
        /// <param name="messageContent">The content of the message.</param>
        /// <param name="timestamp">The time when the message was received.</param>
        /// <returns>A formatted string containing the sender, message content, and timestamp.</returns>
        public string FormatPeerMessage(string sender, string messageContent, DateTime timestamp)
        {
            return $"{sender}: {messageContent} ({timestamp:HH:mm:ss})";
        }

        /// <summary>
        /// Adds a peer's name to the peer list based on their address.
        /// </summary>
        /// <param name="address">The address of the peer.</param>
        /// <param name="name">The name of the peer to associate with the address.</param>
        /// <remarks>
        /// This method is used to associate a peer's name with their network address for later reference.
        /// </remarks>
        public void AddPeerName(string address, string name)
        {
            peerNames[address] = name;
        }

        /// <summary>
        /// Retrieves the name of a peer based on their address.
        /// </summary>
        /// <param name="address">The address of the peer.</param>
        /// <returns>The name of the peer if found, otherwise <c>null</c>.</returns>
        public string GetPeerName(string address)
        {
            return peerNames.TryGetValue(address, out var name) ? name : null!;
        }
    }
}
