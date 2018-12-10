CREATE TABLE [dbo].[DealerIgnored] (
    [DealerId]  NUMERIC (18) NOT NULL,
    [IgnoredId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DealerIgnored] PRIMARY KEY CLUSTERED ([DealerId] ASC, [IgnoredId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DealerIgnored_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [FK_DealerIgnored_Dealers1] FOREIGN KEY ([IgnoredId]) REFERENCES [dbo].[Dealers] ([ID])
);

