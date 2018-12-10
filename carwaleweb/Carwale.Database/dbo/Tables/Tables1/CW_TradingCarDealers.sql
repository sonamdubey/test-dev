CREATE TABLE [dbo].[CW_TradingCarDealers] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [TradingCarDealerName] VARCHAR (250) NULL,
    CONSTRAINT [PK_CW_TradingCarDealers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

