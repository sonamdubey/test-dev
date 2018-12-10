CREATE TABLE [dbo].[CWCTDealerMappingLog] (
    [Id]                   INT          IDENTITY (1, 1) NOT NULL,
    [CWCTDealerMappingId]  INT          NULL,
    [CWDealerID]           INT          NULL,
    [CTDealerID]           INT          NULL,
    [PackageId]            INT          NULL,
    [PackageStartDate]     DATETIME     NULL,
    [PackageEndDate]       DATETIME     NULL,
    [CreatedOn]            DATETIME     CONSTRAINT [DF_CWCTDealerMappingLog_CreatedOn] DEFAULT (getdate()) NULL,
    [MaskingNumber]        VARCHAR (20) NULL,
    [HasSellerLeadPackage] BIT          NULL,
    [HasBannerAd]          BIT          NULL,
    [IsMigrated]           BIT          NULL,
    [MigrationRequestDate] DATETIME     NULL,
    [MigrationSuccessDate] DATETIME     NULL,
    [UpdatedBy]            INT          NULL,
    CONSTRAINT [PK_CWCTDealerMappingLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

