CREATE TABLE [dbo].[CRM_ADM_TeamMembers] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TeamId]    NUMERIC (18) NOT NULL,
    [RoleId]    NUMERIC (18) NOT NULL,
    [UserId]    NUMERIC (18) NOT NULL,
    [CreatedOn] DATETIME     NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_ADM_TeamMembers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CRM_ADM_TeamMembers_CRM_ADM_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[CRM_ADM_Roles] ([Id]),
    CONSTRAINT [FK_CRM_ADM_TeamMembers_CRM_ADM_Teams] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[CRM_ADM_Teams] ([ID])
);

