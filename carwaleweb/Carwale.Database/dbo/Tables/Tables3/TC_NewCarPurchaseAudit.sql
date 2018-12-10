CREATE TABLE [dbo].[TC_NewCarPurchaseAudit] (
    [Id]                        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]                  INT          NULL,
    [UserId]                    INT          NULL,
    [CustomerId]                NUMERIC (18) NULL,
    [BuyDate]                   DATETIME     NULL,
    [NewCarPurchaseInquiriesId] NUMERIC (18) NULL,
    [BuyPoints]                 INT          NOT NULL,
    [VersionId]                 NUMERIC (18) NULL,
    [TC_InquiriesLeadId]        BIGINT       NULL,
    CONSTRAINT [PK_TC_NewCarPurchaseAudit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

