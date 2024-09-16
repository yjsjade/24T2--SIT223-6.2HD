# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj files and restore dependencies
COPY Project/54HD.csproj ./Project/
COPY Project.test/54HD.csproj ./Project.test/
RUN dotnet restore Project/54HD.csproj
RUN dotnet restore Project.test/54HD.csproj

# Copy the rest of the source code
COPY Project/. ./Project/
COPY Project.test/. ./Project.test/

# Build the application
RUN dotnet build Project/54HD.sln --configuration Release

# Publish the application to the /publish folder
RUN dotnet publish Project/54HD.sln -c Release -o /app/publish

# Use the ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "54HD.dll"]
