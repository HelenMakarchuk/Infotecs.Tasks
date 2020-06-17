# NODE

FROM node:12.18.0 AS build
WORKDIR /app

ENV PATH /app/node_modules/.bin:$PATH

COPY package.json ./

RUN npm install

COPY *.json ./
COPY browserslist ./
COPY src/ src/
COPY e2e/ e2e/

RUN ng build --configuration=production --output-path=dist

# NGINX

FROM nginx:1.18.0
EXPOSE 5498

RUN rm etc/nginx/conf.d/default.conf
COPY nginx.conf etc/nginx
COPY content var/www/html
COPY --from=build app/dist var/www/html

COPY certificates/localhost.pem /etc/ssl/certs/certificate.pem
COPY certificates/localhost.key /etc/ssl/certs/certificate.key
COPY certificates/localhost.crt /etc/ssl/certs/localhost.crt

RUN ["update-ca-certificates"]

ENTRYPOINT ["nginx", "-g", "daemon off;"]
