CREATE TABLE [dbo].[CW_RenaultLodgyQuestion] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Question]       VARCHAR (300) NULL,
    [QuestionNumber] INT           NULL,
    [IsActive]       BIT           NULL,
    CONSTRAINT [PK_RenaultLodgyQuestion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

