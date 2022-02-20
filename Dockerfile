#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["src/BabyCareApi/BabyCareApi.csproj", "."]
RUN dotnet restore "BabyCareApi.csproj"
COPY src/BabyCareApi .
RUN dotnet build "BabyCareApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BabyCareApi.csproj" -c Release -o /app/publish /p:GenerateDocumentationFile=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BabyCareApi.dll"]