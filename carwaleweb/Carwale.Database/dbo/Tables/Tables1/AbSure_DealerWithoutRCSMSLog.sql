CREATE TABLE [dbo].[AbSure_DealerWithoutRCSMSLog] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [CarId]    BIGINT       NULL,
    [DealerId] INT          NULL,
    [MobileNo] VARCHAR (15) NULL,
    [StockId]  BIGINT       NULL,
    [SendedOn] DATETIME     NULL
);

