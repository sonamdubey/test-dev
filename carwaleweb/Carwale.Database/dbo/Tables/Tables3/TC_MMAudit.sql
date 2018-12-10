CREATE TABLE [dbo].[TC_MMAudit] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]    INT          NOT NULL,
    [UserId]      INT          NOT NULL,
    [CustomerId]  NUMERIC (18) NOT NULL,
    [BuyDate]     DATETIME     NULL,
    [CWInquiryId] NUMERIC (18) NOT NULL,
    [BuyPoints]   INT          NOT NULL,
    [ABInquiryId] NUMERIC (18) NULL,
    [SellerType]  TINYINT      NULL,
    CONSTRAINT [PK_TC_MMAudit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

