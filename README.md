### Backend Inventory

Controllers Folder
    UserController.cs (Controller)
    *Controller files handle our endpoints and reference the service layer for logic and data handling

Services Folder
    UserServices.cs ("Service Layer")
    *Handles the logic for our controllers. this also points to the database referencing a context file to store our data in our backend

Context Folder
    DataContext
    *This is the bridge that allows our API to point to our database and interact with/handle our data

Model Folder
    UserModel - define our full set of data for a given user
    UserDTO - user's email and password only (partial set of data)
    PasswordDTO - Hold the hash and salt that our password is comprised of to keep our data safe.

Program.cs 
    CORS
    Connect our Service Layer
    Authentication (for checking our JWT Token)

Database hosted on Azure
Server Login Info: 
username: BackendTokenDemo
password: CodeStack123


### CLI Commands for this project
## API Setup
- dotnet new webapi --use-controllers

## EF Core installs
- dotnet tool install --global dotnet-ef
- dotnet add package Microsoft.EntityFrameworkCore -v 8
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 8
- dotnet add package Microsoft.EntityFrameworkCore.Tools -v 8

## Database Migrations
After setting up your database on Azure, establishing your connection string in your appsettings.json, and logging into the SQL Server extension, you will need your migrations files to have your API communicate with the database appropriately. This is generated and put to use with the following CLI commands. 
- dotnet ef migrations add init
- dotnet ef database update

## JWT Bearer Token Authentication
- dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3