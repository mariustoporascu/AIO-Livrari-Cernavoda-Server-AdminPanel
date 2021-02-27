FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["OShop.ReactUI/OShop.ReactUI.csproj", "OShop.ReactUI/"]
RUN dotnet restore "OShop.ReactUI/OShop.ReactUI.csproj"
COPY . .
WORKDIR "/src/OShop.ReactUI"
RUN dotnet build "OShop.ReactUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OShop.ReactUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OShop.ReactUI.dll"]
