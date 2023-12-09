FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-alpine3.18-amd64 AS base
WORKDIR /app
EXPOSE 443  
EXPOSE 80  
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Profile.Service/Pb.Profile.Service.csproj", "src/Pb.Profile.Service/"]
COPY ["src/Pb.Common/Protos", "src/Pb.Common/Protos"]
COPY . .

RUN apt-get update && apt-get install -y unzip
RUN unzip src/Pb.Profile.Service/Data/hotels.zip -d src/Pb.Profile.Service/Data
RUN dotnet restore "src/Pb.Profile.Service/Pb.Profile.Service.csproj"

WORKDIR "/src/src/Pb.Profile.Service"
RUN dotnet build "Pb.Profile.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Profile.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Profile.Service.dll"]
