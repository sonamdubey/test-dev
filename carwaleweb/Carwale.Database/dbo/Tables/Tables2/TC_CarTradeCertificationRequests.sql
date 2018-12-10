CREATE TABLE [dbo].[TC_CarTradeCertificationRequests] (
    [TC_CarTradeCertificationRequestId] INT           IDENTITY (1, 1) NOT NULL,
    [RegistrationNo]                    VARCHAR (20)  NOT NULL,
    [DealerId]                          INT           NOT NULL,
    [ListingId]                         INT           NOT NULL,
    [DealerName]                        VARCHAR (100) NOT NULL,
    [DealerMobile]                      VARCHAR (15)  NOT NULL,
    [DealerAddress]                     VARCHAR (500) NOT NULL,
    [Make]                              VARCHAR (20)  NOT NULL,
    [Model]                             VARCHAR (20)  NOT NULL,
    [Variant]                           VARCHAR (20)  NOT NULL,
    [Color]                             VARCHAR (20)  NOT NULL,
    [ManufacturingYear]                 INT           NOT NULL,
    [CarTradeCertificationId]           INT           NOT NULL,
    [CertificationStatus]               TINYINT       NULL,
    [EntryDate]                         DATETIME      NULL,
    [StatusDate]                        DATETIME      NULL,
    [CertificationStatusDesc]           VARCHAR (150) NULL,
    [DealerEmail]                       VARCHAR (150) NULL,
    [DealerContactPerson]               VARCHAR (100) NULL,
    [DealerPinCode]                     VARCHAR (10)  NULL,
    [DealerCity]                        VARCHAR (20)  NULL,
    [PolicyNo]                          VARCHAR (50)  NULL,
    CONSTRAINT [PK_TC_CTCertRequests] PRIMARY KEY CLUSTERED ([TC_CarTradeCertificationRequestId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarTradeCertificationRequests_ListingId]
    ON [dbo].[TC_CarTradeCertificationRequests]([ListingId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_CarTradeCertificationRequests', @level2type = N'COLUMN', @level2name = N'CertificationStatus';

