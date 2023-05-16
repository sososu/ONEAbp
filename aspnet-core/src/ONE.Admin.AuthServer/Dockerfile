#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/ONE.Admin.AuthServer/ONE.Admin.AuthServer.csproj", "src/ONE.Admin.AuthServer/"]
COPY ["src/ONE.Admin.EntityFrameworkCore/ONE.Admin.EntityFrameworkCore.csproj", "src/ONE.Admin.EntityFrameworkCore/"]
COPY ["src/ONE.Admin.Domain/ONE.Admin.Domain.csproj", "src/ONE.Admin.Domain/"]
COPY ["src/ONE.Admin.Domain.Shared/ONE.Admin.Domain.Shared.csproj", "src/ONE.Admin.Domain.Shared/"]
RUN dotnet restore "src/ONE.Admin.AuthServer/ONE.Admin.AuthServer.csproj"
COPY . .
WORKDIR "/src/src/ONE.Admin.AuthServer"
RUN dotnet build "ONE.Admin.AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ONE.Admin.AuthServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ONE.Admin.AuthServer.dll"]