FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["PostOfficeShipment.slnx", "./"]

COPY ["src/PostOfficeShipment.Api/PostOfficeShipment.Api.csproj", "src/PostOfficeShipment.Api/"]
COPY ["src/PostOfficeShipment.Application/PostOfficeShipment.Application.csproj", "src/PostOfficeShipment.Application/"]
COPY ["src/PostOfficeShipment.Domain/PostOfficeShipment.Domain.csproj", "src/PostOfficeShipment.Domain/"]
COPY ["src/PostOfficeShipment.Infrastructure/PostOfficeShipment.Infrastructure.csproj", "src/PostOfficeShipment.Infrastructure/"]

RUN dotnet restore "src/PostOfficeShipment.Api/PostOfficeShipment.Api.csproj"

COPY . .

RUN dotnet publish "src/PostOfficeShipment.Api/PostOfficeShipment.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PostOfficeShipment.Api.dll"]
