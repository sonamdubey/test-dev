CREATE TABLE [dbo].[NCS_CDCompanyClassification] (
    [ClassificationId] NUMERIC (18) NOT NULL,
    [CompanyId]        NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_NCS_CorporateDiscount] PRIMARY KEY CLUSTERED ([ClassificationId] ASC, [CompanyId] ASC) WITH (FILLFACTOR = 90)
);

