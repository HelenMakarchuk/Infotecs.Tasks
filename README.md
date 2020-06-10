# Infotecs.Task_3

#### Технологии

- [x] Angular
- [x] WebApi
- [x] Less
- [x] SignalR
- [x] Docker
- [x] Docker Compose

#### Run

Run application using Docker or Docker Compose.

<details>
  <summary>Docker</summary>
  <p>
      
  ###### DB ######
 
 `docker build ./Database -t db-image` 
 
 `docker run -d --name db-container -p 5436:5432 db-image`
 
  ###### API ######

  ###### Web ######
 
  `docker build ./Source -t web-image`

  `docker run -d -p 63585:8000 --name=web-container web-image`
  </p>
</details>

<details>
  <summary>Docker Compose</summary>
  <p>
 
  `docker-compose stop && docker-compose rm -f && docker-compose build --no-cache && docker-compose up -d --force-recreate --remove-orphans`

  </p>
</details>

#### Debug

<details>
  <summary>PostgreSQL Database</summary>
  <p>
  
  </p>
</details>

<details>
  <summary>ASPNET Core Web API</summary>
  <p>
      
  ###### Set environment variables. Run in cmd on Windows. ######
 
  `setx ASPNETCORE_ENVIRONMENT "Development"`

  `setx ASPNETCORE_URLS "http://*:5080"`
 
  ###### Navigate to project folder. ######
 
  `cd Source/Magazine.API`

  ###### Run project and serve at http://localhost:5080/api. ######
 
  `dotnet run -c Release`

  </p>
</details>

<details>
  <summary>Angular Web Application</summary>
  <p>
 
  ###### Navigate to application folder. ######
  
  `cd Source/Magazine.Web`
  
  ###### Install the dependencies, a new 'node_modules' folder will be created. ######
  `npm install`
  
  ###### Compile the application, a new 'dist' folder will be created. ######
  `ng build --configuration=development`
  
  ###### Serve the application at http://localhost:4200. ######
  `ng serve --configuration=development --open`

  </p>
</details>
