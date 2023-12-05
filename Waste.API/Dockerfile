FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Waste.API/Waste.API.csproj", "Waste.API/"]
COPY ["EPRN.Common/EPRN.Common.csproj", "EPRN.Common/"]
RUN dotnet restore "Waste.API/Waste.API.csproj"
COPY . .
WORKDIR "/src/Waste.API"
RUN dotnet build "Waste.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Waste.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Waste.API.dll"]