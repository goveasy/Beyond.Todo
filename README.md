# Beyond Todo - entorno Docker

Este repositorio incluye una API en .NET 8, una base de datos PostgreSQL, una caché Redis y una aplicación Angular 21. El archivo `docker-compose.yml` permite levantar todo el entorno con un solo comando.

## Requisitos previos
- Docker y Docker Compose instalados.

## Puesta en marcha
1. Desde la raíz del repositorio, construir e iniciar los contenedores:
   ```bash
   docker compose up --build
   ```
2. Una vez que los contenedores estén arriba:
   - La API estará disponible en `http://localhost:5042`.
   - La base de datos PostgreSQL estará en `localhost:5432` con las credenciales `applications / trigger` y la base de datos `TodoListDb`.
   - Redis estará en `localhost:6379`.
   - La aplicación Angular estará en `http://localhost:4200`.

Para detener y limpiar los contenedores y volúmenes de datos:
```bash
docker compose down -v
```

## Acceso a Swagger de la API
El entorno se levanta con `ASPNETCORE_ENVIRONMENT=Development`, por lo que la UI de Swagger está habilitada. Puedes abrir:
```
http://localhost:5042/swagger
```
para explorar y probar los endpoints.

## Acceso a la aplicación Angular
La aplicación construida se sirve con Nginx. Ingresa en tu navegador a:
```
http://localhost:4200
```
La aplicación consumirá la API expuesta en `http://localhost:5042`.
