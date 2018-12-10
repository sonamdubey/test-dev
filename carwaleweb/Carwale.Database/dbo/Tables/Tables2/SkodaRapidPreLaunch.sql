CREATE TABLE [dbo].[SkodaRapidPreLaunch] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ModelId]   INT           NOT NULL,
    [ModelName] VARCHAR (100) NOT NULL,
    [Name]      VARCHAR (250) NULL,
    [Email]     VARCHAR (250) NULL,
    [Mobile]    VARCHAR (15)  NULL,
    [EntryDate] DATETIME      CONSTRAINT [DF_SkodaRapidPreLaunch_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SkodaRapidPreLaunch] PRIMARY KEY CLUSTERED ([Id] ASC)
);

