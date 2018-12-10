CREATE TABLE [dbo].[TC_Deals_StockVINLog] (
    [TC_Deals_VINStatusLogId] INT          IDENTITY (1, 1) NOT NULL,
    [TC_Deals_StockVINId]     INT          NOT NULL,
    [TC_Deals_StockStatusId]  TINYINT      NOT NULL,
    [ModifiedOn]              DATETIME     NOT NULL,
    [ModifiedBy]              INT          NOT NULL,
    [VINNo]                   VARCHAR (20) NULL,
    [PreviousStatus]          INT          DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TC_Deals_VINStatusLog] PRIMARY KEY CLUSTERED ([TC_Deals_VINStatusLogId] ASC)
);

