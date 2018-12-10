CREATE TABLE [dbo].[TC_CarAdditionalInformation] (
    [Id]                             INT           IDENTITY (1, 1) NOT NULL,
    [StockId]                        INT           NULL,
    [LastUpdatedDate]                DATETIME      NULL,
    [EntryDate]                      DATETIME      NULL,
    [ModifiedBy]                     INT           NULL,
    [IsCarInWarranty]                BIT           NULL,
    [WarrantyValidTill]              DATETIME      NULL,
    [WarrantyProvidedBy]             VARCHAR (20)  NULL,
    [ThirdPartyWarrantyProviderName] VARCHAR (50)  NULL,
    [WarrantyDetails]                VARCHAR (200) NULL,
    [HasExtendedWarranty]            BIT           NULL,
    [ExtendedWarrantyValidFor]       VARCHAR (20)  NULL,
    [ExtendedWarrantyProviderName]   VARCHAR (50)  NULL,
    [ExtendedWarrantyDetails]        VARCHAR (200) NULL,
    [HasAnyServiceRecords]           BIT           NULL,
    [ServiceRecordsAvailableFor]     VARCHAR (20)  NULL,
    [HasRSAAvailable]                BIT           NULL,
    [RSAValidTill]                   DATETIME      NULL,
    [RSAProviderName]                VARCHAR (50)  NULL,
    [RSADetails]                     VARCHAR (200) NULL,
    [HasFreeRSA]                     BIT           NULL,
    [FreeRSAValidFor]                VARCHAR (20)  NULL,
    [FreeRSAProvidedBy]              VARCHAR (50)  NULL,
    [FreeRSADetails]                 VARCHAR (200) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarAdditionalInformation_StockId]
    ON [dbo].[TC_CarAdditionalInformation]([StockId] ASC);

