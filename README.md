# BlogEngine

A blogging engine based on ASP.NET Core 3.1

## Run on local
- Clone the repository
- To create the database for this project use the local instance of SQL Server **(localdb)\MSSQLLocalDB** and execute this script: https://raw.githubusercontent.com/sfspacov/BlogEngine/main/BlogEngine/BlogEngine.Core/SQLScripts/Generate-Database.sql
- Set the following project as startup and run it
  - **BlogEngine.Api**

## Credentials
- Role Writer: **shakespeare@email.uk**, **dostoievski@email.ru**
- Role Editor: **editor@email.com**
- Role Public: **public@email.com**
- Role Admin: **admin@email.com**
- Password is the same for all users: **abc123**

- Role Admin is used to manage Blog's Accounts/Roles (the following endpoints):
  - GET /api/Accounts/users
  - GET /api/Accounts/userById
  - PUT /api/Accounts/update/:email
  - DEL /api/Accounts/:id
  - POST /api/Accounts/assignRole
  - POST /api/Accounts/removeRole

## Test the API
- There are two options to test the API:
  - **Postman collection** (RECOMMENDED!!): https://raw.githubusercontent.com/sfspacov/BlogEngine/main/BlogEngine.API.postman_collection.json
  - Swagger page https://localhost:44328/swagger/index.html 

## Unit tests
- There are Unit Tests for all layers: Api, Core and Shared
- To run the Unit Tests, open the Visual Studio and go to Test > Run All Tests

## Technologies used
The project based on modern front edge technologies:
 - C#
 - ASP.NET Core
 - ASP.NET Core Web API
 - Sql Server
 - Entity Framework Core
 - Identity (Authentication/Authorization)
 - Json and JWT
 - Dependency Injection/IoC
 - AutoMapper
 - Postman Collection
 - Swagger
 - NUnit and Moq for unit tests
 
 ## Features
 - Ability to create and manage
   - Post
   - Review
   - Comment
   - User
 - Pagination
 - Register/Login system
 - Json on requests and responses
 - Swagger API documentation
 - Exception handling
