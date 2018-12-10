CREATE TABLE [dbo].[CWCTDealerMapping] (
    [id]                   SMALLINT     IDENTITY (1, 1) NOT NULL,
    [CWDealerID]           INT          NULL,
    [CTDealerID]           INT          NULL,
    [PackageId]            INT          NULL,
    [PackageStartDate]     DATETIME     NULL,
    [PackageEndDate]       DATETIME     NULL,
    [CreatedOn]            DATETIME     NULL,
    [UpdatedOn]            DATETIME     NULL,
    [MaskingNumber]        VARCHAR (20) NULL,
    [HasSellerLeadPackage] BIT          NULL,
    [HasBannerAd]          BIT          NULL,
    [IsMigrated]           BIT          NULL,
    [MigrationRequestDate] DATETIME     NULL,
    [MigrationSuccessDate] DATETIME     NULL,
    [UpdatedBy]            INT          NULL,
    CONSTRAINT [PK_CWCTDealerMapping] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CWCTDealerMapping_CWDealerID]
    ON [dbo].[CWCTDealerMapping]([CWDealerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CWCTDealerMapping_CTDealerID]
    ON [dbo].[CWCTDealerMapping]([CTDealerID] ASC);

