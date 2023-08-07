FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.ApiGateway/Pb.ApiGateway.csproj", "src/Pb.ApiGateway/"]
RUN dotnet restore "src/Pb.ApiGateway/Pb.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/Pb.ApiGateway"
RUN dotnet build "Pb.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.ApiGateway.dll"]
