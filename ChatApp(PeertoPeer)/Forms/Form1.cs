using ChatApp_PeertoPeer_.Components;
using ChatApp_PeertoPeer_.Models;
using ChatApp_PeertoPeer_.Networking;
using ChatApp_PeertoPeer_.Utilities;

namespace ChatApp_PeertoPeer_
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            string peerName = PromptForPeerName();
            peer = new Peer(peerPort, rtbLogs!, RefreshPeerList, peerName);
            peer.OnMessageReceived += Peer_OnMessageReceived;
            _ = peer.StartListenerAsync();
            RefreshPeerList();
            peerDiscovery = new PeerDiscovery(rtbLogs!);
            _ = peerDiscovery.ListenForDiscoveryAsync();
        }

        private string PromptForPeerName()
        {
            using (var dialog = new InputDialog("Enter your name:"))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.InputText;
                }
                else if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    this.Close();
                    Application.Exit();
                }
            }
            return "Unnamed Peer";
        }

        private void Peer_OnMessageReceived(string message)
        {
            if (InvokeRequired)
            {
                lstMessages.Invoke((MethodInvoker)delegate
                {
                    lstMessages.Items.Add(message);
                });
            }
            else
            {
                lstMessages.Items.Add(message);
            }
        }


        private async void btnDiscoverPeers_Click(object sender, EventArgs e)
        {
            rtbLogs.AppendText("Starting peer discovery...\n");
            await peerDiscovery.BroadcastDiscoveryAsync();
        }

        private async void btnConnectToPeer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPeerAddress.Text))
            {
                MessageBox.Show("Please enter a valid peer address.");
                return;
            }

            if (!int.TryParse(txtPeerPort.Text, out int peerPort) || peerPort <= 0 || peerPort > 65535)
            {
                MessageBox.Show("Please enter a valid port number (1-65535).");
                return;
            }

            string peerAddress = txtPeerAddress.Text;
            rtbLogs.AppendText($"Connecting to peer {peerAddress}:{peerPort}...\n");

            try
            {
                await peer.ConnectToPeerAsync(peerAddress, peerPort);
                RefreshPeerList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to peer: {ex.Message}");
            }
        }


        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (lstPeers.SelectedItem == null)
            {
                MessageBox.Show("Please select a peer to send the message.");
                return;
            }

            string selectedItem = lstPeers.SelectedItem.ToString()!;

            int startIndex = selectedItem!.LastIndexOf('(') + 1;
            int length = selectedItem.Length - startIndex - 1;
            string peerAddress = selectedItem.Substring(startIndex, length);

            string[] peerParts = peerAddress.Split(':');
            if (peerParts.Length != 2)
            {
                MessageBox.Show("Invalid peer address format.");
                return;
            }

            string messageContent = txtMessage.Text;

            try
            {
                var message = new PeerMessage
                {
                    Sender = peer.Name,
                    Content = messageContent,
                    Timestamp = DateTime.Now
                };

                string serializedMessage = JsonMessageHandler.SerializeMessage(message);
                await peer.SendMessageToPeerAsync(peerParts[0], int.Parse(peerParts[1]), serializedMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }


        private async void btnBroadcast_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Message cannot be empty.");
                return;
            }

            string messageContent = txtMessage.Text;

            try
            {
                var message = new PeerMessage
                {
                    Sender = peer.Name,
                    Content = messageContent,
                    Timestamp = DateTime.UtcNow
                };

                string serializedMessage = JsonMessageHandler.SerializeMessage(message);
                await peer.BroadcastMessageAsync(serializedMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error broadcasting message: {ex.Message}");
            }
        }

        private void RefreshPeerList()
        {
            if (lstPeers.InvokeRequired)
            {
                lstPeers.Invoke((MethodInvoker)delegate
                {
                    lstPeers.Items.Clear();
                    foreach (var peerAddress in peer.GetConnectedPeers())
                    {
                        string peerName = peer.GetPeerName(peerAddress);
                        lstPeers.Items.Add($"{peerName} ({peerAddress})");
                    }
                });
            }
            else
            {
                lstPeers.Items.Clear();
                foreach (var peerAddress in peer.GetConnectedPeers())
                {
                    string peerName = peer.GetPeerName(peerAddress);
                    lstPeers.Items.Add($"{peerName} ({peerAddress})");
                }
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            peer.CloseAllConnections();
        }
    }
}
