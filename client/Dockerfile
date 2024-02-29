FROM node:latest as build

WORKDIR /usr/src/app

COPY package*.json ./

COPY . .

RUN npm install

RUN npm run build

FROM nginx:latest

COPY nginx.conf /etc/nginx/conf.d

COPY --from=build /usr/src/app/dist/ /usr/share/nginx/html

EXPOSE 80

