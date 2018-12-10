CREATE TABLE [dbo].[carmakes_2014] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (30)  NOT NULL,
    [IsDeleted]    BIT           NOT NULL,
    [LogoUrl]      VARCHAR (50)  NOT NULL,
    [Used]         BIT           NOT NULL,
    [New]          BIT           NOT NULL,
    [Indian]       BIT           NOT NULL,
    [Imported]     BIT           NOT NULL,
    [Futuristic]   BIT           NOT NULL,
    [Classic]      BIT           NOT NULL,
    [Modified]     BIT           NOT NULL,
    [MaCreatedOn]  DATETIME      NULL,
    [MaUpdatedBy]  NUMERIC (18)  NULL,
    [MaUpdatedOn]  DATETIME      NULL,
    [IsReplicated] BIT           NULL,
    [HostURL]      VARCHAR (100) NULL
);

