CREATE TABLE [dbo].[CRM_EventTypes] (
    [Id]       INT           NOT NULL,
    [RoleId]   INT           NOT NULL,
    [Name]     VARCHAR (150) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_CRM_CallActionTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CRM_CallActionTypes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

