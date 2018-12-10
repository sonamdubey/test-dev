CREATE TABLE [dbo].[DoNotSendEmail] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Email]              VARCHAR (100) NOT NULL,
    [UnsubscriptionDate] DATETIME      DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_DoNotSendEmail] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_DoNotSendEmail_email]
    ON [dbo].[DoNotSendEmail]([Email] ASC);

