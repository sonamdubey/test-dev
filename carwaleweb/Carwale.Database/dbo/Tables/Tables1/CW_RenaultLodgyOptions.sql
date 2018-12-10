CREATE TABLE [dbo].[CW_RenaultLodgyOptions] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [QuestionId]   INT           NOT NULL,
    [OptionNumber] INT           NULL,
    [OptionValue]  VARCHAR (150) NULL,
    [IsActive]     BIT           NULL,
    CONSTRAINT [PK_RenaultLodgyOptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

