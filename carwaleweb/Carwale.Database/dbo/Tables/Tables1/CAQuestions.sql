CREATE TABLE [dbo].[CAQuestions] (
    [ID]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CACategoryId]        NUMERIC (18)   NOT NULL,
    [CustomerId]          NUMERIC (18)   NOT NULL,
    [QuestionTitle]       VARCHAR (500)  NOT NULL,
    [QuestionDescription] VARCHAR (2000) NULL,
    [QuestionDateTime]    DATETIME       NOT NULL,
    [IsActive]            BIT            CONSTRAINT [DF_CAQuestions_IsActive] DEFAULT (1) NOT NULL,
    [IsApproved]          BIT            CONSTRAINT [DF_CAQuestions_IsApproved] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_CAQuestions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

