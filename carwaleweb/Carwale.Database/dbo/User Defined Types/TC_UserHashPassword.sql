CREATE TYPE [dbo].[TC_UserHashPassword] AS TABLE (
    [UserId]       INT           NULL,
    [HashSalt]     VARCHAR (10)  NULL,
    [PasswordHash] VARCHAR (100) NULL);

