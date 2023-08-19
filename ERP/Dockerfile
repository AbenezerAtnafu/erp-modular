#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ERP/ERP.csproj", "ERP/"]
RUN dotnet restore "ERP/ERP.csproj"
COPY . .
WORKDIR "/src/ERP"
RUN dotnet build "ERP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.dll"]