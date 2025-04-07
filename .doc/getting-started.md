## Getting Started

This guide provides step-by-step instructions on how to set up and run the project locally.

### Prerequisites

Make sure the following tools are installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Git](https://git-scm.com/)
- An IDE of your choice (e.g., [Rider](https://www.jetbrains.com/rider/), [Visual Studio](https://visualstudio.microsoft.com/), or [VS Code](https://code.visualstudio.com/))

### Clone the repository

```bash
git clone https://github.com/brunomileto/developer-evaluation.git
cd DeveloperStore
```

### Ports

This project uses the following ports:

- `8080` - Application backend
- `8081` - Swagger UI

Make sure these ports are available before continuing.

### Running the containers

Start the infrastructure using Docker Compose:

```bash
docker-compose up -d
```

You can check if the containers are running with:

```bash
docker ps
```

You should see the following containers:

- `ambev_developer_evaluation_webapi` (API)
- `ambev_developer_evaluation_database` (PostgreSQL)
- `ambev_developer_evaluation_nosql` (MongoDB)
- `ambev_developer_evaluation_cache` (Redis)

### Database Migration

Once the containers are running, execute the migration script to create and/or update the database schema.

- On **Windows**:

  ```bash
  ./script/run_migrations.ps1
  ```

- On **Linux/macOS**:

  ```bash
  ./script/run_migrations.sh
  ```

### Accessing the API

With everything running, you can access the API documentation via Swagger UI:

[http://localhost:8081/swagger](http://localhost:8081/swagger)
