CREATE TABLE [dbo].[TC_NewCarInquiries] (
    [TC_NewCarInquiriesId]       INT            IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesLeadId]         BIGINT         NULL,
    [TC_InquirySourceId]         INT            NULL,
    [CreatedOn]                  DATETIME       CONSTRAINT [DF_Tc_NewCarInquiries_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                  INT            NULL,
    [VersionId]                  NUMERIC (18)   NULL,
    [Buytime]                    TINYINT        NULL,
    [BuyDate]                    DATETIME       NULL,
    [TC_LeadDispositionId]       TINYINT        NULL,
    [PQDate]                     DATETIME       NULL,
    [PQStatus]                   TINYINT        NULL,
    [TDDate]                     DATETIME       NULL,
    [TDStatus]                   TINYINT        NULL,
    [BookingDate]                DATETIME       NULL,
    [BookingStatus]              TINYINT        NULL,
    [CityId]                     NUMERIC (18)   NULL,
    [PQRequestedDate]            DATETIME       NULL,
    [TDRequestedDate]            DATETIME       NULL,
    [TC_TDCalendarId]            BIGINT         NULL,
    [TransmissionType]           TINYINT        NULL,
    [FuelType]                   TINYINT        NULL,
    [TC_InquiryOtherSourceId]    TINYINT        NULL,
    [Comments]                   VARCHAR (5000) NULL,
    [IsCorporate]                BIT            DEFAULT ((0)) NOT NULL,
    [CompanyName]                VARCHAR (150)  NULL,
    [IsWillingnessToExchange]    BIT            NULL,
    [IsFinanceRequired]          BIT            NULL,
    [IsAccessoriesRequired]      BIT            NULL,
    [IsSchemesOffered]           BIT            NULL,
    [ExcelInquiryId]             INT            NULL,
    [CarDeliveryDate]            DATETIME       NULL,
    [CarDeliveryStatus]          SMALLINT       NULL,
    [TC_SubDispositionId]        SMALLINT       NULL,
    [LostVersionId]              INT            NULL,
    [DispositionDate]            DATETIME       NULL,
    [BookingEventDate]           DATETIME       NULL,
    [BookingCancelDate]          DATETIME       NULL,
    [IsExchange]                 BIT            NULL,
    [TDStatusEntryDate]          DATETIME       NULL,
    [TC_CampaignSchedulingId]    INT            NULL,
    [TC_NewCarExchangeId]        INT            NULL,
    [DealerCampaignSchedulingId] INT            NULL,
    [NSCCampaignSchedulingId]    INT            NULL,
    [BookingRequest]             BIT            NULL,
    [BookingRequestDate]         DATETIME       NULL,
    [AppointmentRequest]         BIT            NULL,
    [AppointmentRequestDate]     DATETIME       NULL,
    [CwOfferId]                  INT            NULL,
    [CouponCode]                 VARCHAR (10)   NULL,
    [CWInquiryId]                INT            NULL,
    [TC_ActionApplicationId]     INT            NULL,
    [DMSScreenShotHostUrl]       VARCHAR (100)  NULL,
    [DMSScreenShotStatusId]      INT            NULL,
    [DMSScreenShotUrl]           VARCHAR (250)  NULL,
    [TC_LeadDispositionReason]   VARCHAR (200)  NULL,
    [OriginalImgPath]            VARCHAR (250)  NULL,
    [CampaignId]                 INT            NULL,
    [ContractId]                 INT            NULL,
    [MostInterested]             BIT            NULL,
    [TC_DealsStockVINId]         INT            NULL,
    [TC_Deals_StockId]           INT            NULL,
    [IsPaymentSuccess]           BIT            NULL,
    [MaskingNumber]              VARCHAR (15)   NULL,
    [SalesExMobileNo]            VARCHAR (15)   NULL,
    CONSTRAINT [PK_Tc_NewCarInquiries_id] PRIMARY KEY CLUSTERED ([TC_NewCarInquiriesId] ASC),
    CONSTRAINT [DF_TC_NewCarInquiries_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([ID]),
    CONSTRAINT [DF_TC_NewCarInquiries_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionId]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_NewCarInquiries_TC_LeadDispositionBook] FOREIGN KEY ([BookingStatus]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_NewCarInquiries_TC_LeadDispositionPQ] FOREIGN KEY ([PQStatus]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_NewCarInquiries_TC_LeadDispositionTD] FOREIGN KEY ([TDStatus]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_NewCarInquiries_TC_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[TC_Users] ([Id]),
    CONSTRAINT [DF_TC_TC_Appointments_TC_InquiriesLead] FOREIGN KEY ([TC_InquiriesLeadId]) REFERENCES [dbo].[TC_InquiriesLead] ([TC_InquiriesLeadId]),
    CONSTRAINT [Tc_NewCarInquiries_TC_InquirySource] FOREIGN KEY ([TC_InquirySourceId]) REFERENCES [dbo].[TC_InquirySource] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_TC_InquiriesLeadId]
    ON [dbo].[TC_NewCarInquiries]([TC_InquiriesLeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_BookingStatus]
    ON [dbo].[TC_NewCarInquiries]([BookingStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_TDStatus]
    ON [dbo].[TC_NewCarInquiries]([TDStatus] ASC, [TDDate] ASC)
    INCLUDE([TC_NewCarInquiriesId], [TC_InquiriesLeadId], [VersionId]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiriesVersionId]
    ON [dbo].[TC_NewCarInquiries]([VersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_CityId]
    ON [dbo].[TC_NewCarInquiries]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_TC_InquirySourceId]
    ON [dbo].[TC_NewCarInquiries]([TC_InquirySourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_TC_TDCalendarId]
    ON [dbo].[TC_NewCarInquiries]([TC_TDCalendarId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_NewCarInquiries_TC_DealsStockVINId]
    ON [dbo].[TC_NewCarInquiries]([TC_DealsStockVINId] ASC)
    INCLUDE([TC_NewCarInquiriesId], [TC_InquiriesLeadId], [CityId]);

