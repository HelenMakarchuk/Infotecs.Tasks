FROM nginx
EXPOSE 8084
RUN rm /etc/nginx/conf.d/default.conf
COPY content /etc/nginx/html
COPY nginx.conf /etc/nginx
