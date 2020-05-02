# A Cross Platform Service Bus Explorer

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

## Integration Test

Configure [integration_settings.json](./AzureServiceBusExplorerIntegrationTests/integration_settings.json) with your service bus connection

```bash
dotnet test --filter Category=Integration
```
