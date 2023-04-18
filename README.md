# Object Management System (Frontend)
Frontend project for the sample Object Management System application. The backend repository is at https://github.com/jarmanso7/ObjectManagementSystemApi.

## Functional overview

Object Management System is a sample web appliaction that lets the user define objects and establish relationships between those objects.
It is a single page web application and it features a lean user interface with three components, grid of Objects, a grid of relationships and a graph:

![GeneralView](https://user-images.githubusercontent.com/19256433/232626356-adf07dfe-2b42-4d26-a5a8-3004e15e2428.png)

### Grid of Objects

This is the main control of the application. To add a new object, click on "Add New" in the top left corner of the grid. Object records can be edited in-line and deleted by making use of the buttons under the column "Options". This component also enables the user to lookup for objects with autocompletion by Name or by Description. 

![Lookup](https://user-images.githubusercontent.com/19256433/232626597-ada92700-cb7f-473f-8d47-94d756e2531d.png)

 Another option is to use the filters in each column header to search for data with specific criteria:
 
 ![Filters](https://user-images.githubusercontent.com/19256433/232626995-3810e9b4-ef52-4fc1-a5f2-5a0bc1dc4cf2.png)
 
 
### Grid of Relationships

After searching for and selecting a particular object record, the grid of relationships will display the information on the relationships between the selected object and the other objects in the system. It has similar capabilities to the objects grid, enabling the user to delete and edit relationships in-line as well as adding new relationships via button. The column "Type" is a free text field that provides suggestions based on already existing relationships in the system on typing.

Finally note that when selecting a particular object, the graph in the left side of the user interface will display only the objects and relationships relevant to the selected object, mirroring the information shown in the relationships grid. To go back to the full graph of objects and relationships, click on "Clear Selection".

![Selecting and object](https://user-images.githubusercontent.com/19256433/232628113-81c76fa9-0558-48d0-80f1-7f620981341a.png)

## Technology stack

One of the requirements for the assignment was to implement the solution using .NET core. Keeping this in mind, I chose the following technologies to come up with a solution:

![techstack](https://user-images.githubusercontent.com/19256433/232635657-fb5512f7-dd7d-4ca3-95fb-450869b1d2a4.png)

### Hosting

All the different parts of the application, database, backend and front are hosted as Azure resources. I decided to do so I could abstract from the infrastructure-related tasks and focus more on the application itself. Moreover, since one of the requirements is to deliver a solution in .NET Core, Azure is the most suitable cloud platform as opposed to AWS or others.

### Backend

The backend of the application consists of an [Azure Cosmos DB Apache Gremlin](https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/modeling) database resource and a .NET 7 [minimal web API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0). This API serves the information on objects and relationships to the frontend app, and it persist the information in a database. The API is hosted in Azure as a Web App.

A Graph database has been considered instead of a relational database because one of the requirements is to enable the user to create relationships between objects at runtime. The chosen technology is Azure Cosmos DB Apache Gremlin.

### Frontend

The frontend is made up of a single page web application made in [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor). Blazor is a frontend framework that offers similar capabilites as Angular, React or Vue, but it uses C# instead of JavaScript or Typescript. This let me comply with the requirements of writing the solution in (mostly) C# also at the frontend part of the project. Blazor comes in two flavors:

- As a server side .NET hosted application
- As a client side application running entirely on the browser via [WebAssembly](https://webassembly.org/).

The latter has been chosen for this project. The Blazor WASM application is hosted as an (Azure Static Web App)[https://azure.microsoft.com/en-us/products/app-service/static]. In the frontend they are used 2 different Blazor libraries for the following purposes:

- [Radzen](https://www.radzen.com/]https://github.com/Blazor-Diagrams/Blazor.Diagrams): a UI library that provides many useful controls to build up Web Applications in Blazor. It is used in the grid components of the application.
- [Blazor.Diagrams](https://github.com/Blazor-Diagrams/Blazor.Diagrams): a Blazor library that provides components to display data in graphs. In the Object Management System app it is used in the Graph component.

## Design considerations

Most of the transformation, searching and filtering is done in the frontend part of the application. On starting up, the client requests with a Read operation the bulk of the data to the backend. From this point onwards, all the operations of Creation, Deletion and Update of Objects $ Relationships are performed as single operations by making calls to the web API with a small payload and the state of the application is saved while the user interacts with the application in small steps.

The role of the backend API for the most part is that of a mere middleman between the frontend app and the storage in the DB. If the application was to scale and deal with a large number of objects and relationships, a redesign should be considered and change the current philosophy, likely reaching a tradeoff between the size of the backend calls and the amount of records the UI components have to deal with.

To structure the code of the solution, I tried to adhere to the clean architecture design (Domain, Application, Presentation and Infrastructure layers) in the backend solution as well as other good practices such as dependency inversion. As required, I have commented the code (mostly the classes definitions and members) but I believe that in general developers should try to write self explanatory code and refrain from commenting too much. In any case, please don't hesitate to reach out to discuss any part of my proposed solution.

Thank you very much for your time.
