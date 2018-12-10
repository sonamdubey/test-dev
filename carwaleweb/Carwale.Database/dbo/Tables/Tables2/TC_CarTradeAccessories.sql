CREATE TABLE [dbo].[TC_CarTradeAccessories] (
    [TC_CarTradeAccessoryId]         INT          IDENTITY (1, 1) NOT NULL,
    [TC_CarTradeCertificationDataId] INT          NULL,
    [AccessoryName]                  VARCHAR (50) NULL,
    CONSTRAINT [PK_TC_CarTradeAccesories] PRIMARY KEY CLUSTERED ([TC_CarTradeAccessoryId] ASC)
);

