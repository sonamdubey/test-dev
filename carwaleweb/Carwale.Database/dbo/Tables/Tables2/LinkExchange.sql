CREATE TABLE [dbo].[LinkExchange] (
    [Id]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]         VARCHAR (50)   NULL,
    [Link]          VARCHAR (200)  NULL,
    [Description]   VARCHAR (1000) NULL,
    [SubmittedDate] DATETIME       NULL,
    CONSTRAINT [PK_LinkExchange] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

