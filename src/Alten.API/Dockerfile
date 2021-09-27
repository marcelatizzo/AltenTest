FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Alten.API.csproj", "./"]
RUN dotnet restore "Alten.API.csproj"
COPY . .
RUN dotnet build "Alten.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Alten.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Alten.API.dll"]