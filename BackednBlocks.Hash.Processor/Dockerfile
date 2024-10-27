FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BackendBlocks.Hash.Processor/BackendBlocks.Hash.Processor.csproj", "BackendBlocks.Hash.Processor/"]
RUN dotnet restore "BackendBlocks.Hash.Processor/BackendBlocks.Hash.Processor.csproj"
COPY . .
WORKDIR "/BackendBlocks.Hash.Processor"
RUN dotnet build "BackendBlocks.Hash.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendBlocks.Hash.Processor.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendBlocks.Hash.Processor.dll"]