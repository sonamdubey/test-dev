CREATE TABLE [dbo].[Absure_Trans_FailedTransactions] (
    [ID]              NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [DealerId]        NUMERIC (18)    NOT NULL,
    [CarId]           NUMERIC (18)    NOT NULL,
    [DebitAmount]     NUMERIC (18, 2) NULL,
    [ServiceTaxValue] NUMERIC (18, 2) NULL,
    [UserId]          INT             NULL,
    [GoldSalesCost]   NUMERIC (18)    NULL,
    [SilverSalesCost] NUMERIC (18)    NULL,
    CONSTRAINT [PK_AbsTransFailedTran] PRIMARY KEY CLUSTERED ([ID] ASC)
);

