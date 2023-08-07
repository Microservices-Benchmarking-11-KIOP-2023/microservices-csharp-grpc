FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pb.Profile.Service/Pb.Profile.Service.csproj", "src/Pb.Profile.Service/"]
RUN dotnet restore "src/Pb.Profile.Service/Pb.Profile.Service.csproj"
COPY . .
WORKDIR "/src/src/Pb.Profile.Service"
RUN dotnet build "Pb.Profile.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pb.Profile.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pb.Profile.Service.dll"]
