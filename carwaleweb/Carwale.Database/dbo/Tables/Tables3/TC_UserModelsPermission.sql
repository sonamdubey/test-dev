CREATE TABLE [dbo].[TC_UserModelsPermission] (
    [TC_UsersId] INT NULL,
    [ModelId]    INT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_UserModelsPermission_TC_UsersId]
    ON [dbo].[TC_UserModelsPermission]([TC_UsersId] ASC);

