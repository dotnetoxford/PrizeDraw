FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src

COPY *.csproj /src/
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /src/bin/Release/net6.0/publish .
EXPOSE 80
ENV Logging__Console__FormatterName=
ENTRYPOINT dotnet PrizeDraw.ZoomWebhook.Api.dll
