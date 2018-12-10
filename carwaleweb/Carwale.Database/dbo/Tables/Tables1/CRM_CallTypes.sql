CREATE TABLE [dbo].[CRM_CallTypes] (
    [Id]       INT           NOT NULL,
    [RoleId]   INT           CONSTRAINT [DF_CRM_CallTypes_RoleId] DEFAULT ((-1)) NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_CRM_CallTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

