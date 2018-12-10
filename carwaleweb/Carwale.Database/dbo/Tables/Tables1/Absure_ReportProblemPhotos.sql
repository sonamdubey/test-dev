CREATE TABLE [dbo].[Absure_ReportProblemPhotos] (
    [Absure_ReportProblemPhotosId] INT           IDENTITY (1, 1) NOT NULL,
    [Absure_ReportProblemsId]      INT           NULL,
    [HostUrl]                      VARCHAR (100) NULL,
    [DirectoryPath]                VARCHAR (250) NULL,
    [ImageUrlLarge]                VARCHAR (250) NULL,
    [ImageUrlExtraLarge]           VARCHAR (250) NULL,
    [ImageUrlOriginal]             VARCHAR (250) NULL,
    [IsReplicated]                 BIT           NULL,
    [StatusId]                     INT           NULL,
    PRIMARY KEY CLUSTERED ([Absure_ReportProblemPhotosId] ASC)
);

