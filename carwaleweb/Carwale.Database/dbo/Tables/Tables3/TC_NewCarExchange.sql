CREATE TABLE [dbo].[TC_NewCarExchange] (
    [TC_NewCarExchangeId] INT          IDENTITY (1, 1) NOT NULL,
    [ExchangeType]        VARCHAR (50) NULL,
    [IsActive]            BIT          CONSTRAINT [DF_TC_NewCarExchange_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TC_NewCarExchange] PRIMARY KEY CLUSTERED ([TC_NewCarExchangeId] ASC)
);

