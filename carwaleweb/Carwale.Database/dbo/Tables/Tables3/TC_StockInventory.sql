CREATE TABLE [dbo].[TC_StockInventory] (
    [TC_StockInventoryId]           INT          IDENTITY (1, 1) NOT NULL,
    [TC_ExcelStockInventoryId]      INT          NOT NULL,
    [ModelCode]                     VARCHAR (50) NULL,
    [ColourCode]                    VARCHAR (50) NULL,
    [PrCodes]                       VARCHAR (50) NULL,
    [Region]                        VARCHAR (50) NULL,
    [ChassisNumber]                 VARCHAR (50) NOT NULL,
    [DealerCompanyName]             VARCHAR (50) NULL,
    [SellingDealerCode]             VARCHAR (50) NULL,
    [DealerLocation]                VARCHAR (50) NULL,
    [PaymentDealerInvoiceDate]      VARCHAR (50) NULL,
    [ModelYear]                     VARCHAR (50) NULL,
    [CheckpointDate]                VARCHAR (50) NULL,
    [WholesaleDate]                 VARCHAR (50) NULL,
    [EntryDate]                     DATETIME     NULL,
    [TC_UserId]                     INT          NULL,
    [BranchId]                      INT          NULL,
    [ModifiedBy]                    INT          NULL,
    [ModifiedDate]                  DATETIME     NULL,
    [TC_ExcelSheetStockInventoryId] INT          NULL,
    [IsSpecialUser]                 BIT          CONSTRAINT [DF_TC_StockInventory_IsSpecialUser] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TC_StockInventory] PRIMARY KEY CLUSTERED ([TC_StockInventoryId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_UserIdIsSpecialUser]
    ON [dbo].[TC_StockInventory]([TC_UserId] ASC, [IsSpecialUser] ASC);

