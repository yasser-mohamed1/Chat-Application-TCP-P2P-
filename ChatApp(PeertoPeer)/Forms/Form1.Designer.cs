using ChatApp_PeertoPeer_.Networking;

namespace ChatApp_PeertoPeer_
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Peer peer;
        private PeerDiscovery peerDiscovery;
        private int peerPort = 5000;

        private Panel panelPeers;
        private ListBox lstPeers;
        private Label lblPeers;

        private TextBox txtMessage;
        private Label lblMessage;

        private TextBox txtPeerAddress;
        private TextBox txtPeerPort;
        private Label lblPeerAddress;
        private Label lblPeerPort;

        private Button btnSend;
        private Button btnBroadcast;
        private Button btnDiscoverPeers;
        private Button btnConnectToPeer;

        private RichTextBox rtbLogs;
        private Label lblLogs;

        private ListBox lstMessages;


        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panelPeers = new Panel();
            lblPeers = new Label();
            lstPeers = new ListBox();
            txtMessage = new TextBox();
            lblMessage = new Label();
            txtPeerAddress = new TextBox();
            txtPeerPort = new TextBox();
            lblPeerAddress = new Label();
            lblPeerPort = new Label();
            btnSend = new Button();
            btnBroadcast = new Button();
            btnDiscoverPeers = new Button();
            btnConnectToPeer = new Button();
            rtbLogs = new RichTextBox();
            lblLogs = new Label();
            lstMessages = new ListBox();
            panelPeers.SuspendLayout();
            SuspendLayout();
            // 
            // panelPeers
            // 
            panelPeers.BackColor = Color.White;
            panelPeers.BorderStyle = BorderStyle.FixedSingle;
            panelPeers.Controls.Add(lblPeers);
            panelPeers.Controls.Add(lstPeers);
            panelPeers.Location = new Point(10, 10);
            panelPeers.Name = "panelPeers";
            panelPeers.Size = new Size(220, 180);
            panelPeers.TabIndex = 0;
            // 
            // lblPeers
            // 
            lblPeers.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeers.Location = new Point(5, 5);
            lblPeers.Name = "lblPeers";
            lblPeers.Size = new Size(100, 23);
            lblPeers.TabIndex = 0;
            lblPeers.Text = "Connected Peers";
            // 
            // lstPeers
            // 
            lstPeers.BackColor = Color.FromArgb(240, 240, 240);
            lstPeers.BorderStyle = BorderStyle.None;
            lstPeers.Font = new Font("Segoe UI", 9F);
            lstPeers.ItemHeight = 15;
            lstPeers.Location = new Point(5, 30);
            lstPeers.Name = "lstPeers";
            lstPeers.Size = new Size(200, 120);
            lstPeers.TabIndex = 1;
            // 
            // txtMessage
            // 
            txtMessage.BackColor = Color.White;
            txtMessage.BorderStyle = BorderStyle.FixedSingle;
            txtMessage.Font = new Font("Segoe UI", 9F);
            txtMessage.Location = new Point(10, 220);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(220, 50);
            txtMessage.TabIndex = 1;
            // 
            // lblMessage
            // 
            lblMessage.Font = new Font("Segoe UI", 9F);
            lblMessage.Location = new Point(10, 200);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(100, 23);
            lblMessage.TabIndex = 2;
            lblMessage.Text = "Message";
            // 
            // txtPeerAddress
            // 
            txtPeerAddress.BackColor = Color.White;
            txtPeerAddress.Font = new Font("Segoe UI", 9F);
            txtPeerAddress.Location = new Point(250, 35);
            txtPeerAddress.Name = "txtPeerAddress";
            txtPeerAddress.Size = new Size(130, 23);
            txtPeerAddress.TabIndex = 3;
            // 
            // txtPeerPort
            // 
            txtPeerPort.BackColor = Color.White;
            txtPeerPort.Font = new Font("Segoe UI", 9F);
            txtPeerPort.Location = new Point(400, 35);
            txtPeerPort.Name = "txtPeerPort";
            txtPeerPort.Size = new Size(60, 23);
            txtPeerPort.TabIndex = 4;
            // 
            // lblPeerAddress
            // 
            lblPeerAddress.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeerAddress.Location = new Point(250, 10);
            lblPeerAddress.Name = "lblPeerAddress";
            lblPeerAddress.Size = new Size(100, 23);
            lblPeerAddress.TabIndex = 5;
            lblPeerAddress.Text = "Peer Address";
            // 
            // lblPeerPort
            // 
            lblPeerPort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeerPort.Location = new Point(400, 10);
            lblPeerPort.Name = "lblPeerPort";
            lblPeerPort.Size = new Size(100, 23);
            lblPeerPort.TabIndex = 6;
            lblPeerPort.Text = "Peer Port";
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(70, 130, 180);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(10, 280);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(90, 30);
            btnSend.TabIndex = 7;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnBroadcast
            // 
            btnBroadcast.BackColor = Color.FromArgb(60, 179, 113);
            btnBroadcast.FlatStyle = FlatStyle.Flat;
            btnBroadcast.ForeColor = Color.White;
            btnBroadcast.Location = new Point(110, 280);
            btnBroadcast.Name = "btnBroadcast";
            btnBroadcast.Size = new Size(90, 30);
            btnBroadcast.TabIndex = 8;
            btnBroadcast.Text = "Broadcast";
            btnBroadcast.UseVisualStyleBackColor = false;
            btnBroadcast.Click += btnBroadcast_Click;
            // 
            // btnDiscoverPeers
            // 
            btnDiscoverPeers.BackColor = Color.FromArgb(100, 149, 237);
            btnDiscoverPeers.FlatStyle = FlatStyle.Flat;
            btnDiscoverPeers.ForeColor = Color.White;
            btnDiscoverPeers.Location = new Point(250, 70);
            btnDiscoverPeers.Name = "btnDiscoverPeers";
            btnDiscoverPeers.Size = new Size(130, 30);
            btnDiscoverPeers.TabIndex = 9;
            btnDiscoverPeers.Text = "Discover Peers";
            btnDiscoverPeers.UseVisualStyleBackColor = false;
            btnDiscoverPeers.Click += btnDiscoverPeers_Click;
            // 
            // btnConnectToPeer
            // 
            btnConnectToPeer.BackColor = Color.FromArgb(72, 61, 139);
            btnConnectToPeer.FlatStyle = FlatStyle.Flat;
            btnConnectToPeer.ForeColor = Color.White;
            btnConnectToPeer.Location = new Point(400, 70);
            btnConnectToPeer.Name = "btnConnectToPeer";
            btnConnectToPeer.Size = new Size(80, 30);
            btnConnectToPeer.TabIndex = 10;
            btnConnectToPeer.Text = "Connect";
            btnConnectToPeer.UseVisualStyleBackColor = false;
            btnConnectToPeer.Click += btnConnectToPeer_Click;
            // 
            // rtbLogs
            // 
            rtbLogs.BackColor = Color.FromArgb(245, 245, 245);
            rtbLogs.BorderStyle = BorderStyle.FixedSingle;
            rtbLogs.Font = new Font("Segoe UI", 9F);
            rtbLogs.Location = new Point(10, 330);
            rtbLogs.Name = "rtbLogs";
            rtbLogs.Size = new Size(480, 150);
            rtbLogs.TabIndex = 11;
            rtbLogs.Text = "";
            // 
            // lblLogs
            // 
            lblLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLogs.Location = new Point(10, 310);
            lblLogs.Name = "lblLogs";
            lblLogs.Size = new Size(100, 23);
            lblLogs.TabIndex = 12;
            lblLogs.Text = "Logs";
            // 
            // lstMessages
            // 
            lstMessages.FormattingEnabled = true;
            lstMessages.ItemHeight = 17;
            lstMessages.Location = new Point(250, 120);
            lstMessages.Name = "lstMessages";
            lstMessages.Size = new Size(300, 140);
            lstMessages.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(600, 500);
            Controls.Add(panelPeers);
            Controls.Add(txtMessage);
            Controls.Add(lblMessage);
            Controls.Add(txtPeerAddress);
            Controls.Add(txtPeerPort);
            Controls.Add(lblPeerAddress);
            Controls.Add(lblPeerPort);
            Controls.Add(btnSend);
            Controls.Add(btnBroadcast);
            Controls.Add(btnDiscoverPeers);
            Controls.Add(btnConnectToPeer);
            Controls.Add(rtbLogs);
            Controls.Add(lblLogs);
            Controls.Add(lstMessages);
            Font = new Font("Segoe UI", 10F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Chat App P2P";
            panelPeers.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
