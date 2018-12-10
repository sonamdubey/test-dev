CREATE TYPE [dbo].[TC_TempTableSpclUser] AS TABLE (
    [TC_SpecialUsersId] INT          NULL,
    [USERNAME]          VARCHAR (50) NULL,
    [NodeCode]          VARCHAR (20) NULL,
    [ReportsTo]         INT          NULL,
    [lvl]               INT          NULL,
    [Zonename]          VARCHAR (20) NULL,
    [IsDealer]          INT          NULL);

