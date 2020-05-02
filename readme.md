# A Cross Platform Service Bus Explorer

[![continious-integration](https://github.com/scott-the-programmer/azure-service-bus-explorer/workflows/continious-integration/badge.svg)](https://github.com/scott-the-programmer/azure-service-bus-explorer/actions)
![GitHub](https://img.shields.io/github/license/scott-the-programmer/azure-service-bus-explorer?style=flat)
[![Maintainability](https://api.codeclimate.com/v1/badges/9db0f09ee4418d907ac4/maintainability)](https://codeclimate.com/github/scott-the-programmer/azure-service-bus-explorer/maintainability)
[![Test Coverage](https://api.codeclimate.com/v1/badges/9db0f09ee4418d907ac4/test_coverage)](https://codeclimate.com/github/scott-the-programmer/azure-service-bus-explorer/test_coverage)

This is currently a work in progress

The purpose for this project is to deliver a tool that can be used to manage [Azure Service Bus](https://docs.microsoft.com/bs-latn-ba/azure/service-bus-messaging/service-bus-messaging-overview)

## Development

## Prerequisites

* [Dotnet Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Your own Azure Service Bus Namespace](https://azure.microsoft.com/en-us/services/service-bus/)

## Build

Run the following command to build the project

```bash
dotnet build
```

## Unit Test

Run the following command to unit test the project

```bash
dotnet test --filter Category!=Integration
```

### With Coverage

```bash
dotnet test --filter Category!=Integration \
/p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=../lcov.info
```

## Integration Test

Configure [integration_settings.json](./AzureServiceBusExplorerIntegrationTests/integration_settings.json) with your service bus connection

```bash
dotnet test --filter Category=Integration
```
