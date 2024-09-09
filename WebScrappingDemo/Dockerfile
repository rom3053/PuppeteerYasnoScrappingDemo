#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8081
#####################
#PUPPETEER RECIPE
#####################
RUN apt-get update && apt-get -f install && apt-get -y install curl wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
######################
#END PUPPETEER RECIPE
######################
ENV ASPNETCORE_URLS=http://+:8080
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