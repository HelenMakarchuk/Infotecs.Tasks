#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS build
WORKDIR /src
COPY ["Source/Magazine.Web/Magazine.Web.csproj", "Source/Magazine.Web/"]
COPY ["Source/Magazine.Infrastracture/Magazine.Infrastracture.csproj", "Source/Magazine.Infrastracture/"]
COPY ["Source/Magazine.Domain/Magazine.Domain.csproj", "Source/Magazine.Domain/"]
COPY ["Source/Core/Core.csproj", "Source/Core/"]
RUN dotnet restore "Source/Magazine.Web/Magazine.Web.csproj"
COPY . .
WORKDIR "/src/Source/Magazine.Web"
RUN dotnet build "Magazine.Web.csproj" -c Release -o /app/build

RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs

FROM build AS publish
RUN dotnet publish "Magazine.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Magazine.Web.dll"]