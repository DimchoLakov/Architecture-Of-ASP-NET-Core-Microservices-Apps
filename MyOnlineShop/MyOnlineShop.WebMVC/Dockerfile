FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyOnlineShop.WebMVC/MyOnlineShop.WebMVC.csproj", "MyOnlineShop.WebMVC/"]
COPY ["MyOnlineShop.Common/MyOnlineShop.Common.csproj", "MyOnlineShop.Common/"]
RUN dotnet restore "MyOnlineShop.WebMVC/MyOnlineShop.WebMVC.csproj"
COPY . .
WORKDIR "/src/MyOnlineShop.WebMVC"
RUN dotnet build "MyOnlineShop.WebMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyOnlineShop.WebMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyOnlineShop.WebMVC.dll"]