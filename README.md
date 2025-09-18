# Task Manager API (.NET 8, PostgreSQL)

## Points forts à mettre en avant
- Architecture en couches claire: Controllers, Services, Repositories, DTOs, Mapping (AutoMapper).
- Clean Code & SOLID: SRP/DIP/ISP appliqués, middleware d’exception, validation avec FluentValidation.
- Authentification JWT sécurisée: BCrypt pour mots de passe, Swagger sécurisé, [Authorize] sur endpoints.
- EF Core: FKs explicites, AsNoTracking pour lectures, index/contraintes uniques, migrations auto au démarrage.
- DevX: Swagger/OpenAPI, CORS configuré, docker-compose pour environnement reproductible.

## Lancer en local (Docker)
```bash
docker compose up --build
# API: http://localhost:8080/swagger
```

## Lancer en local (dotnet)
```bash
cd TaskManagerAPI
# Configurer PostgreSQL local (voir appsettings.json) ou variables d'env
# Appliquer migrations
dotnet ef database update
# Run
dotnet run
# Swagger: http://localhost:5157/swagger (port selon sortie)
```

## Endpoints principaux
- Auth: POST /api/Auth/register, POST /api/Auth/login
- Users: /api/Users (protégé)
- Projects: /api/Projects (protégé)
- TaskItems: /api/TaskItems (protégé)

## Configuration
- Chaîne de connexion via ConnectionStrings__DefaultConnection (env vars) ou appsettings.json.
- JWT via variables Jwt__Issuer, Jwt__Audience, Jwt__Key, Jwt__ExpiresMinutes.

## Tests rapides
- Register puis Login dans Swagger; cliquer "Authorize" avec Bearer <token>.

## CI (GitHub Actions)
- Ajoutez un workflow simple .github/workflows/dotnet.yml pour build.
