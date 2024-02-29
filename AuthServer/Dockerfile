FROM node:latest as angular-build

WORKDIR /app

COPY ./auth-client/package*.json ./

COPY ./auth-client/ ./

RUN npm install

RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

#copy csproj and restore as destinct layers
COPY *.csproj ./
RUN dotnet restore

#copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

EXPOSE 8080

#build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=angular-build /app/dist /app/wwwroot
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet","AuthServer.dll"]