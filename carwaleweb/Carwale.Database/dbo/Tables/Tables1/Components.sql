CREATE TABLE [dbo].[Components] (
    [Id]                  NUMERIC (18) NOT NULL,
    [Name]                VARCHAR (50) NOT NULL,
    [ComponentCategoryId] INT          NOT NULL,
    CONSTRAINT [PK_Components] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

