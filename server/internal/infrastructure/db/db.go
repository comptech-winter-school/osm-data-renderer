package db

import (
	"log"
	"os"

	"github.com/jackc/pgx/v4"
	"github.com/jackc/pgx/v4/stdlib"
	"github.com/jmoiron/sqlx"
)

func OpenDB() *sqlx.DB {
	config, err := pgx.ParseConfig(os.Getenv("DATABASE_DSN"))
	if err != nil {
		log.Fatalf("cant connect to db: %w", err)
	}
	return sqlx.NewDb(stdlib.OpenDB(*config), "pgx")
}
