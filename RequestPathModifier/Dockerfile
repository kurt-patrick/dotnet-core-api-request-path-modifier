#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RequestPathModifier/RequestPathModifier.csproj", "RequestPathModifier/"]
RUN dotnet restore "RequestPathModifier/RequestPathModifier.csproj"
COPY . .
WORKDIR "/src/RequestPathModifier"
RUN dotnet build "RequestPathModifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RequestPathModifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RequestPathModifier.dll"]