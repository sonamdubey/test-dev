CREATE TABLE [dbo].[Classified_CertifiedOrg_Bkp15082015] (
    [Id]               SMALLINT      IDENTITY (1, 1) NOT NULL,
    [CertifiedOrgName] VARCHAR (100) NOT NULL,
    [LogoURL]          VARCHAR (250) NOT NULL,
    [Description]      VARCHAR (MAX) NULL,
    [IsActive]         BIT           NOT NULL,
    [IsReplicated]     BIT           NULL,
    [HostURL]          VARCHAR (100) NULL,
    [DirectoryPath]    VARCHAR (100) NULL,
    [Advantages]       VARCHAR (MAX) NULL,
    [Criteria]         VARCHAR (MAX) NULL,
    [CoreBenefits]     VARCHAR (MAX) NULL,
    [CheckPoints]      VARCHAR (MAX) NULL,
    [WarrantyServices] VARCHAR (MAX) NULL,
    [OriginalImgPath]  VARCHAR (250) NULL
);

