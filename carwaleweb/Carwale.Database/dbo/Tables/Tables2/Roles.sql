CREATE TABLE [dbo].[Roles] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RoleName]    VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (100) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_Roles_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

