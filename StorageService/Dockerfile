FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY StorageService/*.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

#DEFAULT FILE
ENV DATA__FILELOCATION=tmp/visits.log

WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "StorageService.dll"]