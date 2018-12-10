CREATE TABLE [dbo].[DealerRestricted] (
    [DealerId]     NUMERIC (18) NOT NULL,
    [RestrictedId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DealerRestricted] PRIMARY KEY CLUSTERED ([DealerId] ASC, [RestrictedId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DealerRestricted_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [FK_DealerRestricted_Dealers1] FOREIGN KEY ([RestrictedId]) REFERENCES [dbo].[Dealers] ([ID])
);

