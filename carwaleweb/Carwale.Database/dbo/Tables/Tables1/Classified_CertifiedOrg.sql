CREATE TABLE [dbo].[Classified_CertifiedOrg] (
    [Id]               SMALLINT      IDENTITY (1, 1) NOT NULL,
    [CertifiedOrgName] VARCHAR (100) NOT NULL,
    [LogoURL]          VARCHAR (250) NOT NULL,
    [Description]      VARCHAR (MAX) NULL,
    [IsActive]         BIT           CONSTRAINT [DF_Classified_CertifiedOrg_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated]     BIT           CONSTRAINT [DF__Classifie__IsRep__3B9B6252] DEFAULT ((1)) NULL,
    [HostURL]          VARCHAR (100) CONSTRAINT [DF__Classifie__HostU__5DF07A56] DEFAULT ('img1.aeplcdn.com') NULL,
    [DirectoryPath]    VARCHAR (100) CONSTRAINT [DF_Classified_CertifiedOrg_DirectoryPath] DEFAULT ('/AutoBiz/Certifications/') NULL,
    [Advantages]       VARCHAR (MAX) NULL,
    [Criteria]         VARCHAR (MAX) NULL,
    [CoreBenefits]     VARCHAR (MAX) NULL,
    [CheckPoints]      VARCHAR (MAX) NULL,
    [WarrantyServices] VARCHAR (MAX) NULL,
    [OriginalImgPath]  VARCHAR (250) NULL,
    CONSTRAINT [PK_Classified_CertifiedOrg] PRIMARY KEY CLUSTERED ([Id] ASC)
);

