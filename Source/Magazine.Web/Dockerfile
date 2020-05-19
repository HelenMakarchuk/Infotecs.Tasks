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

# NGINX

FROM nginx
EXPOSE 8080 8094

RUN rm etc/nginx/conf.d/default.conf
COPY nginx.conf etc/nginx
COPY content var/www/html
COPY certificates/ etc/ssl/certs
COPY --from=build app/dist var/www/html

ENTRYPOINT ["nginx", "-g", "daemon off;"]
