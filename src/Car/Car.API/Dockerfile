#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Car/Car.API/CarApi.csproj", "src/Car/Car.API/"]
RUN dotnet restore "src/Car/Car.API/CarApi.csproj"
COPY . .
WORKDIR "/src/src/Car/Car.API"
RUN dotnet build "CarApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarApi.dll"]