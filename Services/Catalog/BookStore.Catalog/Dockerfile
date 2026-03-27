FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Services/Catalog/BookStore.Catalog/BookStore.Catalog.csproj", "Services/Catalog/BookStore.Catalog/"]
COPY ["BuildingBlocks/BuildingBlocks.SharedKernel/BuildingBlocks.SharedKernel.csproj", "BuildingBlocks/BuildingBlocks.SharedKernel/"]
COPY ["BuildingBlocks/BuildingBlocks.Chassis/BuildingBlocks.Chassis.csproj", "BuildingBlocks/BuildingBlocks.Chassis/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
RUN dotnet restore "Services/Catalog/BookStore.Catalog/BookStore.Catalog.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/BookStore.Catalog"
RUN dotnet build "./BookStore.Catalog.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookStore.Catalog.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStore.Catalog.dll"]
