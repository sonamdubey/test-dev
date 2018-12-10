CREATE TABLE [dbo].[SellInquiryAccessories] (
    [CarId]       NUMERIC (18)   NOT NULL,
    [Accessories] VARCHAR (2000) NULL,
    [StockCar]    TINYINT        NOT NULL,
    CONSTRAINT [PK_SellCarAccessories] PRIMARY KEY CLUSTERED ([CarId] ASC, [StockCar] ASC) WITH (FILLFACTOR = 90)
);

