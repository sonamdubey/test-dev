CREATE TABLE [dbo].[AbSure_Trans_Debits] (
    [Id]                 NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [DealerId]           NUMERIC (18)    NOT NULL,
    [CarId]              NUMERIC (18)    NOT NULL,
    [DebitedAmount]      DECIMAL (18, 2) NOT NULL,
    [DiscountValue]      DECIMAL (18, 2) NOT NULL,
    [ServiceTaxValue]    DECIMAL (18, 2) NOT NULL,
    [FinalDebitedAmount] DECIMAL (18, 2) NOT NULL,
    [DebitDate]          DATETIME        NOT NULL,
    [DebitedBy]          INT             NOT NULL,
    [GoldSalesCost]      DECIMAL (18, 2) NULL,
    [SilverSalesCost]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_AbSure_Trans_Debits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

