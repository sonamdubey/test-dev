CREATE TABLE [dbo].[NewCarAveragePrices] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    [AveragePrice] NUMERIC (18) NOT NULL,
    [LastUpdated]  DATETIME     NOT NULL,
    CONSTRAINT [PK_NewCarAveragePrices] PRIMARY KEY CLUSTERED ([CarVersionId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarAveragePrices_CarVersions] FOREIGN KEY ([CarVersionId]) REFERENCES [dbo].[CarVersions] ([ID])
);

