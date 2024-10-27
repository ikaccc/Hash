FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackendBlocks.WebApi/BackendBlocks.WebApi.csproj", "BackendBlocks.WebApi/"]
RUN dotnet restore "BackendBlocks.WebApi/BackendBlocks.WebApi.csproj"
COPY . .
WORKDIR "/WebApi"
RUN dotnet build "BackendBlocks.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendBlocks.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendBlocks.WebApi.dll"]