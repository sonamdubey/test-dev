CREATE TABLE [dbo].[CRM_FeedbackAnswers] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuestionId] NUMERIC (18)  NULL,
    [Answer]     VARCHAR (300) NULL,
    [Alias]      VARCHAR (300) NULL,
    [Rating]     VARCHAR (7)   NULL,
    [IsActive]   BIT           NULL,
    [CreatedBy]  NUMERIC (18)  NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_CRM_FeedbackAnswers_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy]  NUMERIC (18)  NULL,
    [UpdatedOn]  DATETIME      NULL,
    CONSTRAINT [PK_CRM_TDFeedbackAnswers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

