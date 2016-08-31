create table Address
(Id char(36) NOT NULL,
 Street varchar(1000),
 Zip int,
 City varchar(1000),
 State varchar(1000) NOT NULL,
 PRIMARY KEY (Id)
);
create table Employee 
(Id char(36) NOT NULL, 
 FirstName varchar(1000),
 LastName varchar(1000) NOT NULL,
 Age int,
 MainAddressId char(36) NOT NULL,
 PRIMARY KEY(Id),
 FOREIGN KEY(MainAddressId) REFERENCES Address(Id)
);