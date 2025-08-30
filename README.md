# Backend-core

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)
- [Contributing](../CONTRIBUTING.md)

## About <a name = "about"></a>

This repository contains **Neukod backend service journey**.  
The project uses **ASP.NET Core Minimal API** with **PostgreSQL** as the database, and **Adminer** for database administration.  
Feel free to raise issues for better development.

---

## Getting Started <a name = "getting_started"></a>

At this point, this guidance is only for [deployment](#installing) purpose.

### Prerequisites

Make sure you have the following installed:

- [Docker](https://www.docker.com/get-started)  
- [Docker Compose](https://docs.docker.com/compose/)  

You also need a `.env` file in the root directory with PostgreSQL credentials:

```env
POSTGRES_USER=youruser
POSTGRES_PASS=yourpassword
POSTGRES_DB=yourdb
```

### Installing <a name = "installing"></a>

Follow these steps to get the system running:

1. Build and start the containers

   ```bash
   docker-compose up -d --build
   ```

   This will start:
   backend-core → ASP.NET Core Minimal API (port **:5049**)<br>
   postgres → PostgreSQL database (port **:5432**)<br>
   adminer → Database management UI (port **:8080**) <br>
2. Run EF Core migrations
    ```bash
    docker-compose run --rm backend-core dotnet ef database update -d
    ```
    This applies the database schema to PostgreSQL.<br>
3. Verify services are running
    ```terminal
    curl http://localhost:5049/

    output:
    hello from neukod backend core!
    ```
    
You now have the backend-core service, PostgreSQL, and Adminer running with Docker Compose.
From here, you can extend the API, add new endpoints, or integrate with other services.