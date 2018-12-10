CREATE TABLE [dbo].[TC_ExcelSheetStockInventory] (
    [TC_ExcelSheetStockInventoryId] INT           IDENTITY (1, 1) NOT NULL,
    [EntryDate]                     DATETIME      NULL,
    [CreatedBy]                     INT           NULL,
    [BranchId]                      INT           NULL,
    [DirPath]                       VARCHAR (100) NULL,
    [FileName]                      VARCHAR (150) NULL,
    [HostUrl]                       VARCHAR (100) NULL,
    [Location]                      VARCHAR (255) NULL,
    [LastUpdatedDate]               DATETIME      NULL,
    [IsProperExcel]                 BIT           NULL,
    [ExcelSheetName]                VARCHAR (100) NULL,
    [Status]                        BIT           NULL,
    [IsDelated]                     BIT           NULL,
    [TotalRecords]                  INT           NULL,
    [ValidRecords]                  INT           NULL,
    [InValidRecords]                INT           NULL,
    [RejectedRecords]               INT           NULL,
    [IsSpecialUser]                 BIT           NULL,
    CONSTRAINT [PK_TC_ExcelStockInventoryLog] PRIMARY KEY CLUSTERED ([TC_ExcelSheetStockInventoryId] ASC)
);

