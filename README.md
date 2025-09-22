# Sentiment.Api

API RESTful en ASP.NET Core para análisis de sentimientos. Permite registrar comentarios de usuarios, clasificarlos como **positivos**, **negativos** o **neutrales**, y consultarlos por filtros. Incluye persistencia en **SQL Server**.

---

## 📦 Contenido del repo
    Sentiment.Api/
    ├─ Controllers/CommentsController.cs
    ├─ Models/AppDbContext.cs
    ├─ Models/Comment.cs
    ├─ Services/SentimentService.cs
    ├─ SQL/create_table_comments.sql
    ├─ appsettings.json
    ├─ Dockerfile
    ├─ Program.cs
    ├─ launchSettings.json
    └─ Sentiment.Api.http

---

## ✅ Requisitos
- Docker Desktop (Windows)
- SQL Server en Windows (modo mixto habilitado)
- .NET SDK (opcional, solo si ejecutas fuera de Docker)

---

## 📦📦 Cómo clonar repositorio
En terminal, ir a la carpeta donde se va a guardar el proyecto y ejecutar lo siguiente:

    git clone <TU_REPO_URL>
    cd sentiment-api

---

## ⚙️ Configuración de SQL Server (host Windows)
1. En **Server Properties → Security**: habilitar **SQL Server and Windows Authentication mode**.  
2. Reiniciar servicio SQL Server.  
3. Habilitar login `sa` (o crear usuario SQL con permisos).  
4. En **SQL Server Configuration Manager** → *Protocols for MSSQLSERVER*: habilitar **TCP/IP**.  
5. En propiedades de **TCP/IP → IP Addresses**:  
   - Para la IP de tu red local (`192.168.1.71` por ejemplo), `Active = Yes`, `Enabled = Yes`.  
   - En `IPAll`, establecer `TCP Port = 1433`.  
6. Abrir puerto 1433 en Windows Firewall (Inbound Rule).  
7. Crear la base y tabla con el script incluido:

        sqlcmd -S localhost,1433 -U sa -P "YourStrong!Passw0rd" -i .\SQL\create_table_comments.sql

⚠️ Puede cambiar usuario y contraseña por los que usted prefiera

---

## 📄 Script de creación de BD y tabla

    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'sentiment_db')
    BEGIN
        CREATE DATABASE sentiment_db;
    END
    GO

    USE sentiment_db;
    GO

    IF OBJECT_ID(N'dbo.Comments', N'U') IS NULL
    BEGIN
    	CREATE TABLE Comments
    	(
    	    id INT IDENTITY(1,1) PRIMARY KEY,
    		product_id NVARCHAR(100) NOT NULL,
		    user_id NVARCHAR(100) NOT NULL,
		    comment_text NVARCHAR(MAX) NOT NULL,
		    sentiment NVARCHAR(20) NOT NULL,
		    created_at DATETIME2 NOT NULL DEFAULT (SYSUTCDATETIME())
	    );
    END
    GO

## 🔗 Cadena de conexión
Ejemplo (para contenedor Linux conectando a SQL Server en Windows):

    Server=X.X.X.X,1433;Database=sentiment_db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;

⚠️ Cambiar X.X.X.X por la IP asignada en tu red local (Wi-fi o Ethernet).

⚠️ Se recomienda usar variables de entorno (Dockerfile) en lugar de editar appsettings.json.

---

## 💻 Ejecutar en Visual Studio
1. Ir a la carpeta donde se guardó el proyecto.
2. Ejecutar Sentiment.Api.sln, lo cuál va a abrir el proyecto.
3. Una vez abierto el proyecto, abrir el archivo 'appsettings.json'.
4. En **"ConnectionSettings" -> "DefaultConnection"**, cambiar la parte de 'X.X.X.X' por la IP asignada a tu red local.
4. Dar click al botón de ejecutar (asegurarse que esté seleccionada la opción 'Container (Dockerfile)').
6. Abrir en el navegador:  https://localhost:32775/swagger

---

## ▶️ Ejecutar con Docker
⚠️ Antes de construir la imágen, se debe realizar lo siguiente:
- Con cualquier editor de texto abrir el archivo 'appsettings.json' de la carpeta del proyecto.
- En **"ConnectionSettings" -> "DefaultConnection"**, cambiar la parte de 'X.X.X.X' por la IP asignada a tu red local.

Construir imagen:

    docker build -t sentimentapi:dev .

Ejecutar contenedor:

    docker run --rm -e "ConnectionStrings__DefaultConnection=Server=X.X.X.X,1433;Database=sentiment_db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;" -p 5025:80 sentimentapi:dev

Abrir en navegador:

    http://localhost:5025/swagger

---

## 🐳 Alternativa: achivo 'docker-compose.yml'
Si prefieres levantar SQL Server en un contenedor junto con la API:

    version: '3.8'
    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        environment:
          - ACCEPT_EULA=Y
          - MSSQL_SA_PASSWORD=YourStrong!Passw0rd
        ports:
          - "1433:1433"
        volumes:
          - mssql_data:/var/opt/mssql

      api:
        build:
          context: .
          dockerfile: Sentiment.Api/Dockerfile
        depends_on:
          - sqlserver
        environment:
          - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=sentiment_db;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
        ports:
          - "5025:80"

    volumes:
      mssql_data:


Levantar con: docker compose up --build

---

## 📡 Endpoints principales
- POST /api/comments — crea un comentario con análisis de sentimiento.
- GET /api/comments — lista comentarios, con filtros opcionales.
- GET /api/sentiment-summary — devuelve conteos agrupados por sentimiento.

Ejemplo POST /api/comments:

    {
      "product_id": "PROD001",
      "user_id": "USER001",
      "comment_text": "Este producto es excelente, superó mis expectativas."
    }
