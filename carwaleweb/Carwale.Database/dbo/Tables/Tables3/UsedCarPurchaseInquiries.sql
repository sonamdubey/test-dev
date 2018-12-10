CREATE TABLE [dbo].[UsedCarPurchaseInquiries] (
    [Id]                        NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerID]                NUMERIC (18)   NOT NULL,
    [SellInquiryId]             NUMERIC (18)   NULL,
    [CarModelIds]               VARCHAR (100)  NULL,
    [CarModelNames]             VARCHAR (500)  NULL,
    [PriceFrom]                 NUMERIC (18)   NULL,
    [PriceTo]                   NUMERIC (18)   NULL,
    [KmFrom]                    NUMERIC (18)   NULL,
    [KmTo]                      NUMERIC (18)   NULL,
    [YearFrom]                  INT            NULL,
    [YearTo]                    INT            NULL,
    [NoOfCars]                  INT            CONSTRAINT [DF_UsedCarPurchaseInquiries_NoOfCars] DEFAULT (1) NOT NULL,
    [BuyTime]                   VARCHAR (20)   CONSTRAINT [DF_UsedCarPurchaseInquiries_BuyTime] DEFAULT ('1 Week') NULL,
    [Comments]                  VARCHAR (2000) NULL,
    [RequestDateTime]           DATETIME       NOT NULL,
    [IsApproved]                BIT            CONSTRAINT [DF_UsedCarPurchaseInquiries_IsApproved] DEFAULT (0) NOT NULL,
    [IsFake]                    BIT            CONSTRAINT [DF_UsedCarPurchaseInquiries_IsFake] DEFAULT (0) NOT NULL,
    [StatusId]                  SMALLINT       CONSTRAINT [DF_UsedCarPurchaseInquiries_StatusId] DEFAULT (1) NOT NULL,
    [SourceId]                  SMALLINT       CONSTRAINT [DF_UsedCarPurchaseInquiries_SourceId] DEFAULT ((1)) NULL,
    [UsedCarPurchaseInquirybit] BIT            DEFAULT ((0)) NULL,
    [IPAddress]                 VARCHAR (20)   NULL,
    [LTSrc]                     VARCHAR (100)  NULL,
    [Cwc]                       VARCHAR (100)  NULL,
    [UtmaCookie]                VARCHAR (250)  NULL,
    [UtmzCookie]                VARCHAR (500)  NULL,
    [UsedCarPurchaseOriginId]   INT            NULL,
    [IMEICode]                  VARCHAR (50)   NULL,
    [NotificationId]            SMALLINT       NULL,
    [IsSentToCarTrade]          BIT            CONSTRAINT [DF_UsedCarPurchaseInquiries_IsSentToCarTrade] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UsedCarPurchaseInquiry] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_UsedCarPurchaseInquiries_SellInquiryId]
    ON [dbo].[UsedCarPurchaseInquiries]([SellInquiryId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_UsedCarPurchaseInquiries__CustomerID]
    ON [dbo].[UsedCarPurchaseInquiries]([CustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_UsedCarPurchaseInquiries__IsFake__StatusId__RequestDateTime]
    ON [dbo].[UsedCarPurchaseInquiries]([IsFake] ASC, [StatusId] ASC, [RequestDateTime] ASC)
    INCLUDE([CustomerID], [SellInquiryId]);


GO
CREATE NONCLUSTERED INDEX [ix_UsedCarPurchaseInquiries_RequestDateTime]
    ON [dbo].[UsedCarPurchaseInquiries]([RequestDateTime] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'default binding value will be 1(carwale)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsedCarPurchaseInquiries', @level2type = N'COLUMN', @level2name = N'SourceId';

