CREATE TABLE [dbo].[RoadTests] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]     NUMERIC (18)  NULL,
    [VersionId]   NUMERIC (18)  NULL,
    [Path]        VARCHAR (100) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_RoadTests_IsActive] DEFAULT (1) NOT NULL,
    [PublishDate] DATETIME      NULL,
    [LaunchDate]  DATETIME      NULL,
    [Title]       VARCHAR (150) NULL,
    CONSTRAINT [PK_RoadTests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

