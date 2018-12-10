CREATE TABLE [dbo].[HR_Functions] (
    [HR_FunctionId]     INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentId]      INT           NOT NULL,
    [FunctionName]      VARCHAR (100) NOT NULL,
    [FunctionHeadName]  VARCHAR (50)  NOT NULL,
    [FunctionHeadEmail] VARCHAR (50)  NOT NULL,
    [Description]       VARCHAR (MAX) NOT NULL
);

