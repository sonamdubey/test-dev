CREATE TABLE [dbo].[BannerType] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BannerType] VARCHAR (50) NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_BannerType_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_BannerType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

