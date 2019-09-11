FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Health.Checks.In.ASP.NET.Core/Health.Checks.In.ASP.NET.Core.csproj", "Health.Checks.In.ASP.NET.Core/"]
RUN dotnet restore "Health.Checks.In.ASP.NET.Core/Health.Checks.In.ASP.NET.Core.csproj"
COPY . .
WORKDIR "/src/Health.Checks.In.ASP.NET.Core"
RUN dotnet build "Health.Checks.In.ASP.NET.Core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Health.Checks.In.ASP.NET.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Health.Checks.In.ASP.NET.Core.dll"]