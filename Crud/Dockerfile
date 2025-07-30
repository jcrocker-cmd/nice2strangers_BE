
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install EF Tools globally in this stage
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy project and restore dependencies
COPY ["Crud/Crud.csproj", "Crud/"]
RUN dotnet restore "./Crud/Crud.csproj"
COPY . .
WORKDIR "/src/Crud"

# Build the project
RUN dotnet build "./Crud.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Optional: Run EF Migrations (uncomment if needed)
RUN dotnet ef database update

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Crud.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crud.dll"]
