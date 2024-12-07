FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ManagerService/ManagerService.API/ManagerService.API.csproj", "Services/ManagerService/ManagerService.API/"]
COPY ["Services/ManagerService/ManagerService.Common/ManagerService.Common.csproj", "Services/ManagerService/ManagerService.Common/"]
RUN dotnet restore "Services/ManagerService/ManagerService.API/ManagerService.API.csproj"
COPY . .
WORKDIR "/src/Services/ManagerService/ManagerService.API"
RUN dotnet build "ManagerService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ManagerService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagerService.API.dll"]
