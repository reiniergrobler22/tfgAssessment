# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY MosEisleyCantina.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out ./

# Expose the application on port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "MosEisleyCantina.dll"]
