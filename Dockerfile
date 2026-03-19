FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NickysManicurePedicure.slnx ./
COPY NickysManicurePedicure/*.csproj NickysManicurePedicure/
COPY NickysManicurePedicure.Tests/*.csproj NickysManicurePedicure.Tests/

RUN dotnet restore NickysManicurePedicure/NickysManicurePedicure.csproj

COPY . .

RUN dotnet publish NickysManicurePedicure/NickysManicurePedicure.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p /app/App_Data /app/DataProtectionKeys

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "NickysManicurePedicure.dll"]
