FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Magazine.Identity/Magazine.Identity.csproj", "Magazine.Identity/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "Magazine.Identity/Magazine.Identity.csproj"
COPY . .
WORKDIR "/src/Magazine.Identity"
RUN dotnet build "Magazine.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Magazine.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Magazine.Identity.dll"]
