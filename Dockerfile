FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["lyricboxd.csproj", "."]
RUN dotnet restore "./lyricboxd.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "lyricboxd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lyricboxd.csproj" -c Release -o /app/publish

# Use a imagem do ASP.NET Core Runtime 8.0 para a fase final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lyricboxd.dll"]
