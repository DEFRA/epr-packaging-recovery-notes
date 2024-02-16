FROM defradigital/dotnetcore-development:dotnet6.0 AS build-env

ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

COPY --chown=dotnet ["EPRN.Portal/EPRN.Portal.csproj", "EPRN.Portal/"]
COPY --chown=dotnet ["EPRN.Common/EPRN.Common.csproj", "EPRN.Common/"]

RUN dotnet restore "EPRN.Portal/EPRN.Portal.csproj"

#USER root
#RUN apk update && apk add --no-cache npm
#USER dotnet
#ENV NODE_EXTRA_CA_CERTS=/usr/local/share/ca-certificates/internal-ca.crt

COPY --chown=dotnet:dotnet . .
WORKDIR /home/dotnet/EPRN.Portal
RUN dotnet publish -c Release -o out

FROM defradigital/dotnetcore:dotnet6.0
COPY --from=build-env --chown=dotnet /home/dotnet/EPRN.Portal/out .

#USER root
#RUN apk add icu-libs libintl
USER dotnet

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

ARG PORT=3000
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

CMD dotnet EPRN.Portal.dll