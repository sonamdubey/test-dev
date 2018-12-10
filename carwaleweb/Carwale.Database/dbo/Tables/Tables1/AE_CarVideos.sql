CREATE TABLE [dbo].[AE_CarVideos] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]         NUMERIC (18)  NOT NULL,
    [VideoLocation] VARCHAR (150) NULL,
    [IsMainVideo]   BIT           CONSTRAINT [DF_AE_CarVideos_IsMainVideo] DEFAULT ((0)) NOT NULL,
    [Caption]       VARCHAR (150) NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     NUMERIC (18)  NULL,
    CONSTRAINT [PK_AE_CarVideos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

