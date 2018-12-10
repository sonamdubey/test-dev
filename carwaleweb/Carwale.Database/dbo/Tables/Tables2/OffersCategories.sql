CREATE TABLE [dbo].[OffersCategories] (
    [ID]   NUMERIC (18) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OffersCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

