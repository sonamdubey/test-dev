CREATE TABLE [dbo].[CW_PlayerOfTheDay] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [WinnerName]     VARCHAR (100) NULL,
    [PrizeText]      VARCHAR (100) NULL,
    [Day]            INT           NULL,
    [IsActive]       BIT           NULL,
    [EntryDate]      DATETIME      CONSTRAINT [DF_CW_PlayerOfTheDay_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]   DATETIME      NULL,
    [WinnerComments] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CW_PlayerOfTheDay] PRIMARY KEY CLUSTERED ([Id] ASC)
);

