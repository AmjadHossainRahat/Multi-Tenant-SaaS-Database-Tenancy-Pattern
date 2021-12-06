
# Multi-Tenant SaaS Database Tenancy Pattern experiment

It's an experiemntal sample project to explore [Multi-tenant SaaS database tenancy pattern](https://docs.microsoft.com/en-us/azure/azure-sql/database/saas-tenancy-app-design-patterns)

## Framework, Tools and libraries

- [netcore WebAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio) - version: 3.1
- [Azure Cosmos](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction) - used Azure Cosmos Emulator during Development
- [xUnit](https://xunit.net/) - for unit tests
- [Moq](https://www.moqthis.com/moq4/) - for mocking dependencies
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - version: 3.1
- [Kledex](https://lucabriguglia.github.io/Kledex/) - To simplify the implemnetation of DDD and CQRS
- [Fluent Validation](https://fluentvalidation.net/) - To validate Command class object

## Architecural style, Design and patterns
- [Microservice architecture](https://microservices.io/)
- [DDD Design](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [CQRS pattern](https://martinfowler.com/bliki/CQRS.html)
- [Mediator pattern](https://refactoring.guru/design-patterns/mediator)


## What it does

- User can Register as non-paid user
    -
    - Few records (including dummy contents) gets created in a "shared database"
    - A mapping gets created into "User Management Database"
- User can subscribe as a paid user
    -
    - A tenant specific database gets created dynamically based on tenant-id found in the header of the request
    - User and its related data copied and gets inserted in new "Tenant Specific Database" after some modification 
    - Mapping for the user in "User Managemnt Database" gets updated
    - User and its related records gets deleted from the "Shared Database"
- User can unsubscribe to become a non-paid user
    -
    - User and its related data goes back to "Shared Database"
    - Mapping for the user in "User Managemnt Database" gets updated
    - The tennat specific database that was created for the user gets deleted


## API Reference
  Note: A [Postman](https://www.postman.com/) Collection export is available in the repository: [link](https://github.com/AmjadHossainRahat/multi-tenancy/blob/main/MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.postman_collection.json)

#### Health check endpoint

```http
  GET /command/Ping
```

Expected response:
```json
  Status: 200 OK
  Pong
```

#### Register a user

```http
  POST /command/Register
```

Body:

| Key | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `CorrelationId` | `GUID` | **Required**, mostly used for logging |
| `Email` | `String` | **Required**, Email ID of the user |
| `ContactNumber` | `String` | Contact number of the user |
| `FirstName` | `String` | First name of the user |
| `LastName` | `String` | Last name of the user |

Sample response:
```json
  Status: 200 OK
  {
    "id": "9cecd34b-1e7d-43a5-959f-c152e5554d2c",
    "email": "hello@mail.com",
    "firstName": "John",
    "lastName": "Doe"
  }
```

#### Subscribe an existing user

```http
  POST /command/Subscribe
```

Body:
| Key | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `CorrelationId` | `GUID` | **Required**, mostly used for logging |
| `Id` | `GUID` | **Required**, Users ID |
| `Amount` | `Decimal` | **Required**, The amount user paying |

Headers:
| Key | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `Tenant-Id` | `GUID` | **Required**, Tenant ID of the user |

Sample response:
```json
  Status: 200 OK
  {
    "id": "9cecd34b-1e7d-43a5-959f-c152e5554d2c",
    "tenantId": "253c40a9-7ebf-4ea3-90bc-e2428ec24bfa",
    "email": "hello@mail.com",
    "firstName": "John",
    "lastName": "Doe"
}
```

#### Cancel subscription of a user

```http
  POST /command/Unsubscribe
```

Body:

| Key | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `CorrelationId` | `GUID` | **Required**, mostly used for logging |
| `Id` | `GUID` | **Required**, Users ID |

Headers:
| Key | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `Tenant-Id` | `GUID` | **Required**, Tenant ID of the user |

Sample response:
```json
  Status: 204 No Content
```

## To do:
- Use Azure AD to keep record of users
- Refactoring:
  - Introduce Domain Service layer to reduce responsibilities of Command handlers
  - Add more unit tests to improve code coverage
  - Use view model class instead of C# objects (like new {})

## License

[Apache-2.0](https://licenses.nuget.org/Apache-2.0)
