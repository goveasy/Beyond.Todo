# Beyond Todo - entorno Docker

Este repositorio incluye una API en .NET 8, una base de datos PostgreSQL y una caché Redis. El archivo `docker-compose.yml` permite levantar estos servicios con un solo comando. La aplicación Angular 21 **ya no se ejecuta en contenedor** y debe iniciarse de forma local.

## Requisitos previos
- Docker y Docker Compose instalados (para API, PostgreSQL y Redis).
- Node.js 24.11.1 instalado (para ejecutar el frontend de Angular localmente).

## Puesta en marcha de los servicios Dockerizados
1. Desde la raíz del repositorio, construir e iniciar los contenedores:
   ```bash
   docker compose up --build
   ```
2. Una vez que los contenedores estén arriba:
   - La API estará disponible en `http://localhost:5042`.
   - La base de datos PostgreSQL estará en `localhost:5432` con las credenciales `applications / trigger` y la base de datos `TodoListDb`.
   - Redis estará en `localhost:6379`.

Para detener y limpiar los contenedores y volúmenes de datos:
```bash
docker compose down -v
```

## Levantar el frontend de Angular (sin Docker)
1. Usa Node.js **24.11.1**.
2. Desde la carpeta `Beyong.Client`, instala las dependencias:
   ```bash
   npm install
   ```
3. Inicia el servidor de desarrollo:
   ```bash
   npm start
   ```
4. Abre tu navegador en `http://localhost:4200`. La aplicación consumirá la API disponible en `http://localhost:5042`.

## Acceso a Swagger de la API
El entorno se levanta con `ASPNETCORE_ENVIRONMENT=Development`, por lo que la UI de Swagger está habilitada. Puedes abrir:
```
http://localhost:5042/swagger
```
para explorar y probar los endpoints.
