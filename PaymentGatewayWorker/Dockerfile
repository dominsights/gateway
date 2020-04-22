FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PaymentGatewayWorker/PaymentGatewayWorker.csproj", "PaymentGatewayWorker/"]
COPY ["MongoDbRepository/MongoDbRepository.csproj", "MongoDbRepository/"]
COPY ["RabbitMQService/RabbitMQService.csproj", "RabbitMQService/"]
RUN dotnet restore "PaymentGatewayWorker/PaymentGatewayWorker.csproj"
COPY . .
WORKDIR "/src/PaymentGatewayWorker"
RUN dotnet build "PaymentGatewayWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentGatewayWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGatewayWorker.dll"]