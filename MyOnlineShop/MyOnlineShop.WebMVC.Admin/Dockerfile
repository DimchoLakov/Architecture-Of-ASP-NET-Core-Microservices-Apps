FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MyOnlineShop.WebMVC.Admin/MyOnlineShop.WebMVC.Admin.csproj", "MyOnlineShop.WebMVC.Admin/"]
COPY ["MyOnlineShop.Common/MyOnlineShop.Common.csproj", "MyOnlineShop.Common/"]
RUN dotnet restore "MyOnlineShop.WebMVC.Admin/MyOnlineShop.WebMVC.Admin.csproj"
COPY . .
WORKDIR "/src/MyOnlineShop.WebMVC.Admin"
RUN dotnet build "MyOnlineShop.WebMVC.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyOnlineShop.WebMVC.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyOnlineShop.WebMVC.Admin.dll"]