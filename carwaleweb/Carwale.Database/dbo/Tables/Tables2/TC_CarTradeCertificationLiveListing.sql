CREATE TABLE [dbo].[TC_CarTradeCertificationLiveListing] (
    [TC_CarTradeCertificationLiveId]    INT IDENTITY (1, 1) NOT NULL,
    [ListingId]                         INT NOT NULL,
    [TC_CarTradeCertificationRequestId] INT NOT NULL,
    CONSTRAINT [PK_TC_CTCertListing] PRIMARY KEY CLUSTERED ([TC_CarTradeCertificationLiveId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarTradeCertificationLiveListing_ListingId]
    ON [dbo].[TC_CarTradeCertificationLiveListing]([ListingId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StockId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarTradeCertificationLiveListing', @level2type = N'COLUMN', @level2name = N'ListingId';

