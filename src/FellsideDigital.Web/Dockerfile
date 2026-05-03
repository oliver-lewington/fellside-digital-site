# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN mkdir -p /app/keys && chown $APP_UID /app/keys
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage — needs Node.js for Tailwind CSS compilation
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Install Node.js (LTS)
RUN apt-get update && apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Copy project files first for layer caching
COPY FellsideDigital.UI/FellsideDigital.UI.csproj FellsideDigital.UI/
COPY FellsideDigital.Web/FellsideDigital.Web.csproj FellsideDigital.Web/

RUN dotnet restore FellsideDigital.Web/FellsideDigital.Web.csproj

# Copy remaining source
COPY FellsideDigital.UI/ FellsideDigital.UI/
COPY FellsideDigital.Web/ FellsideDigital.Web/

WORKDIR /src/FellsideDigital.Web

# Install npm dependencies (needed for Tailwind)
RUN npm install

# Publish in release mode (Tailwind is also compiled via MSBuild targets)
RUN dotnet publish FellsideDigital.Web.csproj -c Release -o /app/publish --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FellsideDigital.Web.dll"]