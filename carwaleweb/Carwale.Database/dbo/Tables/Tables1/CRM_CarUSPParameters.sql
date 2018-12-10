CREATE TABLE [dbo].[CRM_CarUSPParameters] (
    [Id]       INT           NOT NULL,
    [Name]     VARCHAR (100) NULL,
    [IsActive] BIT           NULL,
    CONSTRAINT [PK_CRM_CarUSPParameters] PRIMARY KEY CLUSTERED ([Id] ASC)
);

