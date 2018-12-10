CREATE TABLE [dbo].[TC_UsersRole] (
    [TC_UsersRoleId] INT      IDENTITY (1, 1) NOT NULL,
    [UserId]         INT      NOT NULL,
    [RoleId]         SMALLINT NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_UsersRole_UserId]
    ON [dbo].[TC_UsersRole]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_UsersRole_RoleId]
    ON [dbo].[TC_UsersRole]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_UsersRole_user_roleId]
    ON [dbo].[TC_UsersRole]([UserId] ASC, [RoleId] ASC);

