USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'studentDB')
BEGIN
    CREATE DATABASE [studentDB];
END
GO

USE [studentDB];
GO

CREATE TABLE IF NOT EXISTS Departments (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    CreditHours INT NOT NULL,
    Description NVARCHAR(255) NULL,
    DepartmentID INT NULL FOREIGN KEY REFERENCES Departments(DepartmentID)
);

CREATE TABLE IF NOT EXISTS Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Age INT,
    Gender NVARCHAR(10),
    Address NVARCHAR(100) NOT NULL,
    DepartmentID INT NULL FOREIGN KEY REFERENCES Departments(DepartmentID)
);

CREATE TABLE IF NOT EXISTS Grades (
    GradeID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
    CourseID INT NOT NULL FOREIGN KEY REFERENCES Courses(CourseID),
    Grade FLOAT NOT NULL,
    GradeDate DATE DEFAULT GETDATE()
);

CREATE TABLE IF NOT EXISTS Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);

IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Password, Role)
    VALUES ('admin', 'admin123', 'Admin');
END
GO
