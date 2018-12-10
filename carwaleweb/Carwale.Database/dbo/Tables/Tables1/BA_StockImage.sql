CREATE TABLE [dbo].[BA_StockImage] (
    [ID]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [StockId]    BIGINT       NOT NULL,
    [EntryDate]  DATETIME     NULL,
    [ModifyDate] DATETIME     NULL,
    [Directory]  VARCHAR (50) NULL,
    [IsActive]   BIT          CONSTRAINT [DF_BA.StockImage_IsActive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BA.StockImage] PRIMARY KEY CLUSTERED ([ID] ASC)
);

