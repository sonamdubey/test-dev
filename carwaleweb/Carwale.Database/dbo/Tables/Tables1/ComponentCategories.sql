CREATE TABLE [dbo].[ComponentCategories] (
    [Id]       INT          NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_ComponentCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ComponentCategories] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

