FROM node:latest
WORKDIR /app
EXPOSE 4200

COPY *.json ./
COPY *.editorconfig ./
COPY browserslist ./
COPY src/ src/
COPY e2e/ e2e/

RUN npm install

ENTRYPOINT npm start
