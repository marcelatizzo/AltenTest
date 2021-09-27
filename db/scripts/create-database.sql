USE master
GO

CREATE DATABASE Hotel
GO

USE Hotel
GO

CREATE TABLE Reservation (
    Id integer not null IDENTITY(1,1) primary key,
    GuestName varchar(300) not null,
    AccomodationStart datetime not null,
    AccomodationEnd datetime not null
)
GO