FROM defradigital/dotnetcore-development:dotnet6.0 AS build-env

COPY --chown=dotnet . .
RUN dotnet restore "EPRN.PRNS.API/EPRN.PRNS.API.csproj"

#USER root
#RUN apk update && apk add --no-cache npm
#USER dotnet
#ENV NODE_EXTRA_CA_CERTS=/usr/local/share/ca-certificates/internal-ca.crt

COPY --chown=dotnet:dotnet . .
WORKDIR /home/dotnet/EPRN.PRNS.API
RUN dotnet publish -c Release /p:AzureBuild=true -o out

FROM defradigital/dotnetcore:dotnet6.0
COPY --from=build-env --chown=dotnet /home/dotnet/EPRN.PRNS.API/out .

#USER root
#RUN apk add icu-libs libintl
USER dotnet

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

CMD dotnet EPRN.PRNS.API.dll