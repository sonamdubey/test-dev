CREATE TABLE [dbo].[TC_FeedbackCalling_Questions] (
    [TC_FeedbackCalling_QuestionsId] INT           IDENTITY (1, 1) NOT NULL,
    [Question]                       VARCHAR (100) NOT NULL,
    [IsActive]                       BIT           NOT NULL,
    CONSTRAINT [PK_TC_FeedbackCalling_Questions] PRIMARY KEY CLUSTERED ([TC_FeedbackCalling_QuestionsId] ASC)
);

