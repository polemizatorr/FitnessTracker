# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

# Copy the project file and restore dependencies
COPY ./*.csproj ./
RUN dotnet restore

# Copy the entire project directory
COPY ./ ./

# Build the project
RUN dotnet publish -c Release -o /app

# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build /app .

EXPOSE 80
ENTRYPOINT ["dotnet", "FitnessTracker.WebAPI.dll"]
