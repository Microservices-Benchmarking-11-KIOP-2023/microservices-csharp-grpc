﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-alpine3.18-amd64 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Search.Service/Pb.Search.Service.csproj", "src/Pb.Search.Service/"]
COPY ["src/Pb.Common/Protos", "src/Pb.Common/Protos"]
RUN dotnet restore "src/Pb.Search.Service/Pb.Search.Service.csproj"
COPY . .
WORKDIR "/src/src/Pb.Search.Service"
RUN dotnet build "Pb.Search.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Search.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Search.Service.dll"]
