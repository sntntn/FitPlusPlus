FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/AnalyticsService/AnalyticsService.API/AnalyticsService.API.csproj", "Services/AnalyticsService/AnalyticsService.API/"]
COPY ["Services/AnalyticsService/AnalyticsService.Common/AnalyticsService.Common.csproj", "Services/AnalyticsService/AnalyticsService.Common/"]
RUN dotnet restore "Services/AnalyticsService/AnalyticsService.API/AnalyticsService.API.csproj"
COPY . .
WORKDIR "/src/Services/AnalyticsService/AnalyticsService.API"
RUN dotnet build "AnalyticsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AnalyticsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnalyticsService.API.dll"]
