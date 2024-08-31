# Peer-to-Peer Chat Application

## Overview

This project is a Peer-to-Peer (P2P) Chat Application built with C# and .NET WinForms. The app allows users to discover peers, connect with them over TCP, and send/receive messages. It features custom peer discovery using UDP broadcasts and message exchange through serialized objects.

## Features

- **Peer Discovery:** Using UDP broadcasts, peers can discover each other on the same network.
- **Message Exchange:** Peers can send messages to specific connected peers or broadcast messages to all peers.
- **Named Peers:** Each peer can have a unique name that is displayed in the peer list.
- **Asynchronous Communication:** Message sending and receiving happen asynchronously, ensuring smooth communication between peers.

## Requirements

- .NET 6.0 SDK or later
- Visual Studio or any other C# IDE
- A local network for peer discovery (using UDP)

## Getting Started

### Prerequisites

Before running the application, ensure you have the following installed:

- .NET SDK 6.0 or later
- Visual Studio 2022 or any preferred IDE

### Clone the Repository

```bash
git clone git@github.com:yasser-mohamed1/Chat-Application-TCP-P2P-.git
```

## Buld and Run

```bash
dotnet build
dotnet run
```

## Folder Structure

ChatApp PeerToPeer/
│
├── ChatApp(PeertoPeer).sln # Solution file
├── src/ # Source files
│ ├── Forms/ # Contains WinForms classes and UI logic
│ ├── Models/ # Peer message and other data models
│ ├── Networking/ # Networking logic for peer discovery and communication
│ ├── Utilities/ # Helper and utility classes (e.g., serialization)
│ └── Program.cs # Main entry point of the application
│
├── docs/ # Documentation (if any)
├── README.md # Readme file
└── .gitignore # Git ignore file
