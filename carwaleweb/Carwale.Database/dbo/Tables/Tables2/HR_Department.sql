CREATE TABLE [dbo].[HR_Department] (
    [HR_DepartmentId] INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentName]  VARCHAR (100) NOT NULL,
    [BUHeadName]      VARCHAR (50)  NOT NULL,
    [BUHeadEmail]     VARCHAR (50)  NOT NULL,
    [Description]     VARCHAR (MAX) NOT NULL
);

