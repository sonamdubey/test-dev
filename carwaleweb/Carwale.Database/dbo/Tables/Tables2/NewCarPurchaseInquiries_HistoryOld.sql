CREATE TABLE [dbo].[NewCarPurchaseInquiries_HistoryOld] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [CarVersionId]        NUMERIC (18)   NOT NULL,
    [Color]               VARCHAR (50)   NULL,
    [NoOfCars]            INT            CONSTRAINT [DF_NewCarPurchaseInquiries_NoOfCars] DEFAULT ((1)) NOT NULL,
    [BuyTime]             VARCHAR (50)   CONSTRAINT [DF_NewCarPurchaseInquiries_BuyTime] DEFAULT ('1 Week') NULL,
    [Comments]            VARCHAR (2000) NULL,
    [RequestDateTime]     DATETIME       NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsFake]              BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsFake] DEFAULT ((0)) NOT NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_NewCarPurchaseInquiries_StatusId] DEFAULT ((1)) NOT NULL,
    [IsForwarded]         BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsForwarded] DEFAULT ((0)) NOT NULL,
    [IsRejected]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsViewed]            BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsViewed] DEFAULT ((0)) NOT NULL,
    [IsMailSend]          BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_IsMailSend] DEFAULT ((0)) NOT NULL,
    [TestdriveDate]       VARCHAR (100)  NULL,
    [TestDriveLocation]   VARCHAR (300)  NULL,
    [LatestOffers]        BIT            NULL,
    [ForwardedLead]       BIT            CONSTRAINT [DF_NewCarPurchaseInquiries_ForwardedLead] DEFAULT ((1)) NOT NULL,
    [SourceId]            SMALLINT       CONSTRAINT [DF_NewCarPurchaseInquiries_SourceId] DEFAULT ((1)) NOT NULL,
    [ReqDateTimeDatePart] DATETIME       CONSTRAINT [DF_NewCarPurchaseInquiries_ReqDateTimeDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [VisitedDealership]   BIT            NULL,
    CONSTRAINT [PK_NewCarPurchaseInquiries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_FwdLead_Custid_CarVer_RqstDT]
    ON [dbo].[NewCarPurchaseInquiries_HistoryOld]([ForwardedLead] ASC)
    INCLUDE([CustomerId], [CarVersionId], [RequestDateTime]);

