# syntax=docker/dockerfile:1

########################################
# BUILD STAGE
########################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

# Copy hele solution
COPY . .

# Gå ind i projektet
WORKDIR /source/PizzaGrandiosa

# Restore + publish (simpel og stabil)
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

########################################
# RUNTIME STAGE
########################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy compiled app
COPY --from=build /app/publish .

# Start app
ENTRYPOINT ["dotnet", "PizzaGrandiosa.dll"]