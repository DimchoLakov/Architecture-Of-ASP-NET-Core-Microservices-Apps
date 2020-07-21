FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyOnlineShop.Catalog/MyOnlineShop.Catalog.csproj", "MyOnlineShop.Catalog/"]
COPY ["MyOnlineShop.Common/MyOnlineShop.Common.csproj", "MyOnlineShop.Common/"]
RUN dotnet restore "MyOnlineShop.Catalog/MyOnlineShop.Catalog.csproj"
COPY . .
WORKDIR "/src/MyOnlineShop.Catalog"
RUN dotnet build "MyOnlineShop.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyOnlineShop.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyOnlineShop.Catalog.dll"]
