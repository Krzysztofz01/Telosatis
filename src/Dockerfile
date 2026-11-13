FROM node:24-alpine AS web
WORKDIR /src/Telosatis.Web
COPY Telosatis.Web/package.json Telosatis.Web/package-lock.json ./
RUN npm ci
COPY Telosatis.Web/ ./
RUN npm run build
 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Packages.props nuget.config ./
COPY Telosatis.Api/Telosatis.Api.csproj                       Telosatis.Api/
COPY Telosatis.Application/Telosatis.Application.csproj       Telosatis.Application/
COPY Telosatis.Common/Telosatis.Common.csproj                 Telosatis.Common/
COPY Telosatis.Domain/Telosatis.Domain.csproj                 Telosatis.Domain/
COPY Telosatis.Infrastructure/Telosatis.Infrastructure.csproj Telosatis.Infrastructure/
RUN dotnet restore Telosatis.Api/Telosatis.Api.csproj
 
COPY Telosatis.Api/            Telosatis.Api/
COPY Telosatis.Application/    Telosatis.Application/
COPY Telosatis.Common/         Telosatis.Common/
COPY Telosatis.Domain/         Telosatis.Domain/
COPY Telosatis.Infrastructure/ Telosatis.Infrastructure/
 
COPY --from=web /src/Telosatis.Web/dist/ Telosatis.Web/dist/
RUN dotnet publish Telosatis.Api/Telosatis.Api.csproj -c Release -o /app --no-restore -p:SkipWebAppBuild=true
 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
 
ENV ASPNETCORE_HTTP_PORTS=8080
RUN mkdir /var/lib/telosatis && mkdir -p /data && chmod 0777 /data
COPY --from=build /app .
 
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "Telosatis.Api.dll"]

