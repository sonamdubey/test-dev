CREATE TABLE [dbo].[PartsCategories] (
    [ID]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_PartsCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_PartsCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

