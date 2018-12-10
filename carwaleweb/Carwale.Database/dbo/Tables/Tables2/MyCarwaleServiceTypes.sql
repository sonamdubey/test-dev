CREATE TABLE [dbo].[MyCarwaleServiceTypes] (
    [ID]   NUMERIC (18) NOT NULL,
    [Name] VARCHAR (50) NULL,
    CONSTRAINT [PK_MyCarwaleServiceTypes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

