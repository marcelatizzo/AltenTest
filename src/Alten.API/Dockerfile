FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV ASPNETCORE_ENVIRONMENT=Development

COPY bin/Release/net6.0/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "Alten.API.dll"]