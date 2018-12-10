CREATE TABLE [dbo].[TempFollowupStatus] (
    [ID]   SMALLINT     NOT NULL,
    [Name] VARCHAR (50) NULL,
    CONSTRAINT [PK_TempFollowupStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

