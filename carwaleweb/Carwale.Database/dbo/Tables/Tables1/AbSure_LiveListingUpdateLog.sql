CREATE TABLE [dbo].[AbSure_LiveListingUpdateLog] (
    [AbSure_LiveListingUpdateLogId] INT           IDENTITY (1, 1) NOT NULL,
    [CertifiedLogoUrl]              VARCHAR (250) NULL,
    [CertificationId]               INT           NULL,
    [AbSure_CarDetailsId]           BIGINT        NULL,
    [AbSure_Score]                  INT           NULL,
    [TC_StockId]                    INT           NULL,
    [EntryDate]                     DATETIME      NULL,
    [AbSure_WarrantyTypeId]         INT           NULL,
    CONSTRAINT [PK_AbSure_LiveListingUpdateLog] PRIMARY KEY CLUSTERED ([AbSure_LiveListingUpdateLogId] ASC)
);

