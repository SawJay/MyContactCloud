FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["MyContactCloud/MyContactCloud.csproj", "MyContactCloud/"]
COPY ["MyContactCloud.Client/MyContactCloud.Client.csproj", "MyContactCloud.Client/"]
RUN dotnet restore "MyContactCloud/MyContactCloud.csproj"
COPY . .
WORKDIR "/src/MyContactCloud"
RUN dotnet build "MyContactCloud.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyContactCloud.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyContactCloud.dll"]
