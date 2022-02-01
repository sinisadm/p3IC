#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5066

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Dropbox.API/Dropbox.API.csproj", "src/Dropbox.API/"]
COPY ["src/Dropbox.Application/Dropbox.Application.csproj", "src/Dropbox.Application/"]
COPY ["src/Dropbox.Domain/Dropbox.Domain.csproj", "src/Dropbox.Domain/"]
COPY ["src/Dropbox.Infrastructure/Dropbox.Infrastructure.csproj", "src/Dropbox.Infrastructure/"]

RUN dotnet restore "src/Dropbox.API/Dropbox.API.csproj"
COPY . .
#WORKDIR "/src/Dropbox/Dropbox.API"
RUN dotnet build "src/Dropbox/Dropbox.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Dropbox/Dropbox.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dropbox.API.dll"]
