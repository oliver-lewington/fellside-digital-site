# Docker Compose Setup

## Development (with local PostgreSQL)

Run with included PostgreSQL database:

```bash
docker-compose up --build
```

This uses `docker-compose.yml` which includes:
- **FellsideDigital app** on ports 8080/8081
- **PostgreSQL 15** database with persistent volume
- **Admin user seeding** (configured via .env)

### Environment Variables (.env)

Create a `.env` file in the root directory (see `.env.example` for reference):

```env
POSTGRES_DB=fellsidedigital
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
ADMIN_EMAIL=admin@example.com
ADMIN_PASSWORD=AdminPassword123!
CONNECTIONSTRINGS__REDIS=
```

**Important:** Do NOT commit `.env` to git. It's already in `.gitignore`.

## Production (without database)

Use `docker-compose.prod.yml` when deploying to production:

```bash
docker-compose -f docker-compose.prod.yml up --build
```

This excludes the PostgreSQL service. In production:
- Provide `CONNECTIONSTRINGS__DEFAULTCONNECTION` from Railway, AWS RDS, or other managed service
- Admin credentials are still seeded from `ADMIN_EMAIL` and `ADMIN_PASSWORD` env vars

### Railway Deployment

On Railway:
1. Link a PostgreSQL service to your app
2. Set environment variables in Railway UI:
   - `ADMIN_EMAIL` (e.g., `admin@yoursite.com`)
   - `ADMIN_PASSWORD` (strong password)
   - `CONNECTIONSTRINGS__REDIS` (optional, if using Redis)

Railway automatically injects `DATABASE_URL` which the app converts to Npgsql format.

## Admin User Seeding

The `AdminUserSeeder` runs on every app startup if:
- `ADMIN_EMAIL` environment variable is set
- `ADMIN_PASSWORD` environment variable is set

It creates:
- **Roles**: `SiteAdmin`, `AuctionAdmin`
- **Admin user** with both roles assigned

This is idempotent — it won't duplicate the user if it already exists.

## Local Development (without Docker)

To run locally without Docker:

1. Install PostgreSQL 15 locally
2. Create a database: `createdb fellsidedigital`
3. Update `appsettings.Development.json` connection string if needed
4. Run migrations: `dotnet ef database update`
5. Seed admin (optional): Set env vars and run the app

## Troubleshooting

### "relation "__EFMigrationsHistory" does not exist"
The database exists but schema hasn't been created yet. The app calls `EnsureCreated()` then `Migrate()` to handle this automatically on startup.

### Admin user not created
Check that:
- `ADMIN_EMAIL` and `ADMIN_PASSWORD` env vars are set in docker-compose.yml or .env
- The app started successfully (check logs)
- Run the seeder manually if needed
