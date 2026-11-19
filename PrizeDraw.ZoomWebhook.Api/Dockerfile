FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.csproj /src/
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
# .NET 8+ images listen on 8080 by default and provide a non-root 'app' user
EXPOSE 8080
USER $APP_UID
ENV Logging__Console__FormatterName=
ENTRYPOINT ["dotnet", "PrizeDraw.ZoomWebhook.Api.dll"]
