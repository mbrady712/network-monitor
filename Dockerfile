# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY network-monitor/*.csproj ./
RUN dotnet restore

# Copy the rest of the application files and build the app
COPY network-monitor/ ./
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /out .

# Expose the port the application listens on
EXPOSE 8080

# Run the application
CMD ["dotnet", "NetworkMonitor.dll"]
