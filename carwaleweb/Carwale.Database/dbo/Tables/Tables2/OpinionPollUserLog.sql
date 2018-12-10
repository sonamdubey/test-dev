CREATE TABLE [dbo].[OpinionPollUserLog] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuesId]       NUMERIC (18)  NOT NULL,
    [AnsId]        NUMERIC (18)  NOT NULL,
    [UserId]       NUMERIC (18)  NULL,
    [PollDateTime] DATETIME      NOT NULL,
    [IPAddress]    VARCHAR (50)  NOT NULL,
    [PageRefferer] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_OpinionPollUserLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

