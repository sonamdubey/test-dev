CREATE TABLE [dbo].[TC_Deals_NewCarInquiriesLog] (
    [TC_Deals_NewCarInquiriesLogId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiriesId]          INT      NULL,
    [TC_InquiriesLeadId]            INT      NULL,
    [TC_InquirySourceId]            INT      NULL,
    [InqCreatedOn]                  DATETIME NULL,
    [TC_Deals_StockId]              INT      NULL,
    [TC_DealsStockVinId]            INT      NULL,
    [IsPaymentSuccess]              BIT      NULL,
    [EntryDate]                     DATETIME NULL,
    PRIMARY KEY CLUSTERED ([TC_Deals_NewCarInquiriesLogId] ASC)
);

