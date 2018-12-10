CREATE TABLE [dbo].[AE_AuctionCarStatus] (
    [Id]   SMALLINT     NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AE_AuctionCarStatus] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

