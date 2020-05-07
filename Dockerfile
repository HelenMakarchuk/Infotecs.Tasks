#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["Source/Magazine.Web.Docker/Magazine.Web.Docker.csproj", "Source/Magazine.Web.Docker/"]

COPY ["Source/Magazine.Infrastracture/Magazine.Infrastracture.csproj", "Source/Magazine.Infrastracture/"]
COPY ["Source/Magazine.Domain/Magazine.Domain.csproj", "Source/Magazine.Domain/"]
COPY ["Source/Core/Core.csproj", "Source/Core/"]

RUN dotnet restore "Source/Magazine.Web.Docker/Magazine.Web.Docker.csproj"
COPY . .
WORKDIR "/src/Source/Magazine.Web.Docker"
RUN dotnet build "Magazine.Web.Docker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Magazine.Web.Docker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Magazine.Web.Docker.dll"]