# NODE

FROM node AS build
WORKDIR /app

COPY *.json ./
COPY *.editorconfig ./
COPY browserslist ./
COPY src/ src/
COPY e2e/ e2e/

RUN npm install    # node_modules

RUN npm run build    # dist

#RUN npm run start    # serve 4200 (compiling angular project)

# NGINX

FROM nginx
EXPOSE 8080 8094

RUN rm etc/nginx/conf.d/default.conf

COPY content var/www/html
COPY nginx.conf var/www
COPY certificates var/www/certificates
COPY --from=build app/dist var/www/html

ENTRYPOINT ["nginx", "-g", "daemon off;"]
