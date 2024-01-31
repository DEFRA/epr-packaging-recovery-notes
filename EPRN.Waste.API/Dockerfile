FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "EPRN.Waste.API/EPRN.Waste.API.csproj"

WORKDIR "/src/EPRN.Waste.API"
RUN dotnet build "EPRN.Waste.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EPRN.Waste.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EPRN.Waste.API.dll"]