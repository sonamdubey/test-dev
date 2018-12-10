CREATE ROLE [u_ExecuteProcs]
    AUTHORIZATION [dbo];


GO
EXECUTE sp_addrolemember @rolename = N'u_ExecuteProcs', @membername = N'cwreadonly';

