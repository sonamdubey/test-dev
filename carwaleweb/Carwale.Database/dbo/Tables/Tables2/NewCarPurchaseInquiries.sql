CREATE TABLE [dbo].[NewCarPurchaseInquiries] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_NoOfCars] DEFAULT ((1)) NOT NULL,
    [BuyTime]             VARCHAR (50)   CONSTRAINT [DF_NewCarPurchaseInquiries_070916_BuyTime] DEFAULT ('1 Week') NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsFake]              BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsFake] DEFAULT ((0)) NOT NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_NewCarPurchaseInquiries_070916_StatusId] DEFAULT ((1)) NOT NULL,
    [IsForwarded]         BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsForwarded] DEFAULT ((0)) NOT NULL,
    [IsRejected]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsViewed]            BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsViewed] DEFAULT ((0)) NOT NULL,
    [IsMailSend]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_IsMailSend] DEFAULT ((0)) NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_070916_ForwardedLead] DEFAULT ((1)) NOT NULL,
    [SourceId]            SMALLINT       CONSTRAINT [DF_NewCarPurchaseInquiries_070916_SourceId] DEFAULT ((1)) NOT NULL,
    [ReqDateTimeDatePart] DATETIME       CONSTRAINT [DF_NewCarPurchaseInquiries_070916_ReqDateTimeDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [VisitedDealership]   BIT            NULL,
    [CRM_LeadId]          NUMERIC (18)   NULL,
    [ClientIP]            VARCHAR (100)  NULL,
    [PQPageId]            SMALLINT       NULL,
    [LTSRC]               VARCHAR (50)   NULL,
    [UtmaCookie]          VARCHAR (250)  NULL,
    [UtmzCookie]          VARCHAR (500)  NULL,
    CONSTRAINT [PK_NewCarPurchaseInquiries_070916] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ NewCarPurchaseInquiries_070916_SourceId]
    ON [dbo].[NewCarPurchaseInquiries]([SourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_070916_CarVersionId]
    ON [dbo].[NewCarPurchaseInquiries]([CarVersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_070916_ForwardedLead]
    ON [dbo].[NewCarPurchaseInquiries]([ForwardedLead] ASC, [ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_070916_ReqDateTimeDatePart]
    ON [dbo].[NewCarPurchaseInquiries]([ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_070916_RequestDateTime]
    ON [dbo].[NewCarPurchaseInquiries]([CustomerId] ASC, [CarVersionId] ASC, [Id] ASC, [RequestDateTime] ASC);

