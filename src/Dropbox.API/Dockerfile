#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Dropbox.API/Dropbox.API.csproj", "src/Dropbox.API/"]
COPY ["src/Dropbox.Infrastructure/Dropbox.Infrastructure.csproj", "src/Dropbox.Infrastructure/"]
COPY ["src/Dropbox.Application/Dropbox.Application.csproj", "src/Dropbox.Application/"]
COPY ["src/Dropbox.Domain/Dropbox.Domain.csproj", "src/Dropbox.Domain/"]
RUN dotnet restore "src/Dropbox.API/Dropbox.API.csproj"
COPY . .
WORKDIR "/src/src/Dropbox.API"
RUN dotnet build "Dropbox.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dropbox.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dropbox.API.dll"]
