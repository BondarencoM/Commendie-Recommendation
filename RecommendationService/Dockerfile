#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RecommendationService/RecommendationService.csproj", "RecommendationService/"]
RUN dotnet restore "RecommendationService/RecommendationService.csproj"
COPY . .
WORKDIR "/src/RecommendationService"
RUN dotnet build "RecommendationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecommendationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecommendationService.dll"]