CREATE TABLE [dbo].[CAAnswers] (
    [ID]               NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]       NUMERIC (18)   NOT NULL,
    [CAQuestionId]     NUMERIC (18)   NOT NULL,
    [Answer]           VARCHAR (2000) NOT NULL,
    [AnswerDateTime]   DATETIME       NOT NULL,
    [IsActive]         BIT            CONSTRAINT [DF_CAAnswers_IsActive] DEFAULT (1) NOT NULL,
    [IsApproved]       BIT            CONSTRAINT [DF_CAAnswers_IsApproved] DEFAULT (1) NOT NULL,
    [QuestionerRating] INT            NULL,
    CONSTRAINT [PK_CAAnswers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_CAAnswers__CAQuestionId]
    ON [dbo].[CAAnswers]([CAQuestionId] ASC);

