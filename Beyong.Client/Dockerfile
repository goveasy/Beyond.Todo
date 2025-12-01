# Build Angular application
FROM node:24.11.1-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

# Serve with nginx
FROM nginx:1.27-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Remove the default nginx landing page so the Angular build replaces it.
RUN rm -rf /usr/share/nginx/html/*

# Copy the built Angular assets (the "browser" output) regardless of the
# project name. The wildcard keeps the image working if the Angular project
# name changes.
COPY --from=build /app/dist/*/browser/ /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
