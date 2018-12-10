CREATE TABLE [dbo].[DoNotSentSMS] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MobileNumber] VARCHAR (50)  NULL,
    [Message]      VARCHAR (300) NULL,
    [EntryDate]    DATETIME      NULL,
    CONSTRAINT [PK_DoNotSentSMS] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

