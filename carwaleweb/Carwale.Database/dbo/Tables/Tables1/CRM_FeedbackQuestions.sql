CREATE TABLE [dbo].[CRM_FeedbackQuestions] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Question]       VARCHAR (500) NULL,
    [FeedbackType]   INT           NULL,
    [Alias]          VARCHAR (500) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_CRM_FeedbackQuestions_IsActive] DEFAULT ((1)) NOT NULL,
    [IsCommentType]  BIT           CONSTRAINT [DF_CRM_FeedbackQuestions_IsCommentType] DEFAULT ((0)) NOT NULL,
    [AnswerType]     INT           NULL,
    [IsDependent]    BIT           NULL,
    [ParentAnswerId] NUMERIC (18)  NULL,
    [QuestionOrder]  INT           CONSTRAINT [DF_CRM_FeedbackQuestions_QuestionOrder] DEFAULT ((0)) NULL,
    [IsMandatory]    BIT           NULL,
    [CreatedBy]      NUMERIC (18)  NULL,
    [CreatedOn]      DATETIME      CONSTRAINT [DF_CRM_FeedbackQuestions_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy]      NUMERIC (18)  NULL,
    [UpdatedOn]      DATETIME      NULL,
    CONSTRAINT [PK_CRM_TDFeedbackQuestions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

