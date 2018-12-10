CREATE TABLE [dbo].[NewCarPurchaseInquiries_070916_07_00] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_NoOfCars_0616] DEFAULT ((1)) NOT NULL,
    [BuyTime]             VARCHAR (50)   CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_BuyTime_0616] DEFAULT ('1 Week') NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsApproved_0616] DEFAULT ((0)) NOT NULL,
    [IsFake]              BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsFake_0616] DEFAULT ((0)) NOT NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_StatusId_0616] DEFAULT ((1)) NOT NULL,
    [IsForwarded]         BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsForwarded_0616] DEFAULT ((0)) NOT NULL,
    [IsRejected]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries010520165_IsRejected_0616] DEFAULT ((0)) NOT NULL,
    [IsViewed]            BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsViewed_0616] DEFAULT ((0)) NOT NULL,
    [IsMailSend]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_IsMailSend_0616] DEFAULT ((0)) NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_ForwardedLead_0616] DEFAULT ((1)) NOT NULL,
    [SourceId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_SourceId_0616] DEFAULT ((1)) NOT NULL,
    [ReqDateTimeDatePart] DATETIME       CONSTRAINT [DF_New_NewCarPurchaseInquiries01052016_ReqDateTimeDatePart_0616] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [VisitedDealership]   BIT            NULL,
    [CRM_LeadId]          NUMERIC (18)   NULL,
    [ClientIP]            VARCHAR (100)  NULL,
    [PQPageId]            SMALLINT       NULL,
    [LTSRC]               VARCHAR (50)   NULL,
    [UtmaCookie]          VARCHAR (250)  NULL,
    [UtmzCookie]          VARCHAR (500)  NULL,
    CONSTRAINT [PK_NewCarPurchaseInquiries0616] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ NewCarPurchaseInquiries_01052016_SourceId_0616]
    ON [dbo].[NewCarPurchaseInquiries_070916_07_00]([SourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_CarVersionId_0616]
    ON [dbo].[NewCarPurchaseInquiries_070916_07_00]([CarVersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_ForwardedLead_0616]
    ON [dbo].[NewCarPurchaseInquiries_070916_07_00]([ForwardedLead] ASC, [ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_ReqDateTimeDatePart_0616]
    ON [dbo].[NewCarPurchaseInquiries_070916_07_00]([ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_01052016_RequestDateTime_0616]
    ON [dbo].[NewCarPurchaseInquiries_070916_07_00]([CustomerId] ASC, [CarVersionId] ASC, [Id] ASC, [RequestDateTime] ASC);

