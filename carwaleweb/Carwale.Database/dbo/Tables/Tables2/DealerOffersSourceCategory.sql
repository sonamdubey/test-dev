CREATE TABLE [dbo].[DealerOffersSourceCategory] (
    [Id]       NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Category] VARCHAR (50) NULL,
    CONSTRAINT [PK_DealerOffersSourceCategory] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

