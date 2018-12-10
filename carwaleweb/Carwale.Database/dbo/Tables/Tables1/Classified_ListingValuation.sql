CREATE TABLE [dbo].[Classified_ListingValuation] (
    [Id]                 BIGINT   IDENTITY (1, 1) NOT NULL,
    [InquiryId]          BIGINT   NOT NULL,
    [SellerType]         TINYINT  NOT NULL,
    [FairValuation]      BIGINT   NULL,
    [GoodValuation]      BIGINT   NULL,
    [ExcellantValuation] BIGINT   NULL,
    [EntryDate]          DATETIME NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'primary key id for respective tables for dealers and individuals', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'InquiryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for dealer and 2 for individual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'SellerType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fair value of the listed car for sell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'FairValuation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'good value of the listed car for sell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'GoodValuation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'excellant value of the listed car for sell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'ExcellantValuation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Valuation entry date.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ListingValuation', @level2type = N'COLUMN', @level2name = N'EntryDate';

