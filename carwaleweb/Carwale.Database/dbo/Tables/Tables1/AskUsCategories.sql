CREATE TABLE [dbo].[AskUsCategories] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (500) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_AskUsCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AskUsCategories] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

