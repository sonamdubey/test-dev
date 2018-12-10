CREATE TABLE [dbo].[NewCarLaunched] (
    [VersionId]      NUMERIC (18)  NOT NULL,
    [LaunchedDate]   DATETIME      NOT NULL,
    [DestinationUrl] VARCHAR (200) NOT NULL,
    [isActive]       BIT           NOT NULL,
    CONSTRAINT [PK_NewCarLaunched] PRIMARY KEY CLUSTERED ([VersionId] ASC) WITH (FILLFACTOR = 90)
);

