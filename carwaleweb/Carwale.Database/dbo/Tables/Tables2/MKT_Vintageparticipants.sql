CREATE TABLE [dbo].[MKT_Vintageparticipants] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [Mobile]    VARCHAR (20) NOT NULL,
    [EntryDate] DATETIME     CONSTRAINT [DF_MKT_Vintageparticipants_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MKT_Vintageparticipants] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

