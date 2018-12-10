CREATE TABLE [dbo].[AbSure_EligibleModels] (
    [Id]                      INT             IDENTITY (1, 1) NOT NULL,
    [ModelId]                 INT             NULL,
    [SilverPrice]             INT             NULL,
    [GoldPrice]               INT             NULL,
    [IsActive]                BIT             NULL,
    [UpdatedBy]               INT             NULL,
    [UpdatedOn]               DATETIME        NULL,
    [GoldSalesCost]           DECIMAL (18, 2) NULL,
    [SilverSalesCost]         DECIMAL (18, 2) NULL,
    [IsEligibleWarranty]      BIT             NULL,
    [IsEligibleCertification] BIT             NULL,
    [ModelEntryDate]          DATETIME        CONSTRAINT [DF_AbSure_EligibleModels_ModelEntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AbSure_EligibleModels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

