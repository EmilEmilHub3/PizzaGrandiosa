# syntax=docker/dockerfile:1

########################################
# BUILD STAGE
########################################
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /source

# Copy only project files first (better caching)
COPY PizzaWebAPI/*.csproj PizzaWebAPI/
COPY PizzaModels/*.csproj PizzaModels/
COPY PizzaWeb/*.csproj PizzaWeb/

# Restore dependencies
RUN dotnet restore PizzaWebAPI/PizzaWebAPI.csproj

# Copy the rest of the source code
COPY . .

WORKDIR /source/PizzaWebAPI

# Publish application
RUN dotnet publish -c Release -o /app --no-restore

########################################
# RUNTIME STAGE
########################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

WORKDIR /app

# Copy built app from build stage
COPY --from=build /app .

# Run as non-root user
USER $APP_UID

# Start application
ENTRYPOINT ["dotnet", "PizzaWebAPI.dll"]