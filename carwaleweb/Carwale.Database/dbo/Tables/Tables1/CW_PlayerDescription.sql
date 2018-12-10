CREATE TABLE [dbo].[CW_PlayerDescription] (
    [Id]                NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Day]               INT            NULL,
    [PlayerName]        VARCHAR (100)  NULL,
    [PlayerDescription] VARCHAR (2000) NULL,
    [IsActive]          BIT            NULL,
    [EntryDate]         DATETIME       CONSTRAINT [DF_CW_PlayerDescription_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]      DATETIME       NULL,
    CONSTRAINT [PK_CW_PlayerDescription] PRIMARY KEY CLUSTERED ([Id] ASC)
);

