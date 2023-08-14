FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-alpine3.18-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Geo.Service/Pb.Geo.Service.csproj", "src/Pb.Geo.Service/"]
COPY ["src/Pb.Common/Protos", "src/Pb.Common/Protos"]
RUN dotnet restore "src/Pb.Geo.Service/Pb.Geo.Service.csproj"
COPY . .
WORKDIR "/src/src/Pb.Geo.Service"
RUN dotnet build "Pb.Geo.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Geo.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Geo.Service.dll"]
