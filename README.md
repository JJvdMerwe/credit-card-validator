# Credit Card Validator
A solution that validates credit card numbers and stores them if they are valid.  
The current version is configured for SQLite ease of testing and data is seeded on application start.  

## Clean Architecture
Clean architecture is the chosen architecture for this project. The solution consists of multiple projects: 
* CreditCardValidator (Presentation)
* Application
* Domain
* Infrastructure
* Application.UnitTests

Mediator & CQRS pattern implementations are used to accomplish a separation between the CreditCardValidator and Application projects. 
This promotes decoupling and eases the exchange of presentation technologies. 

UnitOfWork and Generic repository patterns are used to abstract away and decouple persistence technologies to the Infrastructure project.  
This adheres to a fundamental principle of Clean Architecture which states that the application logic should not be tightly coupled to infrastructure technologies that it might need to perform it's business functions.
Technologies should be interchangeable and application code should be written against interfaces, not implementations.

### CreditCardValidator
This is an ASP.NET MVC project that serves as the presentation technology of the solution.  
Controller methods are typically kept thin and composition of business logic is avoided in this project.  
This layer issues commands and queries as requests the Application project by making use of the widely known [MediatR](https://github.com/jbogard/MediatR) package.  
Responses from the Application project make use of Data Transfer Objects (DTOs) to promote the decoupling of our domain Entities from this project.  
Some JavaScript / JQuery validation exists on the Views.CreditCards.Create.cshtml page to avoid posting of invalid form data to the server.  

### Application
This is a C# class library. This is where business logic is composed mostly in the form of queries and commands.  
There are two domains, CreditCards & CreditCardProviders. To promote CQRS the Queries and Commands are kept in separate namespaces.  
Requests and RequestHandler classes are housed in the same file to promote ease of navigation.  
DTOs are kept in another namespace and are using to communicate back to the client.  
Common.Interfaces houses interfaces that can be coded against but should not have implementations in this project.  

### Domain
This is a C# class library. It houses the Entities that describe our Domain.  
Other objects like enterprise wide ValueObjects and enterprise behaviours can be added here when needed.  

### Infrastructure
The ORM of choice is Entity Framework Core. It has been configured for SQLite.  
It is ensured that the database exists on application startup. The database will be created in Persistence.Database if it doesn't exist.  
A DataSeeder is implemented that seeds the CreditCardProvider table with 4 sample records if no records exist on application start.  
Entity Framework specific implementations if the IUnitOfWork and IGenericRepository interfaces exist in the Persistence.EF namespace.  
We can just as easily create implementations of these interfaces for any other ORM technology and swop out the implementation when registering it to the IOC container in program.cs.  


### Application.UnitTests
This is an NUnit Test Project. Testing focussed mostly on testing the SubmitCreditCardCommand.  
[Moq](https://github.com/moq/moq4) is used to test against the IUnitOfWork and IGenericRepository interfaces and our testing framework is also nicely separated from the persistence technology.

