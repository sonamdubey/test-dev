CREATE TABLE [dbo].[FeedbackAnswers] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuestionId] NUMERIC (18)  NULL,
    [Answer]     VARCHAR (300) NULL,
    [Alias]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_FeedbackAnswers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

