CREATE TABLE [dbo].[NewCarPurchaseInquiries_archive_0616] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_NoOfCars] DEFAULT ((1)) NOT NULL,
    [BuyTime]             VARCHAR (50)   CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_BuyTime] DEFAULT ('1 Week') NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsFake]              BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsFake] DEFAULT ((0)) NOT NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_StatusId] DEFAULT ((1)) NOT NULL,
    [IsForwarded]         BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsForwarded] DEFAULT ((0)) NOT NULL,
    [IsRejected]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries010520165_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsViewed]            BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsViewed] DEFAULT ((0)) NOT NULL,
    [IsMailSend]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsMailSend] DEFAULT ((0)) NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_ForwardedLead] DEFAULT ((1)) NOT NULL,
    [SourceId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_SourceId] DEFAULT ((1)) NOT NULL,
    [ReqDateTimeDatePart] DATETIME       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_ReqDateTimeDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [VisitedDealership]   BIT            NULL,
    [CRM_LeadId]          NUMERIC (18)   NULL,
    [ClientIP]            VARCHAR (100)  NULL,
    [PQPageId]            SMALLINT       NULL,
    [LTSRC]               VARCHAR (50)   NULL,
    [UtmaCookie]          VARCHAR (250)  NULL,
    [UtmzCookie]          VARCHAR (500)  NULL,
    CONSTRAINT [PK_NewCarPurchaseInquiriesFrom_01052016] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_CarVersionId]
    ON [dbo].[NewCarPurchaseInquiries_archive_0616]([CarVersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_ForwardedLead]
    ON [dbo].[NewCarPurchaseInquiries_archive_0616]([ForwardedLead] ASC, [ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_ReqDateTimeDatePart]
    ON [dbo].[NewCarPurchaseInquiries_archive_0616]([ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_RequestDateTime]
    ON [dbo].[NewCarPurchaseInquiries_archive_0616]([CustomerId] ASC, [CarVersionId] ASC, [Id] ASC, [RequestDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ NewCarPurchaseInquiries_01052016_SourceId]
    ON [dbo].[NewCarPurchaseInquiries_archive_0616]([SourceId] ASC);

