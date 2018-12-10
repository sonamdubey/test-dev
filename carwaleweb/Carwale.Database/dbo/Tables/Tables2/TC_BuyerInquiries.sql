CREATE TABLE [dbo].[TC_BuyerInquiries] (
    [TC_BuyerInquiriesId]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]           BIGINT         NULL,
    [StockId]                  BIGINT         NULL,
    [UsedCarPurchaseInquiryId] BIGINT         NULL,
    [TC_InquiriesLeadId]       BIGINT         NULL,
    [CreatedBy]                INT            NULL,
    [CreatedOn]                DATETIME       CONSTRAINT [DF_TC_BuyerInquiries_CreatedOn] DEFAULT (getdate()) NULL,
    [MakeYearFrom]             SMALLINT       NULL,
    [MakeYearTo]               SMALLINT       NULL,
    [PriceMin]                 INT            NULL,
    [PriceMax]                 INT            NULL,
    [TC_InquirySourceId]       INT            NULL,
    [Lastupdateddate]          DATETIME       NULL,
    [LastUpdatedBy]            INT            NULL,
    [TC_LeadDispositionId]     TINYINT        NULL,
    [BuyDate]                  DATETIME       NULL,
    [Buytime]                  TINYINT        NULL,
    [BookingDate]              DATETIME       NULL,
    [BookingStatus]            TINYINT        NULL,
    [Temp_BuyerWithoutStock]   BIGINT         NULL,
    [TC_InquiryOtherSourceId]  TINYINT        NULL,
    [Comments]                 VARCHAR (5000) NULL,
    [IsTDRequested]            BIT            NULL,
    [TDRequestedDate]          DATETIME       NULL,
    [BookingRequested]         BIT            NULL,
    [BookingRequestedDate]     DATETIME       NULL,
    [TC_ActionApplicationId]   INT            NULL,
    [MostInterested]           BIT            NULL,
    [cteapiresponse]           VARCHAR (2000) NULL,
    [cteinquiryid]             INT            NULL,
    CONSTRAINT [DF_TC_BuyerInquiries_TC_InquiriesLead] FOREIGN KEY ([TC_InquiriesLeadId]) REFERENCES [dbo].[TC_InquiriesLead] ([TC_InquiriesLeadId]),
    CONSTRAINT [DF_TC_BuyerInquiries_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionId]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_BuyerInquiries_TC_users] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[TC_Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [Ix_TC_Buyerinquiries]
    ON [dbo].[TC_BuyerInquiries]([TC_InquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_BuyerInquiries_StockId]
    ON [dbo].[TC_BuyerInquiries]([StockId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_UsedCarPurchaseInquiryId]
    ON [dbo].[TC_BuyerInquiries]([UsedCarPurchaseInquiryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_Temp_BuyerWithoutStock]
    ON [dbo].[TC_BuyerInquiries]([Temp_BuyerWithoutStock] ASC)
    INCLUDE([TC_BuyerInquiriesId], [CreatedOn]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_TC_InquiriesLeadId]
    ON [dbo].[TC_BuyerInquiries]([TC_InquiriesLeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_TC_InquirySourceId]
    ON [dbo].[TC_BuyerInquiries]([TC_InquirySourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_TC_LeadDispositionId]
    ON [dbo].[TC_BuyerInquiries]([TC_LeadDispositionId] ASC)
    INCLUDE([TC_BuyerInquiriesId], [StockId], [TC_InquiriesLeadId], [CreatedOn], [BookingDate], [BookingStatus]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_BuyerInquiries_CreatedOn]
    ON [dbo].[TC_BuyerInquiries]([CreatedOn] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_BuyerInquiries_TC_BuyerInquiriesId]
    ON [dbo].[TC_BuyerInquiries]([TC_BuyerInquiriesId] ASC);

