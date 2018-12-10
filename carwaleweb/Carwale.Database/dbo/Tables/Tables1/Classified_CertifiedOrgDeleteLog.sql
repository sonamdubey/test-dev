CREATE TABLE [dbo].[Classified_CertifiedOrgDeleteLog] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [CertifiedOrgName] VARCHAR (100) NOT NULL,
    [LogoURL]          VARCHAR (100) NOT NULL,
    [Description]      VARCHAR (MAX) NULL,
    [IsActive]         BIT           NOT NULL,
    [HostURL]          VARCHAR (100) NOT NULL,
    [IsReplicated]     BIT           NULL,
    [DirectoryPath]    VARCHAR (100) NOT NULL,
    [Advantages]       VARCHAR (MAX) NULL,
    [Criteria]         VARCHAR (MAX) NULL,
    [CoreBenefits]     VARCHAR (MAX) NULL,
    [CheckPoints]      VARCHAR (MAX) NULL,
    [WarrantyServices] VARCHAR (MAX) NULL,
    [DeletedBy]        INT           NOT NULL,
    [DeletedOn]        DATETIME      CONSTRAINT [DF_Classified_CertifiedOrgDeleteLog_DeletedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ClassCertDelLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

