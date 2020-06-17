FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5080

COPY certificates/localhost.crt /etc/ssl/certs/myroot.crt

RUN ["update-ca-certificates"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Magazine.API/Magazine.API.csproj", "Magazine.API/"]
COPY ["Magazine.Infrastracture/Magazine.Infrastracture.csproj", "Magazine.Infrastracture/"]
COPY ["Magazine.Domain/Magazine.Domain.csproj", "Magazine.Domain/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "Magazine.API/Magazine.API.csproj"
COPY . .
WORKDIR "/src/Magazine.API"
RUN dotnet build "Magazine.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Magazine.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Infotecs.Magazine.API.dll"]
