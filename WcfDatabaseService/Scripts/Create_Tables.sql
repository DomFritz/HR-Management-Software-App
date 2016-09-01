create table Address
(Id char(36) NOT NULL,
 Street varchar(1000),
 Zip int,
 City varchar(1000),
 State varchar(1000) NOT NULL,
 EmployeeId char(36) NOT NULL,
 PRIMARY KEY (Id),
 FOREIGN KEY(EmployeeId) REFERENCES Employee(Id)
);
create table Employee 
(Id char(36) NOT NULL, 
 FirstName varchar(1000),
 LastName varchar(1000) NOT NULL,
 Age int,
 PRIMARY KEY(Id)
);