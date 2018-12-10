CREATE TABLE [dbo].[MessageToDealers] (
    [Id]        NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]  NUMERIC (18)   NOT NULL,
    [Message]   VARCHAR (1000) NOT NULL,
    [EntryDate] DATETIME       NOT NULL,
    [isActive]  BIT            CONSTRAINT [DF_MessageToDealers_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_MessageToDealers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

