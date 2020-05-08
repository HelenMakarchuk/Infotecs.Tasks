FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base


WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build


WORKDIR /src
COPY ["Source/Magazine.Web/Magazine.Web.csproj", "Source/Magazine.Web/"]

COPY ["Source/Magazine.Infrastracture/Magazine.Infrastracture.csproj", "Source/Magazine.Infrastracture/"]
COPY ["Source/Magazine.Domain/Magazine.Domain.csproj", "Source/Magazine.Domain/"]
COPY ["Source/Core/Core.csproj", "Source/Core/"]

RUN dotnet restore "Source/Magazine.Web/Magazine.Web.csproj"
COPY . .
WORKDIR "/src/Source/Magazine.Web"
RUN dotnet build "Magazine.Web.csproj" -c Release -o /app/build

FROM build AS publish

RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs

RUN dotnet publish "Magazine.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Magazine.Web.dll"]