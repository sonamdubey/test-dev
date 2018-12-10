CREATE TABLE [dbo].[DealerSettings] (
    [DealerId]             NUMERIC (18) NOT NULL,
    [MaxSellInquiries]     SMALLINT     NOT NULL,
    [MaxPurchaseInquiries] SMALLINT     NOT NULL,
    CONSTRAINT [PK_DealerSettings] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DealerSettings_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE
);

