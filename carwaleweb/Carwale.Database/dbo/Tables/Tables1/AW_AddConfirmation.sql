CREATE TABLE [dbo].[AW_AddConfirmation] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [EntryDate]  DATETIME     NOT NULL,
    CONSTRAINT [PK_AW_AddConfirmation] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

