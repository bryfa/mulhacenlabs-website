# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/MulhacenLabs.Website/mulhacenlabs-website.csproj", "src/MulhacenLabs.Website/"]
RUN dotnet restore "src/MulhacenLabs.Website/mulhacenlabs-website.csproj"

COPY . .
RUN dotnet publish "src/MulhacenLabs.Website/mulhacenlabs-website.csproj" \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "mulhacenlabs-website.dll"]
