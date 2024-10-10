# Base image for the final container running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the entire solution to the build context
COPY . .

# If the .csproj is inside a subfolder, make sure the path is correct
RUN dotnet restore "./Flight_Quality_Analysis.Api/Flight_Quality_Analysis.Api.csproj"

# Build the project
WORKDIR "/src/Flight_Quality_Analysis.Api"
RUN dotnet build "Flight_Quality_Analysis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Flight_Quality_Analysis.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage to build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

## Copy any additional resources
#COPY ./Flight_Quality_Analysis.Infrastructure/Data/flights.csv /path/in/container/

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Flight_Quality_Analysis.Api.dll"]
