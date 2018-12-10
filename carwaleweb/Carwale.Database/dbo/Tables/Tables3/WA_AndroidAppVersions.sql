CREATE TABLE [dbo].[WA_AndroidAppVersions] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [VersionId]       INT          NULL,
    [IsSupported]     BIT          NULL,
    [IsLatest]        BIT          NULL,
    [Description]     VARCHAR (50) NULL,
    [ApplicationType] TINYINT      CONSTRAINT [DF_WA_AndroidAppVersions_ApplicationType] DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

