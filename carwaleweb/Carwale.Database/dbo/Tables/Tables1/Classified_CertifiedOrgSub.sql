CREATE TABLE [dbo].[Classified_CertifiedOrgSub] (
    [CertifiedOrgId] SMALLINT     NOT NULL,
    [DealerId]       NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Classified_CertifiedOrgSub] PRIMARY KEY CLUSTERED ([CertifiedOrgId] ASC, [DealerId] ASC)
);

