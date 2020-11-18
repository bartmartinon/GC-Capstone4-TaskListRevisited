CREATE DATABASE TaskListDB;
USE TaskListDB;

-- Tables
CREATE TABLE Users(
Id INT PRIMARY KEY IDENTITY(1,1),
Email NVARCHAR(50),
[Password] NVARCHAR(50)
);

CREATE TABLE ToDoItems(
Id INT PRIMARY KEY IDENTITY(1,1),
UserId INT FOREIGN KEY REFERENCES Users(Id),
[Description] NVARCHAR(100) NOT NULL,
Deadline DATE NOT NULL,
IsDone BIT NOT NULL
);

-- Default Values
INSERT INTO Users(Email,[Password])
VALUES ('barthelemy.martinon@gmail.com','bartpassword'),
('adrien.dupre@gmail.com','adrienpassword')

INSERT INTO ToDoItems(UserId,[Description],DeadLine,IsDone)
VALUES (1,'Get a new task manager web app developped.','11/23/2020',0),
(2,'Exist!','11/19/2020',0)

-- Users cleanup
DELETE FROM Users WHERE Id > 2;