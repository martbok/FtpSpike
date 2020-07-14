#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ChilkatFtp/ChilkatFtp.csproj", "ChilkatFtp/"]
RUN dotnet restore "ChilkatFtp/ChilkatFtp.csproj"
COPY . .
WORKDIR "/src/ChilkatFtp"
RUN dotnet build "ChilkatFtp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChilkatFtp.csproj" -c Release -o /app/publish

FROM base AS final
RUN mkdir -p /var/ftp/upload
RUN mkdir -p /var/ftp/download
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChilkatFtp.dll"]