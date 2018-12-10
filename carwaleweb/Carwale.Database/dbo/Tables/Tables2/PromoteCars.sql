CREATE TABLE [dbo].[PromoteCars] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProfileId]          VARCHAR (50) NULL,
    [BestDealsProfileId] VARCHAR (50) NULL,
    [isActive]           BIT          CONSTRAINT [DF_PromoteCars_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_PromoteCars] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

