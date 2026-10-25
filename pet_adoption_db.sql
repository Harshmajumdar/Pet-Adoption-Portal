-- OPTIONAL: the app creates these tables automatically on first run (EnsureCreated).
-- This script is only a REFERENCE for your poster / viva (column types may differ slightly
-- from what EF Core generates). If you run it manually, do it BEFORE starting the app.

CREATE DATABASE IF NOT EXISTS pet_adoption_db;
USE pet_adoption_db;

CREATE TABLE IF NOT EXISTS Pets (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Name        VARCHAR(50)  NOT NULL,
    Species     VARCHAR(20)  NOT NULL,
    Breed       VARCHAR(50)  NULL,
    AgeInMonths INT          NOT NULL,
    Gender      VARCHAR(10)  NOT NULL,
    Description VARCHAR(500) NULL,
    ImageUrl    VARCHAR(300) NULL,
    Status      VARCHAR(20)  NOT NULL DEFAULT 'Available',
    DateAdded   DATETIME     NOT NULL
);

CREATE TABLE IF NOT EXISTS VaccinationRecords (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    PetId       INT          NOT NULL,
    VaccineName VARCHAR(80)  NOT NULL,
    DateGiven   DATETIME     NOT NULL,
    NextDueDate DATETIME     NULL,
    Notes       VARCHAR(200) NULL,
    CONSTRAINT FK_Vaccinations_Pets FOREIGN KEY (PetId) REFERENCES Pets(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS AdoptionApplications (
    Id            INT AUTO_INCREMENT PRIMARY KEY,
    PetId         INT          NOT NULL,
    ApplicantName VARCHAR(100) NOT NULL,
    Email         VARCHAR(100) NOT NULL,
    Phone         VARCHAR(10)  NOT NULL,
    City          VARCHAR(50)  NOT NULL,
    Reason        VARCHAR(500) NOT NULL,
    HasOtherPets  TINYINT(1)   NOT NULL,
    Status        VARCHAR(20)  NOT NULL DEFAULT 'Pending',
    AppliedOn     DATETIME     NOT NULL,
    CONSTRAINT FK_Applications_Pets FOREIGN KEY (PetId) REFERENCES Pets(Id) ON DELETE RESTRICT
);

-- Relationships:
--   Pets 1 ---- * VaccinationRecords   (a pet has many care records)
--   Pets 1 ---- * AdoptionApplications (a pet can receive many applications)
