#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["FluentRenciFtp/FluentRenciFtp.csproj", "FluentRenciFtp/"]
RUN dotnet restore "FluentRenciFtp/FluentRenciFtp.csproj"
COPY . .
WORKDIR "/src/FluentRenciFtp"
RUN dotnet build "FluentRenciFtp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluentRenciFtp.csproj" -c Release -o /app/publish

FROM base AS final
RUN mkdir -p /var/ftp/upload
RUN mkdir -p /var/ftp/download
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluentRenciFtp.dll"]