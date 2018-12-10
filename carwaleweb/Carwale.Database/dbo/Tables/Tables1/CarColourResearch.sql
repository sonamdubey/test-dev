CREATE TABLE [dbo].[CarColourResearch] (
    [CCR_Id]      NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]     INT          NOT NULL,
    [HexCode]     VARCHAR (6)  NOT NULL,
    [IsFavourate] BIT          CONSTRAINT [DF_CarColourResearch_IsFavourate] DEFAULT ((0)) NOT NULL,
    [EntryDate]   DATETIME     NOT NULL,
    CONSTRAINT [PK_CarColourResearch] PRIMARY KEY CLUSTERED ([CCR_Id] ASC) WITH (FILLFACTOR = 90)
);

