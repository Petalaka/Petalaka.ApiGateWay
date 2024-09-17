#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Petalaka.ApiGateWay/Petalaka.ApiGateWay.csproj", "Petalaka.ApiGateWay/"]
RUN dotnet restore "./Petalaka.ApiGateWay/Petalaka.ApiGateWay.csproj"
COPY . .
WORKDIR "/src/Petalaka.ApiGateWay"
RUN dotnet build "./Petalaka.ApiGateWay.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Petalaka.ApiGateWay.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Petalaka.ApiGateWay.dll"]