CREATE TABLE [dbo].[AP_DealerPackageInquiries] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]      NUMERIC (18) NULL,
    [SellInquiryId] NUMERIC (18) NULL,
    [SendDate]      DATETIME     NULL,
    CONSTRAINT [PK_AP_DealerPackageInquiries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_AP_DealerPackageInquiries__DealerId__SendDate]
    ON [dbo].[AP_DealerPackageInquiries]([DealerId] ASC, [SendDate] ASC)
    INCLUDE([SellInquiryId]);


GO
CREATE NONCLUSTERED INDEX [IX_AP_DealerPackageInquiries_SellInquiryId]
    ON [dbo].[AP_DealerPackageInquiries]([SellInquiryId] ASC);

