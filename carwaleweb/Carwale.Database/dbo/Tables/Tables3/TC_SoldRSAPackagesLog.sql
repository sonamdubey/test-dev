CREATE TABLE [dbo].[TC_SoldRSAPackagesLog] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [SoldRSAId]      INT          NULL,
    [Name]           VARCHAR (50) NULL,
    [MobileNo]       VARCHAR (10) NULL,
    [EmailId]        VARCHAR (40) NULL,
    [MakeYear]       DATETIME     NULL,
    [CarMakeId]      INT          NULL,
    [CarModelId]     INT          NULL,
    [CarVersionId]   INT          NULL,
    [RegistrationNo] VARCHAR (40) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

