CREATE TABLE [dbo].[TC_FeedbackCalling_InqFeedback] (
    [TC_FeedbackCalling_InqFeedbackId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesLeadId]               INT           NOT NULL,
    [TC_FeedbackCalling_QuestionsId]   INT           NOT NULL,
    [TC_FeedbackCalling_AnswerId]      INT           NULL,
    [TC_FeedbackCalling_AnswerText]    VARCHAR (100) NULL,
    [EntryDate]                        DATETIME      NOT NULL,
    [EnteredBy]                        INT           NOT NULL,
    CONSTRAINT [PK_TC_FeedbackCalling_InqFeedback] PRIMARY KEY CLUSTERED ([TC_FeedbackCalling_InqFeedbackId] ASC)
);

