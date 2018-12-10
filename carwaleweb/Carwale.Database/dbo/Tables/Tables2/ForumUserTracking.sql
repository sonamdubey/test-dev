CREATE TABLE [dbo].[ForumUserTracking] (
    [SessionID]        VARCHAR (100) NOT NULL,
    [UserID]           NUMERIC (18)  NOT NULL,
    [ActivityId]       NUMERIC (18)  NULL,
    [CategoryId]       NUMERIC (18)  NULL,
    [ThreadId]         NUMERIC (18)  NULL,
    [ActivityDateTime] DATETIME      NULL,
    CONSTRAINT [PK_ForumUserTracking] PRIMARY KEY CLUSTERED ([SessionID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ForumUserTracking__UserID]
    ON [dbo].[ForumUserTracking]([UserID] ASC)
    INCLUDE([ActivityDateTime]);

