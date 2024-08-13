#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ReviewService/ReviewService.GRPC/ReviewService.GRPC.csproj", "Services/ReviewService/ReviewService.GRPC/"]
COPY ["Services/ReviewService/ReviewService.Common/ReviewService.Common.csproj", "Services/ReviewService/ReviewService.Common/"]
RUN dotnet restore "./Services/ReviewService/ReviewService.GRPC/ReviewService.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/ReviewService/ReviewService.GRPC"
RUN dotnet build "./ReviewService.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReviewService.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewService.GRPC.dll"]