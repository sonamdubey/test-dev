CREATE TABLE [dbo].[CW_RenaultLodgyCustomerAnswers] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT          NULL,
    [QuestionId] VARCHAR (50) NULL,
    [OptionId]   VARCHAR (50) NULL,
    [EntryDate]  DATETIME     CONSTRAINT [DF_CW_RenaultLodgyCustomerAnswers_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CW_RenaultLodgyCustomerAnswers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

