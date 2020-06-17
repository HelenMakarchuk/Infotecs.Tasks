FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5082

COPY certificates/localhost.pem /etc/ssl/certs/certificate.pem
COPY certificates/localhost.key /etc/ssl/certs/certificate.key
COPY certificates/localhost.crt /etc/ssl/certs/localhost.crt

RUN ["update-ca-certificates"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "Identity/Identity.csproj"
COPY . .
WORKDIR "/src/Identity"
RUN dotnet build "Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Infotecs.Identity.dll"]
