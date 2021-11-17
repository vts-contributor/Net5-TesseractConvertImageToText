FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build website
COPY . .
WORKDIR /app
RUN dotnet publish -c release -o /AsposeNet5Output/Website

# Final stage / image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app/Website
RUN apt-get update && \
    apt-get install -y --allow-unauthenticated libgdiplus libc6-dev
COPY --from=build /AsposeNet5Output/Website ./
ENTRYPOINT ["dotnet", "AsposeNetCore5.dll"]