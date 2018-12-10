CREATE TABLE [dbo].[AndroidAppVersions] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [VersionId]   INT          NULL,
    [IsSupported] BIT          NULL,
    [IsLatest]    BIT          NULL,
    [Description] VARCHAR (50) NULL,
    CONSTRAINT [PK__AndroidA__3214EC2745BA94C0] PRIMARY KEY CLUSTERED ([ID] ASC)
);

