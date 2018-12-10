CREATE TABLE [dbo].[UploadedArticles] (
    [Id]         NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId] NUMERIC (18)   NOT NULL,
    [CustomerId] NUMERIC (18)   NOT NULL,
    [Title]      VARCHAR (100)  NOT NULL,
    [Synopsis]   NVARCHAR (250) NOT NULL,
    [FileName]   VARCHAR (30)   NULL,
    CONSTRAINT [PK_UploadedArticles] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

