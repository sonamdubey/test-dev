CREATE TABLE [dbo].[CRM_FeedbackAnswerType] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [AnswerType] VARCHAR (150) NULL,
    CONSTRAINT [PK_CRM_FeedbackAnswerType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

