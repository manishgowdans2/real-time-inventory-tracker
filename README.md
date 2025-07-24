# 🧾 Real-Time Inventory Tracker 🚀✨

[![.NET](https://img.shields.io/badge/.NET-6.0-blueviolet?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-.NET-blue?logo=windows)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-blue?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Kafka](https://img.shields.io/badge/Kafka-Event%20Streaming-231f20?logo=apachekafka&logoColor=white)](https://kafka.apache.org/)
[![Debezium](https://img.shields.io/badge/Debezium-CDC-lightgrey?logo=data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjRkYzMzMzIiB2aWV3Qm94PSIwIDAgMzIgMzIiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHBhdGggZD0iTTE2IDJDOS40MiAyIDQgNy40MiA0IDE0czUuNDIgMTIgMTIgMTIgMTItNS40MiAxMi0xMi01LjQyLTEyLTEyLTEyeiIvPjwvc3ZnPg==)](https://debezium.io/)
[![Docker](https://img.shields.io/badge/Docker-Containerized-blue?logo=docker)](https://www.docker.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

A full-stack real-time inventory management system built using **.NET**, **WPF**, **PostgreSQL**, **Kafka**, and **Debezium (CDC)**. This application allows multiple users to perform CRUD operations on inventory items and see live updates across all connected clients through a reactive backend.

---

## 📚 Table of Contents

- [✨ Core Features](#-core-features)
- [🧠 System Architecture](#-system-architecture)
- [🧰 Tech Stack](#-tech-stack)
- [🗂️ Project Structure](#-project-structure)
- [✅ Prerequisites](#-prerequisites)
- [🚀 Getting Started](#-getting-started)
- [⚙️ How It Works](#-how-it-works)
- [🔌 API Endpoints](#-api-endpoints)
- [🔧 Configuration](#-configuration)

---

## ✨ Core Features

- ✅ **CRUD Operations** – Add, edit, delete, and view inventory items.
- 🔁 **Real-Time UI Updates** – All connected clients receive instant updates using SignalR.
- 🖥️ **WPF Desktop Client** – Smooth desktop experience for inventory operations.
- 🧩 **Decoupled Services** – Backend components communicate via APIs and Kafka.
- 🕵️‍♂️ **Change Data Capture (CDC)** – Real-time detection of DB changes via Debezium.
- 🐳 **Fully Containerized** – Use Docker Compose to spin up all backend services.

---

## 🧠 System Architecture

The system follows **event-driven architecture** with two primary flows:

- **Command Flow**: Initiated by user (CRUD) → updates DB
- **Event Flow**: DB change → Kafka event → SignalR broadcast

![System Design](assets/system-desgin.png)

---

## 🧰 Tech Stack

| Component         | Technology                      |
|------------------|----------------------------------|
| Backend API       | .NET Core, ASP.NET Core, SignalR |
| Desktop Client    | WPF (.NET)                       |
| Database          | PostgreSQL 🐘                    |
| CDC Platform      | Debezium ⚡                      |
| Messaging Broker  | Apache Kafka 🦄                 |
| Background Service| .NET Core Worker Service         |
| Containerization  | Docker & Docker Compose 🐳       |

---

## 🗂️ Project Structure

```
.
├── docker-compose.yml
├── .gitignore
├── README.md
└── src
    ├── Inventory.Api/           # ASP.NET Core API + SignalR
    ├── Inventory.Consumer/      # Kafka Consumer Worker Service
    └── Inventory.WpfClient/     # WPF Desktop Application
```

---

## ✅ Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (latest or as per `global.json`)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Postman](https://www.postman.com/) or `curl` for testing HTTP requests

---

## 🚀 Getting Started

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/your-username/your-repository-name.git
cd your-repository-name
```

### 2️⃣ Start Infrastructure

```bash
docker-compose up -d
```

Wait ~1 minute for services to initialize.

### 3️⃣ Configure Debezium Connector

```bash
curl -i -X POST -H "Accept:application/json" -H "Content-Type:application/json" localhost:8083/connectors/ -d '{
  "name": "inventory-connector",
  "config": {
    "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
    "plugin.name": "pgoutput",
    "database.hostname": "postgres",
    "database.port": "5432",
    "database.user": "postgres",
    "database.password": "postgres",
    "database.dbname": "inventory_db",
    "database.server.name": "dbserver1",
    "table.include.list": "public.inventory_items",
    "topic.prefix": "inventory-cdc"
  }
}'
```

This sets up Debezium to monitor the `inventory_items` table and stream changes to Kafka.

### 4️⃣ Run Backend Services

```bash
# Run API with SignalR
cd src/Inventory.Api
dotnet run
```

```bash
# Run Kafka Consumer
cd src/Inventory.Consumer
dotnet run
```

### 5️⃣ Launch WPF Client

```bash
cd src/Inventory.WpfClient
dotnet run
```

> 🪟 Open multiple WPF clients to test real-time sync in action!

---

## ⚙️ How It Works

### 🔁 User-Initiated CRUD Flow

1. User creates/updates/deletes inventory item via WPF Client.
2. API processes request and updates PostgreSQL.
3. Client receives HTTP response.

### 📡 Real-Time Update via CDC

1. PostgreSQL WAL logs the data change.
2. Debezium reads the change and sends JSON event to Kafka.
3. Kafka Consumer (Worker Service) processes the event.
4. Event pushed to SignalR hub.
5. All WPF Clients receive and render the update instantly.

---

## 🔌 API Endpoints

| Method | Endpoint              | Description                  |
|--------|-----------------------|------------------------------|
| GET    | `/api/inventory`      | Get all inventory items      |
| GET    | `/api/inventory/{id}` | Get item by ID               |
| POST   | `/api/inventory`      | Create a new item            |
| PUT    | `/api/inventory/{id}` | Update an item               |
| DELETE | `/api/inventory/{id}` | Delete an item               |

SignalR Hub: `/inventoryHub`

---

## 🔧 Configuration

### 📁 `Inventory.Api/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=inventory_db;Username=postgres;Password=postgres"
}
```

### 📁 `Inventory.Consumer/appsettings.json`

```json
"Kafka": {
  "BootstrapServers": "localhost:9092",
  "TopicName": "inventory-cdc.public.inventory_items"
},
"SignalR": {
  "HubUrl": "http://localhost:5000/inventoryHub"
}
```

---

## 📬 Feedback & Contributions

Feel free to submit issues or pull requests. Contributions are welcome!

---

## ⭐ Star this project if you find it useful!
