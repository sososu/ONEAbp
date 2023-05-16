#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/ONE.Admin.HttpApi.Host/ONE.Admin.HttpApi.Host.csproj", "src/ONE.Admin.HttpApi.Host/"]
COPY ["src/ONE.Admin.Application/ONE.Admin.Application.csproj", "src/ONE.Admin.Application/"]
COPY ["src/ONE.Admin.Domain/ONE.Admin.Domain.csproj", "src/ONE.Admin.Domain/"]
COPY ["src/ONE.Admin.Domain.Shared/ONE.Admin.Domain.Shared.csproj", "src/ONE.Admin.Domain.Shared/"]
COPY ["src/ONE.Admin.Application.Contracts/ONE.Admin.Application.Contracts.csproj", "src/ONE.Admin.Application.Contracts/"]
COPY ["src/ONE.Admin.EntityFrameworkCore/ONE.Admin.EntityFrameworkCore.csproj", "src/ONE.Admin.EntityFrameworkCore/"]
COPY ["src/ONE.Admin.HttpApi/ONE.Admin.HttpApi.csproj", "src/ONE.Admin.HttpApi/"]
RUN dotnet restore "src/ONE.Admin.HttpApi.Host/ONE.Admin.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/ONE.Admin.HttpApi.Host"
RUN dotnet build "ONE.Admin.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ONE.Admin.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ONE.Admin.HttpApi.Host.dll"]