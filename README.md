# Infotecs.Task_3

#### Технологии

* Angular
* WebApi
* Less
* SignalR
* Docker
* Docker Compose

#### Docker

 * DB

 docker build ./Database -t db-image

 docker run -d --name db-container -p 5436:5432 db-image

 * Web

 docker build ./Source -t web-image

 docker run -d -p 63585:8000 --name=web-container web-image
