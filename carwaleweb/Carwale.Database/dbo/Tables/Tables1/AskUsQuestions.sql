CREATE TABLE [dbo].[AskUsQuestions] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]       NUMERIC (18)  NOT NULL,
    [Title]            VARCHAR (200) NULL,
    [CustomerId]       NUMERIC (18)  NOT NULL,
    [RequestDateTime]  DATETIME      NOT NULL,
    [IsActive]         BIT           CONSTRAINT [DF_AskUsQuestions_IsActive] DEFAULT (1) NOT NULL,
    [IsPublished]      BIT           CONSTRAINT [DF_AskUsQuestions_IsPublished] DEFAULT (0) NOT NULL,
    [Liked]            NUMERIC (18)  NULL,
    [Disliked]         NUMERIC (18)  NULL,
    [Viewed]           NUMERIC (18)  NULL,
    [QuestionerRating] INT           NULL,
    CONSTRAINT [PK_AskUsQuestions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

