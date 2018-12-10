CREATE TABLE [dbo].[TC_DealerCertificationDetail] (
    [DealersID]                 NUMERIC (18)  NOT NULL,
    [Classified_CertifiedOrgId] SMALLINT      NULL,
    [Description]               VARCHAR (MAX) NULL,
    [Advantages]                VARCHAR (MAX) NULL,
    [Criteria]                  VARCHAR (MAX) NULL,
    [CoreBenefits]              VARCHAR (MAX) NULL,
    [CheckPoints]               VARCHAR (MAX) NULL,
    [WarrantyServices]          VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([DealersID] ASC)
);

