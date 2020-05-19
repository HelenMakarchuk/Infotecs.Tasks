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

COPY content etc/nginx/html
COPY nginx.conf etc/nginx
COPY certificates etc/nginx/certificates
COPY --from=build app/dist etc/nginx/html

ENTRYPOINT ["nginx", "-g", "daemon off;"]
