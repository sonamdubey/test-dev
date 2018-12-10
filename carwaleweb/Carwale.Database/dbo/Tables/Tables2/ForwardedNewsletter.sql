CREATE TABLE [dbo].[ForwardedNewsletter] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SenderEmail]   VARCHAR (100) NOT NULL,
    [ReceiverEmail] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ForwardedNewsletter] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

