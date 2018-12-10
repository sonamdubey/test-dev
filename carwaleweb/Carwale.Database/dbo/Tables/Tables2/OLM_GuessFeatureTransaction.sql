CREATE TABLE [dbo].[OLM_GuessFeatureTransaction] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50)  NOT NULL,
    [Email]     VARCHAR (50)  NOT NULL,
    [Contact]   VARCHAR (50)  NOT NULL,
    [EntryDate] DATETIME      CONSTRAINT [DF_OLM_GuessFeatureTransaction_EntryDate] DEFAULT (getdate()) NOT NULL,
    [Answer]    VARCHAR (100) NULL,
    [Score]     INT           NULL,
    CONSTRAINT [PK_OLM_GuessFeatureTransaction] PRIMARY KEY CLUSTERED ([Id] ASC)
);

