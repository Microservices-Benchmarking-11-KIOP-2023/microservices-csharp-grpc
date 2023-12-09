FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-alpine3.18-amd64 AS base
WORKDIR /app
EXPOSE 443  
EXPOSE 80  
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Geo.Service/Pb.Geo.Service.csproj", "src/Pb.Geo.Service/"]
COPY ["src/Pb.Common/Protos", "src/Pb.Common/Protos"]
COPY . .

RUN apt-get update && apt-get install -y unzip
RUN unzip src/Pb.Geo.Service/Data/geo.zip -d src/Pb.Geo.Service/Data
RUN dotnet restore "src/Pb.Geo.Service/Pb.Geo.Service.csproj"

WORKDIR "/src/src/Pb.Geo.Service"
RUN dotnet build "Pb.Geo.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Geo.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Geo.Service.dll"]
