CREATE DATABASE colegiopublicgenerico_db;

use colegiopublicgenerico_db;

CREATE TABLE users(
    Id VARCHAR(45) NOT NULL PRIMARY KEY,
    Name TEXT NOT NULL,
    Emal TEXT NOT NULL,
    password TEXT NOT NULL
);
