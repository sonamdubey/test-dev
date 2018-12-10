CREATE TABLE [dbo].[FollowupCriterias] (
    [ID]   SMALLINT      NOT NULL,
    [Name] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_FollowupCriterias] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

