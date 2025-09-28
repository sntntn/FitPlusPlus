FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiraj csproj fajlove
COPY ["Services/NutritionService/NutritionService.API/NutritionService.API.csproj", "Services/NutritionService/NutritionService.API/"]
COPY ["Common/ConsulConfig.Settings/ConsulConfig.Settings.csproj", "Common/ConsulConfig.Settings/"]
COPY ["Common/EventBus.Messages/EventBus.Messages.csproj", "Common/EventBus.Messages/"]

# Restore
RUN dotnet restore "Services/NutritionService/NutritionService.API/NutritionService.API.csproj"

# Kopiraj ceo projekat
COPY . .

# Build
WORKDIR "/src/Services/NutritionService/NutritionService.API"
RUN dotnet build "NutritionService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NutritionService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NutritionService.API.dll"]


