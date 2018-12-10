CREATE TABLE [dbo].[RVN_DealerPackageFeatures] (
    [DealerPackageFeatureID]  INT             IDENTITY (1, 1) NOT NULL,
    [DealerId]                INT             NULL,
    [PackageId]               INT             NULL,
    [EntryDate]               DATETIME        NULL,
    [AmountPaid]              BIGINT          NULL,
    [LeadCount]               INT             NULL,
    [DeliveryCount]           INT             DEFAULT ((0)) NULL,
    [PackageStartDate]        DATETIME        NULL,
    [PackageEndDate]          DATETIME        NULL,
    [IsActive]                BIT             DEFAULT ((0)) NULL,
    [LeadType]                TINYINT         NULL,
    [PackageStatus]           TINYINT         NULL,
    [OprUserId]               SMALLINT        NULL,
    [SalesDealerId]           INT             NULL,
    [ModelId]                 SMALLINT        NULL,
    [PackageStatusDate]       DATETIME        NULL,
    [IsReminderSent]          BIT             NULL,
    [ReminderDate]            DATETIME        NULL,
    [MakeId]                  SMALLINT        NULL,
    [AttachedFileExtension]   VARCHAR (50)    NULL,
    [Comments]                VARCHAR (1000)  NULL,
    [HostURL]                 VARCHAR (100)   NULL,
    [ClosingAmount]           INT             NULL,
    [PaymentMode]             TINYINT         NULL,
    [DiscountAmount]          INT             NULL,
    [ProductAmount]           INT             NULL,
    [ServiceTax]              FLOAT (53)      NULL,
    [IsTDSGiven]              BIT             NULL,
    [TDSAmount]               DECIMAL (18, 2) NULL,
    [UpdatedBy]               INT             NULL,
    [UpdatedOn]               DATETIME        NULL,
    [ApprovedBy]              INT             NULL,
    [PANNumber]               VARCHAR (10)    NULL,
    [TANNumber]               VARCHAR (15)    NULL,
    [PackageQuantity]         INT             NULL,
    [IsRenew]                 BIT             NULL,
    [RenewDate]               DATETIME        NULL,
    [ApprovedOn]              DATETIME        NULL,
    [RenewSalesDealerId]      NUMERIC (18)    NULL,
    [ProductSalesDealerId]    NUMERIC (18)    NULL,
    [SuspendedBy]             INT             NULL,
    [TransactionId]           INT             NULL,
    [PercentageSlab]          FLOAT (53)      NULL,
    [AttachedLPA]             VARCHAR (250)   NULL,
    [CampaignType]            SMALLINT        NULL,
    [TotalLead]               INT             NULL,
    [ProductPitchingComments] VARCHAR (1000)  NULL,
    [Model]                   VARCHAR (100)   NULL,
    [ExceptionModel]          VARCHAR (100)   NULL,
    [LeadPerDay]              INT             NULL,
    [ContractType]            INT             NULL,
    [IsMultiOutlet]           BIT             NULL,
    [Source]                  TINYINT         NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_RVN_DealerPackageFeatures_IsActive]
    ON [dbo].[RVN_DealerPackageFeatures]([IsActive] ASC)
    INCLUDE([DealerPackageFeatureID], [PackageId]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No. of leads need to be delivered as per package', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'LeadCount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'No. of leads delivered till date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'DeliveryCount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package activation date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'PackageStartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package expiry date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'PackageEndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package active for a dealer.  There will same package active for a same dealer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'IsActive';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LeadType identify package is new or renewal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'LeadType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key of PackageStatuses table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'PackageStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key of OprUsers table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'OprUserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign key of DCRM_SalesDealer table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'SalesDealerId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To get the details of Dealer package for which model leads need to delivered', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RVN_DealerPackageFeatures', @level2type = N'COLUMN', @level2name = N'ModelId';

