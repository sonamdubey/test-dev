CREATE TABLE [dbo].[FeedbackRemarks] (
    [Id]         NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId] NUMERIC (18)   NULL,
    [QuestionId] NUMERIC (18)   NULL,
    [Answer]     VARCHAR (1000) NULL,
    [FBDate]     DATETIME       NULL,
    [UpdatedOn]  DATETIME       NULL,
    [UpdatedBy]  NUMERIC (18)   NULL,
    CONSTRAINT [PK_FeedbackRemarks] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

