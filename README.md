# Infotecs.Task_3

#### Технологии

- [x] Angular
- [x] WebApi
- [x] Less
- [x] SignalR
- [x] Docker
- [x] Docker Compose

#### Run

Run application using Docker Compose or Docker.

<details>
  <summary>Docker Compose</summary>
  <p>

  Run in cmd on Windows docker host to get default certificate location:
  echo %USERPROFILE%

  Copy and paste ${HOME}/.aspnet/https/localhost.pfx to location obtained above

  Run in cmd on Windows docker host to trust certificate
  dotnet dev-certs https --trust

  `docker-compose stop && docker-compose rm -f && docker-compose build --no-cache && docker-compose up -d --force-recreate --remove-orphans`

  <details>
    <summary>For docker compose offline use</summary>

    On machine with internet access:
    
    docker-compose stop && docker-compose rm -f && docker-compose build --no-cache && docker-compose up -d --force-recreate --remove-orphans

    docker save postgres -o postgres.tar
    docker save magazine-api-image -o magazine-api-image.tar
    docker save infotecs-identity-image -o infotecs-identity-image.tar
    docker save magazine-web-image -o magazine-web-image.tar

    Copy and paste image files to docker host

    On docker host:

    docker load -i postgres.tar
    docker load -i magazine-api-image.tar
    docker load -i infotecs-identity-image.tar
    docker load -i magazine-web-image.tar

    docker-compose -f docker-compose.offline.yml stop && docker-compose -f docker-compose.offline.yml rm && docker-compose -f docker-compose.offline.yml build --no-cache && docker-compose -f docker-compose.offline.yml up -d --force-recreate --remove-orphans

  </details>
  </p>
</details>

<details>
  <summary>Docker</summary>
  <p>
      
  ###### DB ######
 
 `docker build ./Database -t db-image` 
 
 `docker run -d --name db-container -p 5436:5432 db-image`

  ###### Web ######
 
  `docker build ./Source -t web-image`

  `docker run -d -p 63585:8000 --name=web-container web-image`
  </p>
</details>

#### Debug

<details>
  <summary>ASPNET Core Web API</summary>
  <p>
      
  ###### Set environment variables. Run in cmd on Windows. ######
 
  `setx ASPNETCORE_ENVIRONMENT "Development"`

  `setx ASPNETCORE_URLS "http://*:5080"`
 
  ###### Navigate to project folder. ######
 
  `cd Source/Magazine.API`

  ###### Run project and serve at http://localhost:5080. ######
 
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
  
  ###### Serve the application at http://localhost:5498. ######
  `ng serve --configuration=development --port=5498 --open`

  </p>
</details>
