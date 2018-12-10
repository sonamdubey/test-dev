CREATE TABLE [dbo].[OLM_UE_Answers] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [QuestionId] INT           NOT NULL,
    [Answer]     VARCHAR (100) NOT NULL,
    [IsCorrect]  BIT           NOT NULL,
    CONSTRAINT [PK_OLM_UE_Answers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

