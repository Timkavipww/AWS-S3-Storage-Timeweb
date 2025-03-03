FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

EXPOSE 80

COPY . .


RUN dotnet restore .dotnetaws.csproj
RUN dotnet publish .dotnetaws.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .



ENTRYPOINT ["dotnet", ".dotnetaws.dll"]