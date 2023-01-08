# BetCommerce

BetCommerce is a web api and microservices solution.

## Technologies

Asp.net core 6.0 LTS project created using CQRS , DDD and MediatR ,rabbitMq as the broker 
and Sql server for the database *

Others:

- ASP.NET Core
- MediatR
- Mapster
- Fluent Validation
- Serilog
- RabbitMq
- Sql server

## Requirements

- https://www.microsoft.com/net/download/windows#/current the latest .NET Core 0.x SDK

### Running in Visual Studio

- Set Startup projects:
  - Core.Api
  - Core.Api.Authentication
  - Core.Web
  - EmailService

## EF Core & Data Access

- The solution uses these `DbContexts`:

  - `ApplicationDbContext`
  - `IdentityContext`
  
### Database providers:

- SqlServer

## Logging

- We are using `Serilog` with pre-definded following Sinks - white are available in `serilog.json`:

  - Console
  - File
  - MSSqlServer


## How to configure API & Swagger

- For development is running on url - `http://localhost:40767` and swagger UI is available on url - `https://localhost:7184/swagger`
- For swagger UI is configured an API


## RabbitMq service

- To set up rabbitMq via:

### RabbitMQ


"RabbitMqQueueSettings": {
        "HostName": "",
        "VirtualHost": "",
        "UserName": "",
        "Port": "",
        "Password": "",
        "Uri": "",
        "EmailRequestPath": ""
    }


### Solution structure:

- Core.Api:

  - `Core.Api` - project that contains the web api to receive requests from other channels as such mobile, with swagger support as well

- EmailWorker:

  - `EmailWorker` - a microservice project to process email  requests

  ###Project folder structure
  - 'Domain': Core.Domain
  - contain entities, entity configurations, enums, exceptions,DbContext and Dependency injection setup
  
   - 'Application': Application
   - contains service definition and implementation,DTOs,Exceptions and  Dependency injection setup

    - 'External': RabbitMQ
    -contains the rabbimtMq implementation

     - 'Asp.NetCore RIA':Core.Api
     -project that contains the web api to receive requests from other channels as such mobile, with swagger support as well

     - 'Asp.NetCore RIA':Core.Api.Authentication
     -project that contains the Identity logic, with swagger support as well

      - 'External':'MicroServices':EmailWorker
      - a microservice project to process email  requests
