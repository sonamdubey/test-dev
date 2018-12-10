CREATE TABLE [dbo].[UserFeedback] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]        NUMERIC (18)  NOT NULL,
    [Feedback]          VARCHAR (20)  NOT NULL,
    [Comments]          VARCHAR (200) NULL,
    [SourceURL]         VARCHAR (200) NOT NULL,
    [ClientIP]          VARCHAR (50)  NULL,
    [ClientBrowserType] VARCHAR (150) NULL,
    [EntryDate]         DATETIME      NOT NULL,
    CONSTRAINT [PK_UserFeedback] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

