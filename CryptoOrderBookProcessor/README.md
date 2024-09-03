# OrderBook Microservice Documentation

## Overview

The **OrderBook Microservice** is a system designed to process and store real-time order book data from a WebSocket feed into a MongoDB database. The microservice is built using .NET and follows various design patterns to ensure scalability, maintainability, and testability.

## Project Structure

The project is organized into multiple layers, each responsible for a specific aspect of the system. The key layers include:

1. **Application Layer**: Contains the business logic and service interfaces.
2. **Domain Layer**: Contains the core business entities and domain interfaces.
3. **Infrastructure Layer**: Responsible for database interaction and external system integrations.
4. **Worker Layer**: The entry point for background services that run the core functionalities.
5. **Test Layer**: Contains unit and integration tests for the microservice.

### Project Organization

```
OrderBookMicroservice/
├── OrderBookMicroservice.Application/
│   ├── Configurations/
│   │   └── ApplicationServicesConfiguration.cs
│   ├── Interfaces/
│   │   ├── IMetricsService.cs
│   │   ├── IOrderBookProcessingService.cs
│   │   └── IWebSocketService.cs
│   ├── Services/
│   │   ├── MetricsService.cs
│   │   ├── OrderBookProcessingService.cs
│   │   └── WebSocketService.cs
│   └── UseCases/
│       └── SaveOrderBookData.cs
├── OrderBookMicroservice.Domain/
│   ├── Entities/
│   │   ├── MetricsSnapshot.cs
│   │   ├── Order.cs
│   │   └── OrderBook.cs
│   └── Interfaces/
│       └── IOrderBookRepository.cs
├── OrderBookMicroservice.Infrastructure/
│   ├── Configurations/
│   │   └── MongoDbConfiguration.cs
│   └── Repositories/
│       └── MongoOrderBookRepository.cs
├── OrderBookMicroservice.Worker/
│   ├── Dependencies/
│   ├── Properties/
│   ├── Dockerfile
│   ├── Program.cs
│   └── Worker.cs
└── OrderBookMicroservice.Test/
    ├── Dependencies/
    ├── Fixtures/
    │   ├── MetricsServiceFixture.cs
    │   ├── MongoOrderBookRepositoryFixture.cs
    │   ├── WebSocketServiceFixture.cs
    │   └── WorkerFixture.cs
    ├── IntegrationTests/
    │   ├── OrderBookIntegrationTests.cs
    │   ├── WebSocketIntegrationTests.cs
    │   └── WorkerIntegrationTests.cs
    ├── Repositories/
    │   └── MongoOrderBookRepositoryTests.cs
    ├── Services/
    │   ├── MetricsServiceTests.cs
    │   ├── OrderBookProcessingServiceTests.cs
    │   └── WebSocketServiceTests.cs
    ├── UseCases/
    │   └── SaveOrderBookDataTests.cs
    └── GlobalUsings.cs
```


## Application Layer

### Configurations

- **ApplicationServicesConfiguration.cs**: This class is responsible for configuring the application services using the Dependency Injection pattern. It registers services such as `IWebSocketService`, `IOrderBookProcessingService`, and `IMetricsService` as singletons or transient services.

### Interfaces

- **IWebSocketService**: Interface for WebSocket communication, defining methods for connecting, subscribing to order books, and receiving messages.
- **IOrderBookProcessingService**: Interface for processing WebSocket messages into `OrderBook` entities.
- **IMetricsService**: Interface for processing order book data and calculating metrics periodically.

### Services

- **WebSocketService**: Implementation of `IWebSocketService`. Manages WebSocket connections, subscriptions, and message reception from a real-time order book feed.
- **OrderBookProcessingService**: Implementation of `IOrderBookProcessingService`. Processes incoming WebSocket messages, extracting order book data, and converting it into `OrderBook` entities.
- **MetricsService**: Implementation of `IMetricsService`. Processes order book data to track prices and quantities and periodically calculates metrics such as max, min, and average prices.

### UseCases

- **SaveOrderBookData**: Use case that handles the saving of `OrderBook` data into a MongoDB repository. It utilizes the `IOrderBookRepository` from the Infrastructure layer.

## Domain Layer

### Entities

- **OrderBook.cs**: Represents an order book, containing lists of bids and asks along with the instrument identifier.
- **Order.cs**: Represents a single order in the order book, with properties for price and quantity.
- **MetricsSnapshot.cs**: A snapshot of current metrics, including counts of prices and quantities for both BTC and ETH.

### Interfaces

- **IOrderBookRepository**: Interface defining the contract for saving `OrderBook` data to a repository.

## Infrastructure Layer

### Configurations

- **MongoDbConfiguration.cs**: Configures MongoDB-related services, such as the MongoDB client and database, and registers the `IOrderBookRepository` implementation.

### Repositories

- **MongoOrderBookRepository.cs**: Implementation of `IOrderBookRepository`, responsible for saving `OrderBook` data to a MongoDB collection.

## Worker Layer

### Worker.cs

- **Worker**: A background service that orchestrates the main workflow of the microservice. It handles WebSocket connection, data reception, processing, and metrics calculation. The class follows the BackgroundService pattern from .NET, ensuring that the service runs in the background until the application shuts down.

## Design Patterns Used

### 1. **Dependency Injection**

The microservice extensively uses the Dependency Injection (DI) pattern to manage the dependencies between classes. This pattern allows for greater flexibility, easier testing, and better separation of concerns. Services like `IWebSocketService`, `IOrderBookProcessingService`, and `IMetricsService` are all injected into the `Worker` class and other services as needed.

### 2. **Singleton Pattern**

Certain services, such as `MetricsService`, are registered as singletons in the DI container. The Singleton pattern ensures that a single instance of these services is created and used throughout the application's lifetime, which is essential for services that maintain state, such as metrics tracking.

### 3. **Factory Pattern**

The `MongoDbConfiguration` class indirectly uses the Factory pattern to create and configure instances of the MongoDB client and database, which are then registered in the DI container.

### 4. **Snapshot Pattern**

The `MetricsSnapshot` class represents a Snapshot pattern, where a current state of metrics is captured and can be accessed without directly exposing the internal state of the `MetricsService`. This pattern is useful for safely sharing state across different parts of the system without risking unintended modifications.

### 5. **Background Service Pattern**

The `Worker` class implements the Background Service pattern by extending `BackgroundService`. This pattern is used to run long-running processes in the background, which in this case includes receiving WebSocket messages and processing them continuously.

## Testing

### Fixtures

Fixtures are used in testing to set up shared context across multiple test methods. They help in reducing redundancy and managing setup/teardown logic more effectively.

### Test Structure

Tests are organized into the following categories:

- **Services Tests**: Unit tests for application services like `MetricsService`, `WebSocketService`, and `OrderBookProcessingService`.
- **Repositories Tests**: Unit tests for the MongoDB repository implementation.
- **Integration Tests**: Tests that verify the interaction between various components, such as how the `Worker` class integrates with services and repositories.

## Conclusion

The OrderBook Microservice is a well-architected system utilizing various design patterns to ensure that the codebase is modular, testable, and maintainable. The use of Dependency Injection and other patterns allows for easy extension and modification of the system, which is essential for handling the complexities of real-time data processing.



## docker build -t cryptoorderbookprocessor:latest -f CryptoOrderBookProcessor.Worker/Dockerfile .
