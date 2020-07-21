FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyOnlineShop.ShoppingCart/MyOnlineShop.ShoppingCart.csproj", "MyOnlineShop.ShoppingCart/"]
COPY ["MyOnlineShop.Common/MyOnlineShop.Common.csproj", "MyOnlineShop.Common/"]
RUN dotnet restore "MyOnlineShop.ShoppingCart/MyOnlineShop.ShoppingCart.csproj"
COPY . .
WORKDIR "/src/MyOnlineShop.ShoppingCart"
RUN dotnet build "MyOnlineShop.ShoppingCart.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyOnlineShop.ShoppingCart.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyOnlineShop.ShoppingCart.dll"]