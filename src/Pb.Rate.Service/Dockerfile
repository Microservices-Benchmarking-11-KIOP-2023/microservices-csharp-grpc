FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Rate.Service/Pb.Rate.Service.csproj", "src/Pb.Rate.Service/"]
RUN dotnet restore "src/Pb.Rate.Service/Pb.Rate.Service.csproj"
COPY . .
WORKDIR "/src/src/Pb.Rate.Service"
RUN dotnet build "Pb.Rate.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Rate.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Rate.Service.dll"]
