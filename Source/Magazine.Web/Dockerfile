FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq

WORKDIR /src
COPY ["Magazine.Web/Magazine.Web.csproj", "Magazine.Web/"]

RUN dotnet restore "Magazine.Web/Magazine.Web.csproj"
COPY . .
WORKDIR "/src/Magazine.Web"
RUN dotnet build "Magazine.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Magazine.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Magazine.Web.dll"]
