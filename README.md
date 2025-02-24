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