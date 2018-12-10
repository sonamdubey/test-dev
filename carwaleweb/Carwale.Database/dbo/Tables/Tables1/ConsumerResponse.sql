CREATE TABLE [dbo].[ConsumerResponse] (
    [Id]               NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PriceRange]       VARCHAR (50) NOT NULL,
    [Response]         NUMERIC (18) NOT NULL,
    [AvailableCars]    NUMERIC (18) NOT NULL,
    [EntryDate]        DATETIME     NOT NULL,
    [IsDealerResponse] BIT          NOT NULL,
    CONSTRAINT [PK_ConsumerResponse] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

