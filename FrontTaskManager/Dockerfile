FROM node:lts-alpine AS build
WORKDIR /app

# Copier les fichiers du projet
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

FROM nginx:stable-alpine

RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/FrontTaskManager/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]