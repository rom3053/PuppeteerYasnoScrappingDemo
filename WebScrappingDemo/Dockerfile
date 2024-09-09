#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080
RUN sudo apt install libxkbcommon0 libxshmfence1 ca-certificates fonts-liberation libasound2 libatk-bridge2.0-0 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgbm1 libgcc1 libglib2.0-0 libgtk-3-0 libnspr4 libnss3 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 lsb-release wget xdg-utils
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebScrappingDemo/WebScrappingDemo.csproj", "WebScrappingDemo/"]
RUN dotnet restore "./WebScrappingDemo/WebScrappingDemo.csproj"
COPY . .
WORKDIR "/src/WebScrappingDemo"
RUN dotnet build "./WebScrappingDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./WebScrappingDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebScrappingDemo.dll"]