CREATE TABLE [dbo].[OpinionPollAnswer] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuestionId] NUMERIC (18)  NOT NULL,
    [Answer]     VARCHAR (500) NOT NULL,
    [AnsCount]   NUMERIC (18)  CONSTRAINT [DF_OpinionPollAnswer_AnsCount] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_OpinionPollAnswer] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_OpinionPollAnswer_OpinionPollQues] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[OpinionPollQues] ([ID])
);

