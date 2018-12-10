CREATE TABLE [dbo].[DealerImports] (
    [DealerId]     NUMERIC (18) NOT NULL,
    [ImportingUrl] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DealerImports] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DealerImports_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE
);

