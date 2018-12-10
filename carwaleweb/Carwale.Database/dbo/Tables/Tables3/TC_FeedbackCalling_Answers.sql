CREATE TABLE [dbo].[TC_FeedbackCalling_Answers] (
    [TC_FeedbackCalling_AnswerId] INT          IDENTITY (1, 1) NOT NULL,
    [Answer]                      VARCHAR (10) NOT NULL,
    [IsActive]                    BIT          NOT NULL,
    CONSTRAINT [PK_TC_FeedbackCalling_Answers] PRIMARY KEY CLUSTERED ([TC_FeedbackCalling_AnswerId] ASC)
);

