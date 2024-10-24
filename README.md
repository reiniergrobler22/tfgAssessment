Mos Eisley Cantina - REST API
This project is a .NET 8.0 REST API for managing a fictional cantina's food and drink menu. It provides functionality for creating, updating, retrieving, and deleting dishes and drinks, along with listing items from the menu. The API also uses SQLite as its database, which can be managed and run through Docker.

Features
CRUD Operations:
Create, Read, Update, Delete for Dishes and Drinks.
Supports unique names for dishes and drinks.
SQLite Integration:
Uses SQLite as a lightweight database, stored locally within Docker containers.
In-memory Caching:
Frequently requested data such as dishes and drinks is cached in memory to improve performance.
Technologies
.NET 8.0: The core framework for building the REST API.
SQLite: The database engine used for storage.
Docker: Containerizes the application and database.
Docker Compose: For managing multi-container Docker applications.
Prerequisites
.NET 8 SDK
Docker
Optional: Visual Studio Code for development.
Setup
Clone the repository:

bash
Copy code
git clone https://github.com/your-username/mos-eisley-cantina.git
cd mos-eisley-cantina
Run Locally Without Docker:

Restore NuGet packages:
bash
Copy code
dotnet restore
Run the API:
bash
Copy code
dotnet run
Run Using Docker:

Build and run the application with Docker Compose:
bash
Copy code
docker-compose up --build
Access the API via:

arduino
Copy code
http://localhost:5000
Docker Instructions
Known Docker Error: NU1301
If you encounter the following error during Docker build:

bash
Copy code
error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json.
This error indicates that the container is unable to reach the NuGet server. You can solve it by configuring Docker to use a local NuGet source or by ensuring your internet connection is stable.

Workaround:

Add a local NuGet source by creating a nuget.config file:

xml
Copy code
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <!-- Add your local source here if required -->
  </packageSources>
</configuration>
Place the nuget.config file in the root of your project and update your Dockerfile to copy it:

Dockerfile
Copy code
COPY nuget.config .
Rebuild the Docker image.

Docker Volumes for SQLite Database
To persist the SQLite database, Docker volumes are used. If you need to access the data outside the container or keep the database between container runs, make sure the volume is configured properly in the docker-compose.yml file:

yaml
Copy code
volumes:
  - ./data:/app/data
This mounts the local data folder to the container, ensuring the SQLite database persists.

Google OAuth Error
If you're integrating Google OAuth and run into issues, such as incorrect credentials or authorization errors:

Ensure Proper Google Client Credentials:

Make sure you have set up your OAuth 2.0 credentials in the Google Cloud Console.
Update your appsettings.json with the correct ClientId and ClientSecret values for Google.
Common Error:

javascript
Copy code
Error: redirect_uri_mismatch
This error occurs when the redirect URI in the OAuth request doesn't match the one registered with Google. Make sure the registered redirect URI in the Google Cloud Console matches the one being used in your code.

Running Migrations
To run migrations for the SQLite database, use the following command in the container:

bash
Copy code
dotnet ef migrations add InitialCreate
dotnet ef database update
Make sure Entity Framework Core tools are installed and configured in your project.
