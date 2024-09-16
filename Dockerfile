FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY Project.test/*.csproj ./Project.test/
COPY Project/*.csproj ./Project/
RUN dotnet restore
COPY Project.test/ ./Project.test/
COPY Project/ ./Project/
RUN dotnet build Project.test/54HD.sln --configuration Release
RUN dotnet publish Project.test/54HD.sln --configuration Release --output /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Project.test.dll"]
