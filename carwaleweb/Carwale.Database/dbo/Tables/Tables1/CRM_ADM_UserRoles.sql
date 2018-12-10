CREATE TABLE [dbo].[CRM_ADM_UserRoles] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserId]    NUMERIC (18) NOT NULL,
    [RoleId]    NUMERIC (18) NOT NULL,
    [IsActive]  BIT          NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_UserRoles] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_ADM_UserRoles_CRM_ADM_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[CRM_ADM_Roles] ([Id])
);

