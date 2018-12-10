CREATE TABLE [dbo].[HR_Jobs] (
    [HR_JobsId]        INT           IDENTITY (1, 1) NOT NULL,
    [FunctionId]       INT           NOT NULL,
    [RoleDescription]  VARCHAR (MAX) NULL,
    [RoleSummary]      VARCHAR (100) NULL,
    [PositionOpenDate] DATETIME      NOT NULL,
    [JobTitle]         VARCHAR (100) NULL,
    [IsActive]         BIT           NOT NULL,
    [UpdatedBy]        INT           NULL,
    [UpdatedOn]        DATETIME      NULL
);

