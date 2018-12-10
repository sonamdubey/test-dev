CREATE TABLE [dbo].[DebitSubCategories] (
    [ID]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CatId]    NUMERIC (18) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_DebitSubCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_DebitSubCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DebitSubCategories_DebitCategories] FOREIGN KEY ([CatId]) REFERENCES [dbo].[DebitCategories] ([Id])
);

