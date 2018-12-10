CREATE TABLE [dbo].[UnregisteredCustomers] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RegistrationType] SMALLINT      NOT NULL,
    [Email]            VARCHAR (100) NULL,
    [ContactNo]        VARCHAR (100) NULL,
    [Comment]          VARCHAR (500) NULL,
    [RecordId]         NUMERIC (18)  NOT NULL,
    [EntryDateTime]    DATETIME      NOT NULL,
    CONSTRAINT [PK_UnregisteredCustomers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

