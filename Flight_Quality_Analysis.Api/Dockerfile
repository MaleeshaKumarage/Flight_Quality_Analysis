# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Flight_Quality_Analysis.Api.csproj", "."]
RUN dotnet restore "./Flight_Quality_Analysis.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Flight_Quality_Analysis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Flight_Quality_Analysis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy the published output
COPY --from=publish /app/publish .

# Copy the CSV file into the Docker image
COPY Flight_Quality_Analysis.Infrastructure/Data/flights.csv ./Flight_Quality_Analysis.Infrastructure/Data/flights.csv

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Flight_Quality_Analysis.Api.dll"]
