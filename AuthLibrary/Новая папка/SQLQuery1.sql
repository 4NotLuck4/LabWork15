CREATE TABLE CinemaUserRole (
Id INT IDENTITY(1,1) PRIMARY KEY,
RoleName VARCHAR(20) NOT NULL
);

INSERT INTO CinemaUserRole (RoleName) VALUES
('јдминистратор'),
('Ѕилетер'),
('ѕосетитель');

CREATE TABLE CinemaUser (
Id INT IDENTITY(1,1) PRIMARY KEY,
[Login] VARCHAR(50) NOT NULL UNIQUE,
PasswordHash VARCHAR(200) NOT NULL,
FailedLoginAttempts INT DEFAULT 0,
UnlockDate DATETIME NULL
);

CREATE TABLE CinemaPrivilege (
Id INT IDENTITY(1,1) PRIMARY KEY,
[Name] VARCHAR (100) NOT NULL
);

INSERT INTO CinemaPrivilege([Name]) VALUES
('доступ в личный кабинет'),
('проверка билетов'),
('просмотр списка фильмов'),
('добавление пользователей'),
('редактирование пользователей');

CREATE TABLE CinemaRolePrivilege (
RoleId INT NOT NULL,
PrivilegeId INT NOT NULL,
PRIMARY KEY (RoleId,PrivilegeId),
FOREIGN KEY (RoleId) REFERENCES CinemaUserRole(Id),
FOREIGN KEY (PrivilegeId) REFERENCES CinemaPrivilege(Id)
);

INSERT INTO CinemaRolePrivilege(RoleId, PrivilegeId) VALUES
(1, 1), (1, 4), (1, 5), 
(2, 1), (2, 2), (2, 3), 
(3, 1), (3, 3);