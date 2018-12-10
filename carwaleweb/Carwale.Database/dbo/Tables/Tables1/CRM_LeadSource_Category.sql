CREATE TABLE [dbo].[CRM_LeadSource_Category] (
    [Id]   SMALLINT     NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CRM_LeadSource_Category] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

