CREATE TABLE [dbo].[ClassifiedRequests] (
    [Id]                      NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]              NUMERIC (18)  NOT NULL,
    [SellInquiryId]           NUMERIC (18)  NOT NULL,
    [Comments]                VARCHAR (500) NULL,
    [RequestDateTime]         DATETIME      NOT NULL,
    [IsActive]                BIT           CONSTRAINT [DF_ClassifiedRequests_IsActive] DEFAULT (1) NOT NULL,
    [SourceId]                SMALLINT      NULL,
    [IPAddress]               VARCHAR (20)  NULL,
    [LTSrc]                   VARCHAR (100) NULL,
    [Cwc]                     VARCHAR (100) NULL,
    [UtmaCookie]              VARCHAR (250) NULL,
    [UtmzCookie]              VARCHAR (500) NULL,
    [UsedCarPurchaseOriginId] INT           NULL,
    [IMEICode]                VARCHAR (50)  NULL,
    [NotificationId]          SMALLINT      NULL,
    CONSTRAINT [PK_ClassifiedRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_SellInquiryId_ClassifiedRequests]
    ON [dbo].[ClassifiedRequests]([SellInquiryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerId_ClassifiedRequests]
    ON [dbo].[ClassifiedRequests]([CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClassifiedRequests_LTSrc]
    ON [dbo].[ClassifiedRequests]([LTSrc] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClassifiedRequests_RequestDateTime]
    ON [dbo].[ClassifiedRequests]([RequestDateTime] ASC)
    INCLUDE([SellInquiryId]);

