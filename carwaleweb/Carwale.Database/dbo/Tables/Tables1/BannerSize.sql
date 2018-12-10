CREATE TABLE [dbo].[BannerSize] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BannerSize] VARCHAR (50) NOT NULL,
    [BannerType] NUMERIC (18) NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_BannerSize_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_BannerSize] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

