# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY Beyond.Todo.Application/Beyond.Todo.Application.csproj Beyond.Todo.Application/
COPY Beyond.Todo.Domain/Beyond.Todo.Domain.csproj Beyond.Todo.Domain/
COPY Beyond.Todo.Infraestructure/Beyond.Todo.Infraestructure.csproj Beyond.Todo.Infraestructure/
COPY Beyond.Todo.WebApi/Beyond.Todo.WebApi.csproj Beyond.Todo.WebApi/
RUN dotnet restore Beyond.Todo.WebApi/Beyond.Todo.WebApi.csproj

# Copy the remaining source code and build
COPY . .
RUN dotnet publish Beyond.Todo.WebApi/Beyond.Todo.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5042
EXPOSE 5042

ENTRYPOINT ["dotnet", "Beyond.Todo.WebApi.dll"]
