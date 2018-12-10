CREATE TABLE [dbo].[NewCarPurchaseInquiries_Prod] (
    [Id]                  NUMERIC (18)   IDENTITY (18404972, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_NoOfCars] DEFAULT ((1)) NOT NULL,
    [BuyTime]             VARCHAR (50)   CONSTRAINT [DF_New_NewCarPurchaseInquiries_BuyTime] DEFAULT ('1 Week') NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsFake]              BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsFake] DEFAULT ((0)) NOT NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries_StatusId] DEFAULT ((1)) NOT NULL,
    [IsForwarded]         BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsForwarded] DEFAULT ((0)) NOT NULL,
    [IsRejected]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsViewed]            BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsViewed] DEFAULT ((0)) NOT NULL,
    [IsMailSend]          BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_IsMailSend] DEFAULT ((0)) NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            CONSTRAINT [DF_New_NewCarPurchaseInquiries_ForwardedLead] DEFAULT ((1)) NOT NULL,
    [SourceId]            SMALLINT       CONSTRAINT [DF_New_NewCarPurchaseInquiries_SourceId] DEFAULT ((1)) NOT NULL,
    [ReqDateTimeDatePart] DATETIME       CONSTRAINT [DF_New_NewCarPurchaseInquiries_ReqDateTimeDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [VisitedDealership]   BIT            NULL,
    [CRM_LeadId]          NUMERIC (18)   NULL,
    [ClientIP]            VARCHAR (100)  NULL,
    [PQPageId]            SMALLINT       NULL,
    CONSTRAINT [PK_NewCarPurchaseInquiries_New] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_ForwardedLead]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([ForwardedLead] ASC)
    INCLUDE([CustomerId], [CarVersionId], [RequestDateTime]);


GO
CREATE NONCLUSTERED INDEX [IX_newcarpurchaseinquiries_CarVersionId]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([CarVersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_newcarpurchaseinquiries_ReqDateTimeDatePart]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([ReqDateTimeDatePart] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_newcarpurchaseinquiries_customerid]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NewCarPurchaseInquiries_SourceId]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([SourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarPurchaseInquiries__CustomerId__CarVersionId__Id__RequestDateTime]
    ON [dbo].[NewCarPurchaseInquiries_Prod]([CustomerId] ASC, [CarVersionId] ASC, [Id] ASC, [RequestDateTime] ASC)
    INCLUDE([BuyTime]);

