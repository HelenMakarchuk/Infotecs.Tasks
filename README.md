# Infotecs.Task_3

#### Технологии

- [x] Angular
- [ ] WebApi
- [ ] Less
- [ ] SignalR
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
