FROM mcr.microsoft.com/dotnet/aspnet:9.0  AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
RUN dotnet dev-certs https -v -ep openiddict.pfx -p 2e89d73c-aa44-4e3c-ab78-68988c8179e1

FROM base AS final
WORKDIR /app
COPY --from=build /src/openiddict.pfx .
COPY ./src/StoreSample.HttpApi.Host/bin/Release/net9.0/publish .

ENTRYPOINT ["dotnet", "StoreSample.HttpApi.Host.dll"]