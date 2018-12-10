CREATE TABLE [dbo].[OLM_MansoonCamp] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FullName]  VARCHAR (50)  NOT NULL,
    [Email]     VARCHAR (50)  NOT NULL,
    [Answer]    VARCHAR (100) NOT NULL,
    [PlaceId]   SMALLINT      NOT NULL,
    [EntryDate] DATETIME      CONSTRAINT [DF_OLM_MansoonCamp_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_MansoonCamp] PRIMARY KEY CLUSTERED ([Id] ASC)
);

