CREATE TABLE [dbo].[TC_UsersLog] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [BranchId]   NUMERIC (18) NULL,
    [UserId]     NUMERIC (18) NULL,
    [LoggedTime] DATETIME     NULL,
    [IpAddress]  VARCHAR (50) NULL,
    [LoginFrom]  VARCHAR (50) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_UsersLog_BranchId]
    ON [dbo].[TC_UsersLog]([BranchId] ASC, [UserId] ASC)
    INCLUDE([LoggedTime]);

