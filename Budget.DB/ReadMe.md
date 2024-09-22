# Instructions for EF Migrations
## To initialize Migrations for existing database, run `Add-Migration InitialMigration`
## To Add a new Migration after making update to entity models, run `Add-Migration {name}`
NOTE: for this to work, the main api needs to be the "startup project", and the Budget.DB needs to be the "default project" in the package manager console.

## To Update Database with specific migration, run 
	`update-database -Migration {migrationName}`

## You should be able to run `$env:ASPNETCORE_ENVIRONMENT='Staging'` to specify which db to update, but I haven't tested it.