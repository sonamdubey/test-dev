CREATE TABLE [dbo].[OLM_SantaWish] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50)  NULL,
    [Contact]    VARCHAR (15)  NULL,
    [Email]      VARCHAR (50)  NULL,
    [Wish]       VARCHAR (200) NULL,
    [FacebookId] VARCHAR (20)  NULL,
    [ClientIp]   VARCHAR (20)  NULL,
    [EntryDate]  DATETIME      CONSTRAINT [DF_OLM_SantaWish_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_OLM_SantaWish] PRIMARY KEY CLUSTERED ([Id] ASC)
);

