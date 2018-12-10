CREATE TABLE [dbo].[NewCarDealers] (
    [DealerId]    NUMERIC (18) NOT NULL,
    [IsExclusive] BIT          CONSTRAINT [DF_NewCarDealers_IsExclusive] DEFAULT (0) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_NewCarDealers_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_NewCarDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarDealers_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID])
);

