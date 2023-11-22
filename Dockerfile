FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EncryptEvoCoreApi/EncryptEvoCoreApi.csproj", "EncryptEvoCoreApi/"]
RUN dotnet restore "EncryptEvoCoreApi/EncryptEvoCoreApi.csproj"
COPY . .
WORKDIR "/src/EncryptEvoCoreApi"
RUN dotnet build "EncryptEvoCoreApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EncryptEvoCoreApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EncryptEvoCoreApi.dll"]
